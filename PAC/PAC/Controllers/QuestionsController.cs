using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PAC.Models;

namespace PAC.Controllers
{
    public class QuestionsController : Controller
    {

        DatePickerContext _context;

        public QuestionsController(DatePickerContext context)
        {
            _context = context;
        }
        

        [Authorize(Roles = "ProfDeSoutien")]
        public IActionResult Index()
        {
            ViewBag.Possible = true;
            int nb = _context.tblRencontre.Select(e => e).ToList().Count();
            if (nb > 0)
            {
                ViewBag.Possible = false;
            }

            IEnumerable<Question> lstQuestion = _context.tblQuestion.OrderBy(e=>e.position);
            return View(lstQuestion);
        }
        /*
        [Authorize(Roles = "ProfDeSoutien")]
        [HttpPost]
        public IActionResult Supprimer()
        {

            int questionId = Convert.ToInt32(HttpContext.Request.Form["select"]);
            foreach (Question q in _context.tblQuestion.Where(e => e.position > _context.tblQuestion.Find(questionId).position))
                q.position--;
            _context.tblQuestion.Remove(_context.tblQuestion.Find(questionId));
            
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        */
        [Authorize(Roles = "ProfDeSoutien")]
        public IActionResult Supprimer(int id)
        {

            _context.tblQuestion.Remove(_context.tblQuestion.Find(id));
            foreach (Question q in _context.tblQuestion.Where(e => e.position > _context.tblQuestion.Find(id).position))
                q.position--;

            _context.SaveChanges();
            return RedirectToAction("index");
        }

        [Authorize(Roles = "ProfDeSoutien")]
        [HttpPost]
        public IActionResult Ajouter()
        {
            
            Question tquestion = new Question();
            int pos;
            tquestion.pointsTotal = 3;
            string questionId = HttpContext.Request.Form["input"];
            if (_context.tblQuestion.Select(e => e).Count() != 0)
                pos = _context.tblQuestion.Select(e => e).Count() + 1;
            else
                pos = 1;

            tquestion.position = pos ;
            tquestion.question = questionId;
            _context.tblQuestion.Add(tquestion);
            _context.SaveChanges();
            return RedirectToAction("index");
        }

        [Authorize(Roles = "ProfDeSoutien")]
        
        public IActionResult Up(int pos)
        {
            if (pos != 1)
            {
                _context.tblQuestion.Where(e => e.position == pos).Select(e => e).First().position = pos - 1;
                _context.tblQuestion.Where(e => e.position == pos - 1).Select(e => e).First().position = pos;
                _context.SaveChanges();
            }
            return RedirectToAction("index");
        }
        public IActionResult Down(int pos)
        {
            if (pos < _context.tblQuestion.Count())
            {
                _context.tblQuestion.Where(e => e.position == pos).Select(e => e).First().position = pos + 1;
                _context.tblQuestion.Where(e => e.position == pos + 1).Select(e => e).First().position = pos;
                _context.SaveChanges();
            }
            return RedirectToAction("index");
        }
    }
}
