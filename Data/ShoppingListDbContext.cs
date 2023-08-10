using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShoppingList.Models;

namespace ShoppingList.Data
{
    public class ShoppingListDbContext : DbContext
    {
        public ShoppingListDbContext(DbContextOptions<ShoppingListDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<UserShoppingList> UserShoppingLists { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<UserShoppingListItem> ShoppingListItem { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            modelBuilder.Entity<UserModel>()
                .HasMany(e => e.ShoppingLists)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .HasPrincipalKey(e => e.Id);

            modelBuilder.Entity<UserShoppingList>()
                .HasMany(e => e.Items)
                .WithOne(e => e.UserShoppingList)
                .HasForeignKey(e => e.UserShoppingListId)
                .HasPrincipalKey(e => e.Id);
        }
    }
}
