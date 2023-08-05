using Bookstore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Data.Repositories.IRepository
{
    public interface ICompanyRepository: IRepository<Company>
    {
        void Update(Company company);
    }
}
