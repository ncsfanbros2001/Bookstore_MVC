using Bookstore.Data.Repositories;
using Bookstore.Models;
using Bookstore.Models.ViewModels;
using Bookstore.Utility;
using BookstoreWeb.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;

namespace BookstoreWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetail.Role_Admin)]
    public class UserController : Controller
    {
        private readonly DatabaseContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        public UserController(DatabaseContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RoleManagement(string userId)
        {
            string userRoleId = _db.UserRoles.FirstOrDefault(x => x.UserId == userId).RoleId;

            RoleVM roleVM = new RoleVM()
            {
                ApplicationUser = _db.ApplicationUsers.Include(x => x.Company).FirstOrDefault(u => u.Id == userId),
                Roles = _db.Roles.Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Name
                }),
                Companies = _db.Companies.Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };

            roleVM.ApplicationUser.Role = _db.Roles.FirstOrDefault(u => u.Id == userRoleId).Name;

            return View(roleVM);
        }

        [HttpPost]
        public IActionResult RoleManagement(RoleVM roleVM)
        {
            string userRoleId = _db.UserRoles.FirstOrDefault(x => x.UserId == roleVM.ApplicationUser.Id).RoleId;
            string oldRole = _db.Roles.FirstOrDefault(u => u.Id == userRoleId).Name;

            if (!(roleVM.ApplicationUser.Role == oldRole))
            {
                ApplicationUser applicationUser = _db.ApplicationUsers
                    .FirstOrDefault(u => u.Id == roleVM.ApplicationUser.Id);

                if (roleVM.ApplicationUser.Role != StaticDetail.Role_Company) { 
                    applicationUser.CompanyId = roleVM.ApplicationUser.CompanyId;
                }
                if (oldRole == StaticDetail.Role_Company)
                {
                    applicationUser.CompanyId = null;
                }
                _db.SaveChanges();

                _userManager.RemoveFromRoleAsync(applicationUser, oldRole).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(applicationUser, roleVM.ApplicationUser.Role).GetAwaiter().GetResult();
            }

            return RedirectToAction("Index");
        }

        #region API_CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<ApplicationUser> userList = _db.ApplicationUsers.Include(u => u.Company).ToList();

            var userRoles = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();

            foreach (var user in userList)
            {
                var roleId = userRoles.FirstOrDefault(u => u.UserId == user.Id).RoleId;

                user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;

                if (user.Company == null)
                {
                    user.Company = new() { Name = "" };
                }
            }

            return Json(new { data = userList });
        }

        [HttpPost]
        public IActionResult LockUnlock([FromBody]string id)
        {
            var userFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if (userFromDb == null)
            {
                return Json(new { success = false, message = "Error while locking/unlocking" });
            }

            if (userFromDb.LockoutEnd != null && userFromDb.LockoutEnd > DateTime.Now) // User has been locked
            {
                userFromDb.LockoutEnd = DateTime.Now;
            }
            else
            {
                userFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
            }

            _db.SaveChanges();
            return Json(new { success = true, message = "Operation successful" });
        }

        #endregion
    }
}
