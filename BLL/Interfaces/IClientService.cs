using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IClientService 
    {
        IQueryable<Client> GetAll();
        IEnumerable<Client> GetAll_Enumerable();
        IQueryable<Client> GetAll_Queryable();
        Client Get(Func<Client, bool> func);
        void Create(Client item);
        void CreateRange(IEnumerable<Client> items);
        void Update(Client item);
        Task UpdateAsync(Client item);
        void Delete(Client item);
    }
}
