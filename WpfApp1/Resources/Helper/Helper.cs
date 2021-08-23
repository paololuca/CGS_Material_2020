using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using System.Configuration;
using BusinessEntity.Entity;
using BusinessEntity.DAO;
using Report;

namespace Resources
{
    static class Helper
    {
        public const String MASCHILE = "MASCHILE";
        public const String FEMMINILE = "FEMMINILE";
        public const String TEST = "TEST";


        public static string GetConnectionString()
        {
            if (GetDbType() == TEST)
                return ConfigurationManager.AppSettings["SqlDBTest"];
            else if (GetDbType() == FEMMINILE)
                return ConfigurationManager.AppSettings["SqlDBFemale"];
            else
                return ConfigurationManager.AppSettings["SqlDB"];
        }

        

        ///TODO per ora funziona solo testuale, poi i gironi vanno fisicamente calcolati sull'oggetto GIRONE
        ///(cioè su una lista di persone)
        ///il risutlato sarà una lista di incontri

        /// <summary>
        /// [T4]
        ///    0	1			
        ///    2	3			
        ///    1	2			
        ///    0	3			
        ///    0	2			
        ///    1	3
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static String ElaborateT4(TreeNodeCollection t)
        {
            string incontri = "";

            foreach (TreeNode n in t)
                incontri += n.Text + " \r\n";

            incontri += " \r\n \r\n";

            incontri += t[0].Text + "\t\t" + t[1].Text + " \r\n";
            incontri += t[2].Text + "\t\t" + t[3].Text + " \r\n";
            incontri += t[1].Text + "\t\t" + t[2].Text + " \r\n";
            incontri += t[0].Text + "\t\t" + t[3].Text + " \r\n";
            incontri += t[0].Text + "\t\t" + t[2].Text + " \r\n";
            incontri += t[1].Text + "\t\t" + t[3].Text + " \r\n";

            return incontri;
        }

        /// <summary>
        /// [T5]
        /// 0	1			
        /// 2	3			
        /// 4	0			
        /// 1	2			
        /// 3	4			
        /// 0	2			
        /// 4	1			
        /// 0	3			
        /// 4	2			
        /// 1	3
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static String ElaborateT5(TreeNodeCollection t)
        {
            string incontri = "";

            foreach (TreeNode n in t)
                incontri += n.Text + " \r\n";

            incontri += " \r\n \r\n";

            incontri += t[0].Text + "\t\t" + t[1].Text + " \r\n";
            incontri += t[2].Text + "\t\t" + t[3].Text + " \r\n";
            incontri += t[4].Text + "\t\t" + t[0].Text + " \r\n";
            incontri += t[1].Text + "\t\t" + t[2].Text + " \r\n";
            incontri += t[3].Text + "\t\t" + t[4].Text + " \r\n";
            incontri += t[0].Text + "\t\t" + t[2].Text + " \r\n";
            incontri += t[4].Text + "\t\t" + t[1].Text + " \r\n";
            incontri += t[0].Text + "\t\t" + t[3].Text + " \r\n";
            incontri += t[4].Text + "\t\t" + t[2].Text + " \r\n";
            incontri += t[1].Text + "\t\t" + t[3].Text + " \r\n";
            return incontri;
        }

        /// <summary>
        /// [T6]
        /// 0	1			
        /// 2	3			
        /// 4	5			
        /// 0	3			
        /// 1	2			
        /// 3	5			
        /// 2	4			
        /// 1	5			
        /// 0	2			
        /// 3	4
        /// 
        /// 2	5			
        /// 0	4			
        /// 1	3			
        /// 0	5			
        /// 1	4
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static String ElaborateT6(TreeNodeCollection t)
        {
            string incontri = "";

            foreach (TreeNode n in t)
                incontri += n.Text + " \r\n";

            incontri += "[INCONTRI] \r\n \r\n";
            incontri += " \r\n \r\n";

            incontri += t[0].Text + "\t\t" + t[1].Text + " \r\n";
            incontri += t[2].Text + "\t\t" + t[3].Text + " \r\n";
            incontri += t[4].Text + "\t\t" + t[5].Text + " \r\n";
            incontri += t[0].Text + "\t\t" + t[3].Text + " \r\n";
            incontri += t[1].Text + "\t\t" + t[2].Text + " \r\n";
            incontri += t[3].Text + "\t\t" + t[5].Text + " \r\n";
            incontri += t[2].Text + "\t\t" + t[4].Text + " \r\n";
            incontri += t[1].Text + "\t\t" + t[5].Text + " \r\n";
            incontri += t[0].Text + "\t\t" + t[2].Text + " \r\n";
            incontri += t[3].Text + "\t\t" + t[4].Text + " \r\n";
            incontri += t[2].Text + "\t\t" + t[5].Text + " \r\n";
            incontri += t[0].Text + "\t\t" + t[4].Text + " \r\n";
            incontri += t[1].Text + "\t\t" + t[3].Text + " \r\n";
            incontri += t[0].Text + "\t\t" + t[5].Text + " \r\n";
            incontri += t[1].Text + "\t\t" + t[4].Text + " \r\n";

            return incontri;
        }

        internal static List<MatchEntity> ElaborateT4(List<AtletaEntity> g)
        {
            List<MatchEntity> match = new List<MatchEntity>();

            match.Add(new MatchEntity(g[0], g[1]));
            match.Add(new MatchEntity(g[2], g[3]));
            match.Add(new MatchEntity(g[1], g[2]));
            match.Add(new MatchEntity(g[0], g[3]));
            match.Add(new MatchEntity(g[0], g[2]));
            match.Add(new MatchEntity(g[1], g[3]));

            return match;
        }

        internal static List<MatchEntity> ElaborateT5(List<AtletaEntity> g)
        {

            List<MatchEntity> match = new List<MatchEntity>();

            match.Add(new MatchEntity(g[0], g[1]));
            match.Add(new MatchEntity(g[2], g[3]));
            match.Add(new MatchEntity(g[4], g[0]));
            match.Add(new MatchEntity(g[1], g[2]));
            match.Add(new MatchEntity(g[3], g[4]));
            match.Add(new MatchEntity(g[0], g[2]));
            match.Add(new MatchEntity(g[4], g[1]));
            match.Add(new MatchEntity(g[0], g[3]));
            match.Add(new MatchEntity(g[4], g[2]));
            match.Add(new MatchEntity(g[1], g[3]));

            return match;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        internal static List<MatchEntity> ElaborateT6(List<AtletaEntity> g)
        {
            List<MatchEntity> match = new List<MatchEntity>();

            match.Add(new MatchEntity(g[0], g[1]));
            match.Add(new MatchEntity(g[2], g[3]));
            match.Add(new MatchEntity(g[4], g[5]));
            match.Add(new MatchEntity(g[0], g[3]));
            match.Add(new MatchEntity(g[1], g[2]));
            match.Add(new MatchEntity(g[3], g[5]));
            match.Add(new MatchEntity(g[2], g[4]));
            match.Add(new MatchEntity(g[1], g[5]));
            match.Add(new MatchEntity(g[0], g[2]));
            match.Add(new MatchEntity(g[3], g[4]));
            match.Add(new MatchEntity(g[2], g[5]));
            match.Add(new MatchEntity(g[0], g[4]));
            match.Add(new MatchEntity(g[1], g[3]));
            match.Add(new MatchEntity(g[0], g[5]));
            match.Add(new MatchEntity(g[1], g[4]));

            return match;
        }


        ///TODO implementare i metodi che restituiscono liste di incontri
        ///


        #region READ
        public static bool TestConnectionString()
        {
            SqlConnection c = null;

            try
            {

                c = new SqlConnection(GetConnectionString());

                c.Open();

                if (c.State == System.Data.ConnectionState.Open)
                    return true;
                else
                    return false;
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                c.Close();
            }
        }

        public static string GetDbType()
        {
            DataBaseType dbType = SqlDal_MasterDB.GetSelectedDB();

            if (dbType == DataBaseType.Maschile)
                return MASCHILE;
            else if (dbType == DataBaseType.Femminile)
                return FEMMINILE;
            else
                return TEST;
        }

        /// <summary>
        /// Return the version of the DB Stored on the table SYSTEM_INFO
        /// </summary>
        /// <returns></returns>
        public static String GetDbVersion()
        {
            SqlConnection c = null;

            try
            {

                c = new SqlConnection(GetConnectionString());

                String sqlText = "SELECT * FROM SYSTEM_INFO WHERE NAME = 'DB_VERS'";
                c.Open();

                SqlCommand command = new SqlCommand(sqlText, c);
                SqlDataReader reader = command.ExecuteReader();

                string dbVersion = "";
                while (reader.Read())
                {
                    dbVersion = reader["VALUE"].ToString();
                }

                return dbVersion;
            }
            catch (Exception e)
            {
                return "";
            }
            finally
            {
                c.Close();
            }
        }
        public static bool CheckAdminPwd(string pwd)
        {
            SqlConnection c = null;

            try
            {

                c = new SqlConnection(GetConnectionString());

                String sqlText = "SELECT * FROM SYSTEM_INFO WHERE NAME = 'AdminPwd'";
                c.Open();

                SqlCommand command = new SqlCommand(sqlText, c);
                SqlDataReader reader = command.ExecuteReader();

                string pwdOnDb = "";
                while (reader.Read())
                {
                    pwdOnDb = reader["VALUE"].ToString();
                }

                return pwdOnDb.ToUpper() == pwd.ToUpper();
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                c.Close();
            }
        }


        public static String GetDisciplinaById(int idDisciplina)
        {
            String commandText = "select Nome FROM Discipline WHERE Id = " + idDisciplina;

            SqlConnection c = null;

            try
            {
                c = new SqlConnection(GetConnectionString());

                c.Open();

                string nomeDisciplina = "";

                SqlCommand command = new SqlCommand(commandText, c);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    nomeDisciplina = (String)reader["nome"];
                }

                return nomeDisciplina;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                c.Close();
            }
        }
        public static TorneoEntity GetTorneoById(int idTorneo)
        {
            SqlConnection c = null;

            try
            {
                String sqlText = "SELECT * FROM TORNEO WHERE Id = " + idTorneo;
                c = new SqlConnection(GetConnectionString());

                c.Open();
                TorneoEntity torneo = new TorneoEntity();

                SqlCommand command = new SqlCommand(sqlText, c);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    torneo = new TorneoEntity()
                    {
                        Name = reader["NomeTorneo"].ToString(),
                        Id = Int32.Parse(reader["Id"].ToString()),
                        StartDate = Convert.ToDateTime(reader["DataInizio"].ToString())
                    };
                }

                return torneo;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                c.Close();
            }
        }
        
        public static List<DisciplinaEntity> GetDisciplineByIdTorneo(int idTorneo)
        {
            SqlConnection c = null;

            try
            {
                //TODO 
                String sqlText = "select * from TorneoVsDiscipline td, Discipline d " +
                                    "where td.IdDisciplina = d.Id " +
                                    "and td.IdTorneo = " + idTorneo;

                c = new SqlConnection(GetConnectionString());

                c.Open();
                List<DisciplinaEntity> discipline = new List<DisciplinaEntity>();
                discipline.Add(new DisciplinaEntity() { IdDisciplina = 0 });

                SqlCommand command = new SqlCommand(sqlText, c);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    discipline.Add(new DisciplinaEntity()
                    {
                        IdDisciplina = Convert.ToInt32(reader["IdDisciplina"])
                        ,
                        Nome = Convert.ToString(reader["Nome"]),
                        Descrizione = Convert.ToString(reader["Descrizione"])
                    });
                }


                return discipline;

            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                c.Close();
            }
        }
        
       
        
        

        
        
        

        #endregion

        #region DELETE

        

        public static bool EliminaPartecipanteDaTorneo(int idTorneo, int idDisciplina, int idAtleta, string categoria)
        {
            String sqlText = "delete AtletiVsTorneoVsDiscipline where IdAtleta = " + idAtleta +
                                "and IdTorneoVsDiscipline in (select Id from TorneoVsDiscipline where " +
                                "IdDisciplina = " + idDisciplina + " " +
                                "and IdTorneo = " + idTorneo + " " +
                                "and Categoria = '" + categoria + "') ";

            SqlConnection c = null;

            try
            {
                c = new SqlConnection(GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(sqlText, c);
                Int32 rowAffected = command.ExecuteNonQuery();

                if (rowAffected == 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                c.Close();
            }
        }
        public static bool EliminaAtleta(int idAtleta)
        {
            string commandText = "DELETE Atleti where id = " + idAtleta;

            SqlConnection c = null;

            try
            {
                c = new SqlConnection(GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                Int32 rowAffected = command.ExecuteNonQuery();

                if (rowAffected == 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                c.Close();
            }
        }
        public static bool EliminaAtletaDaGironi(int idAtleta)
        {
            string commandText = "DELETE Gironi where id = " + idAtleta;

            SqlConnection c = null;

            try
            {
                c = new SqlConnection(GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                Int32 rowAffected = command.ExecuteNonQuery();

                if (rowAffected == 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                c.Close();
            }
        }
        public static bool EliminaAtletaDaRanking(int idAtleta)
        {
            string commandText = "DELETE Ranking where IdAtleta = " + idAtleta;

            SqlConnection c = null;

            try
            {
                c = new SqlConnection(GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                Int32 rowAffected = command.ExecuteNonQuery();

                if (rowAffected == 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                c.Close();
            }
        }
        public static bool EliminaAtletaDaTorneo(int idAtleta)
        {
            string commandText = "DELETE AtletiVsTorneoVsDiscipline were IdAtleta = " + idAtleta;

            SqlConnection c = null;

            try
            {
                c = new SqlConnection(GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                Int32 rowAffected = command.ExecuteNonQuery();

                if (rowAffected == 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                c.Close();
            }
        }

        

        public static bool EliminaAtletiVsTorneoVsDiscipline(Int32 idTorneo)
        {
            String commandText = "DELETE AtletiVsTorneoVsDiscipline WHERE IdTorneoVsDiscipline in " +
                                    "(SELECT Id FROM TorneoVsDiscipline WHERE IdTorneo = " + idTorneo + ")";

            SqlConnection c = null;

            try
            {
                c = new SqlConnection(GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                Int32 rowAffected = command.ExecuteNonQuery();

                if (rowAffected == 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                c.Close();
            }
        }

        public static bool EliminaTorneoVsDiscipline(Int32 idTorneo)
        {
            String commandText = "DELETE TorneoVsDiscipline WHERE IdTorneo = " + idTorneo + ")";

            SqlConnection c = null;

            try
            {
                c = new SqlConnection(GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                Int32 rowAffected = command.ExecuteNonQuery();

                if (rowAffected == 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                c.Close();
            }
        }

        

        #endregion


        #region UPDATE

        

        #endregion

        #region REPORT

        public static List<OutputRisultatiTorneo> GetExportGironiTorneo(int idTorneo, int idDisciplina, string categoria)
        {
            List<OutputRisultatiTorneo> result = new List<OutputRisultatiTorneo>();

            String commandText = "SELECT rosso.Id IdRosso, rosso.Cognome as CognomeAtletaRosso, rosso.Nome as NomeAtletaRosso, gi.PuntiAtletaRosso, blu.Id IdBlu, blu.Cognome CognomeAtletaBlu, blu.Nome as NomeAtletaBlu, gi.PuntiAtletaBlu, NumeroGirone as Field " +
                                    "FROM GironiIncontri gi " +
                                    "join Atleti rosso on gi.IdAtletaRosso = rosso.Id " +
                                    "join Atleti blu on gi.IdAtletaBlu = blu.Id " +
                                    "where gi.IdTorneo = " + idTorneo + " " +
                                    "and gi.IdDisciplina = " + idDisciplina + " " +
                                    "and rosso.Sesso = '" + categoria + "' " +
                                    "and blu.Sesso = '" + categoria + "' " +
                                    "order by NumeroGirone ";

            SqlConnection c = null;

            try
            {
                c = new SqlConnection(GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(new OutputRisultatiTorneo
                    {
                        IdRosso = (int)reader["IdRosso"],
                        CognomeRosso = (string)reader["CognomeAtletaRosso"],
                        NomeRosso = (string)reader["NomeAtletaRosso"],
                        PuntiRosso = (int)reader["PuntiAtletaRosso"],
                        Idblu = (int)reader["IdBlu"],
                        CognomeBlu = (string)reader["CognomeAtletaBlu"],
                        NomeBlu = (string)reader["NomeAtletaBlu"],
                        PuntiBlu = (int)reader["PuntiAtletaBlu"],
                        Campo = (int)reader["Field"]
                    });
                }
                if (result.Count > 0)
                    return result;
                else
                    return null;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                c.Close();
            }
        }

        public static List<OutputRisultatiEliminatorieTorneo> GetExportSedicesimiTorneo(int idTorneo, int idDisciplina, string categoria)
        {
            List<OutputRisultatiEliminatorieTorneo> result = new List<OutputRisultatiEliminatorieTorneo>();

            String commandText = "select g.Posizione, g.IdAtleta, a.Cognome, a.Nome, g.PuntiFatti, g.PuntiSubiti, Campo " +
                                    "from Qualificati32 g " +
                                    "join Atleti a on a.Id = g.IdAtleta " +
                                    "where IdTorneo = " + idTorneo + " " +
                                    "and IdDisciplina = " + idDisciplina + " " +
                                    "and a.Sesso = '" + categoria + "' " +
                                    "order by posizione";
            SqlConnection c = null;

            try
            {
                c = new SqlConnection(GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(new OutputRisultatiEliminatorieTorneo
                    {
                        Posizione = (int)reader["Posizione"],
                        IdAtleta = (int)reader["IdAtleta"],
                        CognomeAtleta = (string)reader["Cognome"],
                        NomeAtleta = (string)reader["Nome"],
                        PuntiFatti = (int)reader["PuntiFatti"],
                        PuntiSubiti = (int)reader["PuntiSubiti"],
                        Campo = Convert.ToInt32(reader["Campo"])
                    });
                }
                if (result.Count > 0)
                    return result;
                else
                    return null;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                c.Close();
            }

        }

        public static List<OutputRisultatiEliminatorieTorneo> GetExportOttaviTorneo(int idTorneo, int idDisciplina, string categoria)
        {
            List<OutputRisultatiEliminatorieTorneo> result = new List<OutputRisultatiEliminatorieTorneo>();

            String commandText = "select g.Posizione, g.IdAtleta, a.Cognome, a.Nome, g.PuntiFatti, g.PuntiSubiti, Campo " +
                                    "from Qualificati16 g " +
                                    "join Atleti a on a.Id = g.IdAtleta " +
                                    "where IdTorneo = " + idTorneo + " " +
                                    "and IdDisciplina = " + idDisciplina + " " +
                                    "and a.Sesso = '" + categoria + "' " +
                                    "order by Campo";
            SqlConnection c = null;

            try
            {
                c = new SqlConnection(GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(new OutputRisultatiEliminatorieTorneo
                    {
                        Posizione = (int)reader["Posizione"],
                        IdAtleta = (int)reader["IdAtleta"],
                        CognomeAtleta = (string)reader["Cognome"],
                        NomeAtleta = (string)reader["Nome"],
                        PuntiFatti = (int)reader["PuntiFatti"],
                        PuntiSubiti = (int)reader["PuntiSubiti"],
                        Campo = Convert.ToInt32(reader["Campo"])
                    });
                }
                if (result.Count > 0)
                    return result;
                else
                    return null;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                c.Close();
            }

        }

        public static List<OutputRisultatiEliminatorieTorneo> GetExportQuartiTorneo(int idTorneo, int idDisciplina, string categoria)
        {
            List<OutputRisultatiEliminatorieTorneo> result = new List<OutputRisultatiEliminatorieTorneo>();

            String commandText = "select g.Posizione, g.IdAtleta, a.Cognome, a.Nome, g.PuntiFatti, g.PuntiSubiti, Campo " +
                                    "from Qualificati8 g " +
                                    "join Atleti a on a.Id = g.IdAtleta " +
                                    "where IdTorneo = " + idTorneo + " " +
                                    "and IdDisciplina = " + idDisciplina + " " +
                                    "and a.Sesso = '" + categoria + "' " +
                                    "order by Campo";
            SqlConnection c = null;

            try
            {
                c = new SqlConnection(GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(new OutputRisultatiEliminatorieTorneo
                    {
                        Posizione = (int)reader["Posizione"],
                        IdAtleta = (int)reader["IdAtleta"],
                        CognomeAtleta = (string)reader["Cognome"],
                        NomeAtleta = (string)reader["Nome"],
                        PuntiFatti = (int)reader["PuntiFatti"],
                        PuntiSubiti = (int)reader["PuntiSubiti"],
                        Campo = Convert.ToInt32(reader["Campo"])
                    });
                }
                if (result.Count > 0)
                    return result;
                else
                    return null;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                c.Close();
            }

        }

        public static List<OutputRisultatiEliminatorieTorneo> GetExportSemifinaliTorneo(int idTorneo, int idDisciplina, string categoria)
        {
            List<OutputRisultatiEliminatorieTorneo> result = new List<OutputRisultatiEliminatorieTorneo>();

            String commandText = "select g.Posizione, g.IdAtleta, a.Cognome, a.Nome, g.PuntiFatti, g.PuntiSubiti, Campo " +
                                    "from Semifinali g " +
                                    "join Atleti a on a.Id = g.IdAtleta " +
                                    "where IdTorneo = " + idTorneo + " " +
                                    "and IdDisciplina = " + idDisciplina + " " +
                                    "and a.Sesso = '" + categoria + "' " +
                                    "order by Campo";
            SqlConnection c = null;

            try
            {
                c = new SqlConnection(GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(new OutputRisultatiEliminatorieTorneo
                    {
                        Posizione = (int)reader["Posizione"],
                        IdAtleta = (int)reader["IdAtleta"],
                        CognomeAtleta = (string)reader["Cognome"],
                        NomeAtleta = (string)reader["Nome"],
                        PuntiFatti = (int)reader["PuntiFatti"],
                        PuntiSubiti = (int)reader["PuntiSubiti"],
                        Campo = Convert.ToInt32(reader["Campo"])
                    });
                }
                if (result.Count > 0)
                    return result;
                else
                    return null;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                c.Close();
            }

        }

        public static List<OutputRisultatiEliminatorieTorneo> GetExportFinaliTorneo(int idTorneo, int idDisciplina, string categoria)
        {
            List<OutputRisultatiEliminatorieTorneo> result = new List<OutputRisultatiEliminatorieTorneo>();

            String commandText = "select g.Posizione, g.IdAtleta, a.Cognome, a.Nome, g.PuntiFatti, g.PuntiSubiti, Campo " +
                                    "from Finali g " +
                                    "join Atleti a on a.Id = g.IdAtleta " +
                                    "where IdTorneo = " + idTorneo + " " +
                                    "and IdDisciplina = " + idDisciplina + " " +
                                    "and a.Sesso = '" + categoria + "' " +
                                    "order by Campo";
            SqlConnection c = null;

            try
            {
                c = new SqlConnection(GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(new OutputRisultatiEliminatorieTorneo
                    {
                        Posizione = (int)reader["Posizione"],
                        IdAtleta = (int)reader["IdAtleta"],
                        CognomeAtleta = (string)reader["Cognome"],
                        NomeAtleta = (string)reader["Nome"],
                        PuntiFatti = (int)reader["PuntiFatti"],
                        PuntiSubiti = (int)reader["PuntiSubiti"],
                        Campo = Convert.ToInt32(reader["Campo"])
                    });
                }
                if (result.Count > 0)
                    return result;
                else
                    return null;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                c.Close();
            }

        }
        #endregion
    }
}
