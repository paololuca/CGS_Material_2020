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
    public static class SqlDal_Tournaments
    {
        public static List<TorneoEntity> GetTorneiAttivi(Boolean onlyList)
        {
            SqlConnection c = null;

            try
            {
                String sqlText = "SELECT * FROM TORNEO WHERE CONCLUSO = 0 and ATTIVO = 1";
                c = new SqlConnection(Helper.GetConnectionString());

                c.Open();
                List<TorneoEntity> tornei = new List<TorneoEntity>();

                if (!onlyList)
                    tornei.Add(new TorneoEntity() { Id = 0 });

                SqlCommand command = new SqlCommand(sqlText, c);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    tornei.Add(new TorneoEntity()
                    {
                        Name = reader["NomeTorneo"].ToString(),
                        Id = Int32.Parse(reader["Id"].ToString()),
                        StartDate = Convert.ToDateTime(reader["DataInizio"].ToString()),
                        EndDate = Convert.ToDateTime(reader["DataFine"].ToString()),
                        Place = reader["Luogo"].ToString(),
                        Note = reader["Commenti"].ToString()
                    });
                }

                return tornei;
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

        public static List<TorneoEntity> GetTorneiToActivate(Boolean onlyList)
        {
            SqlConnection c = null;

            try
            {
                String sqlText = "SELECT * FROM TORNEO WHERE ATTIVO = 0";
                c = new SqlConnection(Helper.GetConnectionString());

                c.Open();
                List<TorneoEntity> tornei = new List<TorneoEntity>();

                if (!onlyList)
                    tornei.Add(new TorneoEntity() { Id = 0 });

                SqlCommand command = new SqlCommand(sqlText, c);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    tornei.Add(new TorneoEntity()
                    {
                        Name = reader["NomeTorneo"].ToString(),
                        Id = Int32.Parse(reader["Id"].ToString()),
                        StartDate = Convert.ToDateTime(reader["DataInizio"].ToString()),
                        EndDate = Convert.ToDateTime(reader["DataFine"].ToString()),
                        Place = reader["Luogo"].ToString(),
                        Note = reader["Commenti"].ToString()
                    });
                }

                return tornei;
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

        public static List<TorneoEntity> GetTorneiConclusi(Boolean onlyList)
        {
            SqlConnection c = null;

            try
            {
                String sqlText = "SELECT * FROM TORNEO WHERE CONCLUSO = 1";
                c = new SqlConnection(Helper.GetConnectionString());

                c.Open();
                List<TorneoEntity> tornei = new List<TorneoEntity>();

                if (!onlyList)
                    tornei.Add(new TorneoEntity() { Id = 0 });

                SqlCommand command = new SqlCommand(sqlText, c);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    tornei.Add(new TorneoEntity()
                    {
                        Name = reader["NomeTorneo"].ToString(),
                        Id = Int32.Parse(reader["Id"].ToString()),
                        StartDate = Convert.ToDateTime(reader["DataInizio"].ToString()),
                        EndDate = Convert.ToDateTime(reader["DataFine"].ToString()),
                        Place = reader["Luogo"].ToString(),
                        Note = reader["Commenti"].ToString()
                    });
                }

                return tornei;
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

        public static Int32 InserNewTorneo(TorneoEntity t)
        {
            String startDate = "@startDate";
            String endDate = "@endDate";

            String commandText = "INSERT INTO Torneo VALUES ('" + t.Name + "','" + t.Place + "'," + startDate + "," + endDate + ", -1, '')" +
                                "SELECT SCOPE_IDENTITY();";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter(startDate, SqlDbType.DateTime) { Value = t.StartDate},
                new SqlParameter(endDate, SqlDbType.DateTime) { Value = t.EndDate}
            };

            SqlConnection c = null;

            try
            {

                c = new SqlConnection(Helper.GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                command.Parameters.AddRange(parameters.ToArray());
                SqlDataReader reader = command.ExecuteReader();

                Int32 idInserted = 0;
                reader.Read();

                idInserted = Convert.ToInt32(reader[0]);

                if (idInserted > 0)
                    return idInserted;
                else
                    return 0;
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

        internal static void AddDisciplineToTorneo(int newTournamentId,
                                                    bool singleSword,
                                                    bool swordAndDagger,
                                                    bool swordAndBuckler,
                                                    bool swordAndShield,
                                                    bool twoHandSword,
                                                    string categoria)
        {
            String commandText = "";

            if (singleSword)
                commandText += "INSERT INTO TorneoVsDiscipline VALUES ()";
        }

        public static bool EliminaTorneo(Int32 idTorneo)
        {
            String commandText = "DELETE Torneo where Id = " + idTorneo;

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

        public static List<TorneoEntity> GetAllTornei()
        {
            List<TorneoEntity> tornei = new List<TorneoEntity>();

            String commandText = "SELECT * FROM Torneo WHERE Concluso = 0";

            SqlConnection c = null;

            try
            {
                c = new SqlConnection(Helper.GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    tornei.Add(new TorneoEntity()
                    {
                        Id = (int)reader["Id"],
                        Name = Convert.ToString(reader["NomeTorneo"]),
                        Place = Convert.ToString(reader["Luogo"]),
                        StartDate = Convert.ToDateTime(reader["DataInizio"].ToString()),
                        EndDate = Convert.ToDateTime(reader["DataFine"].ToString())
                    }
                    );
                }
                if (tornei.Count > 0)
                    return tornei;
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

        public static List<AtletaEntity> GetAtletiTorneoVsDisciplinaAssoluti(int idTorneo, int idDisciplina)
        {
            List<AtletaEntity> atleti = new List<AtletaEntity>();

            AtletaEntity a;

            String sqlText = "select * from AtletiVsTorneoVsDiscipline atd, TorneoVsDiscipline td, atleti a, ASD, Ranking r " +
                                "where atd.IdTorneoVsDiscipline = td.Id " +
                                "and a.Id = atd.IdAtleta " +
                                "and asd.Id = a.IdASD " +
                                "and r.IdAtleta = a.Id " +
                                "and td.IdTorneo = " + idTorneo + " " +
                                "and td.IdDisciplina = " + idDisciplina + " " +
                                "and r.IdDisciplina = " + idDisciplina + " " +
                                "order by r.Punteggio DESC, ASD.Nome_ASD ASC, a.Cognome ASC";

            SqlConnection c = null;

            try
            {
                c = new SqlConnection(Helper.GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(sqlText, c);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    a = new AtletaEntity()
                    {
                        IdAsd = Convert.ToInt32(reader["IdASD"]),
                        Asd = Convert.ToString(reader["Nome_ASD"]),
                        Nome = Convert.ToString(reader["Nome"]),
                        Cognome = Convert.ToString(reader["Cognome"]),
                        IdAtleta = Convert.ToInt32(reader["IdAtleta"]),
                        Ranking = Convert.ToDouble(reader["Punteggio"])
                    };

                    atleti.Add(a);
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
        
        public static List<AtletaEntity> GetAtletiTorneoVsDisciplina(int idTorneo, int idDisciplina)
        {

            List<AtletaEntity> atleti = new List<AtletaEntity>();
            List<AtletaEntity> atletiAsd = new List<AtletaEntity>();
            String currentAsd = "";
            AtletaEntity a;

            String sqlText = "select * from AtletiVsTorneoVsDiscipline atd, TorneoVsDiscipline td, atleti a, ASD, Ranking r " +
                                "where atd.IdTorneoVsDiscipline = td.Id " +
                                "and a.Id = atd.IdAtleta " +
                                "and asd.Id = a.IdASD " +
                                "and r.IdAtleta = a.Id " +
                                "and td.IdTorneo = " + idTorneo + " " +
                                "and td.IdDisciplina = " + idDisciplina + " " +
                                "and r.IdDisciplina = " + idDisciplina + " " +
                                "order by r.Punteggio DESC, ASD.Nome_ASD ASC, a.Cognome ASC";

            SqlConnection c = null;

            try
            {
                c = new SqlConnection(Helper.GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(sqlText, c);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    a = new AtletaEntity()
                    {
                        IdAsd = Convert.ToInt32(reader["IdASD"]),
                        Asd = Convert.ToString(reader["Nome_ASD"]),
                        Nome = Convert.ToString(reader["Nome"]),
                        Cognome = Convert.ToString(reader["Cognome"]),
                        IdAtleta = Convert.ToInt32(reader["IdAtleta"]),
                        Ranking = Convert.ToDouble(reader["Punteggio"])
                    };

                    if (currentAsd != a.Asd)
                    {
                        if (currentAsd != "")
                        {
                            var rnd = new Random();
                            atleti.AddRange(atletiAsd.OrderBy(item => rnd.Next()));
                        }

                        currentAsd = a.Asd;
                        atletiAsd.Clear();
                        atletiAsd.Add(a);
                    }
                    else
                    {
                        atletiAsd.Add(a);
                    }
                }

                if (atletiAsd.Count > 0)
                {
                    var rnd = new Random();

                    atleti.AddRange(atletiAsd.OrderBy(item => rnd.Next()));
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
        
        public static List<AtletaEntity> GetAtletiIscrittiTorneoVsDisciplina(int idTorneo, int idDisciplina)
        {

            List<AtletaEntity> atleti = new List<AtletaEntity>();

            AtletaEntity a;

            String sqlText = "select * from AtletiVsTorneoVsDiscipline atd, TorneoVsDiscipline td, atleti a, ASD, Ranking r " +
                                "where atd.IdTorneoVsDiscipline = td.Id " +
                                "and a.Id = atd.IdAtleta " +
                                "and asd.Id = a.IdASD " +
                                "and r.IdAtleta = a.Id " +
                                "and td.IdTorneo = " + idTorneo + " " +
                                "and td.IdDisciplina = " + idDisciplina + " " +
                                "and r.IdDisciplina = " + idDisciplina +
                                "order by r.Punteggio DESC, ASD.Nome_ASD ASC, a.Cognome ASC";

            SqlConnection c = null;

            try
            {
                c = new SqlConnection(Helper.GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(sqlText, c);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    a = new AtletaEntity()
                    {
                        IdAsd = Convert.ToInt32(reader["IdASD"]),
                        Asd = Convert.ToString(reader["Nome_ASD"]),
                        Nome = Convert.ToString(reader["Nome"]),
                        Cognome = Convert.ToString(reader["Cognome"]),
                        IdAtleta = Convert.ToInt32(reader["IdAtleta"]),
                        Posizionamento = Convert.ToInt32(reader["Posizionamento"]),
                        Ranking = Convert.ToDouble(reader["Punteggio"])
                    };

                    atleti.Add(a);
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
        
        public static List<AtletaEntity> GetAtletiOffTournament(int idTorneo, int idDisciplina, string categoria)
        {
            List<AtletaEntity> atleti = new List<AtletaEntity>();

            String sqlText = "SELECT * FROM ATLETI WHERE Id NOT IN (SELECT atd.IdAtleta FROM AtletiVsTorneoVsDiscipline atd JOIN TorneoVsDiscipline td " +
                    " ON atd.IdTorneoVsDiscipline = td.Id " +
                    " WHERE td.IdTorneo = " + idTorneo + " AND td.IdDisciplina = " + idDisciplina + ") ";


            if (categoria != "O")
                sqlText += "And Sesso = '" + categoria + "'";

            SqlConnection c = null;

            try
            {
                c = new SqlConnection(Helper.GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(sqlText, c);
                SqlDataReader reader = command.ExecuteReader();

                atleti.Add(new AtletaEntity() { IdAtleta = 0 });

                while (reader.Read())
                {
                    atleti.Add(new AtletaEntity()
                    {
                        IdAsd = Convert.ToInt32(reader["IdASD"]),
                        Nome = Convert.ToString(reader["Nome"]),
                        Cognome = Convert.ToString(reader["Cognome"]),
                        IdAtleta = Convert.ToInt32(reader["Id"])
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

        public static bool InsertAtletaOnTournament(int idTorneo, int idDisciplina, int idAtleta)
        {
            String sqlText = "DECLARE @IdTorneoVsDisciplina int; " +
                                "SET @IdTorneoVsDisciplina = (SELECT Id from TorneoVsDiscipline WHERE IdTorneo = " + idTorneo + " and IdDisciplina = " + idDisciplina + " ); " +
                                "INSERT INTO AtletiVsTorneoVsDiscipline values (@IdTorneoVsDisciplina, " + idAtleta + ")";
            SqlConnection c = null;

            try
            {
                c = new SqlConnection(Helper.GetConnectionString());

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


        public static String GetDisciplinaById(int idDisciplina)
        {
            String commandText = "select Nome FROM Discipline WHERE Id = " + idDisciplina;

            SqlConnection c = null;

            try
            {
                c = new SqlConnection(Helper.GetConnectionString());

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
                c = new SqlConnection(Helper.GetConnectionString());

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

                c = new SqlConnection(Helper.GetConnectionString());

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
    }
}
