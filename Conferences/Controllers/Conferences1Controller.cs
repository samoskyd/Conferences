using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Conferences;
using Microsoft.AspNetCore.Http;
using ClosedXML.Excel;
using System.IO;

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {
            if (ModelState.IsValid)
            {
                if (fileExcel != null)
                {
                    using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
                    {
                        await fileExcel.CopyToAsync(stream);
                        using (XLWorkbook workBook = new XLWorkbook(stream, XLEventTracking.Disabled))
                        {
                            //перегляд усіх листів (в даному випадку категорій)
                            foreach (IXLWorksheet worksheet in workBook.Worksheets)
                            {
                                //worksheet.Name - назва категорії. Пробуємо знайти в БД, якщо відсутня, то створюємо нову
                                Location newloc;
                                var l = (from loc in _context.Locations
                                         where loc.City.Contains(worksheet.Name)
                                         select loc).ToList();
                                if (l.Count() > 0)
                                {
                                    newloc = l[0];
                                }
                                else
                                {
                                    newloc = new Location();
                                    newloc.City = worksheet.Name;
                                    newloc.Country = "можливо Україна";
                                    newloc.Capacity = 0;
                                    //додати в контекст
                                    _context.Locations.Add(newloc);
                                }
                                //перегляд усіх рядків                    
                                foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                                {
                                    try
                                    {
                                        Conference conf = new Conference();
                                        DateTime date = new DateTime();
                                        conf.Title = row.Cell(1).Value.ToString();
                                        conf.FormId = (int)row.Cell(2).Value;
                                        conf.OrganizerId = (int)row.Cell(3).Value;
                                        conf.Aim = "невідомо";
                                        conf.Topic = "невідомо";
                                        conf.RequirementsForWorks = "невідомо";
                                        conf.RequirementsForParticipants = "невідомо";
                                        conf.Price = 0;
                                        conf.DateAndTime = date;
                                        conf.Location = newloc;
                                        _context.Conferences.Add(conf);
                                        
                                        for (int i = 4; i <= 30; i++)
                                        {
                                            if (row.Cell(i).Value.ToString().Length > 0)
                                            {
                                                Participant participant;
                                        
                                                var p = (from par in _context.Participants
                                                         where par.FullName.Contains(row.Cell(i).Value.ToString())
                                                         select par).ToList();
                                                if (p.Count > 0)
                                                {
                                                    participant = p[0];
                                                }
                                                else
                                                {
                                                    participant = new Participant();
                                                    participant.FullName = row.Cell(i).Value.ToString();
                                                    DateTime date1 = new DateTime();
                                                    participant.BirthDate = date1;
                                                    participant.Occupation = "невідомо";
                                                    participant.Occupation = "невідомо";
                                                    _context.Add(participant);
                                                }
                                                ConferencesAndParticipant cap = new ConferencesAndParticipant();
                                                cap.Conference = conf;
                                                cap.Participant = participant;
                                                _context.ConferencesAndParticipants.Add(cap);
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    { }
                                }
                            }
                        }
                    }
                }

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Export()
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var locations = _context.Locations.Include(l => l.Conferences).ToList();
                //тут потрібно поставити умову на експорт. яку? не знаю.
                foreach (var l in locations)
                {
                    if (!(l.Conferences.ToList().Count > 0)) continue;
                    var worksheet = workbook.Worksheets.Add(l.City);

                    worksheet.Cell("A1").Value = "Назва";
                    worksheet.Cell("B1").Value = "Ціль";
                    worksheet.Cell("C1").Value = "Тема";
                    worksheet.Cell("D1").Value = "Форма проведення";
                    worksheet.Cell("E1").Value = "Вимоги до робіт";
                    worksheet.Cell("F1").Value = "Вимоги до учасників";
                    worksheet.Cell("G1").Value = "Ціна";
                    worksheet.Cell("H1").Value = "Дата та час проведення";
                    worksheet.Cell("I1").Value = "Організатор";
                    worksheet.Cell("J1").Value = "Локація";
                    worksheet.Row(1).Style.Font.Bold = true;
                    var conferences = l.Conferences.ToList();

                    for (int i = 0; i < conferences.Count; i++)
                    {
                        try
                        {
                            worksheet.Cell(i + 2, 1).Value = conferences[i].Title;
                            worksheet.Cell(i + 2, 2).Value = conferences[i].Aim;
                            worksheet.Cell(i + 2, 3).Value = conferences[i].Topic;
                            worksheet.Cell(i + 2, 4).Value = conferences[i].FormId;
                            worksheet.Cell(i + 2, 5).Value = conferences[i].RequirementsForWorks;
                            worksheet.Cell(i + 2, 6).Value = conferences[i].RequirementsForParticipants;
                            worksheet.Cell(i + 2, 7).Value = conferences[i].Price;
                            worksheet.Cell(i + 2, 8).Value = conferences[i].DateAndTime;
                            worksheet.Cell(i + 2, 9).Value = conferences[i].OrganizerId;
                            worksheet.Cell(i + 2, 10).Value = conferences[i].LocationId;

                            var cap = _context.ConferencesAndParticipants.Where(c => c.ConferenceId == conferences[i].ConferenceId).Include(c => c.Participant).ToList();
                            int j = 0;
                            foreach (var c in cap)
                            {
                                if (j < 16)
                                {
                                    worksheet.Cell(i + 2, j + 11).Value = c.Participant.FullName;
                                    j++;
                                }
                            }
                        }
                        catch (Exception ex)
                        { }
                            
                    }
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"library_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }

    }
}
