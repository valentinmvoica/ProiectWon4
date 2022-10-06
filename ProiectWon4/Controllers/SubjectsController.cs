using Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProiectWon4.DTOs;
using ProiectWon4.Extensions;

namespace ProiectWon4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        private readonly DataLayer dataLayer;

        public SubjectsController(DataLayer dataLayer)
        {
            this.dataLayer = dataLayer;
        }

        /// <summary>
        /// Adds a new subject tot the system
        /// </summary>
        /// <param name="subject">Subject data</param>
        /// <returns>Created subject data</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SubjectToGet))]
        public IActionResult AddCourse([FromBody] SubjectToCreate subject)
        {
            return Ok(dataLayer.AddSubject(subject.Name, subject.TeacherId).ToDto());
        }


    }
}