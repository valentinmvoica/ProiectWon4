using Data.Models;
using ProiectWon4.DTOs;

namespace ProiectWon4.Extensions
{
    public static class DtoToEntityExtensions
    {
        public static Student ToEntity(this StudentToCreate studentToCreate) =>
     new Student
     {
         FirstName = studentToCreate.FirstName,
         LastName = studentToCreate.LastName,
         Age = studentToCreate.Age
     };
        public static Student ToEntity(this StudentToUpdate studentToCreate) =>
           new Student
           {
               FirstName = studentToCreate.FirstName,
               LastName = studentToCreate.LastName,
               Age = studentToCreate.Age
           };
        public static Address ToEntity(this AddressToUpdate addressToUpdate) =>
            new Address
            {
                City = addressToUpdate.City,
                Street = addressToUpdate.Street,
                Number = addressToUpdate.Number
            };
    }
}
