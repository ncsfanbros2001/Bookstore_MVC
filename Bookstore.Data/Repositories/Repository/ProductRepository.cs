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
            var productFromDB = _db.Products.FirstOrDefault(x => x.Id == product.Id);

            if (productFromDB != null)
            {
                productFromDB.Title = product.Title;
                productFromDB.Description = product.Description;
                productFromDB.CategoryId = product.CategoryId;
                productFromDB.ISBN = product.ISBN;
                productFromDB.Author = product.Author;

                productFromDB.ListPrice = product.ListPrice;
                productFromDB.Price = product.Price;
                productFromDB.Price50 = product.Price50;
                productFromDB.Price100 = product.Price100;

                if (product.ImageURL != null)
                {
                    productFromDB.ImageURL = product.ImageURL;
                }
            }
            //_db.Products.Update(product);
        }
    }
}
