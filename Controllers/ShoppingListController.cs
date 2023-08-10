using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoppingList.Data;
using ShoppingList.Models;
using System.Collections.Generic;

[Authorize]
public class ShoppingListController : Controller
{
	private static List<UserShoppingList> shoppingLists = new List<UserShoppingList>();

	private readonly ShoppingListDbContext _dbContext;

	public ShoppingListController(ShoppingListDbContext dbContext)
	{
		_dbContext = dbContext;
	}
    [Authorize]
    public IActionResult Index()
	{
		int userId = HttpContext.Session.GetInt32("Id") ?? 0;
		var shoppingLists = _dbContext.UserShoppingLists.Where(x => x.UserId == userId).ToList();

        return View(shoppingLists);
	}
    [Authorize]
    [HttpPost]
    public IActionResult CreateList(string listName)
    {
        var userId = HttpContext.Session.GetInt32("Id") ?? 0;

        var newList = new UserShoppingList
        {
            Name = listName,
            UserId = userId
        };

        _dbContext.UserShoppingLists.Add(newList);
        _dbContext.SaveChanges();

        return RedirectToAction("Index");
    }


    [Authorize]
    [HttpPost]
    public IActionResult AddItemUserShoppingList(int productId, string description, int Id)
    {
        var item = new UserShoppingListItem()
        {
            ProductId = productId,
            Description = description,
            UserShoppingListId = Id
        }; 

        _dbContext.ShoppingListItem.Add(item);
        _dbContext.SaveChanges();


        return RedirectToAction("ShowList", new { id = Id });
    }
    [Authorize]
    [HttpPost]
    public IActionResult RemoveItem(int listId, int itemId)
    {
        var shoppingList = _dbContext.UserShoppingLists
            .Include(l => l.Items)
            .FirstOrDefault(sl => sl.Id == listId);

        if (shoppingList == null)
        {
            return NotFound(); 
        }

        var item = shoppingList.Items.FirstOrDefault(i => i.Id == itemId);

        if (item != null)
        {
            _dbContext.ShoppingListItem.Remove(item);
            _dbContext.SaveChanges();
        }

        return RedirectToAction("ShowList", new { id = listId }); 
    }
    [Authorize]
    [HttpPost]
    public IActionResult EditDescription(int listId, int itemId, string newDescription)
    {
        var shoppingList = _dbContext.UserShoppingLists
                                      .Include(l => l.Items)
                                      .FirstOrDefault(sl => sl.Id == listId);

        if (shoppingList == null)
        {
            return NotFound();
        }

        var item = shoppingList.Items.FirstOrDefault(i => i.Id == itemId);

        if (item == null)
        {
            return NotFound();
        }

        item.Description = newDescription;
        _dbContext.SaveChanges();

        return RedirectToAction("ShowList", new { id = listId });
    }





    [Authorize]
    [HttpPost]
    public IActionResult DeleteList(int listId)
    {
        var list = _dbContext.UserShoppingLists.FirstOrDefault(x => x.Id == listId);
        if (list != null)
        {
            _dbContext.UserShoppingLists.Remove(list);
            _dbContext.SaveChanges();
        }

        return RedirectToAction("Index");
    }
    [Authorize]
    public IActionResult GoShopping(int id)
    {
        var shoppingList = _dbContext.UserShoppingLists.FirstOrDefault(l => l.Id == id);
        if (shoppingList != null)
        {
            shoppingList.IsShopping = true; // Alışveriş başladığında IsShopping özelliğini true olarak işaretle
            _dbContext.SaveChanges();
        }

        return RedirectToAction("ShowList", new { id });
    }


    [Authorize]
    [HttpPost]
    public IActionResult CompleteShopping(int listId, List<int> selectedItems)
    {
        var shoppingList = _dbContext.UserShoppingLists
                                      .Include(l => l.Items)
                                      .FirstOrDefault(sl => sl.Id == listId);

        if (shoppingList == null)
        {
            return NotFound();
        }

        foreach (var itemId in selectedItems)
        {
            var item = shoppingList.Items.FirstOrDefault(i => i.Id == itemId);
            if (item != null)
            {
                shoppingList.Items.Remove(item);
            }
        }

        _dbContext.SaveChanges();

        return RedirectToAction("Index");
    }




    [Authorize]
    [HttpPost]
    public IActionResult MarkAsPurchased(int itemId)
    {
        var item = _dbContext.ShoppingListItem.Find(itemId);

        if (item == null)
        {
            return NotFound();
        }

        item.IsPurchased = true;
        _dbContext.SaveChanges();

        return RedirectToAction("ShowList", new { id = item.UserShoppingListId });
    }
    [Authorize]
    [HttpPost]
    public IActionResult RemovePurchasedItems(int listId)
    {
        var shoppingList = _dbContext.UserShoppingLists.Include(l => l.Items).FirstOrDefault(sl => sl.Id == listId);

        if (shoppingList == null)
        {
            return NotFound();
        }

        var purchasedItems = shoppingList.Items.Where(item => item.IsPurchased).ToList();

        foreach (var item in purchasedItems)
        {
            _dbContext.ShoppingListItem.Remove(item);
        }

        _dbContext.SaveChanges();

        return RedirectToAction("ShowList", new { id = listId });
    }

    [Authorize]
    public IActionResult ShowList(int id)
    {
        var shoppingList = _dbContext.UserShoppingLists.Include(l => l.Items).FirstOrDefault(sl => sl.Id == id);

        if (shoppingList == null)
        {
            return NotFound();
        }

        var products = _dbContext.Products.ToList();

        ViewBag.Products = products; 

        return View(shoppingList); 
    }
    [Authorize]
    public IActionResult ShowListShopping(int id)
    {
        var shoppingList = _dbContext.UserShoppingLists.Include(l => l.Items).ThenInclude(l => l.Product).FirstOrDefault(sl => sl.Id == id);

        if (shoppingList == null)
        {
            return NotFound();
        }

        return View("ShowListShopping", shoppingList);
    }





}
