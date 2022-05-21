using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vjezba.DAL;
using Vjezba.Model;
using Vjezba.Web.Models;

namespace Vjezba.Web.Controllers
{
    public class ClientController : Controller
    {
        private ClientManagerDbContext _dbContext;
        private IWebHostEnvironment _hostingEnvironment;

        public ClientController(ClientManagerDbContext dbContext, IWebHostEnvironment hostingEnvironment)
        {
            this._dbContext = dbContext;
            _hostingEnvironment = hostingEnvironment;
        }

        public ActionResult Index()
        {
            return View("Index");
        }

        public IActionResult Details(int? id = null)
        {
            var client = this._dbContext.Clients
                .Include(p => p.City)
                .Where(p => p.ID == id)
                .FirstOrDefault();

            return View(client);
        }

        public IActionResult Create()
        {
            this.FillDropdownValues();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Client model)
        {
            if (ModelState.IsValid)
            {
                this._dbContext.Clients.Add(model);
                this._dbContext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                this.FillDropdownValues();
                return View();
            }
        }

        [ActionName(nameof(Edit))]
        public IActionResult Edit(int id)
        {
            var model = this._dbContext.Clients.FirstOrDefault(c => c.ID == id);
            this.FillDropdownValues();
            return View(model);
        }

        [HttpPost]
        [ActionName(nameof(Edit))]
        public async Task<IActionResult> EditPost(int id)
        {
            var client = this._dbContext.Clients.Single(c => c.ID == id);
            var ok = await this.TryUpdateModelAsync(client);

            if (ok && this.ModelState.IsValid)
            {
                this._dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            this.FillDropdownValues();
            return View();
        }

        [HttpPost]
        public IActionResult IndexAjax (ClientFilterModel filter)
        {
            var clientQuery = this._dbContext.Clients.Include(p => p.City).AsQueryable();

            //Primjer iterativnog građenja upita - dodaje se "where clause" samo u slučaju da je parametar doista proslijeđen.
            //To rezultira optimalnijim stablom izraza koje se kvalitetnije potencijalno prevodi u SQL
            if (!string.IsNullOrWhiteSpace(filter.FullName))
                clientQuery = clientQuery.Where(p => (p.FirstName + " " + p.LastName).ToLower().Contains(filter.FullName.ToLower()));

            if (!string.IsNullOrWhiteSpace(filter.Address))
                clientQuery = clientQuery.Where(p => p.Address.ToLower().Contains(filter.Address.ToLower()));

            if (!string.IsNullOrWhiteSpace(filter.Email))
                clientQuery = clientQuery.Where(p => p.Email.ToLower().Contains(filter.Email.ToLower()));

            if (!string.IsNullOrWhiteSpace(filter.City))
                clientQuery = clientQuery.Where(p => p.CityID != null && p.City.Name.ToLower().Contains(filter.City.ToLower()));

            var model = clientQuery.ToList();

            return PartialView("_IndexTable", model);
        }

        public IActionResult UploadAttachment(int clientId, IFormFile file)
        {
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");

            string filePath = Path.Combine(path, file.FileName);
            using (Stream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                file.CopyTo(fileStream);
            }

            this._dbContext.Attachements.Add(new Attachement()
            {
                ClientID = clientId,
                FilePath = filePath
            });
            this._dbContext.SaveChanges();

            return Json(new { success = true });
        }

        [HttpDelete]
        public IActionResult DeleteAttachment(int attachmentId)
        {
            Attachement attachement = this._dbContext.Attachements.Where(a => a.ID == attachmentId).FirstOrDefault();

            if(attachement == null)
            {
                return Json(new { success = false });
            }

            this._dbContext.Attachements.Remove(attachement);
            this._dbContext.SaveChanges();

            return GetAttachments(attachement.ClientID);
        }

        [HttpGet]
        public IActionResult GetAttachments(int clientId)
        {
            List<Attachement> attachements = _dbContext.Attachements
                .Where(a => a.ClientID == clientId)
                .ToList();

            return PartialView("_AttachmentList", attachements);
        }

        private void FillDropdownValues()
        {
            var selectItems = new List<SelectListItem>();

            //Polje je opcionalno
            var listItem = new SelectListItem();
            listItem.Text = "- odaberite -";
            listItem.Value = "";
            selectItems.Add(listItem);

            foreach (var category in this._dbContext.Cities)
            {
                listItem = new SelectListItem(category.Name, category.ID.ToString());
                selectItems.Add(listItem);
            }

            ViewBag.PossibleCities = selectItems;
        }
    }
}
