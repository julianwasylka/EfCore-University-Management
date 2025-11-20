using UniversitySystem.Domain;

namespace UniversitySystem.Application
{
    public interface IStudentService
    {
        Student DodajStudenta(string imie, string nazwisko, int rok, Adres adres);

        void UsunStudenta(string indeks);

        List<Student> PobierzWszystkich();
    }
}