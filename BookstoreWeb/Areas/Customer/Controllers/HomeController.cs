using Bookstore.Data.Repositories;
using Bookstore.Models;
using BookstoreWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BookstoreWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category");
            return View(productList);
        }

        public IActionResult Details(int id)
        {
            ShoppingCart cart = new()
            {
                Product = _unitOfWork.Product.GetOne(u => u.Id == id, includeProperties: "Category"),
                Count = 1,
                ProductId = id
            };
            
            return View(cart);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            shoppingCart.ApplicationUserId = userId;

            ShoppingCart cartItemFromDb = _unitOfWork.ShoppingCart.GetOne(u => u.ApplicationUserId == userId
                && u.ProductId == shoppingCart.ProductId);

            if (cartItemFromDb != null) // Shopping Cart already exist
            {
                cartItemFromDb.Count += shoppingCart.Count;
                _unitOfWork.ShoppingCart.Update(cartItemFromDb);    
            }
            else // Add Item To Cart
            {
                shoppingCart.Id = 0;
                _unitOfWork.ShoppingCart.Add(shoppingCart);
            }

            TempData["success"] = "Added To Cart";

            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}