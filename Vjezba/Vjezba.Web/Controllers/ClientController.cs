using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vjezba.DAL;
using Vjezba.Model;
using Microsoft.EntityFrameworkCore;

namespace Vjezba.Web.Controllers
{
    public class ClientController : Controller
    {
        private ClientManagerDbContext clientManagerDbContext;

        public ClientController(ClientManagerDbContext clientManagerDbContext)
        {
            this.clientManagerDbContext = clientManagerDbContext;
        }

        public IActionResult Index(string query = null)
        {
            var clientQuery = this.clientManagerDbContext.Clients.Include(c => c.City).ToList();

            if (!string.IsNullOrWhiteSpace(query))
                clientQuery = clientQuery.Where(p => p.FullName.ToLower().Contains(query)).ToList();

            ViewBag.ActiveTab = 1;

            return View(clientQuery.ToList());
        }

        [HttpPost]
        public ActionResult Index(string queryName, string queryAddress)
        {
            var clientQuery = this.clientManagerDbContext.Clients.Include(c => c.City).ToList();

            //Primjer iterativnog građenja upita - dodaje se "where clause" samo u slučaju da je parametar doista proslijeđen.
            //To rezultira optimalnijim stablom izraza koje se kvalitetnije potencijalno prevodi u SQL
            if (!string.IsNullOrWhiteSpace(queryName))
                clientQuery = clientQuery.Where(p => p.FullName.ToLower().Contains(queryName)).ToList();

            if (!string.IsNullOrWhiteSpace(queryAddress))
                clientQuery = clientQuery.Where(p => p.Address.ToLower().Contains(queryAddress)).ToList();

            ViewBag.ActiveTab = 2;

            var model = clientQuery.ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult AdvancedSearch(ClientFilterModel filter)
        {
            var clientQuery = this.clientManagerDbContext.Clients.Include(c => c.City).ToList();

            //Primjer iterativnog građenja upita - dodaje se "where clause" samo u slučaju da je parametar doista proslijeđen.
            //To rezultira optimalnijim stablom izraza koje se kvalitetnije potencijalno prevodi u SQL
            if (!string.IsNullOrWhiteSpace(filter.FullName))
                clientQuery = clientQuery.Where(p => p.FullName.ToLower().Contains(filter.FullName.ToLower())).ToList();

            if (!string.IsNullOrWhiteSpace(filter.Address))
                clientQuery = clientQuery.Where(p => p.Address.ToLower().Contains(filter.Address.ToLower())).ToList();

            if (!string.IsNullOrWhiteSpace(filter.Email))
                clientQuery = clientQuery.Where(p => p.Email.ToLower().Contains(filter.Email.ToLower())).ToList();

            if (!string.IsNullOrWhiteSpace(filter.City))
                clientQuery = clientQuery.Where(p => p.City != null && p.City.Name.ToLower().Contains(filter.City.ToLower())).ToList();

            ViewBag.ActiveTab = 3;

            var model = clientQuery.ToList();
            return View("Index", model);
        }

        public IActionResult Details(int? id = null)
        {
            var model = id != null ? this.clientManagerDbContext.Clients.Include(c => c.City).Where(c => c.Id == id).FirstOrDefault() : null;
            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Cities = this.clientManagerDbContext.Cities.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(ClientInputBinder formClient)
        {
            Client newClient = new Client();

            newClient.FirstName = formClient.FirstName;
            newClient.LastName = formClient.LastName;
            newClient.Email = formClient.Email;
            newClient.Gender = formClient.Gender;
            newClient.Address = formClient.Address;
            newClient.PhoneNumber = formClient.PhoneNumber;
            newClient.CityId = formClient.CityId;
            //ViewBag.ActiveTab = 1;

            this.clientManagerDbContext.Clients.Add(newClient);
            this.clientManagerDbContext.SaveChanges();

            return View("Index", this.clientManagerDbContext.Clients.ToList());

        }
    }
}
