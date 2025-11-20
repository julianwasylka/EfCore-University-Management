using UniversitySystem.Domain;

namespace UniversitySystem.Application
{
    public interface IStudentService
    {
        Student DodajStudenta(string imie, string nazwisko, int rok, Adres adres);

        StudentMagisterski DodajMagistranta(string imie, string nazwisko, int rok, Adres adres, string temat, int promotorId);

        void UsunStudenta(string indeks);

        List<Student> PobierzWszystkich();
    }
}