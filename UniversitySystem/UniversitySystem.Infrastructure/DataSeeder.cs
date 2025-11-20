using Bogus;
using UniversitySystem.Application;
using UniversitySystem.Domain;

namespace UniversitySystem.Infrastructure
{
    public class DataSeeder
    {
        private readonly IStudentService _studentService;
        private readonly UniversityDbContext _context;

        public DataSeeder(IStudentService studentService, UniversityDbContext context)
        {
            _studentService = studentService;
            _context = context;
        }

        public void Seed()
        {
            if (_context.Studenci.Any()) return;

            var wydzialy = new List<Wydzial>
            {
                new Wydzial { Nazwa = "ETI" },
                new Wydzial { Nazwa = "WILIŚ" },
                new Wydzial { Nazwa = "WIMIO" }
            };
            _context.Wydzialy.AddRange(wydzialy);
            _context.SaveChanges();

            var fakerKurs = new Faker<Kurs>()
                .RuleFor(k => k.Nazwa, f => f.Company.CatchPhrase())
                .RuleFor(k => k.KodKursu, f => f.Random.AlphaNumeric(5).ToUpper())
                .RuleFor(k => k.PunktyECTS, f => f.Random.Int(2, 8))
                .RuleFor(k => k.Wydzial, f => f.PickRandom(wydzialy));

            var kursy = fakerKurs.Generate(10);
            _context.Kursy.AddRange(kursy);
            _context.SaveChanges();

            var fakerDaneOsobowe = new Faker("pl");

            for (int i = 0; i < 20; i++)
            {
                string imie = fakerDaneOsobowe.Name.FirstName();
                string nazwisko = fakerDaneOsobowe.Name.LastName();
                int rok = fakerDaneOsobowe.Random.Int(1, 5);

                var adres = new Adres(
                    fakerDaneOsobowe.Address.StreetAddress(),
                    fakerDaneOsobowe.Address.City(),
                    fakerDaneOsobowe.Address.ZipCode()
                );

                _studentService.DodajStudenta(imie, nazwisko, rok, adres);
            }
        }
    }
}