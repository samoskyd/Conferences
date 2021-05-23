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
    public class WorksAndParticipantsController : Controller
    {
        private readonly istatpContext _context;

        public WorksAndParticipantsController(istatpContext context)
        {
            _context = context;
        }

        // GET: WorksAndParticipants
        public async Task<IActionResult> Index()
        {
            var istatpContext = _context.WorksAndParticipants.Include(w => w.Participant).Include(w => w.Work);
            return View(await istatpContext.ToListAsync());
        }

        // GET: WorksAndParticipants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var worksAndParticipant = await _context.WorksAndParticipants
                .Include(w => w.Participant)
                .Include(w => w.Work)
                .FirstOrDefaultAsync(m => m.WorkAndParticipantId == id);
            if (worksAndParticipant == null)
            {
                return NotFound();
            }

            return View(worksAndParticipant);
        }

        // GET: WorksAndParticipants/Create
        public IActionResult Create()
        {
            ViewData["ParticipantId"] = new SelectList(_context.Participants, "ParticipantId", "FullName");
            ViewData["WorkId"] = new SelectList(_context.Works, "WorkId", "Title");
            return View();
        }

        // POST: WorksAndParticipants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WorkAndParticipantId,WorkId,ParticipantId")] WorksAndParticipant worksAndParticipant)
        {
            if (ModelState.IsValid)
            {
                _context.Add(worksAndParticipant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParticipantId"] = new SelectList(_context.Participants, "ParticipantId", "FullName", worksAndParticipant.ParticipantId);
            ViewData["WorkId"] = new SelectList(_context.Works, "WorkId", "Title", worksAndParticipant.WorkId);
            return View(worksAndParticipant);
        }

        // GET: WorksAndParticipants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var worksAndParticipant = await _context.WorksAndParticipants.FindAsync(id);
            if (worksAndParticipant == null)
            {
                return NotFound();
            }
            ViewData["ParticipantId"] = new SelectList(_context.Participants, "ParticipantId", "FullName", worksAndParticipant.ParticipantId);
            ViewData["WorkId"] = new SelectList(_context.Works, "WorkId", "Title", worksAndParticipant.WorkId);
            return View(worksAndParticipant);
        }

        // POST: WorksAndParticipants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WorkAndParticipantId,WorkId,ParticipantId")] WorksAndParticipant worksAndParticipant)
        {
            if (id != worksAndParticipant.WorkAndParticipantId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(worksAndParticipant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorksAndParticipantExists(worksAndParticipant.WorkAndParticipantId))
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
            ViewData["ParticipantId"] = new SelectList(_context.Participants, "ParticipantId", "FullName", worksAndParticipant.ParticipantId);
            ViewData["WorkId"] = new SelectList(_context.Works, "WorkId", "Title", worksAndParticipant.WorkId);
            return View(worksAndParticipant);
        }

        // GET: WorksAndParticipants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var worksAndParticipant = await _context.WorksAndParticipants
                .Include(w => w.Participant)
                .Include(w => w.Work)
                .FirstOrDefaultAsync(m => m.WorkAndParticipantId == id);
            if (worksAndParticipant == null)
            {
                return NotFound();
            }

            return View(worksAndParticipant);
        }

        // POST: WorksAndParticipants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var worksAndParticipant = await _context.WorksAndParticipants.FindAsync(id);
            _context.WorksAndParticipants.Remove(worksAndParticipant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorksAndParticipantExists(int id)
        {
            return _context.WorksAndParticipants.Any(e => e.WorkAndParticipantId == id);
        }
    }
}
