using DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Extensions
{
    public static class CategoryExtension
    {
        public static string FastToString(this Category category)
        {
            switch(category)
            {
                case Category.childrensWorld:
                    return nameof(Category.childrensWorld);
                case Category.realEstate:
                    return nameof(Category.realEstate);
                case Category.auto:
                    return nameof(Category.auto);
                case Category.sparePartsForTransport:
                    return nameof(Category.sparePartsForTransport);
                case Category.work:
                    return nameof(Category.work);
                case Category.animals:
                    return nameof(Category.animals);
                case Category.aHouseAndAGrden:
                    return nameof(Category.aHouseAndAGrden);
                case Category.electronics:
                    return nameof(Category.electronics);
                case Category.businessServices:
                    return nameof(Category.businessServices);
                case Category.fashionAndStyle:
                    return nameof(Category.fashionAndStyle);
                case Category.hobbies:
                    return nameof(Category.hobbies);
                case Category.recreationAndSports:
                    return nameof(Category.recreationAndSports);
                case Category.giveItAway:
                    return nameof(Category.giveItAway);
                case Category.exchange:
                    return nameof(Category.exchange);
                case Category.onlineSecurity:
                    return nameof(Category.onlineSecurity);
                default: return "";
            }
        }
    }
}
