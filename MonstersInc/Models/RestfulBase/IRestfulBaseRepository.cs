using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonstersAPI.Models
{
    public interface IRestfulBaseRepository<T>
    {
        IQueryable<T> Get();
        void Post(T entity);
        void Put(T entity);
        void Delete(T entity);
        void Save();
    }
}
