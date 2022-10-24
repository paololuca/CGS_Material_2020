using Report;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resources
{
    public static class SqlDal_Report
    {
        public static List<OutputRisultatiTorneo> GetExportGironiTorneo(int idTorneo, int idDisciplina)
        {
            List<OutputRisultatiTorneo> result = new List<OutputRisultatiTorneo>();

            String commandText = "SELECT rosso.Id IdRosso, rosso.Cognome as CognomeAtletaRosso, rosso.Nome as NomeAtletaRosso, gi.PuntiAtletaRosso, blu.Id IdBlu, blu.Cognome CognomeAtletaBlu, blu.Nome as NomeAtletaBlu, gi.PuntiAtletaBlu, NumeroGirone as Field, DoppiaMorte " +
                                    "FROM GironiIncontri gi " +
                                    "join Atleti rosso on gi.IdAtletaRosso = rosso.Id " +
                                    "join Atleti blu on gi.IdAtletaBlu = blu.Id " +
                                    "where gi.IdTorneo = " + idTorneo + " " +
                                    "and gi.IdDisciplina = " + idDisciplina + " " +
                                    "order by NumeroGirone ";

            SqlConnection c = null;

            try
            {
                c = new SqlConnection(Helper.GetConnectionString());

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
                        IdBlu = (int)reader["IdBlu"],
                        CognomeBlu = (string)reader["CognomeAtletaBlu"],
                        NomeBlu = (string)reader["NomeAtletaBlu"],
                        PuntiBlu = (int)reader["PuntiAtletaBlu"],
                        Campo = (int)reader["Field"],
                        DoppiaMorte = Convert.ToBoolean((int)reader["DoppiaMorte"])
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

        public static List<OutputRisultatiEliminatorieTorneo> GetExportSedicesimiTorneo(int idTorneo, int idDisciplina)
        {
            List<OutputRisultatiEliminatorieTorneo> result = new List<OutputRisultatiEliminatorieTorneo>();

            String commandText = "select g.Posizione, g.IdAtleta, a.Cognome, a.Nome, g.PuntiFatti, g.PuntiSubiti, Campo " +
                                    "from Qualificati32 g " +
                                    "join Atleti a on a.Id = g.IdAtleta " +
                                    "where IdTorneo = " + idTorneo + " " +
                                    "and IdDisciplina = " + idDisciplina + " " +
                                    "order by posizione";
            SqlConnection c = null;

            try
            {
                c = new SqlConnection(Helper.GetConnectionString());

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

        public static List<OutputRisultatiEliminatorieTorneo> GetExportOttaviTorneo(int idTorneo, int idDisciplina)
        {
            List<OutputRisultatiEliminatorieTorneo> result = new List<OutputRisultatiEliminatorieTorneo>();

            String commandText = "select g.Posizione, g.IdAtleta, a.Cognome, a.Nome, g.PuntiFatti, g.PuntiSubiti, Campo " +
                                    "from Qualificati16 g " +
                                    "join Atleti a on a.Id = g.IdAtleta " +
                                    "where IdTorneo = " + idTorneo + " " +
                                    "and IdDisciplina = " + idDisciplina + " " +
                                    "order by Campo";
            SqlConnection c = null;

            try
            {
                c = new SqlConnection(Helper.GetConnectionString());

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

        public static List<OutputRisultatiEliminatorieTorneo> GetExportQuartiTorneo(int idTorneo, int idDisciplina)
        {
            List<OutputRisultatiEliminatorieTorneo> result = new List<OutputRisultatiEliminatorieTorneo>();

            String commandText = "select g.Posizione, g.IdAtleta, a.Cognome, a.Nome, g.PuntiFatti, g.PuntiSubiti, Campo " +
                                    "from Qualificati8 g " +
                                    "join Atleti a on a.Id = g.IdAtleta " +
                                    "where IdTorneo = " + idTorneo + " " +
                                    "and IdDisciplina = " + idDisciplina + " " +
                                    "order by Campo";
            SqlConnection c = null;

            try
            {
                c = new SqlConnection(Helper.GetConnectionString());

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

        public static List<OutputRisultatiEliminatorieTorneo> GetExportSemifinaliTorneo(int idTorneo, int idDisciplina)
        {
            List<OutputRisultatiEliminatorieTorneo> result = new List<OutputRisultatiEliminatorieTorneo>();

            String commandText = "select g.Posizione, g.IdAtleta, a.Cognome, a.Nome, g.PuntiFatti, g.PuntiSubiti, Campo " +
                                    "from Semifinali g " +
                                    "join Atleti a on a.Id = g.IdAtleta " +
                                    "where IdTorneo = " + idTorneo + " " +
                                    "and IdDisciplina = " + idDisciplina + " " +
                                    "order by Campo";
            SqlConnection c = null;

            try
            {
                c = new SqlConnection(Helper.GetConnectionString());

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

        public static List<OutputRisultatiEliminatorieTorneo> GetExportFinaliTorneo(int idTorneo, int idDisciplina)
        {
            List<OutputRisultatiEliminatorieTorneo> result = new List<OutputRisultatiEliminatorieTorneo>();

            String commandText = "select g.Posizione, g.IdAtleta, a.Cognome, a.Nome, g.PuntiFatti, g.PuntiSubiti, Campo " +
                                    "from Finali g " +
                                    "join Atleti a on a.Id = g.IdAtleta " +
                                    "where IdTorneo = " + idTorneo + " " +
                                    "and IdDisciplina = " + idDisciplina + " " +
                                    "order by Campo";
            SqlConnection c = null;

            try
            {
                c = new SqlConnection(Helper.GetConnectionString());

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
    }
}
