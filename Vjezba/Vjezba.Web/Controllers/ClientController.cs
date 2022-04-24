using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vjezba.Web.Mock;
using Vjezba.Web.Models;

namespace Vjezba.Web.Controllers
{
    public class ClientController : Controller
    {
        public IActionResult Index(string query = null)
        {
            var clientQuery = MockClientRepository.Instance.All();

            if (!string.IsNullOrWhiteSpace(query))
                clientQuery = clientQuery.Where(p => p.FullName.ToLower().Contains(query));

            ViewBag.ActiveTab = 1;

            return View(clientQuery.ToList());
        }

        [HttpPost]
        public ActionResult Index(string queryName, string queryAddress)
        {
            var clientQuery = MockClientRepository.Instance.All();

            //Primjer iterativnog građenja upita - dodaje se "where clause" samo u slučaju da je parametar doista proslijeđen.
            //To rezultira optimalnijim stablom izraza koje se kvalitetnije potencijalno prevodi u SQL
            if (!string.IsNullOrWhiteSpace(queryName))
                clientQuery = clientQuery.Where(p => p.FullName.ToLower().Contains(queryName));

            if (!string.IsNullOrWhiteSpace(queryAddress))
                clientQuery = clientQuery.Where(p => p.Address.ToLower().Contains(queryAddress));

            ViewBag.ActiveTab = 2;

            var model = clientQuery.ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult AdvancedSearch(ClientFilterModel filter)
        {
            var clientQuery = MockClientRepository.Instance.All();

            //Primjer iterativnog građenja upita - dodaje se "where clause" samo u slučaju da je parametar doista proslijeđen.
            //To rezultira optimalnijim stablom izraza koje se kvalitetnije potencijalno prevodi u SQL
            if (!string.IsNullOrWhiteSpace(filter.FullName))
                clientQuery = clientQuery.Where(p => p.FullName.ToLower().Contains(filter.FullName.ToLower()));

            if (!string.IsNullOrWhiteSpace(filter.Address))
                clientQuery = clientQuery.Where(p => p.Address.ToLower().Contains(filter.Address.ToLower()));

            if (!string.IsNullOrWhiteSpace(filter.Email))
                clientQuery = clientQuery.Where(p => p.Email.ToLower().Contains(filter.Email.ToLower()));

            if (!string.IsNullOrWhiteSpace(filter.City))
                clientQuery = clientQuery.Where(p => p.City != null && p.City.Name.ToLower().Contains(filter.City.ToLower()));

            ViewBag.ActiveTab = 3;

            var model = clientQuery.ToList();
            return View("Index", model);
        }

        public IActionResult Details(int? id = null)
        {
            var model = id != null ? MockClientRepository.Instance.FindByID(id.Value) : null;
            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Cities = MockCityRepository.Instance.All().ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(ClientInputBinder formClient)
        {
            Client newClient = new Client();

            newClient.ID = MockClientRepository.Instance.All().OrderByDescending(p => p.ID).First().ID + 1;
            newClient.FirstName = formClient.FirstName;
            newClient.LastName = formClient.LastName;
            newClient.Email = formClient.Email;
            newClient.Gender = formClient.Gender;
            newClient.Address = formClient.Address;
            newClient.PhoneNumber = formClient.PhoneNumber;
            newClient.CityID = formClient.CityID;
            newClient.City = MockCityRepository.Instance.All().Where(c => c.ID == formClient.CityID).First();

            MockClientRepository.Instance.InsertOrUpdate(newClient);
            ViewBag.ActiveTab = 1;

            return View("Index", MockClientRepository.Instance.All().ToList());
        }
    }
}
