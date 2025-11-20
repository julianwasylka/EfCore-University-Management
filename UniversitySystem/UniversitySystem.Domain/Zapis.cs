namespace UniversitySystem.Domain
{
    public class Zapis
    {
        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;

        public int KursId { get; set; }
        public Kurs Kurs { get; set; } = null!;

        public double? Ocena { get; set; }
        public int Semestr { get; set; }
    }
}