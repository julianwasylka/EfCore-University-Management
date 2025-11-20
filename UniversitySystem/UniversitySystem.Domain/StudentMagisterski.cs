namespace UniversitySystem.Domain
{
    public class StudentMagisterski : Student
    {
        public string TematPracyDyplomowej { get; set; } = string.Empty;

        public int? PromotorId { get; set; }
        public Profesor? Promotor { get; set; }
    }
}