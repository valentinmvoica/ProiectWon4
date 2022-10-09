using Data.Exceptions;
using Data;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using ProiectWon4.DTOs;
using ProiectWon4.Extensions;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ProiectWon4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class TeachersController : ControllerBase
    {
        private readonly CatalogueDbContext dbContext;

        public TeachersController(CatalogueDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Creates a new teacher based on the given data
        /// </summary>
        /// <param name="teacherToCreate">techer data</param>
        /// <returns>created teacher information</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK,Type =typeof(TeacherToGetFullInfo))]
        public IActionResult CreateTeacher([FromBody] TeacherToCreate teacherToCreate)
        {
            var teacher = teacherToCreate.ToEntity();

            dbContext.Teachers.Add(teacher);

            dbContext.SaveChanges();
            return Ok(teacher.ToDto());
        }

        /// <summary>
        /// Updates or creates a teacher's address information
        /// </summary>
        /// <param name="teacherId">teacher id</param>
        /// <param name="newAddress">new address info</param>
        [HttpPut("{studentId}/address")]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TeacherWithAddressToGet))]
        public IActionResult ChangeTeacherAddress([FromRoute][Required][Range(1,int.MaxValue)] int teacherId, [FromBody][Required] AddressToUpdate newAddress)
        {

            var teacher = dbContext.Teachers.Include(t=>t.Address).FirstOrDefault(t => t.Id == teacherId);
            if (teacher == null)
            {
                return NotFound($"invalid teacher id {teacherId}");
            }

            if (teacher.Address == null)
            {
                teacher.Address = new Address();
            }

            teacher.Address.City = newAddress.City;
            teacher.Address.Street = newAddress.Street;
            teacher.Address.Number = newAddress.Number;

            dbContext.SaveChanges();

            return Ok(teacher.ToDtoWithAddress());
        }

        /// <summary>
        /// changes the subject for a teacher
        /// </summary>
        /// <param name="teacherId">teacher id</param>
        /// <param name="newSubjectId">new subject id</param>
        /// <returns></returns>
        [HttpPut("change-subject/{teacherId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TeacherWithSubjectToGet))]
        
        public IActionResult ChangeTeacherSubject([FromRoute][Required][Range(1, int.MaxValue)] int teacherId, [FromBody][Required][Range(1, int.MaxValue)] int newSubjectId)
        {
            var teacher = dbContext.Teachers.Include(t => t.Subject).FirstOrDefault();
            if (teacher == null)
            {
                return NotFound($"invalid teacher id {teacherId}");
            }

            var subject = dbContext.Subjects.FirstOrDefault(s => s.Id == newSubjectId);
            if (subject == null) {
                return NotFound($"invalid subject id {newSubjectId}");
            }

            if (teacher.Subject != null)
            {
                teacher.Subject.TeacherId = null;
            }

            teacher.Subject = subject;
            subject.TeacherId = teacher.Id;

            dbContext.SaveChanges();

            return Ok(teacher.ToDtoWithSubject());
        }


        /// <summary>
        /// Changes a teacher's rank
        /// </summary>
        /// <param name="teacherId">teacher id</param>
        /// <param name="newRank">new rank</param>
        /// <returns>teacher information</returns>
        [HttpPut("{teacherId}/promote")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TeacherToGet))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult PromoteTeacher([FromRoute][Required][Range(1, int.MaxValue)] int teacherId)
        {
            var teacher = dbContext.Teachers.FirstOrDefault(t => t.Id == teacherId);
            if (teacher == null)
            {
                return NotFound($"invalid teacher id {teacherId}");
            }
            if (teacher.Rank == Rank.Professor)
            {
                return BadRequest("teacher is already a full professor");
            }

            teacher.Rank++;

            dbContext.SaveChanges();

            return Ok(teacher.ToDto());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        [HttpGet("{teacherId}/marks")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MarkWithInfoToGet>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult GetAllMarks([FromRoute]int teacherId) {
            var teacher = 
                dbContext.Teachers
                .Include(t=>t.Subject)
                .ThenInclude(s=>s.Marks)
                .ThenInclude(m=>m.Student)
                .FirstOrDefault(t => t.Id == teacherId);

            if (teacher == null)
            {
                return NotFound($"invalid teacher id {teacherId}");
            }

            if (teacher.Subject == null)
            {
                return NotFound($"teacher {teacherId} has no associated subject");
            }

            var allMarks = teacher.Subject.Marks
                .Select(
                m => new MarkWithInfoToGet 
                {
                    CreationDate = m.CreationDate, 
                    StudentId = m.StudentId, 
                    Value = m.Value 
                }).ToList();


            return Ok(allMarks);
        }

    }

    /*
     * Stergerea unui curs
• Ce alte stergeri implica?

• Stergerea unui teacher
    • Cum tratati stergerea?



• Obtinerea tuturor notelor acordate de catre un
teacher
• Va returna o lista ce va contine DTO-uri continand
valoarea notei, data acordarii precum si id-ul
studentului
     */
}
