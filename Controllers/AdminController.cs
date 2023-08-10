using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingList.Data;
using ShoppingList.Models;
using System.Linq;

namespace ShoppingList.Controllers
{
    
    public class AdminController : Controller
    {
        private readonly ShoppingListDbContext _dbContext;

        public AdminController(ShoppingListDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            ViewBag.Categories = _dbContext.Categories.ToList();
            ViewBag.Products = _dbContext.Products.Include(p => p.Category).ToList();

            return View();
        }

        [HttpPost]
        public IActionResult AddProduct(string productName, string category, decimal price, string imgUrl)
        {
            var product = new Product
            {
                Name = productName,
                CategoryId = _dbContext.Categories.FirstOrDefault(c => c.Name == category).Id,
                Price = price,
                ImageUrl = imgUrl
            };

            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AddCategory(string categoryName)
        {
            var category = new Category
            {
                Name = categoryName
            };

            if (ModelState.IsValid)
            {
                _dbContext.Categories.Add(category);
                _dbContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult EditProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                var existingProduct = _dbContext.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == product.Id);
                if (existingProduct != null)
                {
                    existingProduct.Name = product.Name;
                    existingProduct.Price = product.Price;
                    existingProduct.CategoryId = product.CategoryId;
                    existingProduct.ImageUrl = product.ImageUrl;

                    _dbContext.SaveChanges(); // Bu satırı unutmayın

                    return RedirectToAction("Index");
                }
            }

            // Eğer güncelleme başarısız olduysa, tekrar düzenleme modalını açın
            return RedirectToAction("Index"); // Veya hata mesajı gösterebilirsiniz
        }


        [HttpPost]
        public IActionResult EditCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Categories.Update(category);
                _dbContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteProduct(int id)
        {
            var product = _dbContext.Products.Find(id);
            if (product != null)
            {
                _dbContext.Products.Remove(product);
                _dbContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteCategory(int id)
        {
            var category = _dbContext.Categories.Find(id);
            if (category != null)
            {
                _dbContext.Categories.Remove(category);
                _dbContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
