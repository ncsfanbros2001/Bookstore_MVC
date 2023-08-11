using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Models.ViewModels
{
    public class RoleVM
    {
        public ApplicationUser ApplicationUser { get; set; }

        public IEnumerable<SelectListItem> Companies { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }

    }
}
