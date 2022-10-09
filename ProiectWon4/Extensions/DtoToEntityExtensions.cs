using Data.Models;
using ProiectWon4.DTOs;

namespace ProiectWon4.Extensions
{
    public static class DtoToEntityExtensions
    {
        public static Student ToEntity(this StudentToCreate studentToCreate)
        {
            if (studentToCreate == null)
            {
                return null;
            }

            return new Student
            {
                FirstName = studentToCreate.FirstName,
                LastName = studentToCreate.LastName,
                Age = studentToCreate.Age
            };
        }

        public static Student ToEntity(this StudentToUpdate studentToUpdate)
        {
            if (studentToUpdate == null)
            {
                return null;
            }

            return new Student
            {
                FirstName = studentToUpdate.FirstName,
                LastName = studentToUpdate.LastName,
                Age = studentToUpdate.Age
            };
        }

        public static Address ToEntity(this AddressToUpdate addressToUpdate)
        {
            if (addressToUpdate == null)
            {
                return null;
            }

            return new Address
            {
                City = addressToUpdate.City,
                Street = addressToUpdate.Street,
                Number = addressToUpdate.Number
            };
        }

        public static Teacher ToEntity(this TeacherToCreate teacherToCreate)
        {
            if (teacherToCreate == null)
            {
                return null;
            }
            return new Teacher
            {
                Name = teacherToCreate.Name,
                Rank = teacherToCreate.Rank
            };
        }
    }
}
