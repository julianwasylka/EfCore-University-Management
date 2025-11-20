using UniversitySystem.Domain;

namespace UniversitySystem.Application
{
    public interface IPrefixManagerService
    {
        void DodajLubZaktualizujLicznik(string prefix, int wartoscPoczatkowa);
        LicznikIndeksow? PobierzLicznik(string prefix);
        List<LicznikIndeksow> PobierzWszystkie();
    }
}