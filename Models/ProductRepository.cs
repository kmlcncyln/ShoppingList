using ShoppingList.Data;
using ShoppingList.Models;
using System.Collections.Generic;

namespace ShoppingList.Models
{
    public class ProductRepository
    {
        private ShoppingListDbContext dbContext;
        public ProductRepository(ShoppingListDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return dbContext.Products;
        }
    }
}
