using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingList.Models
{
    public class UserShoppingList
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }

		[ForeignKey("User")]
		public int UserId { get; set; }
		public virtual UserModel User { get; set; }

		public ICollection<UserShoppingListItem> Items { get; set; }
        public bool IsShopping { get; internal set; }
	}

}
