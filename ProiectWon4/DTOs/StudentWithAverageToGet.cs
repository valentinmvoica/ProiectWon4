namespace ProiectWon4.DTOs
{
    internal class StudentWithAverageToGet
    {
        public int Id { get; }
        public string Name { get; }
        public int Age { get; }
        public double Average { get; }

        public StudentWithAverageToGet(int id, string name, int age, double average)
        {
            Id = id;
            Name = name;
            Age = age;
            Average = average;
        }
    }
}