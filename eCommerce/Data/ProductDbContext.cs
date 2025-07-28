﻿using eCommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Data
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {

        }

        // Entities to be tracked by DbContext
        public DbSet<Product> Products { get; set; }
    }
}
