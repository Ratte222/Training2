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
        public long Weight { get; set; }
        public Category Category { get; set; }

        public string ClientId { get; set; }
        public Client Client { get; set; }

        public ICollection<ProductPhoto> ProductPhotos { get; set; }
    }

    public enum Category:ushort
    {
        childrensWorld = 0,
        realEstate,
        auto,
        sparePartsForTransport,
        work,
        animals,
        aHouseAndAGrden,
        electronics,
        businessServices,
        fashionAndStyle,
        hobbies, 
        recreationAndSports,
        giveItAway,
        exchange,
        onlineSecurity
    };
}
