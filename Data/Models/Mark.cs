namespace Data.Models
{
    public class Mark
    {
        public int Id { get; set; }
        public int Value{ get; set; }
        public int SubjectId { get; set; }

        public Student Student { get; set; }
        public int StudentId { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
