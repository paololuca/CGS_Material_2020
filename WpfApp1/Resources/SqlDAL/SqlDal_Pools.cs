using BusinessEntity.DAO;
using BusinessEntity.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Resources
{
    public static class SqlDal_Pools
    {
        public static int GetNumeroGironiByTorneoDisciplina(int idTorneo, int idDisciplina)
        {
            //anche i tornei vs discipline vanno divisi tra maschile e femminile
            string sqlText = "SELECT * FROM TorneoVsDiscipline WHERE IdTorneo = " + idTorneo + " AND IdDisciplina = " + idDisciplina + " ";

            SqlConnection c = null;

            try
            {
                c = new SqlConnection(Helper.GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(sqlText, c);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    int gironi = Convert.ToInt32(reader["NumeroGironi"]);
                    if (gironi > 0)
                        return gironi;
                    else
                        return 0;
                }
                return 0;
            }
            catch (Exception e)
            {
                return 0;
            }
            finally
            {
                c.Close();
            }
        }

        public static List<List<AtletaEntity>> GetGironiSalvati(int idTorneo, int idDisciplina)
        {
            List<AtletaEntity> atletiGirone = new List<AtletaEntity>();
            List<List<AtletaEntity>> gironi = new List<List<AtletaEntity>>();

            String sqlText = "select a.Id, a.Nome, a.Cognome, asd.Nome_ASD, g.IdGirone, g.IdTorneo, g.IdDisciplina, td.Categoria, a.IdASD, asd.Nome_ASD, r.Punteggio " +
                                "from Gironi g join Atleti a on a.Id = g.IdAtleta " +
                                "join ASD asd on asd.Id = a.IdASD " +
                                "join TorneoVsDiscipline td on td.IdDisciplina = g.IdDisciplina and td.IdDisciplina = g.IdDisciplina " +
                                "join AtletiVsTorneoVsDiscipline atd on atd.IdTorneoVsDiscipline = td.Id and atd.IdAtleta = a.Id " +
                                "join Ranking r on r.IdAtleta = a.Id " +
                                "where td.IdTorneo = g.IdTorneo " +
                                "and g.IdTorneo = " + idTorneo + " " +
                                "and g.IdDisciplina = " + idDisciplina + " " +
                                "and r.IdDisciplina = " + idDisciplina +
                                "order by g.IdGirone ASC, r.Punteggio DESC, asd.Nome_ASD ASC, a.Cognome ASC";

            AtletaEntity a;
            int currentGirone = 0;
            int gironeAtleta = 0;
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
                        IdAtleta = Convert.ToInt32(reader["Id"]),
                        Ranking = Convert.ToDouble(reader["Punteggio"])
                    };

                    gironeAtleta = Convert.ToInt32(reader["IdGirone"]);

                    if ((currentGirone != 0) && (currentGirone != gironeAtleta))
                    {
                        gironi.Add(atletiGirone);
                        atletiGirone = new List<AtletaEntity>();
                    }

                    atletiGirone.Add(a);
                    currentGirone = gironeAtleta;
                }
                //l'ultimo ciclo
                gironi.Add(atletiGirone);

                return gironi;
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

        

        public static void CaricaPunteggiEsistentiGironiIncontri(int idTorneo, int idDisciplina, MatchEntity i, int idGirone)
        {
            String commandText = "SELECT * FROM GironiIncontri WHERE IdTorneo = " + idTorneo + " and IdDisciplina = " + idDisciplina + " and NumeroGirone = " + idGirone + " " +
                                    "AND IdAtletaRosso = " + i.IdRosso + " and IdAtletaBlu = " + i.IdBlu;


            SqlConnection c = null;

            try
            {
                c = new SqlConnection(Helper.GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    i.PuntiRosso = Convert.ToInt32(reader["PuntiAtletaRosso"].ToString());
                    i.PuntiBlu = Convert.ToInt32(reader["PuntiAtletaBlu"].ToString());
                    i.DoppiaMorte = (Convert.ToInt32(reader["DoppiaMorte"].ToString()) == 1 ? true : false);
                }

            }
            catch (Exception e)
            {

            }
            finally
            {
                c.Close();
            }

        }

        public static int CountPhasesMatchs(int idTorneo, int idDisciplina, string tableName)
        {
            int result = 0;

            String commandText = "SELECT COUNT(*) FROM " + tableName + " WHERE IdTorneo = " + idTorneo + " and IdDisciplina = " + idDisciplina;

            SqlConnection c = null;
            List<AtletaEliminatorie> list = new List<AtletaEliminatorie>();

            try
            {
                c = new SqlConnection(Helper.GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    result = Convert.ToInt32(reader[0].ToString());
                }

            }
            catch (Exception e)
            {

            }
            finally
            {
                c.Close();
            }

            return result;

        }

        public static List<AtletaEliminatorie> GetFinali(int idTorneo, int idDisciplina, int campo)
        {
            String commandText = "SELECT a.Nome, a.Cognome, q.* from Atleti a join Finali q on a.Id = q.IdAtleta  WHERE IdTorneo = " + idTorneo + " and IdDisciplina = " + idDisciplina + "and Campo = " + campo + " ORDER BY Posizione, Cognome, nome ASC";

            SqlConnection c = null;
            List<AtletaEliminatorie> list = new List<AtletaEliminatorie>();

            try
            {
                c = new SqlConnection(Helper.GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new AtletaEliminatorie()

                    {
                        IdAtleta = Convert.ToInt32(reader["IdAtleta"].ToString()),
                        IdTorneo = Convert.ToInt32(reader["IdTorneo"].ToString()),
                        idDisciplina = Convert.ToInt32(reader["IdDisciplina"].ToString()),
                        Posizione = Convert.ToInt32(reader["Posizione"].ToString()),
                        Nome = (String)reader["Nome"],
                        Cognome = (String)reader["Cognome"],
                        PuntiFatti = Convert.ToInt32(reader["PuntiFatti"].ToString()),
                        PuntiSubiti = Convert.ToInt32(reader["PuntiSubiti"].ToString()),
                        Campo = Convert.ToInt32(reader["Campo"].ToString())
                    });
                }

            }
            catch (Exception e)
            {

            }
            finally
            {
                c.Close();
            }

            return list;
        }
        public static List<AtletaEliminatorie> GetSemifinali(int idTorneo, int idDisciplina, int campo)
        {
            String commandText = "SELECT a.Nome, a.Cognome, q.* from Atleti a join Semifinali q on a.Id = q.IdAtleta  WHERE IdTorneo = "
                + idTorneo + " and IdDisciplina = " + idDisciplina + "and Campo = " + campo + " ORDER BY Posizione ASC";

            SqlConnection c = null;
            List<AtletaEliminatorie> list = new List<AtletaEliminatorie>();

            try
            {
                c = new SqlConnection(Helper.GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new AtletaEliminatorie()

                    {
                        IdAtleta = Convert.ToInt32(reader["IdAtleta"].ToString()),
                        IdTorneo = Convert.ToInt32(reader["IdTorneo"].ToString()),
                        idDisciplina = Convert.ToInt32(reader["IdDisciplina"].ToString()),
                        Posizione = Convert.ToInt32(reader["Posizione"].ToString()),
                        Nome = (String)reader["Nome"],
                        Cognome = (String)reader["Cognome"],
                        PuntiFatti = Convert.ToInt32(reader["PuntiFatti"].ToString()),
                        PuntiSubiti = Convert.ToInt32(reader["PuntiSubiti"].ToString())
                    });
                }

            }
            catch (Exception e)
            {

            }
            finally
            {
                c.Close();
            }

            return list;
        }
        public static List<AtletaEliminatorie> GetSemifinali(int idTorneo, int idDisciplina)
        {
            String commandText = "SELECT a.Nome, a.Cognome, q.* from Atleti a join Semifinali q on a.Id = q.IdAtleta  WHERE IdTorneo = "
                + idTorneo + " and IdDisciplina = " + idDisciplina + " ORDER BY Posizione ASC";

            SqlConnection c = null;
            List<AtletaEliminatorie> list = new List<AtletaEliminatorie>();

            try
            {
                c = new SqlConnection(Helper.GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new AtletaEliminatorie()

                    {
                        IdAtleta = Convert.ToInt32(reader["IdAtleta"].ToString()),
                        IdTorneo = Convert.ToInt32(reader["IdTorneo"].ToString()),
                        idDisciplina = Convert.ToInt32(reader["IdDisciplina"].ToString()),
                        Posizione = Convert.ToInt32(reader["Posizione"].ToString()),
                        Nome = (String)reader["Nome"],
                        Cognome = (String)reader["Cognome"],
                        PuntiFatti = Convert.ToInt32(reader["PuntiFatti"].ToString()),
                        PuntiSubiti = Convert.ToInt32(reader["PuntiSubiti"].ToString()),
                        Campo = Convert.ToInt32(reader["Campo"].ToString())
                    });
                }

            }
            catch (Exception e)
            {

            }
            finally
            {
                c.Close();
            }

            return list;
        }
        public static List<AtletaEliminatorie> GetQuarti(int idTorneo, int idDisciplina, int campo)
        {
            String commandText = "SELECT a.Nome, a.Cognome, q.* from Atleti a join Qualificati8 q on a.Id = q.IdAtleta  WHERE IdTorneo = "
                + idTorneo + " and IdDisciplina = " + idDisciplina + " and Campo = " + campo + " ORDER BY Posizione ASC";

            SqlConnection c = null;
            List<AtletaEliminatorie> list = new List<AtletaEliminatorie>();

            try
            {
                c = new SqlConnection(Helper.GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new AtletaEliminatorie()

                    {
                        IdAtleta = Convert.ToInt32(reader["IdAtleta"].ToString()),
                        IdTorneo = Convert.ToInt32(reader["IdTorneo"].ToString()),
                        idDisciplina = Convert.ToInt32(reader["IdDisciplina"].ToString()),
                        Posizione = Convert.ToInt32(reader["Posizione"].ToString()),
                        Nome = (String)reader["Nome"],
                        Cognome = (String)reader["Cognome"],
                        PuntiFatti = Convert.ToInt32(reader["PuntiFatti"].ToString()),
                        PuntiSubiti = Convert.ToInt32(reader["PuntiSubiti"].ToString())
                    });
                }

            }
            catch (Exception e)
            {

            }
            finally
            {
                c.Close();
            }

            return list;
        }
        public static List<AtletaEliminatorie> GetQuarti(int idTorneo, int idDisciplina)
        {
            String commandText = "SELECT a.Nome, a.Cognome, q.* from Atleti a join Qualificati8 q on a.Id = q.IdAtleta" +
                " WHERE IdTorneo = " + idTorneo + " and IdDisciplina = " + idDisciplina + " ORDER BY Posizione";

            SqlConnection c = null;
            List<AtletaEliminatorie> list = new List<AtletaEliminatorie>();

            try
            {
                c = new SqlConnection(Helper.GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new AtletaEliminatorie()

                    {
                        IdAtleta = Convert.ToInt32(reader["IdAtleta"].ToString()),
                        IdTorneo = Convert.ToInt32(reader["IdTorneo"].ToString()),
                        idDisciplina = Convert.ToInt32(reader["IdDisciplina"].ToString()),
                        Posizione = Convert.ToInt32(reader["Posizione"].ToString()),
                        Nome = (String)reader["Nome"],
                        Cognome = (String)reader["Cognome"],
                        PuntiFatti = Convert.ToInt32(reader["PuntiFatti"].ToString()),
                        PuntiSubiti = Convert.ToInt32(reader["PuntiSubiti"].ToString()),
                        Campo = Convert.ToInt32(reader["Campo"].ToString())
                    });
                }

            }
            catch (Exception e)
            {
                return new List<AtletaEliminatorie>();
            }
            finally
            {
                if (c != null)
                    c.Close();
            }

            return list;
        }
        public static List<AtletaEliminatorie> GetOttavi(int idTorneo, int idDisciplina, int campo)
        {
            String commandText = "SELECT a.Nome, a.Cognome, q.* from Atleti a join Qualificati16 q on a.Id = q.IdAtleta  WHERE IdTorneo = " + idTorneo + " and IdDisciplina = " + idDisciplina + "and Campo = " + campo + " ORDER BY Posizione ASC";

            SqlConnection c = null;
            List<AtletaEliminatorie> list = new List<AtletaEliminatorie>();

            try
            {
                c = new SqlConnection(Helper.GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new AtletaEliminatorie()

                    {
                        IdAtleta = Convert.ToInt32(reader["IdAtleta"].ToString()),
                        IdTorneo = Convert.ToInt32(reader["IdTorneo"].ToString()),
                        idDisciplina = Convert.ToInt32(reader["IdDisciplina"].ToString()),
                        Posizione = Convert.ToInt32(reader["Posizione"].ToString()),
                        Nome = (String)reader["Nome"],
                        Cognome = (String)reader["Cognome"],
                        PuntiFatti = Convert.ToInt32(reader["PuntiFatti"].ToString()),
                        PuntiSubiti = Convert.ToInt32(reader["PuntiSubiti"].ToString())
                    });
                }

            }
            catch (Exception e)
            {

            }
            finally
            {
                c.Close();
            }

            return list;
        }
        public static List<AtletaEliminatorie> GetOttavi(int idTorneo, int idDisciplina)
        {
            String commandText = "SELECT a.Nome, a.Cognome, q.* from Atleti a join Qualificati16 q on a.Id = q.IdAtleta " +
                " where IdTorneo = " + idTorneo + " and IdDisciplina = " + idDisciplina + " order by Posizione";

            SqlConnection c = null;
            List<AtletaEliminatorie> list = new List<AtletaEliminatorie>();

            try
            {
                c = new SqlConnection(Helper.GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new AtletaEliminatorie()

                    {
                        IdAtleta = Convert.ToInt32(reader["IdAtleta"].ToString()),
                        IdTorneo = Convert.ToInt32(reader["IdTorneo"].ToString()),
                        idDisciplina = Convert.ToInt32(reader["IdDisciplina"].ToString()),
                        Posizione = Convert.ToInt32(reader["Posizione"].ToString()),
                        Nome = (String)reader["Nome"],
                        Cognome = (String)reader["Cognome"],
                        PuntiFatti = Convert.ToInt32(reader["PuntiFatti"].ToString()),
                        PuntiSubiti = Convert.ToInt32(reader["PuntiSubiti"].ToString()),
                        Campo = Convert.ToInt32(reader["Campo"].ToString())
                    });
                }

            }
            catch (Exception e)
            {

            }
            finally
            {
                c.Close();
            }

            return list;
        }
        public static List<AtletaEliminatorie> GetSedicesimi(int idTorneo, int idDisciplina)
        {
            List<AtletaEliminatorie> list = new List<AtletaEliminatorie>();

            String commandText = "select a.Nome, a.Cognome, q.* from Atleti a join Qualificati32 q on a.Id = q.IdAtleta " +
                                " where IdTorneo = " + idTorneo + " and IdDisciplina = " + idDisciplina + " order by Posizione";


            SqlConnection c = null;

            try
            {
                c = new SqlConnection(Helper.GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new AtletaEliminatorie()

                    {
                        IdAtleta = Convert.ToInt32(reader["IdAtleta"].ToString()),
                        IdTorneo = Convert.ToInt32(reader["IdTorneo"].ToString()),
                        idDisciplina = Convert.ToInt32(reader["IdDisciplina"].ToString()),
                        Posizione = Convert.ToInt32(reader["Posizione"].ToString()),
                        Nome = (String)reader["Nome"],
                        Cognome = (String)reader["Cognome"],
                        PuntiFatti = Convert.ToInt32(reader["PuntiFatti"].ToString()),
                        PuntiSubiti = Convert.ToInt32(reader["PuntiSubiti"].ToString())
                    });
                }

            }
            catch (Exception e)
            {

            }
            finally
            {
                c.Close();
            }

            return list;
        }
        public static List<GironiConclusi> GetClassificaGironi(int idTorneo, int idDisciplina)
        {

            List<GironiConclusi> gironiConclusi = new List<GironiConclusi>();

            String commandText = "SELECT a.Cognome, a.Nome, g.*, r.Posizionamento, r.Punteggio FROM Gironi g join Atleti a on a.Id = g.IdAtleta " +
                                    "JOIN Ranking r on r.IdAtleta = a.Id " +
                                    "WHERE g.IdTorneo = " + idTorneo + " and g.IdDisciplina = " + idDisciplina + " AND r.IdDisciplina = " + idDisciplina +
                                    " order by g.Differenziale desc, g.Vittorie desc, g.PuntiFatti desc , g.PuntiSubiti asc, r.Punteggio desc  ";

            SqlConnection c = null;

            try
            {
                c = new SqlConnection(Helper.GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    gironiConclusi.Add(new GironiConclusi()
                    {
                        IdTorneo = idTorneo,
                        IdDisciplina = idDisciplina,
                        IdGirone = Convert.ToInt32(reader["IdGirone"].ToString()),
                        IdAtleta = Convert.ToInt32(reader["IdAtleta"].ToString()),
                        Nome = (String)reader["Nome"],
                        Cognome = (String)reader["Cognome"],
                        Vittorie = Convert.ToInt32(reader["Vittorie"].ToString()),
                        Sconfitte = Convert.ToInt32(reader["Sconfitte"].ToString()),
                        PuntiFatti = Convert.ToInt32(reader["PuntiFatti"].ToString()),
                        PuntiSubiti = Convert.ToInt32(reader["PuntiSubiti"].ToString()),
                        Differenziale = Convert.ToDouble(reader["Differenziale"]),
                        Posizionamento = Convert.ToInt32(reader["Posizionamento"].ToString()),
                        Ranking = Convert.ToDouble(reader["Punteggio"].ToString())

                    });
                }

            }
            catch (Exception e)
            {

            }
            finally
            {
                c.Close();
            }

            return gironiConclusi;

        }

        internal static void DeletePoolsAndMatches(int idTorneo, int idDisciplina)
        {
            DeleteGironiIncontri(idTorneo, idDisciplina);
            DeleteGironi(idTorneo, idDisciplina);
        }

        private static void DeleteGironiIncontri(int idTorneo, int idDisciplina)
        {
            String commandText = "";


            commandText += "DELETE FROM  GironiIncontri " +
                            "WHERE IdTorneo = " + idTorneo + " AND IdDisciplina = " + idDisciplina + "";

            SqlConnection c = null;
            try
            {

                c = new SqlConnection(Helper.GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                int rowAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                c.Close();
            }
        }

        private static void DeleteGironi(int idTorneo, int idDisciplina)
        {
            String commandText = "";


            commandText += "DELETE FROM  Gironi " +
                            "WHERE IdTorneo = " + idTorneo + " AND IdDisciplina = " + idDisciplina + "";

            SqlConnection c = null;
            try
            {

                c = new SqlConnection(Helper.GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                int rowAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                c.Close();
            }
        }

        internal static void InserisciGironiIncontri(int idTorneo, int idDisciplina, List<MatchEntity> incontri, int idgirone)
        {
            String commandText = "";

            foreach (MatchEntity i in incontri)
                commandText += "INSERT INTO GironiIncontri (IdTorneo, IdDisciplina, IdAtletaRosso, PuntiAtletaRosso, IdAtletaBlu, PuntiAtletaBlu, NumeroGirone) " +
                                "VALUES(" + idTorneo + "," + idDisciplina + "," + i.IdRosso + ",0," + i.IdBlu + ",0, " + idgirone + ");";

            SqlConnection c = null;
            try
            {

                c = new SqlConnection(Helper.GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                int rowAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                c.Close();
            }

        }

        internal static void InsertAtletaInGirone(int idTorneo, int idDisciplina, int idGirone, int idAtleta)
        {
            String commandText = "INSERT INTO Gironi VALUES (" +
                                    idTorneo + "," + idDisciplina + "," + idGirone + "," + idAtleta + ",0,0,0,0,0,0)";

            SqlConnection c = null;
            try
            {

                c = new SqlConnection(Helper.GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                int rowAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                c.Close();
            }
        }

        public static void InsertFinali(List<AtletaEliminatorie> listAtleti)
        {
            String commandText = "";
            for (int i = 1; i <= 3; i++)
            {
                foreach (AtletaEliminatorie a in listAtleti)
                    commandText += "INSERT INTO Finali (IdAtleta, IdTorneo, IdDisciplina, PuntiFatti, PuntiSubiti, Posizione, Campo, Round) " +
                                    "VALUES (" + a.IdAtleta + ", " + a.IdTorneo + ", " + a.idDisciplina + ", " + "0,0," + a.Posizione + "," + a.Campo + "," + i + ");";
            }

            SqlConnection c = null;
            try
            {

                c = new SqlConnection(Helper.GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                int rowAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errore: " + ex.Message, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                c.Close();
            }
        }

        public static void InsertSemifinali(List<AtletaEliminatorie> listAtleti)
        {
            String commandText = "";

            foreach (AtletaEliminatorie a in listAtleti)
                commandText += "INSERT INTO Semifinali (IdAtleta, IdTorneo, IdDisciplina, PuntiFatti, PuntiSubiti, Posizione, Campo) " +
                                "VALUES (" + a.IdAtleta + ", " + a.IdTorneo + ", " + a.idDisciplina + ", " + "0,0," + a.Posizione + "," + a.Campo + ");";

            SqlConnection c = null;
            try
            {

                c = new SqlConnection(Helper.GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                int rowAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errore: " + ex.Message, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                c.Close();
            }
        }

        public static void InsertQuarti(List<AtletaEliminatorie> listAtleti)
        {
            String commandText = "";

            foreach (AtletaEliminatorie a in listAtleti)
                commandText += "INSERT INTO Qualificati8 (IdAtleta, IdTorneo, IdDisciplina, PuntiFatti, PuntiSubiti, Posizione, Campo) " +
                                "VALUES (" + a.IdAtleta + ", " + a.IdTorneo + ", " + a.idDisciplina + ", " + "0,0," + a.Posizione + "," + a.Campo + ");";

            SqlConnection c = null;
            try
            {

                c = new SqlConnection(Helper.GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                int rowAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errore: " + ex.Message, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                c.Close();
            }
        }

        public static void InsertOttavi(List<AtletaEliminatorie> listAtleti)
        {
            String commandText = "";

            foreach (AtletaEliminatorie a in listAtleti)
                commandText += "INSERT INTO Qualificati16(IdAtleta, IdTorneo, IdDisciplina, PuntiFatti, PuntiSubiti, Posizione, Campo) " +
                                "VALUES (" + a.IdAtleta + ", " + a.IdTorneo + ", " + a.idDisciplina + ", " + "0,0," + a.Posizione + "," + a.Campo + ");";

            SqlConnection c = null;
            try
            {

                c = new SqlConnection(Helper.GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                int rowAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errore: " + ex.Message, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                c.Close();
            }
        }

        public static void InsertSedicesimi(List<AtletaEliminatorie> listAtleti)
        {
            String commandText = "";

            foreach (AtletaEliminatorie a in listAtleti)
                commandText += "INSERT INTO Qualificati32(IdAtleta, IdTorneo, IdDisciplina, PuntiFatti, PuntiSubiti, Posizione, Campo) " +
                                "VALUES (" + a.IdAtleta + ", " + a.IdTorneo + ", " + a.idDisciplina + ", " + "0, 0," + a.Posizione + ",0);";

            SqlConnection c = null;
            try
            {

                c = new SqlConnection(Helper.GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                int rowAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errore: " + ex.Message, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                c.Close();
            }
        }

        public static bool DeleteAllFinali(int idTorneo, int idDisciplina)
        {
            String commandText = "DELETE FROM Finali WHERE IdTorneo = " + idTorneo + " and IdDisciplina = " + idDisciplina;

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

        public static bool DeleteAllSemifinali(int idTorneo, int idDisciplina)
        {
            String commandText = "DELETE FROM Semifinali WHERE IdTorneo = " + idTorneo + " and IdDisciplina = " + idDisciplina;

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

        public static bool DeleteAllQuarti(int idTorneo, int idDisciplina)
        {
            String commandText = "DELETE FROM Qualificati8 WHERE IdTorneo = " + idTorneo + " and IdDisciplina = " + idDisciplina;

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

        public static bool DeleteAllOttavi(int idTorneo, int idDisciplina)
        {
            String commandText = "DELETE FROM Qualificati16 WHERE IdTorneo = " + idTorneo + " and IdDisciplina = " + idDisciplina;

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

        public static bool DeleteAllSedicesimi(int idTorneo, int idDisciplina)
        {
            String commandText = "DELETE FROM Qualificati32 WHERE IdTorneo = " + idTorneo + " and IdDisciplina = " + idDisciplina;

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

        public static void DeleteAllPahases(int idTorneo, int idDisciplina)
        {
            DeleteAllSedicesimi(idTorneo, idDisciplina);
            DeleteAllOttavi(idTorneo, idDisciplina);
            DeleteAllQuarti(idTorneo, idDisciplina);
            DeleteAllSemifinali(idTorneo, idDisciplina);
            DeleteAllFinali(idTorneo, idDisciplina);
        }

        public static void DeleteAfterSedicesimi(int idTorneo, int idDisciplina)
        {
            DeleteAllOttavi(idTorneo, idDisciplina);
            DeleteAllQuarti(idTorneo, idDisciplina);
            DeleteAllSemifinali(idTorneo, idDisciplina);
            DeleteAllFinali(idTorneo, idDisciplina);
        }
        
        public static void DeleteAfterOttavi(int idTorneo, int idDisciplina)
        {
            DeleteAllQuarti(idTorneo, idDisciplina);
            DeleteAllSemifinali(idTorneo, idDisciplina);
            DeleteAllFinali(idTorneo, idDisciplina);
        }

        public static void DeleteAfterQuarti(int idTorneo, int idDisciplina)
        {
            DeleteAllSemifinali(idTorneo, idDisciplina);
            DeleteAllFinali(idTorneo, idDisciplina);
        }

        public static void DeleteAfterSeimifinali(int idTorneo, int idDisciplina)
        {
            DeleteAllFinali(idTorneo, idDisciplina);
        }

        public static bool EliminaFinaliByCampo(int idCampo, int idTorneo, int idDisciplina, int idAtleta)
        {
            String commandText = "DELETE FROM Finali where IdTorneo = " + idTorneo + " and IdDisciplina = " + idDisciplina + " and IdAtleta = " + idAtleta;

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

        public static bool EliminaSemifinaliByCampo(int idCampo, int idTorneo, int idDisciplina, int idAtleta)
        {
            String commandText = "DELETE FROM Semifinali where Campo = " + idCampo + " and IdTorneo = " + idTorneo + " and IdDisciplina = " + idDisciplina + " and IdAtleta = " + idAtleta;

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

        public static bool EliminaQuartiByCampo(int idCampo, int idTorneo, int idDisciplina)
        {
            String commandText = "DELETE FROM Qualificati8 where Campo = " + idCampo + " and IdTorneo = " + idTorneo + " and IdDisciplina = " + idDisciplina;

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

        public static bool EliminaOttaviByCampo(int idCampo, int idTorneo, int idDisciplina)
        {
            String commandText = "DELETE FROM Qualificati16 where Campo = " + idCampo + " and IdTorneo = " + idTorneo + " and IdDisciplina = " + idDisciplina;

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

        public static bool EliminaSedicesimiByCampo(int idCampo, int idTorneo, int idDisciplina)
        {
            String commandText = "DELETE FROM Qualificati32 where Campo = " + idCampo + " and IdTorneo = " + idTorneo + " and IdDisciplina = " + idDisciplina;

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

        public static void ConcludiGironi(int idTorneo, int idDisciplina)
        {
            String commandText = "UPDATE Gironi SET Concluso = 1 where IdTorneo = " + idTorneo + " and IdDisciplina = " + idDisciplina;


            SqlConnection c = null;

            try
            {
                c = new SqlConnection(Helper.GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                Int32 rowAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                c.Close();
            }
        }

        public static bool UpdateGironi(RisultatiIncontriGironi res, int idTorneo, int idDisciplina, int idGirone)
        {
            String commandText = "UPDATE Gironi SET Vittorie = " + res.Vittorie +
                                                    ", Sconfitte = " + res.Sconfitte +
                                                    ", PuntiFatti = " + res.PuntiFatti +
                                                    ", PuntiSubiti = " + res.PuntiSubiti +
                                                    ", Differenziale = @differenziale" +
                                                    " WHERE IdTorneo = " + idTorneo +
                                                    " and IdDisciplina = " + idDisciplina +
                                                    " and IdGirone = " + idGirone +
                                                    " and IdAtleta = " + res.idAtleta;

            SqlConnection c = null;

            try
            {
                c = new SqlConnection(Helper.GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                SqlParameter p = new SqlParameter("@differenziale", SqlDbType.Float, 0) { Value = res.Differenziale };

                command.Parameters.Add(p);

                Int32 rowAffected = command.ExecuteNonQuery();

                return rowAffected == 1;

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

        public static void UpdateFinali(int IdTorneo, int idDisciplina, int campo, int round, int idAtleta, int puntiFatti, int puntiSubiti)
        {
            String commandText = "UPDATE Finali SET PuntiFatti = " + puntiFatti + ", PuntiSubiti = " + puntiSubiti +
                                    " WHERE IdAtleta = " + idAtleta + 
                                    " and IdTorneo = " + IdTorneo + 
                                    " and IdDisciplina = " + idDisciplina +
                                    " and Round = " + round;

            SqlConnection c = null;

            try
            {
                c = new SqlConnection(Helper.GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                Int32 rowAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                c.Close();
            }
        }

        public static void UpdateSemifinali(int IdTorneo, int idDisciplina, int campo, int posizione, int idAtleta, int puntiFatti, int puntiSubiti)
        {
            String commandText = "UPDATE Semifinali SET PuntiFatti = " + puntiFatti + ", PuntiSubiti = " + puntiSubiti +
                                    " WHERE IdAtleta = " + idAtleta + " and IdTorneo = " + IdTorneo + " and IdDisciplina = " + idDisciplina;

            SqlConnection c = null;

            try
            {
                c = new SqlConnection(Helper.GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                Int32 rowAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                c.Close();
            }
        }

        /// <summary>
        /// update degli quarti
        /// </summary>
        /// <param name="IdTorneo"></param>
        /// <param name="idDisciplina"></param>
        /// <param name="campo"></param>
        /// <param name="idAtleta"></param>
        /// <param name="puntiFatti"></param>
        /// <param name="puntiSubiti"></param>
        public static void UpdateQualificati8(int IdTorneo, int idDisciplina, int campo, int posizione, int idAtleta, int puntiFatti, int puntiSubiti)
        {
            String commandText = "UPDATE Qualificati8 SET PuntiFatti = " + puntiFatti + ", PuntiSubiti = " + puntiSubiti +
                                    " WHERE IdAtleta = " + idAtleta + " and IdTorneo = " + IdTorneo + " and IdDisciplina = " + idDisciplina;

            SqlConnection c = null;

            try
            {
                c = new SqlConnection(Helper.GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                Int32 rowAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                c.Close();
            }
        }

        /// <summary>
        /// update dei ottavi
        /// </summary>
        /// <param name="IdTorneo"></param>
        /// <param name="idDisciplina"></param>
        /// <param name="campo"></param>
        /// <param name="idAtleta"></param>
        /// <param name="puntiFatti"></param>
        /// <param name="puntiSubiti"></param>
        public static void UpdateQualificati16(int IdTorneo, int idDisciplina, int campo, int posizione, int idAtleta, int puntiFatti, int puntiSubiti)
        {
            String commandText = "UPDATE Qualificati16 SET PuntiFatti = " + puntiFatti + ", PuntiSubiti = " + puntiSubiti +
                                    " WHERE IdAtleta = " + idAtleta + " and IdTorneo = " + IdTorneo + " and IdDisciplina = " + idDisciplina;

            SqlConnection c = null;

            try
            {
                c = new SqlConnection(Helper.GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                Int32 rowAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                c.Close();
            }
        }

        /// <summary>
        /// update dei sedicesimi
        /// </summary>
        /// <param name="IdTorneo"></param>
        /// <param name="idDisciplina"></param>
        /// <param name="campo"></param>
        /// <param name="idAtleta"></param>
        /// <param name="puntiFatti"></param>
        /// <param name="puntiSubiti"></param>
        public static void UpdateQualificati32(int IdTorneo, int idDisciplina, int campo, int posizione, int idAtleta, int puntiFatti, int puntiSubiti)
        {
            String commandText = "UPDATE Qualificati32 SET PuntiFatti = " + puntiFatti + ", PuntiSubiti = " + puntiSubiti +
                                    " WHERE IdAtleta = " + idAtleta + " and IdTorneo = " + IdTorneo + " and IdDisciplina = " + idDisciplina;

            SqlConnection c = null;

            try
            {
                c = new SqlConnection(Helper.GetConnectionString());

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                Int32 rowAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                c.Close();
            }
        }

        public static bool UpdateGironiIncontri(
            int idTorneo,
            int idDisciplina,
            int idGirone,
            int idAtletaRosso,
            int puntiRosso,
            int idAtletaBlu,
            int puntiBlu,
            bool doppiaMorte)
        {

            String commandText = "UPDATE GironiIncontri SET PuntiAtletaRosso = " + puntiRosso +
                                    ", PuntiAtletaBlu = " + puntiBlu +
                                    ", DoppiaMorte = " + (doppiaMorte ? 1 : 0) + " " +
                                    "WHERE IdTorneo = " + idTorneo + " and IdDisciplina = " + idDisciplina + " and NumeroGirone = " + idGirone + " " +
                                    "AND ((IdAtletaRosso = " + idAtletaRosso + " and IdAtletaBlu = " + idAtletaBlu
                                    + ") OR (IdAtletaRosso = " + idAtletaBlu + " and IdAtletaBlu = " + idAtletaRosso + "))";


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
