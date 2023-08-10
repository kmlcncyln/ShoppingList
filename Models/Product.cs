using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingList.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; } 
        public virtual Category Category { get; set; }
        public string? ImageUrl { get; set; }
        public decimal Price { get; set; }

    }
}
