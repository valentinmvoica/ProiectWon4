using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Mark
    {
        public int Id { get; set; }
        public int Value{ get; set; }
        public int SubjectId { get; set; }
        public int StudentId { get; set; }
        public DateTime CreationDate { get; set; }
    }

}
