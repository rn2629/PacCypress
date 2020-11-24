using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PAC.Models;
using MimeKit;
using Microsoft.AspNetCore.Identity;

namespace PAC.Controllers
{
    public class AgendaController: Controller
    {
        private readonly DatePickerContext _context;
        public List<Rencontre> renc;

        public AgendaController(DatePickerContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "ProfDeSoutien,Enseignant")]
        public IActionResult Enseignant()
        {
            renc = _context.tblRencontre.Select(e => e).ToList();
            return View(renc);
        }

        [Authorize(Roles = "Etudiant")]
        public IActionResult Etudiant()
        {
            return View();
        }
    }
}
