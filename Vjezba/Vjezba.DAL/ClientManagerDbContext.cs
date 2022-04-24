using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vjezba.Model;

namespace Vjezba.DAL
{
    public  class ClientManagerDbContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Meeting> Meetings { get; set; }

        public ClientManagerDbContext(DbContextOptions<ClientManagerDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<City>().HasData(new City { Id = 1, Name = "Zagreb" });
            modelBuilder.Entity<City>().HasData(new City { Id = 2, Name = "New York" });
            modelBuilder.Entity<City>().HasData(new City { Id = 3, Name = "London" });

            modelBuilder.Entity<Client>().HasData(
                new Client { 
                    Id = 1, 
                    FirstName = "Ivan", 
                    LastName = "Cesar", 
                    Email = "ivan.cesar@mail.hr", 
                    Address = "AdresaUZagrebu 54",
                    Gender = 'M', 
                    PhoneNumber = "0916789941", 
                    CityId = 1 
                } );
        }
    }
}
