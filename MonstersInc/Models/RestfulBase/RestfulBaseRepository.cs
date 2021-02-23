using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonstersAPI.Models
{
    public class RestfulBaseRepository<T> : IRestfulBaseRepository<T> where T: class
    {
        protected MonstersIncDbContext _monstersIncDbContext;
        public RestfulBaseRepository(MonstersIncDbContext monstersIncDBContext)
        {
            _monstersIncDbContext = monstersIncDBContext;
        }
        public IQueryable<T> Get() => _monstersIncDbContext.Set<T>();
        public void Post(T entity)
        {
            _monstersIncDbContext.Set<T>().Add(entity);
            _monstersIncDbContext.SaveChanges();
        }
        public void Put(T entity)
        {
            _monstersIncDbContext.Set<T>().Attach(entity);
            _monstersIncDbContext.Entry(entity).State = EntityState.Modified;
            _monstersIncDbContext.SaveChanges();
        }
        public void Delete(T entity)
        {
            _monstersIncDbContext.Set<T>().Remove(entity);
            _monstersIncDbContext.SaveChanges();
        }
        public void Save()
        {
            _monstersIncDbContext.SaveChanges();
        }
    }
}
