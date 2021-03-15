using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using ClfApi.Context;
using ClfApi.Models;

namespace ClfApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ClfController : ControllerBase
    {
        private readonly ClfDbContext _context;

        public ClfController(ClfDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Clf>>> GetClfs()
        {
            return await _context.Clfs.Take(10).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Clf>> GetClf(Guid id)
        {
            var clf = await _context.Clfs
                .Where(clf => clf.Id == id)
                .FirstOrDefaultAsync();

            if (clf == null)
            {
                return NotFound();
            }

            return clf;
        }

        [HttpGet("by-ip-address/{ipAddress}")]
        public async Task<ActionResult<IEnumerable<Clf>>> GetByIpAddress(string ipAddress)
        {
            var clf = await _context.Clfs
                .Where(clf => clf.IpAddress == ipAddress)
                .ToListAsync();

            if (clf == null)
            {
                return NotFound();
            }

            return clf;
        }
        [HttpGet("by-date-time/{dateTime}")]
        public async Task<ActionResult<IEnumerable<Clf>>> GetByIpAddress(DateTime dateTime)
        {
            var clf = await _context.Clfs
                .Where(clf => clf.RequestDate == dateTime)
                .ToListAsync();

            if (clf == null)
            {
                return NotFound();
            }

            return clf;
        }

        // When we do not use [FromQuery], it results in a 404 (Not Found) response, because the string userAgent contains "/", which confuses the router.
        [HttpGet("by-user-agent")]
        public async Task<ActionResult<IEnumerable<Clf>>> GetByUserAgent([FromQuery] string userAgent)
        {
            var clf = await _context.Clfs
                .Where(clf => clf.UserAgent == userAgent)
                .ToListAsync();

            if (clf == null)
            {
                return NotFound();
            }

            return clf;
        }

        
        // PUT: api/Clfs/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("UpdateClf/{id}")]
        public async Task<IActionResult> PutClf(Guid id, Clf Clf)
        {
            if (id != Clf.Id)
            {
                return BadRequest();
            }

            _context.Entry(Clf).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClfExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Clf>> PostClf(Clf obj)
        {
            _context.Clfs.Add(obj);
            _context.SaveChanges();

            var Clf = await _context.Clfs
                .Where(clf => clf.Id == clf.Id)
                .FirstOrDefaultAsync();

            if (Clf == null)
            {
                return NotFound();
            }

            return Clf;
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult<Clf>> DeleteClf(Guid id)
        {
            var clf = await _context.Clfs.FindAsync(id);
            if (clf == null)
            {
                return NotFound();
            }

            _context.Clfs.Remove(clf);
            await _context.SaveChangesAsync();

            return clf;
        }

        private bool ClfExists(Guid id)
        {
            return _context.Clfs.Any(clf => clf.Id == id);
        }
    }
}