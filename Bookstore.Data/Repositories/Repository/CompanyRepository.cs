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
    public class CompanyRepository : Repo<Company>, ICompanyRepository
    {
        private readonly DatabaseContext _db;
        public CompanyRepository(DatabaseContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Company company)
        {
            _db.Companies.Update(company);
        }
    }
}
