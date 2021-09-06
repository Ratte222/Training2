using DAL.MongoEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IMongoRepoArticle
    {
        List<Article> Get();
        Article Get(string id);
        Article Create(Article article);
        void Update(Article articleIn);
        void Remove(Article articleIn);
        void Remove(string id);
    }
}
