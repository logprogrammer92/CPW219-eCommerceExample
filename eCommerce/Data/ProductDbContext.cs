using eCommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Data
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {

        }

        /// <summary>
        /// Configures the model for the ProductDbContext.
        /// </summary>
        /// <param name="modelBuilder">Used to create entities</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Ensure the Member Username is Unique
            modelBuilder.Entity<Member>()
                .HasIndex(m => m.Username) // Alphanumeric characters only
                .IsUnique(); // This ensures that each username is unique across all members

            // Ensure the Member Email is Unique
            modelBuilder.Entity<Member>()
                    .HasIndex(m => m.Email) // The email for the Member
                    .IsUnique(); // This ensures that each email is unique across all members
        }

        // Entities to be tracked by DbContext
        public DbSet<Product> Products { get; set; }

        public DbSet<Member> Members { get; set; }
    }
}
