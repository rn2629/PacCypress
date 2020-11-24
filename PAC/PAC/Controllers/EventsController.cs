using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PAC.Models;
using System.Data.Entity;
using System.Web;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace PAC.Controllers
{
        [Authorize(Roles = "Enseignant")]    
        [Route("api/[controller]")]
        [ApiController]
        public class EventsController : ControllerBase
        {
            private readonly DatePickerContext _context;
            
            public EventsController(DatePickerContext context)
            {
                _context = context;
            }

            // GET api/events
            [HttpGet]
            public IEnumerable<WebApiEvent> Get()
            {

                return _context.tblSeanceCours
                    .ToList()
                    .Select(e => (WebApiEvent)e).Where(e => e.idUser == User.FindFirst(ClaimTypes.NameIdentifier).Value);
            }

        // GET api/events/5



        // POST api/events
        [HttpPost]
            public ObjectResult Post([FromForm] WebApiEvent apiEvent)
            {
                var newEvent = (DatePickerEvent)apiEvent;
                newEvent.enseignantId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                _context.tblSeanceCours.Add(newEvent);
                _context.SaveChanges();

                return Ok(new
                {
                    tid = newEvent.id,
                    action = "inserted"
                });
            }

            // PUT api/events/5
            [HttpPut("{id}")]
            public ObjectResult Put(int id, [FromForm] WebApiEvent apiEvent)
            {
                var updatedEvent = (DatePickerEvent)apiEvent;
                var dbEvent = _context.tblSeanceCours.Find(id);
                dbEvent.enseignantId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
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

        }
    
}
