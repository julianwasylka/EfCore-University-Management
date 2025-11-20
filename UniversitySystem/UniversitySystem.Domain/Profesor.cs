namespace UniversitySystem.Domain
{
    public class Profesor
    {
        public int Id { get; set; }
        public string Imie { get; set; } = string.Empty;
        public string Nazwisko { get; set; } = string.Empty;
        public string TytulNaukowy { get; set; } = string.Empty;
        public string IndeksUczelniany { get; set; } = string.Empty; // np. P101

        public Adres AdresZamieszkania { get; set; } = null!; // Owned Entity
        public Gabinet? Gabinet { get; set; }
        public ICollection<StudentStudiowMagisterskich> Seminarzysci { get; set; } = new List<StudentStudiowMagisterskich>();
    }
}