using Microsoft.AspNetCore.Mvc;
using ShoppingList.Models;

namespace ShoppingList.Controllers
{
	public class HomeController : Controller
	{
		private ProductRepository productRepository;
		public HomeController(ProductRepository productRepository)
		{
			this.productRepository = productRepository;
		}
		public IActionResult Index()
		{
			return RedirectToAction("Login","User");
		}
	}
}
