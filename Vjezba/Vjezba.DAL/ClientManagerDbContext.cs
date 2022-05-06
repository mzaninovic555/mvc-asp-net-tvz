using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Vjezba.Model;

namespace Vjezba.DAL
{
    public class ClientManagerDbContext : DbContext
    {
        public ClientManagerDbContext(DbContextOptions<ClientManagerDbContext> options)
            : base(options)
        {

        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<City> Cities { get; set; }

        public DbSet<Meeting> Meetings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<City>().HasData(new City { ID = 1, Name = "Zagreb" });
            modelBuilder.Entity<City>().HasData(new City { ID = 2, Name = "Velika Gorica" });
            modelBuilder.Entity<City>().HasData(new City { ID = 3, Name = "Vrbovsko" });
        }

    }
}
