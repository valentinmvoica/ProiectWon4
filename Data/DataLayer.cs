using Data.Exceptions;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class DataLayer
    {
        private CatalogueDbContext ctx;
        public DataLayer(CatalogueDbContext context)
        {
            this.ctx = context;
        }
        public List<Student> GetAllStudents() =>
         ctx.Students.Include(s => s.Address).ToList();
        public Student GetStudentById(int studentId) => 
            ctx.Students.Where(s => s.Id == studentId).FirstOrDefault();
        public Student CreateStudent(Student studentToCreate)
        {
            var student = new Student { FirstName = studentToCreate.FirstName, LastName = studentToCreate.LastName, Age = studentToCreate.Age };

            ctx.Add(student);
            ctx.SaveChanges();
            return student;
        }
        public void DeleteStudent(int studentId, bool deleteAddress)
        {
            var student = ctx.Students
                .Include(student => student.Address)
                .Where(s => s.Id == studentId)
                .FirstOrDefault();

            if (student == null)
            {
                return;
            }

            if (!deleteAddress)
            {
                if (student.Address != null)
                {
                    student.Address.StudentId = null;
                    student.Address = null;
                }
            }
            else
            {
                if (student.Address != null)
                {
                    ctx.Remove(student.Address);
                }
            }

            ctx.Remove(student);
            ctx.SaveChanges();
        }
        public void ChangeStudentData(int studentId, Student newStudentData)
        {
            var student = ctx.Students.FirstOrDefault(s => s.Id == studentId);

            if (student == null)
            {
                throw new EntityNotFoundException($"A student with an id of {studentId} was not found");
            }

            student.FirstName = newStudentData.FirstName;
            student.LastName = newStudentData.LastName;
            student.Age = newStudentData.Age;

            ctx.SaveChanges();
        }
        public void ChangeStudentAddress(int studentId, Address newAddress)
        {
            var student = ctx.Students.Include(s=>s.Address).FirstOrDefault(s => s.Id == studentId);

            if (student == null)
            {
                throw new EntityNotFoundException($"A student with an id of {studentId} was not found");
            }

            if (student.Address==null)
            {
                student.Address = new Address();
            }

            student.Address.City = newAddress.City;
            student.Address.Street = newAddress.Street;
            student.Address.Number = newAddress.Number;

            ctx.SaveChanges();
        }
    }
}
