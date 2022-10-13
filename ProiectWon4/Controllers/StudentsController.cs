using Data;
using Data.Exceptions;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProiectWon4.DTOs;
using ProiectWon4.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace ProiectWon4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly DataLayer dataLayer;
        private readonly CatalogueDbContext context;

        public StudentsController(DataLayer dataLayer, CatalogueDbContext context)
        {
            this.dataLayer = dataLayer;
            this.context = context;
        }

        /// <summary>
        /// Retrieves all students with their addresses
        /// </summary>
        /// <returns>Students list</returns>
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<StudentWithAddressToGet>))]
        public IActionResult GetAllStudents() =>
            Ok(context.Students.Include(s => s.Address).Select(s => s.ToDto()).ToList());
        //            Ok(dataLayer.GetAllStudents().Select(s => s.ToDto()).ToList());

        /// <summary>
        /// Retrieves a student based on its corresponding id
        /// </summary>
        /// <param name="studentId">student id to get</param>
        /// <returns>studednt data</returns>
        [HttpGet("{studentId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentWithAddressToGet))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult GetById([FromRoute] int studentId)
        {
            var student = dataLayer.GetStudentById(studentId);
            if (student == null)
            {
                return NotFound("student not found");
            }

            return Ok(student.ToDto());
        }

        /// <summary>
        /// Creates a student based on the provided data
        /// </summary>
        /// <param name="studentToCreate">Student data</param>
        /// <returns>Created student data</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentWithAddressToGet))]
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
        public IActionResult UpdateStudent([FromRoute] int studentId, [FromBody] StudentToUpdate newStudentData)
        {
            var student = context.Students.FirstOrDefault(s => s.Id == studentId);

            if (student == null)
            {
                return NotFound($"A student with an id of {studentId} was not found");
            }

            student.FirstName = newStudentData.FirstName;
            student.LastName = newStudentData.LastName;
            student.Age = newStudentData.Age;

            context.SaveChanges();

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
            //dataLayer.AddMarkToStudent(studentId, markValue);


            var student = context.Students.FirstOrDefault(s => s.Id == studentId);

            if (student == null)
            {
                throw new EntityNotFoundException($"Student {studentId} is null");
            }

            student.Marks.Add(new Mark { Value = markValue, CreationDate = DateTime.UtcNow });
            context.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Gets all marks per student id
        /// </summary>
        /// <param name="studentId">id of the student</param>
        /// <param name="subjectId">optional subject id for marks filtering</param>
        /// <returns>list of marks</returns>
        [HttpGet("{Studentid}/marks")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MarkToGet>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult GetAllMarks([FromRoute] int studentId, [FromQuery] int? subjectId)
        {
            var studentEnumerable = context.Students.Include(s => s.Marks);
            var student = studentEnumerable.FirstOrDefault(s => s.Id == studentId);

            if (student == null)
            {
                return NotFound();
            }

            if (subjectId == null)
            {
                return Ok(student.Marks.Select(m => m.ToDto()).ToList());
            }
            else
            {
                return Ok(student.Marks.Where(m => m.SubjectId == subjectId).Select(m => m.ToDto()).ToList());
            }
        }

        /// <summary>
        ///  Gets averages per subject
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns>list of avereages per subject</returns>
        [HttpGet("{studentId}/averages/all")]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AverageForSubject>))]
        public IActionResult GetAveragesPerSubject([FromRoute] int studentId)
        {
            var student = context.Students.Include(s => s.Marks).FirstOrDefault(s => s.Id == studentId);

            if (student == null)
            {
                return NotFound("student not found");
            }


            return Ok(
                student.Marks.GroupBy(m => m.SubjectId).Select(
                g => new AverageForSubject { SubjectId = g.Key, Average = g.Average(m => m.Value) }
            ).ToList());
        }

        /// <summary>
        /// Gets the students ordered by the average
        /// </summary>
        /// <param name="orderDescending"></param>
        /// <returns></returns>
        [HttpGet("all/orderedByAverage")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<StudentWithAverageToGet>))]
        public IActionResult GetAllStudentOrdered([Optional][FromQuery] bool orderDescending)
        {
            var allStudentsWithMarks = context.Students.Include(s => s.Marks).ToList();

            var allStudents = allStudentsWithMarks.Select(s => new StudentWithAverageToGet(
                        s.Id,
                        s.FirstName + s.LastName,
                        s.Age,
                        s.Marks.GroupBy(m => m.SubjectId)
                        .Average(//calculul mediei mediilor notelor grupate pe materii
                            marksGroup => marksGroup.Average(mark => mark.Value) // calculul mediei unei materii
                            )
                        )
            );

            if (orderDescending)
            {
                return Ok(allStudents.OrderByDescending(s => s.Average).ToList());               
            }
            else
            {
                return Ok(allStudents.OrderByDescending(s => s.Average).ToList());
            }
        }
    }
}