using BLL.Interfaces;
using DAL.EF;
using DAL.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class ClientService: IClientService
    {
        protected AppDBContext _context;
        public ClientService(AppDBContext context)
        {
            _context = context;
        }

        public void Create(Client item)
        {
            _context.Add(item);
            _context.SaveChanges();
        }

        public void CreateRange(IEnumerable<Client> items)
        {
            _context.AddRange(items);
            _context.SaveChanges();
        }

        public void Delete(Client item)
        {
            _context.Remove(item);
            _context.SaveChanges();
        }

        public Client Get(Func<Client,bool> func /*Predicate<Client> predicate*/)
        {
            //Exception: The key value at position 0 of the call to 'DbSet<Client>.Find' was of type 'Predicate<Client>', which does not match the property type of 'string'
            //return _context.Clients.Find(predicate);

            var client = _context.Clients.AsNoTracking().FirstOrDefault(func);
            //_context.Entry(client).State = EntityState.Detached;
            return client;
        }

        public IQueryable<Client> GetAll()
        {
            return _context.Clients.AsNoTracking();
        }

        public IEnumerable<Client> GetAll_Enumerable()
        {
            return GetAll_Queryable().AsEnumerable();
        }

        public IQueryable<Client> GetAll_Queryable()
        {
            return _context.Clients.Where(i => i.RemoveData == null).AsNoTracking();
        }

        public void Update(Client item)
        {
            _context.Update(item);
            _context.SaveChanges();
        }

        public async Task UpdateAsync(Client item)
        {
            _context.Update(item);
            await _context.SaveChangesAsync();
        }
    }
}
