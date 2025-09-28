
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wger.Api.Data;
using Wger.Api.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Wger.Api.Controllers
{
    [ApiController]
    [Route("api/v2/[controller]")]
    public class LanguageController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LanguageController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/v2/Language
        [HttpGet]
        [AllowAnonymous] // Languages are publicly accessible in Django
        public async Task<ActionResult<IEnumerable<Language>>> GetLanguages()
        {
            return await _context.Languages.OrderBy(l => l.ShortName).ToListAsync();
        }

        // GET: api/v2/Language/{id}
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Language>> GetLanguage(int id)
        {
            var language = await _context.Languages.FindAsync(id);

            if (language == null)
            {
                return NotFound();
            }

            return language;
        }
    }
}

