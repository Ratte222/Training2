using DAL.MongoEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IMongoRepoArticle:IBaseMongoRepo<Article>
    {
        Task<Article> GetArticleById(string articleId); 
    }
}
