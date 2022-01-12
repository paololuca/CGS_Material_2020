namespace HEMATournamentSystem
{
    public interface ITournamentReportByDiscipline
    {
        void GenerateExcel(string tournamentName, string disciplineName, string path);
    }
}