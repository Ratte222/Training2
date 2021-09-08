using BLL.DTO.ProductPhoto;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.DTO.Announcement
{
    public class AnnouncementDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long Cost { get; set; }
        public long Views { get; set; }
        public ICollection<ProductPhotoDTO> ProductPhotos { get; set; }
    }
}
