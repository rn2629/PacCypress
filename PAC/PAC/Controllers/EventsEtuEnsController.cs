using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PAC.Models;
using System.Data.Entity;
using System.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace PAC.Controllers
{
        
        [Route("api/[controller]/[action]")]
        [ApiController]
        public class EventsEtuEnsController : ControllerBase
    {
        private readonly DatePickerContext _context;

        public EventsEtuEnsController(DatePickerContext context)
        {
            _context = context;
        }

        // GET api/events
        [Authorize(Roles = "ProfDeSoutien")]
        [HttpGet]
        public IEnumerable<WebApiEvent> Get()
        {
            return _context.tblSeanceCours
                .ToList()
                .Select(e => (WebApiEvent)e);
        }
        [Authorize(Roles = "ProfDeSoutien")]
        public IEnumerable<WebApiEventEtu> GetEtu()
        {
            return _context.tblDisponibilites
                .ToList()
                .Select(e => (WebApiEventEtu)e);
        }


        // GET api/events/5
        [Authorize(Roles = "ProfDeSoutien")]
        [HttpGet("{id}")]
        public WebApiEvent Get(int id)
        {

            return (WebApiEvent)_context
                .tblSeanceCours
                .Find(id);
        }

        [Authorize(Roles = "ProfDeSoutien")]
        [HttpGet("{id}")]
        public string GetUserNameEtu(int id)
        {
            var req = (WebApiEventEtu)_context
                .tblDisponibilites
                .Find(id);

            return _context
                    .AspNetUsers
                    .Select(e => e).Where(e => e.Id == req.idUser)
                    .Select(e => e.UserName).FirstOrDefault();

        }
        //[HttpGet("{id}")]
        //Cette fonction n'est pas utile pour l'instant mais le sera plus tard
        [Authorize(Roles = "ProfDeSoutien")]
        public bool GetJumelage(string id)
        {
            return _context
                    .tblEtudiant
                    .Select(e => e).Where(e => e.Id == id)
                    .Select(e => e.Jumeler).FirstOrDefault();
        }

        [Authorize(Roles = "Etudiant")]
        public IEnumerable<WebApiEvent> GetRencontreEtu()
        {
            var seance = from s in _context.tblSeanceCours
                         join r in _context.tblRencontre
                         on s.id equals r.seanceCoursId
                         where r.etudiantId == User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value
                         select (WebApiEvent)s;

            return seance.ToList();

        }


        [Authorize(Roles = "Enseignant,ProfDeSoutien")]
        public IEnumerable<WebApiEvent> GetRencontreEns()
        {
            var seance = _context.tblSeanceCours
                .ToList()
                .Where(e => e.enseignantId == User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value)
                .Select(e => (WebApiEvent)e);

            return from s in seance
                   join r in _context.tblRencontre
                   on s.id equals r.seanceCoursId
                   where s.id == r.seanceCoursId
                   select s;
        }

        [Authorize(Roles = "ProfDeSoutien")]
        [HttpGet("{id}")]
        public bool GetRencontre(int id)
        {
            var req = _context.tblRencontre.Select(e => e.seanceCoursId).Where(e => e == id).FirstOrDefault();
            if (req != 0)
                return true;
            else
                return false;
        }
        //Sort la string créer dans GetParticipant
        [Authorize(Roles = "ProfDeSoutien")]
        [HttpGet("{id}")]
        public string GetUserNameEns(int id)
        {
            var req = (WebApiEvent)_context
                .tblSeanceCours
                .Find(id);

            return (_context
                    .AspNetUsers
                    .Select(e => e).Where(e => e.Id == req.idUser)
                    .Select(e => e.UserName).FirstOrDefault()) + GetParticipant(id);

        }

        // POST api/events


        // PUT api/events/5
        [Authorize(Roles = "ProfDeSoutien")]
        [HttpPut("{id}")]
        public ObjectResult Put(int id, [FromForm] WebApiEvent apiEvent)
        {
            var updatedEvent = (DatePickerEvent)apiEvent;
            var dbEvent = _context.tblSeanceCours.Find(id);
            dbEvent.enseignantId = updatedEvent.enseignantId;
            if (updatedEvent.local is null)
                dbEvent.local = "";
            else
                dbEvent.local = updatedEvent.local;


            dbEvent.nomCours = updatedEvent.nomCours;
            dbEvent.startTime = updatedEvent.startTime;
            dbEvent.endTime = updatedEvent.endTime;
            _context.SaveChanges();

            return Ok(new
            {
                action = "updated"
            });
        }

        // DELETE api/events/5
        [Authorize(Roles = "ProfDeSoutien")]
        [HttpDelete("{id}")]
        public ObjectResult DeleteEvent(int id)
        {
            var e = _context.tblSeanceCours.Find(id);
            if (e != null)
            {
                _context.tblSeanceCours.Remove(e);
                _context.SaveChanges();
            }

            return Ok(new
            {
                action = "deleted"
            });
        }


        //Sort le nom des étudiants et crée un string selon si ils ont une rencontre ou non
        [Authorize(Roles = "ProfDeSoutien")]
        public string GetParticipant(int idCours)
        {

            var startCours = _context.tblSeanceCours.Find(idCours).startTime;
            var endCours = _context.tblSeanceCours.Find(idCours).endTime;
            string dataParticipant = "";
            string dataStart = "<div><p> Sélectionner un étudiant</p>";
            bool etuDispo = false;
            IEnumerable<Etudiant> listParticipant = _context.tblEtudiant.ToList().Select(e => (Etudiant)e);
            IEnumerable<WebApiEventEtu> listDisponibilites = _context.tblDisponibilites.ToList().Select(e => (WebApiEventEtu)e);


            try
            {
                if(_context.tblRencontre.Where(e => e.seanceCoursId == idCours).Select(e => e).Count() == 0)
                {
                    foreach (WebApiEventEtu dispo in listDisponibilites)
                    {
                        var start = DateTime.Parse(dispo.start_date,
                            System.Globalization.CultureInfo.InvariantCulture);
                        var end = DateTime.Parse(dispo.end_date,
                            System.Globalization.CultureInfo.InvariantCulture);

                        //Fait le trie entre les étudiants qui sont disponible dans le cours et ceux qui ne le sont pas 
                        if ((int)start.DayOfWeek == (int)startCours.DayOfWeek)
                        {
                            if (startCours.TimeOfDay >= start.TimeOfDay && endCours.TimeOfDay <= end.TimeOfDay)
                            {
                                if (listParticipant.Select(e => e).Where(e => e.Id == dispo.idUser).ToList().First() != null)
                                {

                                    if (GetJumelage(dispo.idUser))
                                    {
                                        dataParticipant += "<div>" +
                                        $" <input type = 'button' class='etuJum_event' id = '{dispo.idUser}' value = '{GetUserNameEtu(dispo.id)}' onclick=\"deletePlage('{dispo.idUser}',{idCours})\">" +
                                        " </div>";
                                    }
                                    else
                                    {
                                        dataParticipant += "<div>" +
                                        $"<input type = 'button' class='etu_event' id = '{dispo.idUser}' value = '{GetUserNameEtu(dispo.id)}' onclick=\"savePlage('{dispo.idUser}',{idCours})\">" +
                                            "</div>";
                                    }
                                    listParticipant = listParticipant.Select(e => e).Where(e => e.Id != dispo.idUser).ToList();
                                    etuDispo = true;
                                }
                            }
                        }
                
                    }
                    if (etuDispo)
                        dataParticipant = dataStart + dataParticipant +
                                        "<form action='/GestionnaireCalendrier/Forcer' method='post'>" +
                                        @"<button type = 'submit' value=" + idCours + " name='idCours' class='etu_event' >Forcer une rencontre</button>" +
                                        "</form></div>";
                    
                }
                else if(_context.tblRencontre.Where(e => e.seanceCoursId == idCours).Select(e => e).Count() == 1)
                {
                    string etuId = _context.tblRencontre.Where(e => e.seanceCoursId == idCours).Select(e => e.etudiantId).First();
                    string nomEtu = _context.AspNetUsers.Where(e => e.Id == etuId).Select(e => e.UserName).First();
                    dataParticipant = "<div> Étudiant Sélectionné </div>" +
                                    "<div>" +
                                    $" <input type = 'button' class='etuJum_event' id = '{etuId}' value='{nomEtu}'  onclick=\"deletePlage('{etuId}',{idCours})\">" +
                                    " </div>";
                }
            }
            catch (Exception)
            {

            }

            if (dataParticipant == "")
                if(_context.tblRencontre.Where(e=>e.seanceCoursId==idCours).Select(e=>e).Count()==0)
                    dataParticipant = "<div> Aucun étudiant disponible </div>" +
                                  "<form action='/GestionnaireCalendrier/Forcer' method='post'>"+
                                  @"<button type = 'submit' value="+idCours+" name='idCours' class='etu_event' >Forcer une rencontre</button>" +
                                  "</form>"+
                                        "</div>";
                else
                {
                    string etuId = _context.tblRencontre.Where(e => e.seanceCoursId == idCours).Select(e => e.etudiantId).First();
                    string nomEtu = _context.AspNetUsers.Where(e=>e.Id==etuId).Select(e=>e.UserName).First();
                    dataParticipant = "<div> Étudiant Sélectionné </div>" +
                                    "<div>" +
                                    $" <input type = 'button' class='etuJum_event' id = '{etuId}' value='{nomEtu}'  onclick=\"deletePlage('{etuId}',{idCours})\">" +
                                    " </div>";
                }
                 
            return dataParticipant+"</div>";
        }


        //Ajoute les rencontres dans la base de données
        [Authorize(Roles = "ProfDeSoutien")]
        [HttpPost]
        public ObjectResult SavePlage()
        {
            var idUser = Request.Form["idUser"];
            var idCours = Int32.Parse(Request.Form["idCours"]);
            var rencontre = new Rencontre();
            var etudiant = _context.tblEtudiant.Find(idUser);
            if (etudiant.Jumeler == false)
            {
                etudiant.Jumeler = true;
                rencontre.etudiantId = idUser;
                rencontre.seanceCoursId = idCours;
                _context.tblRencontre.Add(rencontre);
            }


            _context.SaveChanges();

            var Enseignant = (from p in _context.AspNetUsers join sceance in _context.tblSeanceCours on p.Id equals sceance.enseignantId where sceance.id == idCours select p).First();
            var cours = _context.tblSeanceCours.Find(idCours);
            var theEtudiant = _context.AspNetUsers.Find(etudiant.Id);

            Courriel message = new Courriel(theEtudiant.UserName, theEtudiant.Email, "Assistance de cours planifiée", "Votre assistance de cours avec le professeur : " + Enseignant.UserName + " est prévue le " + cours.startTime.Date.ToString("dd MMMM") + " de " + cours.startTime.Hour + "h à " + cours.endTime.Hour + "H au local " + cours.local + ".\nPour plus d'information consulter l'application d'assistance de cours"); ;
            message.Envoyer();
            message = new Courriel(Enseignant.UserName, Enseignant.Email, "Assistance de cours planifiée", "Votre assistance de cours avec l'étudiant : " + theEtudiant.UserName + " est prévue le " + cours.startTime.Date.ToString("dd MMMM") + " de " + cours.startTime.Hour + "h à " + cours.endTime.Hour + "H au local " + cours.local + ".\nPour plus d'information consulter l'application d'assistance de cours");
            message.Envoyer();

            return Ok(new
            {
                tid = rencontre.id,
                action = "inserted"
            });

        }

        //Supprime les rencontres dans la base de données
        [Authorize(Roles = "ProfDeSoutien")]
        [HttpPost]
        public ObjectResult DeletePlage()
        {
            try
            {
                var idUser = Request.Form["idUser"];
                var idCours = Int32.Parse(Request.Form["idCours"]);
                var etudiant = _context.tblEtudiant.Find(idUser);
                var rencontre = _context.tblRencontre.Select(e => e).Where(e => e.seanceCoursId == idCours).First();
                if (etudiant.Jumeler == true)
                    etudiant.Jumeler = false;

                _context.tblRencontre.Remove(rencontre);

                _context.SaveChanges();

                return Ok(new
                {
                    action = "deleted"
                });
            }
            catch (Exception)
            {
                return Ok(new
                {
                    action = "Element non existant"
                });
            }
        }

        [Authorize(Roles = "ProfDeSoutien")]
        private List<Pairing> GetPairingFromDatabase()
        {
            IEnumerable<Period> periods = from p in _context.tblSeanceCours
                                          join r in _context.tblRencontre.DefaultIfEmpty() on p.id equals r.seanceCoursId
                                          where r.etudiantId == null
                                          select new Period(p.id, p.startTime, p.endTime);

            IEnumerable<Disponibility> disponibilities = from d in _context.tblDisponibilites
                                                         join e in _context.tblEtudiant on d.etudiantId equals e.Id
                                                         where e.Jumeler == false
                                                         select new Disponibility(d.etudiantId, d.startTime, d.endTime, d.priority);

            return MatchMaker.Match(periods.ToList(), disponibilities.ToList());
        }
    }
}
