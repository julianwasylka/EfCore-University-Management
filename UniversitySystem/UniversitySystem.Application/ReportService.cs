using Microsoft.EntityFrameworkCore;

namespace UniversitySystem.Application
{
    public class ReportService : IReportService
    {
        private readonly IUniversityDbContext _context;

        public ReportService(IUniversityDbContext context)
        {
            _context = context;
        }

        public string PobierzTopProfesora()
        {
            var topProfesor = _context.Profesorowie
                .AsNoTracking()
                .Select(p => new
                {
                    ImieNazwisko = p.Imie + " " + p.Nazwisko,
                    LiczbaStudentow = p.Seminarzysci.Count()
                })
                .OrderByDescending(x => x.LiczbaStudentow)
                .FirstOrDefault();

            if (topProfesor == null) return "Brak danych.";
            return $"{topProfesor.ImieNazwisko} (Seminarzystów: {topProfesor.LiczbaStudentow})";
        }

        public List<KursStatystykaDto> PobierzStatystykiWydzialu(string nazwaWydzialu)
        {
            return _context.Kursy
                .AsNoTracking()
                .Where(k => k.Wydzial.Nazwa == nazwaWydzialu)
                .Select(k => new KursStatystykaDto(
                    k.Nazwa,
                    k.Zapisy.Where(z => z.Ocena.HasValue).Average(z => z.Ocena.Value),
                    k.Zapisy.Count(z => z.Ocena.HasValue)
                ))
                .ToList();
        }

        public string ZnajdzStudentaZNajtrudniejszymPlanem()
        {
            var studentStats = _context.Studenci
                .AsNoTracking()
                .Select(s => new
                {
                    Indeks = s.IndeksUczelniany,
                    DirectEcts = s.Zapisy.Sum(z => z.Kurs.PunktyECTS),
                    PrereqEcts = s.Zapisy
                        .SelectMany(z => z.Kurs.Wymagania)
                        .Distinct()
                        .Sum(req => req.PunktyECTS)
                })
                .OrderByDescending(x => x.DirectEcts + x.PrereqEcts)
                .FirstOrDefault();

            if (studentStats == null) return "Brak studentów.";

            return $"Student {studentStats.Indeks}: Obciążenie {studentStats.DirectEcts + studentStats.PrereqEcts} ECTS " +
                   $"(Bezpośrednie: {studentStats.DirectEcts}, Prerekwizyty: {studentStats.PrereqEcts})";
        }
    }
}