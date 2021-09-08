using BLL.DTO.Announcement;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BLL.DTO.ProductPhoto
{

    public class ProductPhotoDTO
    {
        public long Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public bool IsPublic { get; set; }

        public long AnnouncementId { get; set; }
        [JsonIgnore]
        public AnnouncementDTO Announcement { get; set; }
    }
}
