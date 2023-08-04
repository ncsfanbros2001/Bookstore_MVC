﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Data.Repositories
{
    public interface IRepository<T> where T : class // T is a model from Bookstore.Models
    {
        IEnumerable<T> GetAll(string? includeProperties = null);

        T GetOne(Expression<Func<T, bool>> filter, string? includeProperties = null);

        void Add(T entity);

        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entity);
    }
}
