using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UniversitySystem.Application;
using UniversitySystem.Infrastructure;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
    logging.SetMinimumLevel(LogLevel.Error);
});

builder.ConfigureServices((context, services) =>
{
    var connectionString = context.Configuration.GetConnectionString("DefaultConnection");

    services.AddDbContext<UniversityDbContext>(options =>
        options.UseSqlServer(connectionString));

    services.AddScoped<IUniversityDbContext>(provider =>
        provider.GetRequiredService<UniversityDbContext>());

    services.AddScoped<IStudentService, StudentService>();
    services.AddScoped<IReportService, ReportService>(); 
    services.AddScoped<IPrefixManagerService, PrefixManagerService>();
    services.AddScoped<IProfessorService, ProfessorService>();
    services.AddTransient<DataSeeder>();
});

var host = builder.Build();

using (var scope = host.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var dbContext = services.GetRequiredService<UniversityDbContext>();
        dbContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        Console.WriteLine("Błąd migracji: " + ex.Message);
    }

    var seeder = services.GetRequiredService<DataSeeder>();
    var studentService = services.GetRequiredService<IStudentService>();
    var reportService = services.GetRequiredService<IReportService>();

    while (true)
    {
        Console.Clear();
        Console.WriteLine("=== SYSTEM UCZELNIANY ===");
        Console.WriteLine("1. Generuj dane");
        Console.WriteLine("2. Wyświetl wszystkich studentów");
        Console.WriteLine("3. Dodaj studenta");
        Console.WriteLine("4. Usuń studenta");
        Console.WriteLine("5. Raporty");
        Console.WriteLine("0. Wyjście");
        Console.Write("Wybór: ");

        var wybor = Console.ReadLine();

        switch (wybor)
        {
            case "1":
                Console.WriteLine("Generowanie danych...");
                seeder.Seed();
                Console.WriteLine("Gotowe");
                break;
            case "2":
                var studenci = studentService.PobierzWszystkich();
                foreach (var s in studenci)
                {
                    Console.WriteLine($"{s.IndeksUczelniany}: {s.Imie} {s.Nazwisko} ({s.AdresZamieszkania})");
                }
                break;
            case "3":
                Console.Write("Nazwisko: ");
                var n = Console.ReadLine();
                try
                {
                    var nowy = studentService.DodajStudenta("Test", n, 1, new UniversitySystem.Domain.Adres("Ulica", "Miasto", "00-000"));
                    Console.WriteLine($"Dodano: {nowy.IndeksUczelniany}");
                }
                catch (Exception ex) { Console.WriteLine("Błąd: " + ex.Message); }
                break;
            case "4":
                Console.Write("Indeks studenta do usunięcia (np. S1005): ");
                var indeksDoUsuniecia = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(indeksDoUsuniecia))
                {
                    try
                    {
                        studentService.UsunStudenta(indeksDoUsuniecia);
                        Console.WriteLine("Operacja zakończona.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Błąd: " + ex.Message);
                    }
                }
                break;
            case "5":
                Console.WriteLine("\n--- TOP PROFESOR ---");
                Console.WriteLine(reportService.PobierzTopProfesora());

                Console.WriteLine("\n--- GPA WETI ---");
                var statystyki = reportService.PobierzStatystykiWydzialu("WETI");
                foreach (var st in statystyki) Console.WriteLine($"{st.NazwaKursu}: GPA {st.SredniaOcena:F2} ({st.LiczbaOcen} ocen)");

                Console.WriteLine("\n--- NAJTRUDNIEJSZY PLAN ---");
                Console.WriteLine(reportService.ZnajdzStudentaZNajtrudniejszymPlanem());
                break;
            case "0":
                return;
        }
        Console.WriteLine("\nNaciśnij dowolny klawisz...");
        Console.ReadKey();
    }
}