using Data.Models;
using ProiectWon4.DTOs;

namespace ProiectWon4.Extensions
{
    public static class EntityToDtoExtensions
    {       
        public static StudentToGet ToDto(this Student student)
        {
            if (student == null)
            {
                return null;
            }
            StudentToGet dto = new StudentToGet();
            dto.Id = student.Id;
            dto.FirstName = student.FirstName;
            dto.LastName = student.LastName;
            dto.Age = student.Age;
            dto.Address = student.Address.ToDto();

            return dto;
        }
        public static AddressToGet ToDto(this Address address)
        {
            if (address == null)
            {
                return null;
            }

            return new AddressToGet
            {
                City = address.City,
                Street = address.Street,
                Number = address.Number
            };
        }
    }
}
