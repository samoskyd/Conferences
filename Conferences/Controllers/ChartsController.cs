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
    public class ChartsController : ControllerBase
    {
        private readonly istatpContext _context;

        public ChartsController(istatpContext context)
        {
            _context = context;
        }

        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var locations = _context.Locations.Include(c => c.Conferences).ToList();

            List<object> locConf = new List<object>();
            locConf.Add(new[] { "Локація", "Кількість конференцій" });

            foreach (var l in locations)
            {
                locConf.Add(new object[] { l.City, l.Conferences.Count() });
            }
            return new JsonResult(locConf);
        }
    }
}
