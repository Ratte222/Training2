using DAL.MongoEntity.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IBaseMongoRepo<T> where T : BaseEntityMongo
    {
        Task InsertAsync(T obj);

        Task UpdateAsync(T obj);

        Task DeleteAsync(string id);
    }
}
