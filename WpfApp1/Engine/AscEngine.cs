using BusinessEntity.DAO;
using BusinessEntity.Entity;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HEMATournamentSystem.Engine
{
    public static class AscEngine
    {
        private static int _winnerPool = 1;
        private static int _looserPool = 2;
        private static int _maxScoreCap = 5;

        public static void SaveTournamentPool(
            int idTorneo, int idDisciplina, int poolIndex,
            List<int> idAtleti, 
            int numeroIncontriAdPersonam, 
            DataGrid dataGridPool)
        {
            foreach (Int32 atleta in idAtleti)
            {
                RisultatiIncontriGironi res = new RisultatiIncontriGironi();

                res.idAtleta = atleta;

                foreach (MatchEntity match in dataGridPool.Items)
                {
                    bool doppiaMorte = match.DoppiaMorte;

                    if (!SqlDal_Pools.UpdateGironiIncontri(idTorneo, idDisciplina, poolIndex, match.IdRosso, match.PuntiRosso, match.IdBlu, match.PuntiBlu, doppiaMorte))
                    {
                        new MessageBoxCustom("Error during pool savings", MessageType.Error, MessageButtons.Ok);
                        return;
                    }

                    if (match.IdRosso == atleta) //se sono l'atleta a "sinistra" [ROSSO]
                    {
                        if (doppiaMorte)
                        {
                            res.Sconfitte++;
                            res.PuntiSubiti += Math.Abs(_maxScoreCap - match.PuntiRosso);
                        }
                        else
                        {
                            if (match.PuntiRosso > match.PuntiBlu)
                                res.Vittorie++;
                            else if (match.PuntiRosso < match.PuntiBlu)
                                res.Sconfitte++;
                            
                            res.PuntiFatti += match.PuntiRosso;
                            res.PuntiSubiti += match.PuntiBlu;
                        }
                    }
                    else if (match.IdBlu == atleta)   //se sono l'atleta a "destra" [BLU]
                    {
                        if (doppiaMorte)
                        {
                            res.Sconfitte++;
                            res.PuntiSubiti += Math.Abs(_maxScoreCap - match.PuntiBlu);
                        }
                        else
                        {
                            if (match.PuntiBlu > match.PuntiRosso)
                                res.Vittorie++;
                            else if (match.PuntiBlu < match.PuntiRosso)
                                res.Sconfitte++;
                            
                            res.PuntiFatti += match.PuntiBlu;
                            res.PuntiSubiti += match.PuntiRosso;
                        }
                    }
                }

                int delpaP = res.PuntiFatti - res.PuntiSubiti;
                res.NumeroIncontriDisputati = numeroIncontriAdPersonam;

                res.Differenziale = (Double)(delpaP + res.Vittorie) / res.NumeroIncontriDisputati;

                //salvare res in Gironi:
                //per ogni atleta , torneo e disciplina salvo i punti fatti, subiti, le vittorie ed il differenziale
                SqlDal_Pools.UpdateGironi(res, idTorneo, idDisciplina, poolIndex);
            }
        }

        public static void SaveFinal16Pool(int idTorneo, int idDisciplina, int pool, DataGrid dataGridPool)
        {
            int posizione = 1;

            List<AtletaEliminatorie> listAtleti = new List<AtletaEliminatorie>();

            foreach (MatchEntity match in dataGridPool.Items)
            {
                AtletaEliminatorie a = new AtletaEliminatorie();

                a.IdAtleta = (match.PuntiRosso > match.PuntiBlu) ? match.IdRosso : match.IdBlu;

                SqlDal_Pools.UpdateQualificati32(idTorneo, idDisciplina, pool, posizione, match.IdRosso, match.PuntiRosso, match.PuntiBlu);
                SqlDal_Pools.UpdateQualificati32(idTorneo, idDisciplina, pool, posizione, match.IdBlu, match.PuntiBlu, match.PuntiRosso);

                a.IdTorneo = idTorneo;
                a.idDisciplina = idDisciplina;
                a.Posizione = posizione;
                a.Campo = pool;
                a.PuntiFatti = 0;
                a.PuntiSubiti = 0;

                listAtleti.Add(a);

                posizione++;
            }

            SqlDal_Pools.InsertOttavi(listAtleti);
        }

        public static void SaveFinal8Pool(int idTorneo, int idDisciplina, int pool, DataGrid dataGridPool)
        {
            int posizione = 1;

            List<AtletaEliminatorie> listAtleti = new List<AtletaEliminatorie>();

            foreach (MatchEntity match in dataGridPool.Items)
            {
                AtletaEliminatorie a = new AtletaEliminatorie();

                a.IdAtleta = (match.PuntiRosso > match.PuntiBlu) ? match.IdRosso : match.IdBlu;

                SqlDal_Pools.UpdateQualificati16(idTorneo, idDisciplina, pool, posizione, match.IdRosso, match.PuntiRosso, match.PuntiBlu);
                SqlDal_Pools.UpdateQualificati16(idTorneo, idDisciplina, pool, posizione, match.IdBlu, match.PuntiBlu, match.PuntiRosso);

                a.IdTorneo = idTorneo;
                a.idDisciplina = idDisciplina;
                a.Posizione = posizione;
                a.Campo = pool;
                a.PuntiFatti = 0;
                a.PuntiSubiti = 0;

                listAtleti.Add(a);

                posizione++;
            }

            SqlDal_Pools.InsertQuarti(listAtleti);
        }

        public static void SaveFinal4Pool(int idTorneo, int idDisciplina, int pool, int semifinalPool, DataGrid dataGridPool)
        {
            int posizione = 1;

            List<AtletaEliminatorie> listAtleti = new List<AtletaEliminatorie>();

            foreach (MatchEntity match in dataGridPool.Items)
            {
                AtletaEliminatorie a = new AtletaEliminatorie();

                a.IdAtleta = (match.PuntiRosso > match.PuntiBlu) ? match.IdRosso : match.IdBlu;

                SqlDal_Pools.UpdateQualificati8(idTorneo, idDisciplina, pool, posizione, match.IdRosso, match.PuntiRosso, match.PuntiBlu);
                SqlDal_Pools.UpdateQualificati8(idTorneo, idDisciplina, pool, posizione, match.IdBlu, match.PuntiBlu, match.PuntiRosso);

                a.IdTorneo = idTorneo;
                a.idDisciplina = idDisciplina;
                a.Posizione = posizione;
                a.Campo = semifinalPool;
                a.PuntiFatti = 0;
                a.PuntiSubiti = 0;

                listAtleti.Add(a);

                posizione++;
            }

            SqlDal_Pools.InsertSemifinali(listAtleti);
        }

        public static void SaveSemiFinalPool(int idTorneo, int idDisciplina, int pool, DataGrid dataGridPool)
        {
            int posizione = 1;

            List<AtletaEliminatorie> listAtleti = new List<AtletaEliminatorie>();

            foreach (MatchEntity match in dataGridPool.Items)
            {
                AtletaEliminatorie winner = new AtletaEliminatorie();
                AtletaEliminatorie looser = new AtletaEliminatorie();

                winner.IdAtleta = (match.PuntiRosso > match.PuntiBlu) ? match.IdRosso : match.IdBlu;
                looser.IdAtleta = (match.PuntiRosso > match.PuntiBlu) ? match.IdBlu : match.IdRosso;

                //DeleteOldValues(pool, idTorneo, idDisciplina, match.IdRosso);
                //DeleteOldValues(pool, idTorneo, idDisciplina, match.IdBlu);

                SqlDal_Pools.UpdateSemifinali(idTorneo, idDisciplina, pool, posizione, match.IdRosso, match.PuntiRosso, match.PuntiBlu);
                SqlDal_Pools.UpdateSemifinali(idTorneo, idDisciplina, pool, posizione, match.IdBlu, match.PuntiBlu, match.PuntiRosso);

                CreateFinalsRecords(idTorneo, idDisciplina, posizione, listAtleti, winner, looser);
            }

            SqlDal_Pools.InsertFinali(listAtleti);
        }

        public static Tuple<string, string> SaveFinalPool(int idTorneo, int idDisciplina, int pool, DataGrid dataGridPool, bool thirdsPlaces)
        {
            List<AtletaEliminatorie> listAtleti = new List<AtletaEliminatorie>();

            int round = 1;

            int vittorieRosso = 0;
            int vittorieBlu = 0;
            string winner = "";
            string looser = "";

            foreach (MatchEntity match in dataGridPool.Items)
            {
                if (match.PuntiRosso > match.PuntiBlu)
                    vittorieRosso++;
                else if (match.PuntiRosso < match.PuntiBlu)
                    vittorieBlu++;

                SqlDal_Pools.UpdateFinali(idTorneo, idDisciplina, pool, round, match.IdRosso, match.PuntiRosso, match.PuntiBlu);
                SqlDal_Pools.UpdateFinali(idTorneo, idDisciplina, pool, round, match.IdBlu, match.PuntiBlu, match.PuntiRosso);

                round++;

            }

            var m = ((MatchEntity)dataGridPool.Items[0]);

            if (vittorieRosso > vittorieBlu)
            {
                winner = SqlDal_Fighters.GetAtletaById(m.IdRosso).Asd + " - " + m.CognomeRosso + " " + m.NomeRosso;
                looser = SqlDal_Fighters.GetAtletaById(m.IdBlu).Asd + " - " + m.CognomeBlu + " " + m.NomeBlu;
            }
            else if (vittorieBlu > vittorieRosso)
            {
                looser = SqlDal_Fighters.GetAtletaById(m.IdRosso).Asd + " - " + m.CognomeRosso + " " + m.NomeRosso;
                winner = SqlDal_Fighters.GetAtletaById(m.IdBlu).Asd + " - " + m.CognomeBlu + " " + m.NomeBlu;
            }
            else if (thirdsPlaces)//gestione terzo e quarto posto parimenti
            {
                looser = SqlDal_Fighters.GetAtletaById(m.IdRosso).Asd + " - " + m.CognomeRosso + " " + m.NomeRosso;
                winner = SqlDal_Fighters.GetAtletaById(m.IdBlu).Asd + " - " + m.CognomeBlu + " " + m.NomeBlu;
            }
            else
            {
                looser = "";
                winner = "";
            }

            return new Tuple<string, string>(winner, looser);

        }

        private static void DeleteOldValues(int pool, int idTorneo, int idDisciplina, int fighterId)
        {
            SqlDal_Pools.EliminaSemifinaliByCampo(pool, idTorneo, idDisciplina, fighterId);
        }

        private static void CreateFinalsRecords(int idTorneo, int idDisciplina, int posizione, List<AtletaEliminatorie> listAtleti, AtletaEliminatorie winner, AtletaEliminatorie looser)
        {
            winner.IdTorneo = idTorneo;
            winner.idDisciplina = idDisciplina;
            winner.Posizione = posizione;
            winner.Campo = _winnerPool;
            winner.PuntiFatti = 0;
            winner.PuntiSubiti = 0;

            listAtleti.Add(winner);

            looser.IdTorneo = idTorneo;
            looser.idDisciplina = idDisciplina;
            looser.Posizione = posizione;
            looser.Campo = _looserPool;
            looser.PuntiFatti = 0;
            looser.PuntiSubiti = 0;

            listAtleti.Add(looser);
        }

    }
}
