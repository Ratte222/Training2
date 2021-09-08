using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IBaseService<T> where T : BaseEntity<long>
    {
        IEnumerable<T> GetAll_Enumerable();
        IQueryable<T> GetAll_Queryable();
        IQueryable<T> GetAll();
        T Get(Func<T, bool> func);
        Task<T> GetAsync(Expression<Func<T, bool>> func);
        void Create(T item);
        Task CreateRangeAsync(IEnumerable<T> items);
        void CreateRange(IEnumerable<T> items);
        void Update(T item);
        void Delete(T item);
    }
}
