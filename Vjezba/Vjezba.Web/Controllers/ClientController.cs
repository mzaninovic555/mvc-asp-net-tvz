using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;
using Vjezba.Model;
using Vjezba.Web.Models;

namespace Vjezba.Web.Controllers
{
    public class ClientController : BaseController
    {
        private ClientManagerDbContext _dbContext;
        private IWebHostEnvironment _hostingEnvironment;
        private UserManager<AppUser> _userManager;
        public ClientController(ClientManagerDbContext dbContext, IWebHostEnvironment hostingEnvironment, UserManager<AppUser> _userManager)
        {
            this._dbContext = dbContext;
            _hostingEnvironment = hostingEnvironment;
            this._userManager = _userManager;
            //this.UserId = this._userManager.GetUserId(base.User);
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View("Index");
        }

        [Authorize]
        public IActionResult Details(int? id = null)
        {
            var client = this._dbContext.Clients
                .Include(p => p.City)
                .Where(p => p.ID == id)
                .FirstOrDefault();

            return View(client);
        }

        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Create()
        {
            this.FillDropdownValues();
            return View();
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        public IActionResult Create(Client model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedById = this.UserId;
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

        [Authorize(Roles = "Admin,Manager")]
        [ActionName(nameof(Edit))]
        public IActionResult Edit(int id)
        {
            var model = this._dbContext.Clients.FirstOrDefault(c => c.ID == id);
            this.FillDropdownValues();
            return View(model);
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        [ActionName(nameof(Edit))]
        public async Task<IActionResult> EditPost(int id)
        {
            var client = this._dbContext.Clients.Single(c => c.ID == id);
            client.UpdatedById = this.UserId;
            var ok = await this.TryUpdateModelAsync(client);

            if (ok && this.ModelState.IsValid)
            {
                this._dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            this.FillDropdownValues();
            return View();
        }

        [AllowAnonymous]
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

        [Authorize(Roles = "Admin")]
        //[HttpDelete]
        public IActionResult Delete(int clientId) 
        {
            Client clientToDelete = this._dbContext.Clients.Where(c => c.ID == clientId).FirstOrDefault();

            if(clientToDelete == null) 
            {
                return View(); 
            }

            this._dbContext.Clients.Remove(clientToDelete);
            this._dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin,Manager")]
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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin,Manager")]
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
