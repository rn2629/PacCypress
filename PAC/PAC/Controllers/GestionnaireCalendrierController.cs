using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using PAC.Models;

namespace PAC.Controllers
{
    public class GestionnaireCalendrierController: Controller
    {
        DatePickerContext _context;

        public GestionnaireCalendrierController(DatePickerContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.rencontres = _context.tblRencontre.Select(e => e.seanceCoursId).ToList();
            return View(_context.tblAdminCommand.Select(e => e).First().rencontreFixed);
        }


        public IActionResult VerouillerPost()
        {
            _context.tblAdminCommand.Select(e => e).First().rencontreFixed = true;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult DeverouillerPost()
        {
            _context.tblAdminCommand.Select(e => e).First().rencontreFixed = false;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Forcer()
        {
            if(_context.tblRencontre.Where(e=>e.seanceCoursId== Int32.Parse(Request.Form["idCours"])).Select(e => e).Count() == 0)
            {
                CalendrierControllerModel model = new CalendrierControllerModel();
                model.Cours = _context.tblSeanceCours.Find(Int32.Parse(Request.Form["idCours"]));
                model.Prof = _context.AspNetUsers.Find(model.Cours.enseignantId);
                model.Etudiants = (from p in _context.AspNetUsers
                                   join e in _context.tblEtudiant on p.Id equals e.Id
                                   where e.Jumeler == false
                                   select p).ToList();
                return View(model);
            }
            else
            {
                return RedirectToAction("Index");
            }

        }
        public IActionResult PostForcer()
        {
            var rencontre = new Rencontre();
            var etudiant = _context.tblEtudiant.Find(Request.Form["option"]);
            int coursId = Int32.Parse(Request.Form["cours"]);
            if (etudiant.Jumeler == false)
            {
                etudiant.Jumeler = true;
                rencontre.etudiantId = etudiant.Id;
                rencontre.seanceCoursId = coursId;
                _context.tblRencontre.Add(rencontre);
            }

            _context.SaveChanges();

            return View("Index", _context.tblAdminCommand.Select(e => e).First().rencontreFixed);
        }
        private List<Pairing> GetPairingFromDatabase()
        {
            IEnumerable<DatePickerEvent> tempListCours = (from p in _context.tblSeanceCours join e in _context.tblRencontre on p.id equals e.seanceCoursId select p);

            IEnumerable<DatePickerEvent> thlist = _context.tblSeanceCours.Select(e => e).Except(tempListCours);

            IEnumerable < Period > periods = thlist.Select(p=>new Period(p.id, p.startTime, p.endTime ));

            //////////////////////////////////////////////////////////////////
            IEnumerable<DatePickerEventEtu> tempListDispo = (from p in _context.tblDisponibilites join e in _context.tblRencontre on p.id equals e.seanceCoursId select p);

            IEnumerable<DatePickerEventEtu> dispolist = _context.tblDisponibilites.Select(e => e).Except(tempListDispo);

            IEnumerable<Disponibility> disponibilities = dispolist.Select(p => new Disponibility(p.etudiantId, p.startTime, p.endTime, p.priority));

            return MatchMaker.Match(periods.ToList(), disponibilities.ToList());
        }

        public IActionResult AutomaticJoin()
        {
            
                List<Pairing> pairedLst = GetPairingFromDatabase();
                foreach(Pairing item in pairedLst)
                {
                    Rencontre tempRencontre = new Rencontre();
                    tempRencontre.etudiantId = item.IDStudent;
                    _context.tblEtudiant.Find(tempRencontre.etudiantId).Jumeler = true;

                    tempRencontre.seanceCoursId = item.IDPeriod;
                    _context.tblRencontre.Add(tempRencontre);
                    _context.SaveChanges();

                var Enseignant = (from p in _context.AspNetUsers join sceance in _context.tblSeanceCours on p.Id equals sceance.enseignantId where sceance.id == tempRencontre.seanceCoursId select p).First();
                var cours = _context.tblSeanceCours.Find(tempRencontre.seanceCoursId);
                var theEtudiant = _context.AspNetUsers.Find(tempRencontre.etudiantId);

                Courriel message = new Courriel(theEtudiant.UserName, theEtudiant.Email, "Assistance de cours planifiée", "Votre assistance de cours avec le professeur : " + Enseignant.UserName + " est prévue le " + cours.startTime.Date.ToString("dd MMMM") + " de " + cours.startTime.Hour + "h à " + cours.endTime.Hour + "H au local " + cours.local + ".\nPour plus d'information consulter l'application d'assistance de cours"); ;
                message.Envoyer();
                message = new Courriel(Enseignant.UserName, Enseignant.Email, "Assistance de cours planifiée", "Votre assistance de cours avec l'étudiant : " + theEtudiant.UserName + " est prévue le " + cours.startTime.Date.ToString("dd MMMM") + " de " + cours.startTime.Hour + "h à " + cours.endTime.Hour + "H au local " + cours.local + ".\nPour plus d'information consulter l'application d'assistance de cours");
                message.Envoyer();
                }
           
            return RedirectToAction("index");
        }
    }
}
