using Data.Models;
using ProiectWon4.DTOs;
using System.Runtime.CompilerServices;

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

        public static SubjectToGet ToDto(this Subject subject)
        {
            if (subject == null)
                return null;

            return new SubjectToGet
            {
                Id = subject.Id,
                Name = subject.Name,
                TeacherId = subject.TeacherId
            };
        }

        public static MarkToGet ToDto(this Mark mark)
        {
            if (mark == null)
            {
                return null;
            }
            return new MarkToGet
            {
                Id = mark.Id,
                CreationDate = mark.CreationDate,
                StudentId = mark.StudentId,
                SubjectId = mark.SubjectId,
                Value = mark.Value
            };
        }
    }

}
