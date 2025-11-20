namespace UniversitySystem.Domain
{
    public class Student
    {
        public int Id { get; set; }
        public string Imie { get; set; } = string.Empty;
        public string Nazwisko { get; set; } = string.Empty;
        public string IndeksUczelniany { get; set; } = string.Empty; // np. S1001
        public int RokStudiow { get; set; }

        public Adres AdresZamieszkania { get; set; } = null!; // Owned Entity
        public ICollection<Zapis> Zapisy { get; set; } = new List<Zapis>();
    }
}