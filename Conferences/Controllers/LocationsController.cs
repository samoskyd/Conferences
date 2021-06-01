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
    public class LocationsController : Controller
    {
        private readonly istatpContext _context;

        public LocationsController(istatpContext context)
        {
            _context = context;
        }

        // GET: Locations
        public async Task<IActionResult> Index()
        {
            return View(await _context.Locations.ToListAsync());
        }

        // GET: Locations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _context.Locations
                .FirstOrDefaultAsync(m => m.LocationId == id);
            if (location == null)
            {
                return NotFound();
            }

            //     return View(location);
            return RedirectToAction("Index", "Conferences", new { id = location.LocationId, name = location.City });
        }

        // GET: Locations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Locations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LocationId,Country,City,Capacity")] Location location)
        {
            if (ModelState.IsValid)
            {
                _context.Add(location);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(location);
        }

        // GET: Locations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _context.Locations.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }
            return View(location);
        }

        // POST: Locations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LocationId,Country,City,Capacity")] Location location)
        {
            if (id != location.LocationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(location);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocationExists(location.LocationId))
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
            return View(location);
        }

        // GET: Locations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _context.Locations
                .FirstOrDefaultAsync(m => m.LocationId == id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        // POST: Locations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LocationExists(int id)
        {
            return _context.Locations.Any(e => e.LocationId == id);
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
                                Location newlocation;
                                var l = (from location in _context.Locations
                                         where location.Country.Contains(worksheet.Name)
                                         select location).ToList();
                                if (l.Count() > 0)
                                {
                                    newlocation = l[0];
                                }
                                else
                                {
                                    newlocation = new Location();
                                    newlocation.City = worksheet.Name;
                                    newlocation.Country = "Unknown";
                                    newlocation.Capacity = 0;
                                    //додати в контекст
                                    _context.Locations.Add(newlocation);
                                }
                                //перегляд усіх рядків                    
                                foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                                {
                                    try
                                    {
                                        Conference conf = new Conference();
                                        conf.Title = row.Cell(1).Value.ToString();
                                        conf.Aim = row.Cell(2).Value.ToString();
                                        conf.Topic = row.Cell(3).Value.ToString();
                                        conf.FormId = (int) row.Cell(4).Value;
                                        conf.RequirementsForWorks = row.Cell(5).Value.ToString();
                                        conf.RequirementsForParticipants = row.Cell(6).Value.ToString();
                                        conf.Price = (decimal) row.Cell(7).Value;
                                        conf.DateAndTime = (DateTime) row.Cell(8).Value;
                                        conf.OrganizerId = (int) row.Cell(9).Value;
                                        conf.Location = newlocation;
                                        _context.Conferences.Add(conf);
                                        
                                    }
                                    catch (Exception e)
                                    {
                                        // тут варто розписати але згодом, згодом!
                                    }
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
                var locations = _context.Locations.Include("Conferences").ToList();
                //тут, для прикладу ми пишемо усі книжки з БД, в своїх проектах ТАК НЕ РОБИТИ (писати лише вибрані)
                foreach (var l in locations)
                {
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
                    worksheet.Cell("J1").Value = "Місце проведення";
                    worksheet.Row(1).Style.Font.Bold = true;
                    var conferences = l.Conferences.ToList();

                    //нумерація рядків/стовпчиків починається з індекса 1 (не 0)
                    for (int i = 0; i < conferences.Count; i++)
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
                    }
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"library_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }
    }
}
