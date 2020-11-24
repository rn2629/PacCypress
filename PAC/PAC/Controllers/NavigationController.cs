using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace PAC.Controllers
{
    public class NavigationController : Controller
    {
      
        [Authorize]
        public IActionResult Index()
        {

            if (User == null)
            {
                ViewBag.Message = "index";
                return View();
            }
            if ( User.IsInRole("Admin") == null)
            {
                ViewBag.Message = "index";
                return View();

            }
            if (User.IsInRole("Admin"))
                return RedirectToAction("Index", "Admin");
            String[] pages = Navigation();
            String[] namePages = NameNavigation();

            int nbOfItems = pages.Length;
            int gridSize = 0;

            if (nbOfItems < 2)
                gridSize = 2;
            else
                gridSize = (int)Math.Ceiling(Math.Sqrt(nbOfItems));

            ViewBag.pages = pages;
            ViewBag.namePages = namePages;

            ViewData["nbOfItems"] = nbOfItems;
            ViewData["gridSize"] = gridSize;

            ViewBag.Message = "index";
            return View();
        }
        public String[] Navigation()
        {
            if (User.IsInRole("Enseignant"))
                return new string[] { "horaire/enseignant", "Rencontre", "Agenda/Enseignant" };
            else if (User.IsInRole("Etudiant"))
                return new string[] { "horaire/etudiant", "Rencontre", "Identity/Account/Manage", "Agenda" };
            else if(User.IsInRole("ProfDeSoutien"))
                return new string[] { "horaire/enseignant","GestionnaireCalendrier","Rencontre","Rencontre/ProfDeSoutien","Questions", "Identity/Account/Manage","Agenda" };
            else
                return new string[] { };
        }
        public String[] NameNavigation()
        {
            if (User.IsInRole("Enseignant"))
                return new string[] { "Enseignant", "Rencontre", "Agenda" };
            else if (User.IsInRole("Etudiant"))
                return new string[] { "Etudiant", "Rencontre","Mon Profil", "Agenda" };
            else if (User.IsInRole("ProfDeSoutien"))
                return new string[] { "Enseignant", "Gestionnaire Calendrier", "Rencontre", "Rencontre avec Prof De Soutien","Modifier l'evaluation","Mon Profil","Agenda" };
            else
                return new string[] { };
        }
    }
}
