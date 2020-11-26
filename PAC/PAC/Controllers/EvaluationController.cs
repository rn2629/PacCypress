using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PAC.Models;

namespace PAC.Controllers
{
    public class EvaluationController : Controller
    {
        DatePickerContext _context;
        EvaluationControllerModel model;

        public EvaluationController(DatePickerContext context)
        {
            model = new EvaluationControllerModel();
            _context = context;
        }

        public IActionResult Index(int tRencontreId)
        {
            model.Evaluation = (_context.tblEvaluation.Where(e=>e.rencontreId==tRencontreId).Select(e=>e)).ToList().First();
            model.LstQuestion = (from pQuestion in _context.tblQuestion select pQuestion).ToList();
            model.LstEvaluationQuestion = (from p in _context.tblEvaluationQuestion where p.evaluationId==model.Evaluation.id select p).ToList();
            model.Enseignant = (from o in _context.AspNetUsers join p in _context.tblEnseignant on o.Id equals p.Id join s in _context.tblSeanceCours on p.Id equals s.enseignantId join u in _context.tblRencontre on s.id equals u.seanceCoursId where u.id == tRencontreId select o).ToList().First();
            model.Etudiant = (from o in _context.AspNetUsers join p in _context.tblEtudiant on o.Id equals p.Id join s in _context.tblRencontre on p.Id equals s.etudiantId where s.id == tRencontreId select o).First();


            if (User.IsInRole("Enseignant"))
                return View(model);
            else if (User.IsInRole("Etudiant"))
                return View("Etudiant", model);
            else if (User.IsInRole("ProfDeSoutien"))
                return View(model);
            else return RedirectToAction("Index", "Navigation");

        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult RencontrePost(int tRencontreId)
        {
            _context.tblEvaluation.Where(e => e.rencontreId == tRencontreId).First().disponible = true;
            _context.SaveChanges();


            model.Evaluation = _context.tblEvaluation.Where(e => e.rencontreId == tRencontreId).Select(e => e).ToList().First();
            model.LstQuestion = (from pQuestion in _context.tblQuestion select pQuestion).ToList();
            model.LstEvaluationQuestion = (from p in _context.tblEvaluationQuestion where p.evaluationId == model.Evaluation.id select p).ToList();
            _context.tblEvaluation.Where(e => e.rencontreId == tRencontreId).Select(e => e).ToList().First().commentaire= HttpContext.Request.Form["commentaire"];
            _context.SaveChanges();

            int cpt = 0;
            foreach(Question tQuestion in model.LstQuestion)
            {
                string questionString=tQuestion.id.ToString();
                _context.tblEvaluationQuestion.Find(model.Evaluation.id, tQuestion.id).resultat = Convert.ToInt32(HttpContext.Request.Form[questionString]);
                _context.SaveChanges();
                if (_context.tblEvaluationQuestion.Find(model.Evaluation.id, tQuestion.id).resultat != null)
                    cpt +=(int) _context.tblEvaluationQuestion.Find(model.Evaluation.id,tQuestion.id).resultat;
            }
            _context.tblEvaluation.Find(model.Evaluation.id).resultat = cpt;
           
            _context.SaveChanges();
            if (User.IsInRole("Etudiant"))
                return RedirectToAction("Index", "Rencontre");
            else if (User.IsInRole("Enseignant"))
                return RedirectToAction("Index","Rencontre");
            else if (User.IsInRole("ProfDeSoutien"))
                return RedirectToAction("Index", "Navigation");
            else
                return RedirectToPage("Index", "Navigation");
        }

        public IActionResult AutoEvaluation(int tRencontreId)
        {

            model.AutoEvaluation = _context.tblAutoEvaluation.Where(e => e.rencontreId == tRencontreId).Select(e => e).ToList().First();
            model.LstQuestion = (from pQuestion in _context.tblQuestion select pQuestion).ToList();
            model.LstAutoEvaluationQuestion = (from p in _context.tblAutoEvaluationQuestion where p.evaluationId == model.AutoEvaluation.id select p).ToList();
            model.Enseignant = (from o in _context.AspNetUsers join p in _context.tblEnseignant on o.Id equals p.Id join s in _context.tblSeanceCours on p.Id equals s.enseignantId join u in _context.tblRencontre on s.id equals u.seanceCoursId where u.id == tRencontreId select o).ToList().First();
            model.Etudiant = (from o in _context.AspNetUsers join p in _context.tblEtudiant on o.Id equals p.Id join s in _context.tblRencontre on p.Id equals s.etudiantId where s.id == tRencontreId select o).First();

            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult AutoEvaluationPost(int tRencontreId)
        {

            model.AutoEvaluation = _context.tblAutoEvaluation.Where(e => e.rencontreId == tRencontreId).Select(e => e).ToList().First();
            model.LstQuestion = (from pQuestion in _context.tblQuestion select pQuestion).ToList();
            model.LstAutoEvaluationQuestion = (from p in _context.tblAutoEvaluationQuestion where p.evaluationId == model.AutoEvaluation.id select p).ToList();
 
            _context.tblAutoEvaluation.Where(e => e.rencontreId == tRencontreId).Select(e => e).ToList().First().commentaire = HttpContext.Request.Form["commentaire"];
            _context.SaveChanges();

            int cpt = 0;
            foreach (Question tQuestion in model.LstQuestion)
            {
                string questionString = tQuestion.id.ToString();
                _context.tblAutoEvaluationQuestion.Find(model.AutoEvaluation.id, tQuestion.id).resultat = Convert.ToInt32(HttpContext.Request.Form[questionString]);
                _context.SaveChanges();
                if (_context.tblAutoEvaluationQuestion.Find(model.AutoEvaluation.id, tQuestion.id).resultat != null)
                    cpt += (int)_context.tblAutoEvaluationQuestion.Find(model.AutoEvaluation.id, tQuestion.id).resultat;
            }
            _context.tblAutoEvaluation.Find(model.AutoEvaluation.id).resultat = cpt;
            _context.SaveChanges();
            if (User.IsInRole("Etudiant"))
                return RedirectToAction("Index","Rencontre");
            else if (User.IsInRole("Enseignant"))
                return RedirectToAction("Index", "Rencontre");
            else if (User.IsInRole("ProfDeSoutien"))
                return RedirectToAction("Index", "Rencontre");
            else
                return RedirectToPage("Index", "Navigation");
        }

        [Authorize(Roles = "ProfDeSoutien")]
        public IActionResult Supprimer(int tRencontreId)
        {
            var renc = _context.tblRencontre.Find(tRencontreId);
            _context.tblRencontre.Remove(_context.tblRencontre.Find(tRencontreId));
            _context.tblEtudiant.Where(e => e.Id == renc.etudiantId).Select(e => e).First().Jumeler = false;
            _context.SaveChanges();
            return RedirectToAction("index","navigation");
        }

    }
}
