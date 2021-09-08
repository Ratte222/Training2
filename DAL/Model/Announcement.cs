using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Model
{
    public class Announcement : BaseEntity<long>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public long Cost { get; set; }
        public long Views { get; set; }
        public ICollection<ProductPhoto> ProductPhotos { get; set; }
    }
}
