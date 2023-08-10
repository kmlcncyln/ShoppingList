using System.ComponentModel.DataAnnotations;

namespace ShoppingList.Models
{
	public class UserRegisterForm
	{

		[Required]
		public string Name { get; set; }

		[Required]
		public string Surname { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }
	}
}
