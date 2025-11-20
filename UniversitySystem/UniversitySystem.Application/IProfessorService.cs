using UniversitySystem.Domain;

namespace UniversitySystem.Application
{
    public interface IProfessorService
    {
        Profesor DodajProfesora(string imie, string nazwisko, string tytul, Adres adres);
        List<Profesor> PobierzWszystkich();
    }
}