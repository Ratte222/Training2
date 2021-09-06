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
    public class ArticleService: IMongoRepoArticle
    {
        private readonly IMongoCollection<Article> _articles;

        public ArticleService(MongoDBSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _articles = database.GetCollection<Article>(settings.ArticleDatabaseName);
        }

        public List<Article> Get() =>
            _articles.Find(article => true).ToList();

        public Article Get(string id) =>
            _articles.Find<Article>(article => article.Id == id).FirstOrDefault();

        public Article Create(Article article)
        {
            _articles.InsertOne(article);
            return article;
        }

        public void Update(Article articleIn) =>
            _articles.ReplaceOne(article => article.Id == articleIn.Id, articleIn);

        public void Remove(Article articleIn) =>
            _articles.DeleteOne(article => article.Id == articleIn.Id);

        public void Remove(string id) =>
            _articles.DeleteOne(article => article.Id == id);
    }
}

