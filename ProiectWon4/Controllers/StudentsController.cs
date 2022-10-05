using Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProiectWon4.DTOs;
using ProiectWon4.Extensions;

namespace ProiectWon4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly DataLayer dataLayer;

        public StudentsController(DataLayer dataLayer)
        {
            this.dataLayer = dataLayer;
        }

        /// <summary>
        /// Retrieves all students with their addresses
        /// </summary>
        /// <returns>Students list</returns>
        [HttpGet("all")]
        public List<StudentToGet> GetAllStudents() =>
            dataLayer.GetAllStudents().Select(s => s.ToDto()).ToList();

        /// <summary>
        /// Retrieves a student based on its corresponding id
        /// </summary>
        /// <param name="studentId">student id to get</param>
        /// <returns>studednt data</returns>
        [HttpGet("{studentId}")]
        public StudentToGet GetAllStudents([FromRoute] int studentId) =>
            dataLayer.GetStudentById(studentId).ToDto();

        /// <summary>
        /// Creates a student based on the provided data
        /// </summary>
        /// <param name="studentToCreate">Student data</param>
        /// <returns>Created student data</returns>
        [HttpPost]
        public StudentToGet CreateStudent([FromBody] StudentToCreate studentToCreate) =>
            dataLayer.CreateStudent(studentToCreate.ToEntity()).ToDto();


        /// <summary>
        /// Deletes a student based on an id
        /// </summary>
        /// <param name="studentId">student id</param>
        /// <param name="deleteAddress">if true the student address will be deleted as well</param>
        [HttpDelete("{studentId}")]
        public void DeleteStudent([FromRoute] int studentId, [FromQuery] bool deleteAddress) =>
            dataLayer.DeleteStudent(studentId, deleteAddress);


        /// <summary>
        /// Updates a student's data
        /// </summary>
        /// <param name="studentId">the id of the student to modify</param>
        /// <param name="newStudentData">new student information</param>
        [HttpPut]
        public void UpdateStudent([FromRoute] int studentId, [FromBody] StudentToUpdate newStudentData) =>
            dataLayer.ChangeStudentData(studentId, newStudentData.ToEntity());


        /// <summary>
        /// Updates or creates a student's address information
        /// </summary>
        /// <param name="studentId">student id</param>
        /// <param name="newAddress">new address</param>
        [HttpPut("{studentId}/address")]
        public void ChangeStudentAddress([FromRoute] int studentId, [FromBody] AddressToUpdate newAddress) =>
            dataLayer.ChangeStudentAddress(studentId, newAddress.ToEntity());

    }
}
