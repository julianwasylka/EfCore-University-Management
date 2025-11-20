using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using UniversitySystem.Domain;

namespace UniversitySystem.Application
{
    public interface IUniversityDbContext
    {
        DbSet<Student> Studenci { get; }
        DbSet<Profesor> Profesorowie { get; }
        DbSet<Kurs> Kursy { get; }
        DbSet<Wydzial> Wydzialy { get; }
        DbSet<LicznikIndeksow> LicznikiIndeksow { get; }
        DbSet<Zapis> Zapisy { get; }

        DatabaseFacade Database { get; }

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}