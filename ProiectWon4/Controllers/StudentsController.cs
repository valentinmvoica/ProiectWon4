using Data;
using Data.Exceptions;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProiectWon4.DTOs;
using ProiectWon4.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ProiectWon4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly DataLayer dataLayer;
        private readonly CatalogueDbContext context;

        public StudentsController(DataLayer dataLayer)
        {
            this.dataLayer = dataLayer;
        }

        /// <summary>
        /// Retrieves all students with their addresses
        /// </summary>
        /// <returns>Students list</returns>
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<StudentToGet>))]
        public IActionResult GetAllStudents() =>
            Ok(dataLayer.GetAllStudents().Select(s => s.ToDto()).ToList());

        /// <summary>
        /// Retrieves a student based on its corresponding id
        /// </summary>
        /// <param name="studentId">student id to get</param>
        /// <returns>studednt data</returns>
        [HttpGet("{studentId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentToGet))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult GetById([FromRoute] int studentId)
        {
            var student = dataLayer.GetStudentById(studentId);
            if (student == null)
            {
                return NotFound("studednt not found");
            }

            return Ok(student.ToDto());
        }

        /// <summary>
        /// Creates a student based on the provided data
        /// </summary>
        /// <param name="studentToCreate">Student data</param>
        /// <returns>Created student data</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentToGet))]
        public IActionResult CreateStudent([FromBody] StudentToCreate studentToCreate)
        {
            return Ok(dataLayer.CreateStudent(studentToCreate.ToEntity()).ToDto());
        }



        /// <summary>
        /// Deletes a student based on an id
        /// </summary>
        /// <param name="studentId">student id</param>
        /// <param name="deleteAddress">if true the student address will be deleted as well</param>
        [HttpDelete("{studentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult DeleteStudent([FromRoute] int studentId, [FromQuery] bool deleteAddress)
        {
            dataLayer.DeleteStudent(studentId, deleteAddress);
            return Ok();
        }


        /// <summary>
        /// Updates a student's data
        /// </summary>
        /// <param name="studentId">the id of the student to modify</param>
        /// <param name="newStudentData">new student information</param>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult UpdateStudent([FromRoute] int studentId, [FromBody] StudentToUpdate newStudentData)
        {
            try
            {
                dataLayer.ChangeStudentData(studentId, newStudentData.ToEntity());
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }

            return Ok();
        }


        /// <summary>
        /// Updates or creates a student's address information
        /// </summary>
        /// <param name="studentId">student id</param>
        /// <param name="newAddress">new address</param>
        [HttpPut("{studentId}/address")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult ChangeStudentAddress([FromRoute] int studentId, [FromBody][Required] AddressToUpdate newAddress)
        {
            try
            {
                dataLayer.ChangeStudentAddress(studentId, newAddress.ToEntity());
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
            return Ok();
        }
        /// <summary>
        /// Adds a mark to the student
        /// </summary>
        /// <param name="studentId">student id</param>
        /// <param name="newAddress">new address</param>
        [HttpPost("{studentId}/addmark")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult AddMarktoStudent([FromRoute] int studentId, [FromBody][Range(1, 10)] int markValue)
        {
            dataLayer.AddMarkToStudent(studentId, markValue);
            return Ok();
        }

        /// <summary>
        /// Gets all marks per student id
        /// </summary>
        /// <param name="Studentid">id of the student</param>
        /// <returns>list of marks</returns>
        [HttpGet("{Studentid}/marks")]
        public List<MarkToGEt> GetAllMarks([FromRoute] int Studentid, [FromQuery] int? subjectId)
        {

            var student = context.Students.Include(s => s.Marks).First(s => s.Id == Studentid);
            if (subjectId == null)
            {
                return student.Marks.Select(m => m.ToDto()).ToList();
            }
            else
            {
                return student.Marks.Where(m => m.SubjectId == subjectId).Select(m => m.ToDto()).ToList();
            }

        }


        /// <summary>
        /// Gets all the averages per subject for a student
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns>list of all averages per subject</returns>
        [HttpGet("{studentId}/averages/all")]
        public IActionResult GetAveragesPerSubject([FromRoute] int studentId)
        {
            var student = context.Students.Include(s => s.Marks).First(s => s.Id == studentId);

            if (student == null)
            {
                return NotFound("student not found");
            }

            return Ok(student.Marks.GroupBy(m => m.SubjectId).Select(
                g =>
                new KeyValuePair<int, double>(g.Key, g.Average(m => m.Value))
            ).ToList());
        }

        /// <summary>
        /// Gets the students ordered by the average
        /// </summary>
        /// <param name="orderDescending"></param>
        /// <returns></returns>
        [HttpGet("all/{orderDescending}/orderedByAverage")]
        public IActionResult GetAllstudentsordered([FromRoute] bool orderDescending)
        {
            var allStudentsWithMarks = context.Students.Include(s => s.Marks).ToList();
            var result = allStudentsWithMarks.OrderByDescending(s => s.Marks.Average(m => m.Value)).Select(s => new
            {
                Id = s.Id,
                Name = s.FirstName + s.LastName,
                Age = s.Age,
                Average = s.Marks.Average(m => m.Value)
            });
            return Ok();
        }
    }
}