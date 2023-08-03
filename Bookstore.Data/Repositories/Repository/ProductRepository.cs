using Bookstore.Data.Repositories.IRepository;
using Bookstore.Models;
using BookstoreWeb.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Data.Repositories.Repository
{
    public class ProductRepository : Repo<Product>, IProductRepository
    {
        private readonly DatabaseContext _db;
        public ProductRepository(DatabaseContext db) : base(db)
        {
            _db = db;
        }


        public void Update(Product product)
        {
            _db.Products.Update(product);
        }
    }
}
