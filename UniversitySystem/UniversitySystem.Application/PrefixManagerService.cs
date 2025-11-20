namespace UniversitySystem.Application
{
    public class PrefixManagerService : IPrefixManagerService
    {
        private readonly IUniversityDbContext _context;

        public PrefixManagerService(IUniversityDbContext context)
        {
            _context = context;
        }

        public void DodajLubZaktualizujLicznik(string prefix, int wartoscPoczatkowa)
        {
            var licznik = _context.LicznikiIndeksow.FirstOrDefault(l => l.Prefix == prefix);
            if (licznik == null)
            {
                licznik = new UniversitySystem.Domain.LicznikIndeksow
                {
                    Prefix = prefix,
                    AktualnaWartosc = wartoscPoczatkowa
                };
                _context.LicznikiIndeksow.Add(licznik);
            }
            else
            {
                licznik.AktualnaWartosc = wartoscPoczatkowa;
            }
            _context.SaveChanges();
        }

        public UniversitySystem.Domain.LicznikIndeksow? PobierzLicznik(string prefix)
        {
            return _context.LicznikiIndeksow.FirstOrDefault(l => l.Prefix == prefix);
        }

        public List<UniversitySystem.Domain.LicznikIndeksow> PobierzWszystkie()
        {
            return _context.LicznikiIndeksow.ToList();
        }
    }
}