using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PAC.Models;

namespace PAC.Controllers
{
    public class HoraireController : Controller
    {
        DatePickerContext _context;

        public HoraireController(DatePickerContext context)
        {
            _context = context;
        }

        [Authorize]
        public IActionResult Index()
        {

            return View();
        }

        [Authorize(Roles = "Enseignant,ProfDeSoutien")]
        public IActionResult Enseignant()
        {
            if (_context.tblAdminCommand.Select(e => e).First().rencontreFixed == false)
                return View();
            else
                return View("rencontreFixed");
        }

        [Authorize (Roles = "Etudiant")]
        public IActionResult Etudiant()
        {
            if (_context.tblAdminCommand.Select(e => e).First().rencontreFixed == false)
                return View();
            else
                return View("rencontreFixed");
        }
    }
}
