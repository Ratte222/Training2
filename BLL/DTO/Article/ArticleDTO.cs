using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.DTO.Article
{
    public class ArticleDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Body { get; set; }
        public string[] Tags { get; set; }
    }
}
