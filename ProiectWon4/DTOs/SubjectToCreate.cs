using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ProiectWon4.DTOs
{
    public class SubjectToCreate
    {
        public int TeacherId { get; set; }
        [Required(ErrorMessage ="invalid subject name")]
        public string Name { get; set; }
    }
}
