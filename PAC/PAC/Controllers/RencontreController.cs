using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PAC.Models;

namespace PAC.Controllers
{
    public class RencontreController : Controller
    {
        DatePickerContext _context;
        RencontreModel model;

        public RencontreController(DatePickerContext context)
        {
            model = new RencontreModel();
            _context = context;
            model.listeEtudiant = new List<Microsoft.AspNetCore.Identity.IdentityUser>();
        }

        public IActionResult Index()
        {
            try
            { 
                if (User.IsInRole("Etudiant") == true)
                    return View("Etudiant",Etudiant());
                else if (User.IsInRole("Enseignant") == true || User.IsInRole("ProfDeSoutien"))
                    return View("Enseignant",Enseignant());
                else return RedirectToAction("Index", "Navigation");
            }
            catch(Exception e)
            {
                return View("Erreur");
            }
        }

        [Authorize(Roles = "ProfDeSoutien")]
        public IActionResult ProfDeSoutien()
        {
            string etuId = "";
            var listeEtu = (from EnsRen in _context.tblRencontre
                            join cours in _context.tblSeanceCours on EnsRen.seanceCoursId equals cours.id
                            join etu in _context.tblEtudiant on EnsRen.etudiantId equals etu.Id
                            join user in _context.AspNetUsers on etu.Id equals user.Id
                            select user).ToList();
            model.lstCours = (from EnsRen in _context.tblRencontre
                              join cours in _context.tblSeanceCours on EnsRen.seanceCoursId equals cours.id
                              join etu in _context.tblEtudiant on EnsRen.etudiantId equals etu.Id
                              join user in _context.AspNetUsers on etu.Id equals user.Id
                              select cours).ToList();

            model.listeEtudiant = listeEtu;

            model.enseignant = _context.AspNetUsers.Find(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            
            if (HttpContext.Request.Method == "POST")
                foreach (IdentityUser user in listeEtu)
                    if (HttpContext.Request.Form["lstEtu"] == user.Id)
                        etuId = user.Id;

         

            if (etuId != "")
            {
                var lst = (from user in _context.AspNetUsers
                           join ens in _context.tblEnseignant on user.Id equals ens.Id
                           join cours in _context.tblSeanceCours on ens.Id equals cours.enseignantId
                           join ren in _context.tblRencontre on cours.id equals ren.seanceCoursId
                           where ren.etudiantId == etuId
                           select user).ToList();

                model.enseignant = lst.First();

                model.etudiant = _context.AspNetUsers.Find(etuId);

                IEnumerable<DatePickerEvent> seance = (from se in _context.tblSeanceCours
                                                       join re in _context.tblRencontre on se.id equals re.seanceCoursId
                                                       where re.etudiantId == etuId
                                                       select se).ToList();
                model.cours = seance.First();
                model.rencontre = _context.tblRencontre.Where(e => e.etudiantId == etuId).Select(e => e).First();
            }
            else if (listeEtu.Count > 0)
            {

                var lsts = (from user in _context.AspNetUsers
                           join ens in _context.tblEnseignant on user.Id equals ens.Id
                           join cours in _context.tblSeanceCours on ens.Id equals cours.enseignantId
                           join ren in _context.tblRencontre on cours.id equals ren.seanceCoursId
                           where ren.etudiantId == listeEtu[0].Id
                           select user).ToList();

                model.enseignant = lsts.First();
                model.etudiant = _context.AspNetUsers.Find(listeEtu[0].Id);

                IEnumerable<DatePickerEvent> seance = (from se in _context.tblSeanceCours
                                                       join re in _context.tblRencontre on se.id equals re.seanceCoursId
                                                       where re.etudiantId == listeEtu[0].Id
                                                       select se).ToList();
                model.cours = seance.First();
                model.rencontre = _context.tblRencontre.Where(e => e.etudiantId == model.etudiant.Id).Select(e => e).ToList().First();
            }
            
            return View(model);
        }

        [Authorize(Roles = "Enseignant")]
        public RencontreModel Enseignant()
        {
            
            string etuId="";
            var listeEtu = (from EnsRen in _context.tblRencontre
                         join cours in _context.tblSeanceCours on EnsRen.seanceCoursId equals cours.id
                         join etu in _context.tblEtudiant on EnsRen.etudiantId equals etu.Id
                         join user in _context.AspNetUsers on etu.Id equals user.Id
                         where cours.enseignantId == User.FindFirst(ClaimTypes.NameIdentifier).Value
                         select user).ToList();

            model.lstCours= (from EnsRen in _context.tblRencontre
                             join cours in _context.tblSeanceCours on EnsRen.seanceCoursId equals cours.id
                             join etu in _context.tblEtudiant on EnsRen.etudiantId equals etu.Id
                             join user in _context.AspNetUsers on etu.Id equals user.Id
                             where cours.enseignantId == User.FindFirst(ClaimTypes.NameIdentifier).Value
                             select cours).ToList();

            model.listeEtudiant = listeEtu;

            model.enseignant = _context.AspNetUsers.Find(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if(HttpContext.Request.Method=="POST")
                foreach (IdentityUser user in listeEtu)
                    if (HttpContext.Request.Form["lstEtu"] == user.Id)
                    {
                        etuId = user.Id;
                        model.rencontre=_context.tblRencontre.Where(e=>e.etudiantId==etuId).Select(e=>e).First();
                    }
                        

            if (etuId != "") 
            { 

                model.etudiant= _context.AspNetUsers.Find(etuId);

                IEnumerable<DatePickerEvent> seance = (from se in _context.tblSeanceCours 
                                                         join re in _context.tblRencontre on se.id equals re.seanceCoursId
                                                         where re.etudiantId==etuId select se).ToList();
                model.cours = seance.First();
            }
            else if(listeEtu.Count>0)
            {
                model.etudiant = _context.AspNetUsers.Find(listeEtu[0].Id);

                IEnumerable<DatePickerEvent> seance = (from se in _context.tblSeanceCours
                                                       join re in _context.tblRencontre on se.id equals re.seanceCoursId
                                                       where re.etudiantId == listeEtu[0].Id
                                                       select se).ToList();
                model.cours = seance.First();
                model.rencontre = _context.tblRencontre.Where(e => e.etudiantId == listeEtu[0].Id).Select(e => e).First();
                if (HttpContext.Request.Method == "POST")
                {
                    model.rencontre = _context.tblRencontre.Where(e => e.seanceCoursId ==Int32.Parse(Request.Form["name"])).Select(e => e).First();
                    model.cours= _context.tblSeanceCours.Where(e => e.id == Int32.Parse(Request.Form["name"])).Select(e => e).First();
                    model.etudiant= _context.AspNetUsers.Find(model.rencontre.etudiantId);
                }
                    

            }

            return model;
        }

        [Authorize(Roles = "Etudiant")]
        public RencontreModel Etudiant()
        {
            model.etudiant = _context.AspNetUsers.Find(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var lst = (from user in _context.AspNetUsers
                       join ens in _context.tblEnseignant on user.Id equals ens.Id
                       join cours in _context.tblSeanceCours on ens.Id equals cours.enseignantId
                       join ren in _context.tblRencontre on cours.id equals ren.seanceCoursId
                       where ren.etudiantId == model.etudiant.Id
                       select user).ToList();

            model.enseignant = lst.First();

            IEnumerable<DatePickerEvent> seance = (from se in _context.tblSeanceCours
                                                   join re in _context.tblRencontre on se.id equals re.seanceCoursId
                                                   where re.etudiantId == model.etudiant.Id
                                                   select se).ToList();
            model.cours = seance.First();
            model.rencontre = _context.tblRencontre.Where(e=>e.etudiantId==User.FindFirst(ClaimTypes.NameIdentifier).Value).Select(e => e).First();
            var evaluation= _context.tblEvaluation.Where(e => e.rencontreId == model.rencontre.id).Select(e => e).ToList().First();
            model.evaluation = evaluation;

            return model;
        }
        
       
    }
}
