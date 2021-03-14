using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClfApi.Context;
using ClfApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ClfApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClfController : ControllerBase
    {
        private readonly ILogger<ClfController> _logger;
        private readonly ClfDbContext _context;

        public ClfController(ILogger<ClfController> logger, ClfDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Clf> Get()
        {
            return _context.Clves.ToList();
        }
    }
}
