using Microsoft.AspNetCore.Mvc;
using Vjezba.Web.Mock;
using Vjezba.Web.Models;

namespace Vjezba.Web.Controllers
{
    public class ClientController : Controller
    {
        public IActionResult Index(string query)
        {
            List<Client> mockClients = 
                query != null ? 
                    MockClientRepository.Instance.All()
                        .Where(c => c.FirstName.ToLower().Contains(query.ToLower()) || c.LastName.ToLower().Contains(query.ToLower()))
                        .ToList() :
                    MockClientRepository.Instance.All().ToList();

            return View(model: mockClients);
        }

        [HttpPost]
        public IActionResult Index(string nameQuery, string queryAddress)
        {
            List<Client> mockClients = 
                MockClientRepository.Instance.All()
                .Where(c => nameQuery != null ? 
                    (c.FirstName.ToLower().Contains(nameQuery.ToLower()) || c.LastName.ToLower().Contains(nameQuery.ToLower())) : 
                    true)
                .Where(c => queryAddress != null ? 
                    c.Address.ToLower().Contains(queryAddress.ToLower()) : 
                    true)
                .ToList();

            return View(model: mockClients);
        }

        public IActionResult AdvancedSearch(ClientFilterModel model)
        {
            List<Client> mockClients = 
                MockClientRepository.Instance.All()
                .Where(c => model.Ime != null ?
                    (c.FirstName.ToLower().Contains(model.Ime.ToLower()) || c.LastName.ToLower().Contains(model.Ime.ToLower())) :
                    true)
                .Where(c => model.Email != null ?
                    (c.Email.ToLower().Contains(model.Email.ToLower())) :
                    true)
                .Where(c => model.Adresa != null ?
                    (c.Address.ToLower().Contains(model.Adresa.ToLower())) :
                    true)
                .Where(c => model.Grad != null ?
                    (c.City != null ? c.City.Name.ToLower().Contains(model.Grad.ToLower()) : false) :
                    true)
                .ToList();


            return View("Index", model: mockClients);
        }

        public IActionResult Details(int? id)
        {
            Client foundById = MockClientRepository.Instance.FindByID(id.GetValueOrDefault());

            if (foundById != null)
            {
                return View(model: foundById);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
    }
}
