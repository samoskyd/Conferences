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
    public class Charts2Controller : ControllerBase
    {
        private readonly istatpContext _context;

        public Charts2Controller(istatpContext context)
        {
            _context = context;
        }

        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var participants = _context.Participants.Include(p => p.ConferencesAndParticipants).ToList();

            List<object> conf_part = new List<object>();
            conf_part.Add(new[] { "Учасник", "Кількість конференцій" });

            foreach (var p in participants)
            {
                conf_part.Add(new object[] { p.FullName, p.ConferencesAndParticipants.Count() });
            }
            return new JsonResult(conf_part);
        }
    }
}
