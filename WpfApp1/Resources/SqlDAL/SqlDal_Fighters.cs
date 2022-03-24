using BusinessEntity.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resources
{
    public static class SqlDal_Fighters
    {
        public static Int32 InsertNewAtleta(AtletaEntity a)
        {
            String commandText = "INSERT INTO Atleti(IdAsd,Cognome,Nome,Sesso,Email, IsEnabled)  VALUES (" +
                                a.IdAsd + ",'" + a.Cognome + "','" + a.Nome + "','" + a.Sesso + "','" + a.Email + "', 1); " +
                                    "SELECT SCOPE_IDENTITY();";

            SqlConnection c = null;
            try
            {

                c = new SqlConnection(Helper.GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                SqlDataReader reader = command.ExecuteReader();

                Int32 idInserted = 0;
                reader.Read();

                idInserted = Convert.ToInt32(reader[0]);

                if (idInserted > 0)
                    return idInserted;
                else
                    return -1;
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                c.Close();
            }
        }

        public static Int32 InsertNewRanking(Int32 idAtleta)
        {
            //TODO automatizzare dalla lettura della tabella Delle Discipline
            String date = "@date";
            String commandtext = "INSERT INTO Ranking VALUES (" + idAtleta + ",0," + date + "," + date + ",0,1); " +
                                    "INSERT INTO Ranking VALUES (" + idAtleta + ",0," + date + "," + date + ",0,2); " +
                                    "INSERT INTO Ranking VALUES (" + idAtleta + ",0," + date + "," + date + ",0,3); " +
                                    "INSERT INTO Ranking VALUES (" + idAtleta + ",0," + date + "," + date + ",0,4); " +
                                    "INSERT INTO Ranking VALUES (" + idAtleta + ",0," + date + "," + date + ",0,5); " +
                                    "INSERT INTO Ranking VALUES (" + idAtleta + ",0," + date + "," + date + ",0,6); " +
                                    "INSERT INTO Ranking VALUES (" + idAtleta + ",0," + date + "," + date + ",0,7); " +
                                    "INSERT INTO Ranking VALUES (" + idAtleta + ",0," + date + "," + date + ",0,8); " +
                                    "INSERT INTO Ranking VALUES (" + idAtleta + ",0," + date + "," + date + ",0,9); " +
                                    "INSERT INTO Ranking VALUES (" + idAtleta + ",0," + date + "," + date + ",0,10); " +
                                    "SELECT SCOPE_IDENTITY();";

            SqlParameter p = new SqlParameter(date, System.Data.SqlDbType.DateTime) { Value = DateTime.Today };
            SqlConnection c = null;

            try
            {
                c = new SqlConnection(Helper.GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandtext, c);
                command.Parameters.Add(p);
                Int32 rowAffected = command.ExecuteNonQuery();

                return rowAffected;
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                c.Close();
            }

        }

        public static bool UpdateAngraficDataByAtleta(AtletaEntity a)
        {
            String commandText = "UPDATE Atleti " +
                                    "SET IdASD = " + a.IdAsd + ", Cognome = '" + a.Cognome + "', Nome = '" + a.Nome + "', Sesso = '" + a.Sesso + "', Email = '" + a.Email  +"'" +
                                    "WHERE Id = " + a.IdAtleta;

            SqlConnection c = null;

            try
            {
                c = new SqlConnection(Helper.GetConnectionString());

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

        public static List<AtletaEntity> GetAllAnagraficaAtletiWithRanking()
        {
            List<AtletaEntity> atleti = new List<AtletaEntity>();

            //TODO : devo selezionare il ranking di tutte le discipline
            //per ora metto di default a 0 il campo ranking
            String sqlText = "SELECT a.*, asd.Nome_ASD, 0 as Punteggio FROM Atleti a " +
                                "JOIN ASD asd ON a.IdASD = asd.Id " +
                                //"JOIN Ranking r ON r.IdAtleta = a.Id " +
                                "order by asd.Nome_ASD, a.Cognome, a.Nome";

            SqlConnection c = null;

            try
            {
                c = new SqlConnection(Helper.GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(sqlText, c);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    atleti.Add(new AtletaEntity()
                    {
                        IdAsd = Convert.ToInt32(reader["IdASD"]),
                        Nome = Convert.ToString(reader["Nome"]),
                        Cognome = Convert.ToString(reader["Cognome"]),
                        IdAtleta = Convert.ToInt32(reader["Id"]),
                        Asd = Convert.ToString(reader["Nome_ASD"]),
                        Sesso = Convert.ToString(reader["Sesso"]),
                        Ranking = Convert.ToDouble(reader["Punteggio"]),
                        Email = Convert.ToString(reader["Email"]),
                        IsEnabled = Convert.ToBoolean(reader["IsEnabled"])
                    });
                }
                if (atleti.Count > 0)
                    return atleti;
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

        public static List<AtletaEntity> GetAllAnagraficaAtleti()
        {
            List<AtletaEntity> atleti = new List<AtletaEntity>();

            String sqlText = "SELECT a.*, asd.Nome_ASD, 0 as Punteggio FROM Atleti a " +
                                "JOIN ASD asd ON a.IdASD = asd.Id " +
                                "order by asd.Nome_ASD, a.Cognome, a.Nome";

            SqlConnection c = null;

            try
            {
                c = new SqlConnection(Helper.GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(sqlText, c);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    atleti.Add(new AtletaEntity()
                    {
                        IdAsd = Convert.ToInt32(reader["IdASD"]),
                        Nome = Convert.ToString(reader["Nome"]),
                        Cognome = Convert.ToString(reader["Cognome"]),
                        IdAtleta = Convert.ToInt32(reader["Id"]),
                        Asd = Convert.ToString(reader["Nome_ASD"]),
                        Sesso = Convert.ToString(reader["Sesso"]),
                        Email = Convert.ToString(reader["Email"])
                    });
                }
                if (atleti.Count > 0)
                    return atleti;
                else
                    return null;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                if (c.State == ConnectionState.Open)
                    c.Close();
            }
        }
        
        public static Dictionary<int, String> GetAtletiDictionary()
        {
            Dictionary<int, String> result = new Dictionary<int, String>();

            string sqlText = "select id, Cognome + ' ' + Nome as Atleta from Atleti order by Atleta";

            SqlConnection c = null;

            try
            {
                c = new SqlConnection(Helper.GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(sqlText, c);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(Convert.ToInt32(reader["id"]), Convert.ToString(reader["Atleta"]));
                }

                return result;
            }
            catch (Exception e)
            {
                return result;
            }
            finally
            {
                if (c.State == ConnectionState.Open)
                    c.Close();
            }

        }

        public static AtletaEntity GetAtletaById(int idAtleta)
        {
            AtletaEntity atleta = new AtletaEntity();

            String commandText = "select at.*, asd.Nome_ASD from atleti at join ASD asd on at.IdASD = asd.Id where at.Id = " + idAtleta;

            SqlConnection c = null;

            try
            {
                c = new SqlConnection(Helper.GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    atleta = new AtletaEntity()
                    {
                        IdAtleta = (int)reader["Id"],
                        IdAsd = (int)reader["IdASD"],
                        Cognome = Convert.ToString(reader["Cognome"]),
                        Nome = Convert.ToString(reader["Nome"]),
                        Sesso = Convert.ToString(reader["Sesso"]),
                        Asd = Convert.ToString(reader["Nome_ASD"]),
                        Email = Convert.ToString(reader["Email"])
                    };
                }
                return atleta;
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

        public static List<AtletaEntity> GetAtletiFromNameAndSurname(String name, String surname)
        {
            List<AtletaEntity> atleti = new List<AtletaEntity>();

            String commandText = "select at.*, asd.Nome_ASD from atleti at join ASD asd on at.IdASD = asd.Id where at.Nome like '"
                                    + name + "%' and at.Cognome like '"
                                    + surname + "%' order by at.Cognome ASC";

            SqlConnection c = null;

            try
            {
                c = new SqlConnection(Helper.GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    atleti.Add(new AtletaEntity()
                    {
                        IdAtleta = (int)reader["Id"],
                        IdAsd = (int)reader["IdASD"],
                        Cognome = Convert.ToString(reader["Cognome"]),
                        Nome = Convert.ToString(reader["Nome"]),
                        Sesso = Convert.ToString(reader["Sesso"]),
                        Asd = Convert.ToString(reader["Nome_ASD"]),
                        Email = Convert.ToString(reader["Email"])
                    }
                    );
                }
                if (atleti.Count > 0)
                    return atleti;
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

        public static bool EliminaAtleta(int idAtleta)
        {
            string commandText = "DELETE Atleti where id = " + idAtleta;

            SqlConnection c = null;

            try
            {
                c = new SqlConnection(Helper.GetConnectionString());

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
                c = new SqlConnection(Helper.GetConnectionString());

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
                c = new SqlConnection(Helper.GetConnectionString());

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
                c = new SqlConnection(Helper.GetConnectionString());

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
    }
}
