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

namespace Traiting2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductPhotoController : ControllerBase
    {
        private readonly IProductPhotoService _productPhotoService;
        private readonly AppSettings _appSettings;

        public ProductPhotoController(IProductPhotoService productPhotoService, AppSettings appSettings)
        {
            (_productPhotoService, _appSettings) = (productPhotoService, appSettings);
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


        [HttpPost("AddFiles")]
        public async Task<IActionResult> AddFiles(IFormFileCollection files, long annoucementId)
        {
            List<ProductPhoto> storedFiles = new List<ProductPhoto>();
            foreach (var file in files)
            {
                ProductPhoto storedFile = SaveFiles(file, annoucementId);
                ResizeImage(_productPhotoService.GetFileName(storedFile));
                storedFiles.Add(storedFile);
            }
            await _productPhotoService.SaveProductPhoto(storedFiles);
            return StatusCode(200, "Files added successfully");
        }

        private void ResizeImage(string path)
        {
            using (var image = new MagickImage(path))
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
                storedFile.MimeType = $"application/{FileExtension}";
                // Combines two strings into a path.

                fileName = _productPhotoService.GetFileName(storedFile);
                string partialPath = Path.GetDirectoryName(fileName);

                if (!Directory.Exists(partialPath))
                    Directory.CreateDirectory(partialPath);

                using (FileStream fs = System.IO.File.Create(fileName))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
            }
            return storedFile;
        }
    }
}
