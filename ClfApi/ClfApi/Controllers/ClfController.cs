using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClfApi.Models;
using ClfApi.Services;

namespace ClfApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ClfController : ControllerBase
    {
        private readonly IClfService _clfService;
        private readonly IUtilService _utilService;

        public ClfController(ClfService clfService, UtilService utilService)
        {
            _clfService = clfService;
            _utilService = utilService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Clf>> GetClfs()
        {
            return _clfService.List().ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<Clf> GetClf(Guid id)
        {
            Clf clf = _clfService.Find(id);

            if (clf == null)
            {
                return NotFound();
            }

            return clf;
        }

        [HttpGet("by-client/{client}")]
        public ActionResult<IEnumerable<Clf>> GetByClient(string client)
        {
            List<Clf> clfs = _clfService.FindByClient(client).ToList();

            if (clfs == null)
            {
                return NotFound();
            }

            return clfs;
        }
        [HttpGet("by-date-time/{dateTime}")]
        public ActionResult<IEnumerable<Clf>> GetByRequestDate(DateTime dateTime)
        {
            List<Clf> clfs = _clfService.FindByRequestDate(dateTime).ToList();

            if (clfs == null)
            {
                return NotFound();
            }

            return clfs;
        }

        // When we do not use [FromQuery], it results in a 404 (Not Found) response, because the string userAgent contains "/", which confuses the router.
        [HttpGet("by-user-agent")]
        public ActionResult<IEnumerable<Clf>> GetByUserAgent([FromQuery] string userAgent)
        {
            List<Clf> clfs = _clfService.FindByUserAgent(userAgent).ToList();

            if (clfs == null)
            {
                return NotFound();
            }

            return clfs;
        }

        
        // PUT: api/Clfs/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public IActionResult PutClf(Guid id, Clf clf)
        {
            if (id != clf.Id)
            {
                return BadRequest();
            }

            if (_clfService.UpdateClf(clf) == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public ActionResult<Clf> PostClf(Clf clf)
        {
            if (_clfService.CreateClf(clf) == null)
            {
                return BadRequest("Already exists!");
            }

            return NoContent();
        }

        [HttpPost("batch")]
        public ActionResult PostBatchClf([FromForm] FileUpload fileUpload)
        {
            IEnumerable<Clf> result;
            if (fileUpload.File.Length > 0)
            {
                result = _utilService.BatchToList(fileUpload.File.OpenReadStream());
                _clfService.CreateMultipleClfs(result);
            }
            else
            {
                return BadRequest("Empty file!");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult<Clf> DeleteClf(Guid id)
        {
            Clf clf = _clfService.DeleteClf(id);
            if (clf == null)
            {
                return NotFound();
            }

            return clf;
        }

        public class FileUpload
        {
            public IFormFile File { get; set; }
        }
    }
}