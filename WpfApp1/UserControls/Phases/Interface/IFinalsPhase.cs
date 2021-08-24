namespace UserControls.Phases
{
    internal interface IFinalsPhase
    {
        void LoadFields(int idTorneo, int idDisciplina);
        void SaveFields(int idTorneo, int idDisciplina);
        void PrintPools();
        void PrintBracket();
    }
}