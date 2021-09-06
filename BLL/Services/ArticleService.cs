using BLL.Helpers;
using BLL.Interfaces;
using DAL.MongoEntity;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class ArticleService: BaseMongoRepo<Article>, IMongoRepoArticle
    {
        public ArticleService(IMongoClient mongoClient,
            IClientSessionHandle clientSessionHandle, AppSettings appSettings) 
            : base(mongoClient, clientSessionHandle, "article", appSettings)
        {

        }

        public async Task<Article> GetArticleById(string articleId)
        {
            var filter = Builders<Article>.Filter.Eq(s => s.Id, articleId);
            return await Collection.Find(filter).FirstOrDefaultAsync();
        }
    }
}
