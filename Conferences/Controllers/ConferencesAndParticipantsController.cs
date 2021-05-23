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
    public class ConferencesAndParticipantsController : Controller
    {
        private readonly istatpContext _context;

        public ConferencesAndParticipantsController(istatpContext context)
        {
            _context = context;
        }

        // GET: ConferencesAndParticipants
        public async Task<IActionResult> Index()
        {
            var istatpContext = _context.ConferencesAndParticipants.Include(c => c.Conference).Include(c => c.Participant);
            return View(await istatpContext.ToListAsync());
        }

        // GET: ConferencesAndParticipants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conferencesAndParticipant = await _context.ConferencesAndParticipants
                .Include(c => c.Conference)
                .Include(c => c.Participant)
                .FirstOrDefaultAsync(m => m.ConferenceAndParticipantId == id);
            if (conferencesAndParticipant == null)
            {
                return NotFound();
            }

            return View(conferencesAndParticipant);
        }

        // GET: ConferencesAndParticipants/Create
        public IActionResult Create()
        {
            ViewData["ConferenceId"] = new SelectList(_context.Conferences, "ConferenceId", "Aim");
            ViewData["ParticipantId"] = new SelectList(_context.Participants, "ParticipantId", "FullName");
            return View();
        }

        // POST: ConferencesAndParticipants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ConferenceAndParticipantId,ParticipantId,ConferenceId")] ConferencesAndParticipant conferencesAndParticipant)
        {
            if (ModelState.IsValid)
            {
                _context.Add(conferencesAndParticipant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ConferenceId"] = new SelectList(_context.Conferences, "ConferenceId", "Aim", conferencesAndParticipant.ConferenceId);
            ViewData["ParticipantId"] = new SelectList(_context.Participants, "ParticipantId", "FullName", conferencesAndParticipant.ParticipantId);
            return View(conferencesAndParticipant);
        }

        // GET: ConferencesAndParticipants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conferencesAndParticipant = await _context.ConferencesAndParticipants.FindAsync(id);
            if (conferencesAndParticipant == null)
            {
                return NotFound();
            }
            ViewData["ConferenceId"] = new SelectList(_context.Conferences, "ConferenceId", "Aim", conferencesAndParticipant.ConferenceId);
            ViewData["ParticipantId"] = new SelectList(_context.Participants, "ParticipantId", "FullName", conferencesAndParticipant.ParticipantId);
            return View(conferencesAndParticipant);
        }

        // POST: ConferencesAndParticipants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ConferenceAndParticipantId,ParticipantId,ConferenceId")] ConferencesAndParticipant conferencesAndParticipant)
        {
            if (id != conferencesAndParticipant.ConferenceAndParticipantId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(conferencesAndParticipant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConferencesAndParticipantExists(conferencesAndParticipant.ConferenceAndParticipantId))
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
            ViewData["ConferenceId"] = new SelectList(_context.Conferences, "ConferenceId", "Aim", conferencesAndParticipant.ConferenceId);
            ViewData["ParticipantId"] = new SelectList(_context.Participants, "ParticipantId", "FullName", conferencesAndParticipant.ParticipantId);
            return View(conferencesAndParticipant);
        }

        // GET: ConferencesAndParticipants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conferencesAndParticipant = await _context.ConferencesAndParticipants
                .Include(c => c.Conference)
                .Include(c => c.Participant)
                .FirstOrDefaultAsync(m => m.ConferenceAndParticipantId == id);
            if (conferencesAndParticipant == null)
            {
                return NotFound();
            }

            return View(conferencesAndParticipant);
        }

        // POST: ConferencesAndParticipants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var conferencesAndParticipant = await _context.ConferencesAndParticipants.FindAsync(id);
            _context.ConferencesAndParticipants.Remove(conferencesAndParticipant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConferencesAndParticipantExists(int id)
        {
            return _context.ConferencesAndParticipants.Any(e => e.ConferenceAndParticipantId == id);
        }
    }
}
