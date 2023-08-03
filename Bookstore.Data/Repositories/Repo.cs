using BookstoreWeb.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Data.Repositories
{
    public class Repo<T> : IRepository<T> where T : class
    {
        private readonly DatabaseContext _db;
        internal DbSet<T> dbSet;

        public Repo(DatabaseContext db)
        {
            _db = db;
            dbSet = _db.Set<T>(); // Map T to the indicated dbSet
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = dbSet;

            return query.ToList();
        }

        public T GetOne(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbSet.Where(filter);

            return query.FirstOrDefault();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }
    }
}
