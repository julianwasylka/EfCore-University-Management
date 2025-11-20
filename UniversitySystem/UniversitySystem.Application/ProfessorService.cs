using Microsoft.EntityFrameworkCore;
using UniversitySystem.Domain;

namespace UniversitySystem.Application
{
    public class ProfessorService : IProfessorService
    {
        private readonly IUniversityDbContext _context;
        private const string PREFIX_PROFESORA = "P";

        public ProfessorService(IUniversityDbContext context)
        {
            _context = context;
        }

        public Profesor DodajProfesora(string imie, string nazwisko, string tytul, Adres adres)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var licznik = _context.LicznikiIndeksow.FirstOrDefault(x => x.Prefix == PREFIX_PROFESORA);

                if (licznik == null)
                {
                    licznik = new LicznikIndeksow { Prefix = PREFIX_PROFESORA, AktualnaWartosc = 100 };
                    _context.LicznikiIndeksow.Add(licznik);
                }

                licznik.AktualnaWartosc++;
                var nowyIndeks = $"{PREFIX_PROFESORA}{licznik.AktualnaWartosc}";

                var profesor = new Profesor
                {
                    Imie = imie,
                    Nazwisko = nazwisko,
                    TytulNaukowy = tytul,
                    AdresZamieszkania = adres,
                    IndeksUczelniany = nowyIndeks
                };

                _context.Profesorowie.Add(profesor);

                _context.SaveChanges();
                transaction.Commit();

                return profesor;
            }
            catch
            {
                throw;
            }
        }

        public List<Profesor> PobierzWszystkich()
        {
            return _context.Profesorowie
                .AsNoTracking()
                .Include(p => p.ProwadzoneKursy) 
                .ToList();
        }
    }
}