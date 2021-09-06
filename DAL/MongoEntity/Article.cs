using DAL.MongoEntity.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.MongoEntity
{
    public class Article : BaseEntityMongo
    {

        public string Name { get; set; }
        public string Body { get; set; }
        public string[] Tags { get; set; }

        public Article(string name, string body) =>
            (Name, Body) = (name, body);
    }
}
