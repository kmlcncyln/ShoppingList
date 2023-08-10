namespace ShoppingList.Models
{
    public class UserShoppingListItem
	{
        public int Id { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int UserShoppingListId { get; set; }

        public UserShoppingList UserShoppingList { get; set; }
        public string Description { get; set; }
        public bool IsPurchased { get; internal set; }
    }

}
