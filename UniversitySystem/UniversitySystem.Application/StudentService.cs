using Microsoft.EntityFrameworkCore;
using UniversitySystem.Domain;

namespace UniversitySystem.Application
{
    public class StudentService : IStudentService
    {
        private readonly IUniversityDbContext _context;
        private const string PREFIX_STUDENTA = "S";

        public StudentService(IUniversityDbContext context)
        {
            _context = context;
        }

        public Student DodajStudenta(string imie, string nazwisko, int rok, Adres adres)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var licznik = _context.LicznikiIndeksow
                    .FirstOrDefault(x => x.Prefix == PREFIX_STUDENTA);

                if (licznik == null)
                {
                    licznik = new LicznikIndeksow { Prefix = PREFIX_STUDENTA, AktualnaWartosc = 0 };
                    _context.LicznikiIndeksow.Add(licznik);
                }

                if (licznik.AktualnaWartosc == 0) licznik.AktualnaWartosc = 1000;

                licznik.AktualnaWartosc++;
                var nowyIndeks = $"{PREFIX_STUDENTA}{licznik.AktualnaWartosc}";

                var student = new Student
                {
                    Imie = imie,
                    Nazwisko = nazwisko,
                    RokStudiow = rok,
                    AdresZamieszkania = adres,
                    IndeksUczelniany = nowyIndeks
                };

                _context.Studenci.Add(student);

                _context.SaveChanges();
                transaction.Commit();

                return student;
            }
            catch
            {
                throw;
            }
        }

        public void UsunStudenta(string indeks)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var student = _context.Studenci.FirstOrDefault(s => s.IndeksUczelniany == indeks);
                if (student == null)
                {
                    Console.WriteLine($"Nie znaleziono studenta o indeksie {indeks}");
                    return;
                }

                var numerCzesc = student.IndeksUczelniany.Substring(1);
                if (int.TryParse(numerCzesc, out int numerIndeksu))
                {
                    var licznik = _context.LicznikiIndeksow
                        .FirstOrDefault(x => x.Prefix == PREFIX_STUDENTA);

                    if (licznik != null && numerIndeksu == licznik.AktualnaWartosc)
                    {
                        licznik.AktualnaWartosc--;
                    }
                }

                _context.Studenci.Remove(student);

                _context.SaveChanges();
                transaction.Commit();
            }
            catch
            {
                throw;
            }
        }

        public List<Student> PobierzWszystkich()
        {
            return _context.Studenci
                .AsNoTracking()
                .Include(s => s.Zapisy)
                .ToList();
        }
        public StudentMagisterski DodajMagistranta(string imie, string nazwisko, int rok, Adres adres, string temat, int promotorId)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var licznik = _context.LicznikiIndeksow.FirstOrDefault(x => x.Prefix == PREFIX_STUDENTA);

                if (licznik == null)
                {
                    licznik = new LicznikIndeksow { Prefix = PREFIX_STUDENTA, AktualnaWartosc = 0 };
                    _context.LicznikiIndeksow.Add(licznik);
                }
                if (licznik.AktualnaWartosc == 0) licznik.AktualnaWartosc = 1000;

                licznik.AktualnaWartosc++;
                var nowyIndeks = $"{PREFIX_STUDENTA}{licznik.AktualnaWartosc}";

                var magistrant = new StudentMagisterski
                {
                    Imie = imie,
                    Nazwisko = nazwisko,
                    RokStudiow = rok,
                    AdresZamieszkania = adres,
                    IndeksUczelniany = nowyIndeks,
                    TematPracyDyplomowej = temat,
                    PromotorId = promotorId
                };

                _context.Studenci.Add(magistrant);

                _context.SaveChanges();
                transaction.Commit();

                return magistrant;
            }
            catch
            {
                throw;
            }
        }
    }
}