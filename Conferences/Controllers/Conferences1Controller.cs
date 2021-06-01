using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Conferences;

namespace Conferences.Controllers
{
    public class Conferences1Controller : Controller
    {
        private readonly istatpContext _context;

        public Conferences1Controller(istatpContext context)
        {
            _context = context;
        }

        // GET: Conferences1
        public async Task<IActionResult> Index()
        {
            var istatpContext = _context.Conferences.Include(c => c.Form).Include(c => c.Location).Include(c => c.Organizer);
            return View(await istatpContext.ToListAsync());
        }

        // GET: Conferences1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conference = await _context.Conferences
                .Include(c => c.Form)
                .Include(c => c.Location)
                .Include(c => c.Organizer)
                .FirstOrDefaultAsync(m => m.ConferenceId == id);
            if (conference == null)
            {
                return NotFound();
            }

            return View(conference);
        }

        // GET: Conferences1/Create
        public IActionResult Create()
        {
            ViewData["FormId"] = new SelectList(_context.Forms, "FormId", "FullName");
            ViewData["LocationId"] = new SelectList(_context.Locations, "LocationId", "City");
            ViewData["OrganizerId"] = new SelectList(_context.Organizers, "OrganizerId", "FullName");
            return View();
        }

        // POST: Conferences1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ConferenceId,Title,Aim,Topic,FormId,RequirementsForWorks,RequirementsForParticipants,Price,DateAndTime,OrganizerId,LocationId")] Conference conference)
        {
            if (ModelState.IsValid)
            {
                _context.Add(conference);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FormId"] = new SelectList(_context.Forms, "FormId", "FullName", conference.FormId);
            ViewData["LocationId"] = new SelectList(_context.Locations, "LocationId", "City", conference.LocationId);
            ViewData["OrganizerId"] = new SelectList(_context.Organizers, "OrganizerId", "FullName", conference.OrganizerId);
            return View(conference);
        }

        // GET: Conferences1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conference = await _context.Conferences.FindAsync(id);
            if (conference == null)
            {
                return NotFound();
            }
            ViewData["FormId"] = new SelectList(_context.Forms, "FormId", "FullName", conference.FormId);
            ViewData["LocationId"] = new SelectList(_context.Locations, "LocationId", "City", conference.LocationId);
            ViewData["OrganizerId"] = new SelectList(_context.Organizers, "OrganizerId", "FullName", conference.OrganizerId);
            return View(conference);
        }

        // POST: Conferences1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ConferenceId,Title,Aim,Topic,FormId,RequirementsForWorks,RequirementsForParticipants,Price,DateAndTime,OrganizerId,LocationId")] Conference conference)
        {
            if (id != conference.ConferenceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(conference);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConferenceExists(conference.ConferenceId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["FormId"] = new SelectList(_context.Forms, "FormId", "FullName", conference.FormId);
            ViewData["LocationId"] = new SelectList(_context.Locations, "LocationId", "City", conference.LocationId);
            ViewData["OrganizerId"] = new SelectList(_context.Organizers, "OrganizerId", "FullName", conference.OrganizerId);
            return View(conference);
        }

        // GET: Conferences1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conference = await _context.Conferences
                .Include(c => c.Form)
                .Include(c => c.Location)
                .Include(c => c.Organizer)
                .FirstOrDefaultAsync(m => m.ConferenceId == id);
            if (conference == null)
            {
                return NotFound();
            }

            return View(conference);
        }

        // POST: Conferences1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var conference = await _context.Conferences.FindAsync(id);
            _context.Conferences.Remove(conference);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConferenceExists(int id)
        {
            return _context.Conferences.Any(e => e.ConferenceId == id);
        }
    }
}
