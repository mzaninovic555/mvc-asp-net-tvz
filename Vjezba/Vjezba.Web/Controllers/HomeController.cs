﻿using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Vjezba.Web.Models;

namespace Vjezba.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Contact()
        {
            ViewBag.Message = "Jednostavan način proslijeđivanja poruke iz Controller -> View.";
            //Kao rezultat se pogled /Views/Home/Contact.cshtml renderira u "pravi" HTML
            //Primjetiti - View() je poziv funkcije koja uzima cshtml template i pretvara ga u HTML
            //Zasto bas Contact.cshtml? Jer se akcija zove Contact, te prema konvenciji se "po defaultu" uzima cshtml datoteka u folderu Views/CONTROLLER_NAME/AKCIJA.cshtml


            return View();
        }

        /// <summary>
        /// Ova akcija se poziva kada na formi za kontakt kliknemo "Submit"
        /// URL ove akcije je /Home/SubmitQuery, uz POST zahtjev isključivo - ne može se napraviti GET zahtjev zbog [HttpPost] parametra
        /// </summary>
        /// <param name="formData"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SubmitQuery(IFormCollection formData)
        {
            //Ovdje je potrebno obraditi podatke i pospremiti finalni string u ViewBag
            ViewBag.ime = formData["ime"];
            ViewBag.prezime = formData["prezime"];
            ViewBag.email = formData["email"];
            ViewBag.poruka = formData["poruka"];
            ViewBag.tipPoruke = formData["tipPoruke"];
            ViewBag.newsletter = formData["newsletter"] == "on" ? "obavijestiti ćemo vas" : "nećemo vas obavijestiti";

            //Kao rezultat se pogled /Views/Home/ContactSuccess.cshtml renderira u "pravi" HTML
            //Kao parametar se predaje naziv cshtml datoteke koju treba obraditi (ne koristi se default vrijednost)
            //Trazenu cshtml datoteku je potrebno samostalno dodati u projekt

            return View("ContactSuccess");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult FAQ(int? selected)
        {
            Dictionary<string, string> Faq = new Dictionary<string, string>();

            Faq.Add("Volite li Javu?", "Svakako.");
            Faq.Add("Što mislite o Spring-u?", "Apsolutno ne overloadani framework.");
            Faq.Add("Koliko importova treba jedna java klasa?", "Minimalno 20.");
            Faq.Add("Volite li pointere (zvjezdice)?", "Cesar kaže da su dobri.");
            Faq.Add("Volite li raditi front-end?", "There is no greater pain.");
            ViewBag.ListaFAQ = Faq;
            ViewBag.Selected = selected;

            return View();
        }
    }
}