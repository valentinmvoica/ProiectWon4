using Data.Models;
using System.ComponentModel.DataAnnotations;

namespace ProiectWon4.DTOs
{
    public class TeacherToCreate
    {
        [Required(AllowEmptyStrings =false)]
        public string Name { get; set; }
        [Required]
        public Rank Rank { get; set; }
    }
}
