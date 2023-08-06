using Bookstore.Data.Repositories.IRepository;
using Bookstore.Models;
using BookstoreWeb.Data;
using BookstoreWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Data.Repositories.Repository
{
    public class OrderDetailsRepository : Repo<OrderDetails>, IOrderDetailsRepository
    {
        private readonly DatabaseContext _db;
        public OrderDetailsRepository(DatabaseContext db) : base(db)
        {
            _db = db;
        }


        public void Update(OrderDetails orderDetails)
        {
            _db.OrderDetails.Update(orderDetails);
        }
    }
}
