namespace UniversitySystem.Application
{
    public interface IReportService
    {
        string PobierzTopProfesora();

        List<KursStatystykaDto> PobierzStatystykiWydzialu(string nazwaWydzialu);

        string ZnajdzStudentaZNajtrudniejszymPlanem();
    }

    public record KursStatystykaDto(string NazwaKursu, double SredniaOcena, int LiczbaOcen);
}