using BLL.Interfaces;
using DAL.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ImageMagick;
using BLL.Helpers;
using NetVips;
using Microsoft.Extensions.Logging;
using AuxiliaryLib.Helpers;

namespace Training2.Controllers
{
    //https://habr.com/ru/post/422531/
    //https://libvips.github.io/libvips/
    //https://github.com/kleisauke/net-vips
    [Route("api/[controller]")]
    [ApiController]
    public class ProductPhotoController : ControllerBase
    {
        private readonly IProductPhotoService _productPhotoService;
        private readonly AppSettings _appSettings;
        private readonly ILogger<ProductPhotoController> _logger;

        public ProductPhotoController(IProductPhotoService productPhotoService, AppSettings appSettings,
            ILogger<ProductPhotoController> logger)
        {
            (_productPhotoService, _appSettings, _logger) = (productPhotoService, appSettings, logger);
            if (ModuleInitializer.VipsInitialized)
            {
                _logger.LogInformation($"Inited libvips {NetVips.NetVips.Version(0)}.{NetVips.NetVips.Version(1)}.{NetVips.NetVips.Version(2)}");
            }
            else
            {
                _logger.LogError(ModuleInitializer.Exception.Message);
            }
        }

        [HttpGet("GetAllInformationAboutPhotos")]
        public IActionResult GetAllInformationAboutPhotos(int? pageLength = null,
            int? pageNumber = null)
        {
            PageResponse<ProductPhoto> pageResponse = new PageResponse<ProductPhoto>(pageLength, pageNumber);
            List<ProductPhoto> productPhotos = _productPhotoService.GetAll_Queryable().ToList();
            pageResponse.TotalItems = productPhotos.Count();
            pageResponse.Items = productPhotos.Skip(pageResponse.Skip).Take(pageResponse.Take).ToList();
            return Ok(pageResponse);
        }

        [HttpGet("{id}/{filename}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(long id, string filename)
        {
            var file = await _productPhotoService.GetById(id);
            if (file == null || file.IsPublic == false)
            {
                return NotFound();
            }
            return File(_productPhotoService.GetContent(file), file.MimeType, file.Name, true);
        }


        [HttpPost("Add_and_ResizeImage_ImageMagick")]
        public async Task<IActionResult> Add_and_ResizeImage_ImageMagick(IFormFileCollection files, long annoucementId)
        {            
            await AddImage(ResizeImage_ImageMagick, files, annoucementId);
            return StatusCode(200, "Files added successfully");
        }

        [HttpPost("Add_and_CompressionImage_libvips")]
        public async Task<IActionResult> Add_and_Compression_libvips(IFormFileCollection files, long annoucementId)
        {
            if(await AddImage(Compression_libvips, files, annoucementId))
                return StatusCode(200, "Files added successfully");
            else
                return StatusCode(500);
        }

        private async Task<bool> AddImage(Action<string, Stream> action, IFormFileCollection files, long annoucementId)
        {
            bool result = false;
            List<ProductPhoto> storedFiles = new List<ProductPhoto>();
            try
            {
                foreach (var file in files)
                {
                    ProductPhoto storedFile = SaveFiles(file, annoucementId);
                    action.Invoke(_productPhotoService.GetFileName(storedFile), file.OpenReadStream());
                    storedFiles.Add(storedFile);
                }
                await _productPhotoService.SaveProductPhoto(storedFiles);
                result = true;
            }
            catch(Exception ex)
            {
                _logger.LogWarning($"InnerException = {ex?.InnerException?.ToString()} \r\n" +
                            $"Message = {ex?.Message?.ToString()} \r\n" +
                            $"Source = {ex?.Source?.ToString()} \r\n" +
                            $"StackTrace = {ex?.StackTrace?.ToString()} \r\n" +
                            $"TargetSite = {ex?.TargetSite?.ToString()}");
                foreach(var i in storedFiles)
                {
                    string path = _productPhotoService.GetFileName(i);
                    if(System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                }
            }
            return result;
        }

        private void Compression_libvips(string path, Stream stream)
        {
            using (var image = Image.NewFromStream(stream))
            {
                //string oldExtension = Path.GetExtension(path);
                //image.Jpegsave(path);
                string newPath = $"{path}_{DateTime.Now.ToString("yyyy_MM_dd_hh_mm")}.jpeg";
                image.Jpegsave(newPath, q: 40, optimizeCoding: true);
                //image.WriteToFile(newPath, new VOption
                //{
                //    {"Q", 80}
                //});
                //System.IO.File.Delete(path);

                System.IO.File.Move(newPath, path);
            }
                
            
        }

        
        private void ResizeImage_ImageMagick(string path, Stream stream)
        {
            using (var image = new MagickImage(stream))
            {
                var size = new MagickGeometry(_appSettings.ResizeImageWidht, 0);
                size.IgnoreAspectRatio = false;
                image.Resize(size);
                // Save the result
                image.Write(path);
            }
        }

        private ProductPhoto SaveFiles(IFormFile file, long annoucementId)
        {
            var fileName = string.Empty;
            string PathDB = string.Empty;
            string newFileName = string.Empty;
            ProductPhoto storedFile = new ProductPhoto()
            {
                DateTime = DateTime.Now,
                IsPublic = true,
                AnnouncementId = annoucementId
                //PathToFile = newFileName,
                //MimeType = $"application/{Path.GetExtension(fileName)}",
                //Name = Path.GetFileName(newFileName)
            };
            if (file.Length > 0)
            {
                //Getting FileName
                fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                //Assigning Unique Filename (Guid)
                //var myUniqueFileName = Convert.ToString(Guid.NewGuid());
                var myUniqueFileName = Convert.ToString(storedFile.LocalId);

                //Getting file Extension
                var FileExtension = Path.GetExtension(fileName);

                // concating  FileName + FileExtension
                newFileName = myUniqueFileName + FileExtension;
                storedFile.Name = fileName;
                //storedFile.MimeType = $"application/{FileExtension}";//!!!!!!!!!!!!!!!!!!!
                storedFile.MimeType = $"image/jpeg";
                // Combines two strings into a path.

                fileName = _productPhotoService.GetFileName(storedFile);
                string partialPath = Path.GetDirectoryName(fileName);

                if (!Directory.Exists(partialPath))
                    Directory.CreateDirectory(partialPath);

                //using (FileStream fs = System.IO.File.Create(fileName))
                //{
                //    file.CopyTo(fs);
                //    fs.Flush();
                //}                
            }
            return storedFile;
        }
    }
}
