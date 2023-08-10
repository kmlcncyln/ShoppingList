using Microsoft.AspNetCore.Mvc;
using ShoppingList.Data;
using ShoppingList.Models;
using System.Linq;

namespace ShoppingList.Controllers
{
    public class ProductsController : Controller
    {
        private ProductRepository productRepository;
        private ShoppingListDbContext _dbContext;

        public ProductsController(ProductRepository productRepository, ShoppingListDbContext dbContext)
        {
            this.productRepository = productRepository;
            _dbContext = dbContext;
        }

        public IActionResult Shop(int? categoryId)
        {
            var products = productRepository.GetAllProducts();

            if (categoryId.HasValue)
            {
            products = products.Where(p => p.CategoryId == categoryId.Value).ToList();
            }

            var categories = _dbContext.Categories.ToList();

            ViewBag.Categories = categories;

            return View(products);
        }

    }
}
