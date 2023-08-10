using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace ShoppingList.Models
{
	public class UserModel: IIdentity
	{
		
		[Key]
		public int Id { get; set; }
		public bool IsAdmin { get; set; }
		public string Name { get; set; }
		public string Surname { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public ICollection<UserShoppingList> ShoppingLists { get; set; }

        public string? AuthenticationType => null;

        public bool IsAuthenticated => true;
    }
}
