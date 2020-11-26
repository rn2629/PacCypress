using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PAC.Controllers
{
    public class FAQController : Controller
    {
        public IActionResult index()
        {
            if (User.IsInRole("ProfDeSoutien"))
            {
                return View("ProfDeSoutien");
            }
            else if (User.IsInRole("Enseignant"))
            {
                return View("Enseignant");
            }
            else if (User.IsInRole("Etudiant"))
            {
                return View("Etudiant");
            }
            else
                return RedirectToAction("index", "Navigation");
        }
    }
}

