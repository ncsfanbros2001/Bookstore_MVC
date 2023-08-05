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
    public class UserRepository : Repo<ApplicationUser>, IUserRepository
    {
        private readonly DatabaseContext _db;
        public UserRepository(DatabaseContext db) : base(db)
        {
            _db = db;
        }

    }
}
