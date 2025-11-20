namespace UniversitySystem.Domain
{
    public class Kurs
    {
        public int Id { get; set; }
        public string Nazwa { get; set; } = string.Empty;
        public string KodKursu { get; set; } = string.Empty;
        public int PunktyECTS { get; set; }

        public int WydzialId { get; set; }
        public Wydzial Wydzial { get; set; } = null!;

        public ICollection<Zapis> Zapisy { get; set; } = new List<Zapis>();

        // Self-referencing: Wymagania wstępne
        public ICollection<Kurs> Wymagania { get; set; } = new List<Kurs>();
        public ICollection<Kurs> WymaganePrzez { get; set; } = new List<Kurs>();
    }
}