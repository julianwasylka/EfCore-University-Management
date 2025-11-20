using Microsoft.EntityFrameworkCore;
using UniversitySystem.Application;
using UniversitySystem.Domain;

namespace UniversitySystem.Infrastructure
{
    public class UniversityDbContext : DbContext, IUniversityDbContext
    {
        public UniversityDbContext(DbContextOptions<UniversityDbContext> options) : base(options)
        {
        }

        // Tabele
        public DbSet<Student> Studenci { get; set; }
        public DbSet<Profesor> Profesorowie { get; set; }
        public DbSet<Kurs> Kursy { get; set; }
        public DbSet<Wydzial> Wydzialy { get; set; }
        public DbSet<Gabinet> Gabinety { get; set; }
        public DbSet<LicznikIndeksow> LicznikiIndeksow { get; set; }
        public DbSet<Zapis> Zapisy { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LicznikIndeksow>(entity =>
            {
                entity.HasKey(e => e.Prefix); 
                entity.Property(e => e.Prefix).HasMaxLength(10);
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasIndex(e => e.IndeksUczelniany).IsUnique();

                entity.OwnsOne(e => e.AdresZamieszkania);

                // TPH domyślnie
            });

            modelBuilder.Entity<StudentStudiowMagisterskich>(entity =>
            {
                entity.HasOne(e => e.Promotor)
                      .WithMany(p => p.Seminarzysci)
                      .HasForeignKey(e => e.PromotorId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Profesor>(entity =>
            {
                entity.HasIndex(e => e.IndeksUczelniany).IsUnique(); 
                entity.OwnsOne(e => e.AdresZamieszkania);
            });

            modelBuilder.Entity<Profesor>()
                .HasOne(p => p.Gabinet)
                .WithOne(g => g.Profesor)
                .HasForeignKey<Gabinet>(g => g.ProfesorId);

            modelBuilder.Entity<Zapis>(entity =>
            {
                entity.HasKey(e => new { e.StudentId, e.KursId });

                entity.HasOne(e => e.Student)
                      .WithMany(s => s.Zapisy)
                      .HasForeignKey(e => e.StudentId);

                entity.HasOne(e => e.Kurs)
                      .WithMany(k => k.Zapisy)
                      .HasForeignKey(e => e.KursId);
            });

            modelBuilder.Entity<Kurs>()
                .HasMany(k => k.Wymagania)
                .WithMany(k => k.WymaganePrzez)
                .UsingEntity(j => j.ToTable("WymaganiaKursow")); 
        }
    }
}