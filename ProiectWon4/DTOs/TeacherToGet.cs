using Data.Models;

namespace ProiectWon4.DTOs
{
    public class TeacherToGetFullInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Rank Rank { get; set; }

        public AddressToGet Address { get; set; }
        public SubjectToGet Subject { get; set; }
    }

    public class TeacherWithAddressToGet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Rank Rank { get; set; }

        public AddressToGet Address { get; set; }
    }
    public class TeacherWithSubjectToGet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Rank Rank { get; set; }
        public SubjectToGet Subject { get; set; }
    }
    public class TeacherToGet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Rank Rank { get; set; }
    }
    
}
