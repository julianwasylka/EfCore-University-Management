using Bogus;
using Microsoft.EntityFrameworkCore;
using UniversitySystem.Application;
using UniversitySystem.Domain;

namespace UniversitySystem.Infrastructure
{
    public class DataSeeder
    {
        private readonly IStudentService _studentService;
        private readonly IProfessorService _professorService;
        private readonly UniversityDbContext _context;

        public DataSeeder(IStudentService studentService, IProfessorService professorService, UniversityDbContext context)
        {
            _studentService = studentService;
            _professorService = professorService;
            _context = context;
        }

        public void Seed()
        {
            if (_context.Studenci.Any()) return;

            Console.WriteLine("Generowanie Wydziałów...");
            var wydzialy = new List<Wydzial>
            {
                new Wydzial { Nazwa = "WETI" },
                new Wydzial { Nazwa = "WILIŚ" },
                new Wydzial { Nazwa = "WIMIO" }
            };
            _context.Wydzialy.AddRange(wydzialy);
            _context.SaveChanges();

            Console.WriteLine("Generowanie Profesorów...");
            var profesorowie = new List<Profesor>();
            var fakerProf = new Faker("pl"); 

            for (int i = 0; i < 5; i++)
            {
                var nowyProf = _professorService.DodajProfesora(
                    fakerProf.Name.FirstName(),
                    fakerProf.Name.LastName(),
                    fakerProf.PickRandom("Dr", "Dr hab.", "Prof."),
                    new Adres(fakerProf.Address.StreetAddress(), fakerProf.Address.City(), fakerProf.Address.ZipCode())
                );

                profesorowie.Add(nowyProf);
            }

            Console.WriteLine("Generowanie Kursów...");
            var fakerKurs = new Faker<Kurs>()
                .RuleFor(k => k.Nazwa, f => f.Company.CatchPhrase())
                .RuleFor(k => k.KodKursu, f => f.Random.Replace("??###"))
                .RuleFor(k => k.PunktyECTS, f => f.PickRandom(3, 4, 5, 6, 8))
                .RuleFor(k => k.Wydzial, f => f.PickRandom(wydzialy))
                .RuleFor(k => k.Prowadzacy, f => f.PickRandom(profesorowie));

            var kursy = fakerKurs.Generate(30);

            var trudnyKurs1 = kursy[0];
            trudnyKurs1.Wymagania.Add(kursy[1]);
            trudnyKurs1.Wymagania.Add(kursy[2]);
            trudnyKurs1.Wymagania.Add(kursy[3]);

            var trudnyKurs2 = kursy[5];
            trudnyKurs2.Wymagania.Add(kursy[4]);
            trudnyKurs2.Wymagania.Add(kursy[6]);

            _context.Kursy.AddRange(kursy);
            _context.SaveChanges();


            Console.WriteLine("Generowanie Studentów...");
            var fakerOsoba = new Faker("pl");

            for (int i = 0; i < 15; i++)
            {
                var adres = new Adres(fakerOsoba.Address.StreetAddress(), fakerOsoba.Address.City(), fakerOsoba.Address.ZipCode());
                _studentService.DodajStudenta(fakerOsoba.Name.FirstName(), fakerOsoba.Name.LastName(), fakerOsoba.Random.Int(1, 3), adres);
            }

            Console.WriteLine("Generowanie Magistrantów...");
            for (int i = 0; i < 5; i++)
            {
                var adres = new Adres(fakerOsoba.Address.StreetAddress(), fakerOsoba.Address.City(), fakerOsoba.Address.ZipCode());
                _studentService.DodajMagistranta(
                    fakerOsoba.Name.FirstName(),
                    fakerOsoba.Name.LastName(),
                    fakerOsoba.Random.Int(1, 2),
                    adres,
                    "Narzędzia AI do generowania muzyki",
                    fakerOsoba.PickRandom(profesorowie).Id 
                );
            }


            Console.WriteLine("Generowanie Zapisów...");
            var wszyscyStudenci = _context.Studenci.ToList();

            foreach (var student in wszyscyStudenci)
            {
                var wybraneKursy = fakerOsoba.PickRandom(kursy, fakerOsoba.Random.Int(6, 8)).ToList();

                foreach (var kurs in wybraneKursy)
                {
                    _context.Zapisy.Add(new Zapis
                    {
                        StudentId = student.Id,
                        KursId = kurs.Id,
                        Semestr = 1,
                        Ocena = fakerOsoba.Random.Bool(0.8f) ? fakerOsoba.PickRandom(2.0, 3.0, 3.5, 4.0, 4.5, 5.0) : null
                    });
                }
            }
            _context.SaveChanges();
        }
    }
}