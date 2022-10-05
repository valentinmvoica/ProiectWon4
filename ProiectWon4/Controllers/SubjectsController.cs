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
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(SubjectToGet))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public IActionResult AddCourse([FromBody]string subjectName)
        {
            if (string.IsNullOrWhiteSpace(subjectName))
            {
                return BadRequest("invalid subject name");
            }
            return Ok(dataLayer.AddSubject(subjectName).ToDto());
        }

 
}
