using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PAC.Models;
using System.Data.Entity;
using System.Web;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace PAC.Controllers
{

        [Authorize(Roles = "Etudiant")]
        [Route("api/[controller]")]
        [ApiController]
        public class EventsEtuController : ControllerBase
        {
            private readonly DatePickerContext _context;
            public EventsEtuController(DatePickerContext context)
            {
                _context = context;
            }

            // GET api/events
            [HttpGet]
            public IEnumerable<WebApiEventEtu> Get()
            {
                
                return _context.tblDisponibilites
                    .ToList()
                    .Select(e => (WebApiEventEtu)e).Where(e=>e.idUser== User.FindFirst(ClaimTypes.NameIdentifier).Value);
            }
        // GET api/events/5


        // POST api/events
        [HttpPost]
            public ObjectResult Post([FromForm] WebApiEventEtu apiEvent)
            {
                var newEvent = (DatePickerEventEtu)apiEvent;
                newEvent.etudiantId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                _context.tblDisponibilites.Add(newEvent);
                _context.SaveChanges();

                return Ok(new
                {
                    tid = newEvent.id,
                    action = "inserted"
                });
            }

            // PUT api/events/5
            [HttpPut("{id}")]
            public ObjectResult Put(int id, [FromForm] WebApiEventEtu apiEvent)
            {
                var updatedEvent = (DatePickerEventEtu)apiEvent;
                var dbEvent = _context.tblDisponibilites.Find(id);
                dbEvent.etudiantId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                
                dbEvent.priority = updatedEvent.priority;
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
                var e = _context.tblDisponibilites.Find(id);
                if (e != null)
                {
                    _context.tblDisponibilites.Remove(e);
                    _context.SaveChanges();
                }

                return Ok(new
                {
                    action = "deleted"
                });
            }

        }
    
}
