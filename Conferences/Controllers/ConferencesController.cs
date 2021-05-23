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
    public class ConferencesController : Controller
    {
        private readonly istatpContext _context;

        public ConferencesController(istatpContext context)
        {
            _context = context;
        }

        // GET: Conferences
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null) return RedirectToAction("Locations", "Index");
            ViewBag.LocationId = id;
            ViewBag.LocationCity = name;

            var conferencesByLocation = _context.Conferences.Where(c => c.LocationId == id).Include(c => c.Location).Include(c => c.Form).Include(c => c.Organizer);
            return View(await conferencesByLocation.ToListAsync());
        }

        // GET: Conferences/Details/5
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

        // GET: Conferences/Create

        public IActionResult Create(int? locationId)
        {
            ViewBag.LocationId = locationId;
            ViewData["FormId"] = new SelectList(_context.Forms, "FormId", "AvailableAudienceSize");
            ViewData["OrganizerId"] = new SelectList(_context.Organizers, "OrganizerId", "FullName");
            //ViewBag.LocationCity = _context.Locations.Where(c => c.LocationId == locationId).FirstOrDefault().City;
            // var temp = _context.Locations.Where(c => c.LocationId == locationId).FirstOrDefault();
            // if (temp != null) ViewBag.LocationCity = temp.City;
            return View();
        }

        // POST: Conferences/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int locationId, [Bind("ConferenceId,Title,Aim,Topic,FormId,RequirementsForWorks,RequirementsForParticipants,Price,DateAndTime,OrganizerId,LocationId")] Conference conference)
        {
            conference.LocationId = locationId;
            if (ModelState.IsValid)
            {
                _context.Add(conference);
                await _context.SaveChangesAsync();
                // return RedirectToAction(nameof(Index));

                return RedirectToAction("Index", "Conferences", new { id = locationId, name = _context.Locations.Where(c => c.LocationId == locationId).FirstOrDefault().City });
            }
            ViewData["FormId"] = new SelectList(_context.Forms, "FormId", "AvailableAudienceSize", conference.FormId);
            //ViewData["LocationId"] = new SelectList(_context.Locations, "LocationId", "City", conference.LocationId);
            ViewData["OrganizerId"] = new SelectList(_context.Organizers, "OrganizerId", "FullName", conference.OrganizerId);
            //return View();

            return RedirectToAction("Index", "Conferences", new { id = locationId, name = "" });
            //_context.Locations.Where(c => c.LocationId == locationId).FirstOrDefault().City
        }

        // GET: Conferences/Edit/5
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
            ViewData["FormId"] = new SelectList(_context.Forms, "FormId", "AvailableAudienceSize", conference.FormId);
            ViewData["LocationId"] = new SelectList(_context.Locations, "LocationId", "City", conference.LocationId);
            ViewData["OrganizerId"] = new SelectList(_context.Organizers, "OrganizerId", "FullName", conference.OrganizerId);
            
            return View(conference);
        }

        // POST: Conferences/Edit/5
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
            ViewData["FormId"] = new SelectList(_context.Forms, "FormId", "AvailableAudienceSize", conference.FormId);
            ViewData["LocationId"] = new SelectList(_context.Locations, "LocationId", "City", conference.LocationId);
            ViewData["OrganizerId"] = new SelectList(_context.Organizers, "OrganizerId", "FullName", conference.OrganizerId);
            return View(conference);
        }

        // GET: Conferences/Delete/5
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

        // POST: Conferences/Delete/5
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
