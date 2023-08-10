using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingList.Data;
using ShoppingList.Models;
using System.Drawing.Text;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ShoppingList.Controllers
{
	public class UserController : Controller
	{

        private readonly ShoppingListDbContext dbContext;
        private readonly IConfiguration configuration;

        public UserController(ShoppingListDbContext dbContext, IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this.configuration = configuration;
        }

		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Register(UserRegisterForm registeryForm)
		{
			if (ModelState.IsValid)
			{
				if (!IsPasswordValid(registeryForm.Password))
				{
					ModelState.AddModelError("Password", "Şifre en az 8 karakter olmalı ve büyük harf, küçük harf ve rakam içermelidir.");
					return View();
				}

				var user = new UserModel()
				{
					Name = registeryForm.Name,
					Surname = registeryForm.Surname,
					Email = registeryForm.Email,
					Password = registeryForm.Password
				};

				dbContext.Users.Add(user);
				dbContext.SaveChanges();

				return RedirectToAction("Login");
			}

			return View();
		}
		private bool IsPasswordValid(string password)
		{
			if (password.Length < 8)
			{
				return false;
			}

			bool hasUpperCase = false;
			bool hasLowerCase = false;
			bool hasDigit = false;

			foreach (char c in password)
			{
				if (char.IsUpper(c))
				{
					hasUpperCase = true;
				}
				else if (char.IsLower(c))
				{
					hasLowerCase = true;
				}
				else if (char.IsDigit(c))
				{
					hasDigit = true;
				}
			}

			return hasUpperCase && hasLowerCase && hasDigit;
		}

		public IActionResult Login()
		{
			return View();
		}

        [HttpPost]
public async Task<IActionResult> Login(UserRegisterForm model)
{
    var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);

    if (user != null)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        if (user.IsAdmin) // Varsayılan olarak UserModel içinde bir IsAdmin özelliği olduğunu varsayalım
        {
            claims.Add(new Claim(ClaimTypes.Role, "Admin"));
        }

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

		HttpContext.Session.SetInt32("Id", user.Id);	
        if (user.IsAdmin)
        {
            return RedirectToAction("Index", "Admin");
        }
        else
        {
            return RedirectToAction("Index", "ShoppingList");
        }
    }

    ModelState.AddModelError("", "Hatalı kullanıcı adı veya şifre");
    return View();
}



        private string GenerateJwtToken(UserModel user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                },
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
