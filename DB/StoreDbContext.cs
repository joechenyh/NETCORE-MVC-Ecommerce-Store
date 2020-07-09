using Castle.Core.Configuration;
using Microsoft.EntityFrameworkCore;
using NETCORE_CA_8A.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCORE_CA_8A.DB
{
    public class StoreDbContext:DbContext
    {
        protected IConfiguration configuration;
        public StoreDbContext()
        {

        }
        public StoreDbContext(DbContextOptions<StoreDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Cart { get; set; }

        public DbSet<CartItem> CartItem { get; set; }

        public DbSet<Purchase> Purchase { get; set; }
        public DbSet<Purchased> Purchased { get; set; }

        public DbSet<Recommendation> Recommendation { get; set; }

        public DbSet<Review> Review { get; set; }

    }
}

