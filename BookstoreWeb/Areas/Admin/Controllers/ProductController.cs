﻿using Bookstore.Data.Repositories;
using Bookstore.Models;
using Bookstore.Models.ViewModels;
using Bookstore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Data;

namespace BookstoreWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetail.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Product> productList = _unitOfWork.Product
                .GetAll(includeProperties: "Category").ToList();

            return View(productList);
        }

        public IActionResult Upsert(int? id)
        {
            // Get All Category using Projection
            IEnumerable<SelectListItem> categoryList = _unitOfWork.Category
                .GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });

            ProductVM productVM = new ProductVM()
            {
                CategoryList = categoryList,
                Product = new Product()
            };

            if (id == null || id == 0) // Create
            {
                return View(productVM);
            }

            else // Update
            {
                productVM.Product = _unitOfWork.Product.GetOne(u => u.Id == id);
                return View(productVM);
            }
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images/product");

                    if (!string.IsNullOrEmpty(productVM.Product.ImageURL)) // Update the current image
                    {
                        // Delete the old image
                        var oldImagePath = Path.Combine(wwwRootPath, productVM.Product
                            .ImageURL.Trim('/'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productVM.Product.ImageURL = @"/images/product/" + fileName;
                }

                if (productVM.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVM.Product);
                    TempData["success"] = "Product created successfully";
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                    TempData["success"] = "Product updated successfully";
                }
                
                _unitOfWork.Save();

                return RedirectToAction("Index");
            }
            else
            { 
                productVM.CategoryList = _unitOfWork.Category
                .GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });

                return View(productVM);
            }
        }

        #region API_CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> productList = _unitOfWork.Product
                .GetAll(includeProperties: "Category").ToList();

            return Json(new { data = productList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToDelete = _unitOfWork.Product.GetOne(u => u.Id == id);

            if (productToDelete == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath,
                productToDelete.ImageURL.Trim('/'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Product.Remove(productToDelete);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete successful" });
        }

        #endregion
    }
}
