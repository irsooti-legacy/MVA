using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApiDemo.Models;

namespace WebApiDemo.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class TicketController : Controller
    {
        private TicketContext _context;

        public TicketController(TicketContext context)
        {
            _context = context;
            if (_context.TicketItems.Count() == 0)
            {
                _context.TicketItems.Add(new TicketItem { Concert = "Linkin Park" });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<TicketItem> GetAll()
        {
            return _context.TicketItems.AsNoTracking().ToList();
        }

        [HttpGet("{id}", Name = "GetTicket")]
        public IActionResult GetById(long id)
        {
            var ticket = _context.TicketItems.FirstOrDefault(t => t.Id == id);
            if (ticket == null)
            {
                return NotFound(); // 404
            }

            return new ObjectResult(ticket);
        }

        [HttpPost]
        public IActionResult Create([FromBody] TicketItem ticket)
        {
            if (ticket == null)
            {
                return BadRequest();
            }

            _context.TicketItems.Add(ticket);
            _context.SaveChanges();

            return CreatedAtRoute("GetTicket", new { id = ticket.Id }, ticket);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] TicketItem ticket)
        {
            if (ticket == null || ticket.Id != id)
            {
                return BadRequest();
            }

            var tick = _context.TicketItems.FirstOrDefault(t => t.Id == id);
            if (tick == null)
            {
                return NotFound();
            }

            tick.Artist = ticket.Artist;
            tick.Concert = ticket.Concert;
            tick.Available = ticket.Available;

            _context.TicketItems.Update(tick);
            _context.SaveChanges();

            //return new ObjectResult(tick);
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var tics = _context.TicketItems.FirstOrDefault(t => t.Id == id);
            if (tics == null)
            {
                return NotFound();
            }

            _context.Remove(tics);
            _context.SaveChanges();

            return new NoContentResult();
        }
    }
}