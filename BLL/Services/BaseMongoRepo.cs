using BLL.Helpers;
using BLL.Interfaces;
using DAL.MongoEntity.Base;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class BaseMongoRepo<T> : IBaseMongoRepo<T> where T : BaseEntityMongo
    {
        private readonly IMongoClient _mongoClient;
        private readonly IClientSessionHandle _clientSessionHandle;
        private readonly string _collection;
        private readonly AppSettings _appSettings;

        public BaseMongoRepo(IMongoClient mongoClient, IClientSessionHandle clientSessionHandle, string collection,
            AppSettings appSettings)
        {
            (_mongoClient, _clientSessionHandle, _collection, _appSettings) = 
                (mongoClient, clientSessionHandle, collection, appSettings);

            if (!_mongoClient.GetDatabase(_appSettings.MongoDB_Database).ListCollectionNames()
                    .ToList().Contains(collection))
                _mongoClient.GetDatabase(_appSettings.MongoDB_Database).CreateCollection(collection);
        }

        protected virtual IMongoCollection<T> Collection =>
        _mongoClient.GetDatabase(_appSettings.MongoDB_Database).GetCollection<T>(_collection);

        public async Task InsertAsync(T obj) =>
        await Collection.InsertOneAsync(_clientSessionHandle, obj);

        public async Task UpdateAsync(T obj)
        {
            Expression<Func<T, string>> func = f => f.Id;
            var value = (string)obj.GetType().GetProperty(func.Body.ToString().Split(".")[1]).GetValue(obj, null);
            var filter = Builders<T>.Filter.Eq(func, value);

            if (obj != null)
                await Collection.ReplaceOneAsync(_clientSessionHandle, filter, obj);
        }

        public async Task DeleteAsync(string id) =>
            await Collection.DeleteOneAsync(_clientSessionHandle, f => f.Id == id);
    }
}
