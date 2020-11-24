using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PAC.Data;
using PAC.Models;

namespace PAC.Controllers
{
    public class AdminController : Controller
    {
        DatePickerContext _context;
        AdminControllerModel model;

        public AdminController(DatePickerContext context)
        {
            _context = context;
            model = new AdminControllerModel();
           
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            model.Enseignants = GetEnseignant().ToList();
            model.ProfDesoutiens = GetProfSout().ToList();
            return View(model);
        }

        public IEnumerable<IdentityUser> GetEnseignant()
        {
            var query = from Ens in _context.AspNetUsers
                        join EnseignantRoles in _context.AspNetUserRole on Ens.Id equals EnseignantRoles.UserId
                        join Roles in _context.AspNetRoles on EnseignantRoles.RoleId equals Roles.Id where Roles.Name == "Enseignant" select Ens;
                        return query;
        }

        public IEnumerable<IdentityUser> GetProfSout()
        {
            var query = from Ens in _context.AspNetUsers
                        join EnseignantRoles in _context.AspNetUserRole on Ens.Id equals EnseignantRoles.UserId
                        join Roles in _context.AspNetRoles on EnseignantRoles.RoleId equals Roles.Id
                        where Roles.Name == "ProfDeSoutien"
                        select Ens;
            return query;
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult IndexPost()
        {
            var users = GetEnseignant().ToList();
            foreach(IdentityUser e in GetProfSout())
            {
                users.Add(e);
            }
            foreach (IdentityUser personne in users)
            {
                if (HttpContext.Request.Form[personne.Id] == "oui")
                {
                    IEnumerable<string> Roles = (from p in _context.AspNetRoles where p.Name == "ProfDeSoutien" select p.Id).ToList();
                    IEnumerable<IdentityUserRole<string>> User= (from p in _context.AspNetUserRole where p.UserId == personne.Id select p).ToList();
                    User.First().RoleId = Roles.First();
                    _context.SaveChanges();


                }
                else if(HttpContext.Request.Form[personne.Id] == "non")
                {
                    if (personne.Email.Contains("dmin")==false) 
                    {
                        IEnumerable<string> Roles = (from p in _context.AspNetRoles where p.Name == "Enseignant" select p.Id).ToList();
                        List<IdentityUserRole<string>> data = _context.AspNetUserRole.Where(e => e.UserId == personne.Id).Select(e => e).ToList();
                        _context.AspNetUserRole.Where(e => e.UserId == personne.Id).Select(e => e).First().RoleId = Roles.First();
                        _context.SaveChanges();
                    }

                    
                }
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        [Obsolete]
        public IActionResult ResetPost()
        {
            _context.Database.ExecuteSqlCommand("exec dbo.Resetme");
            return RedirectToAction("index", "admin");
        }
    }
}
