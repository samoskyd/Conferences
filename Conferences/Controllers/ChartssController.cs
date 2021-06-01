using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Conferences.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartssController : ControllerBase
    {
        private readonly istatpContext _context;

        public ChartssController(istatpContext context)
        {
            _context = context;
        }

        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var conferences = _context.Conferences.Include(p => p.ConferencesAndParticipants).ToList();

            List<object> conf_part = new List<object>();
            conf_part.Add(new[] { "Конференції", "Кількість учасників" });

            foreach (var c in conferences)
            {
                conf_part.Add(new object[] { c.Title, c.ConferencesAndParticipants.Count() });
            }
            return new JsonResult(conf_part);
        }
    }
}