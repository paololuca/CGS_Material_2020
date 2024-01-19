using BusinessEntity.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resources
{
    public static class SqlDal_HemaRatings
    {
        
        public static bool InsertFightersIntoDB(List<HemaRatingsFighterEntity> hemaFigthers)
        {
            if (!DeleteFighters())
                return false;

            StringBuilder sb = new StringBuilder();

            foreach (var f in hemaFigthers)
            {
                //l'inserimento deve essere in delta
                sb.AppendLine("IF NOT EXISTS (SELECT * FROM HemaRatingsFighters WHERE Name = '" + f.Name + "')");
                sb.AppendLine("INSERT INTO HemaRatingsFighters VALUES (" + f.Id + " ," + f.IdClub.ToString() + ", '" + f.Name + "', '" + f.Nationality + "')");
                sb.AppendLine("");
            }

            SqlConnection connection = null;

            try
            {

                connection = new SqlConnection(Helper.GetConnectionString());

                connection.Open();

                SqlCommand command = new SqlCommand(sb.ToString(), connection);
                command.ExecuteNonQuery();

                SyncWithLocalFightersId();

                return true;

            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
        }
        public static bool InsertFightersIntoDB(HemaRatingsFighterEntity hemaFigthers)
        {

            StringBuilder sb = new StringBuilder();

                //l'inserimento deve essere in delta
                sb.AppendLine("IF NOT EXISTS (SELECT * FROM HemaRatingsFighters WHERE Name = '" + hemaFigthers.Name + "')");
                sb.AppendLine("INSERT INTO HemaRatingsFighters VALUES (" + hemaFigthers.Id + " ," + hemaFigthers.IdClub.ToString() + ", '" + hemaFigthers.Name + "', '" 
                    + hemaFigthers.Nationality + "')");
                sb.AppendLine("");

            SqlConnection connection = null;

            try
            {

                connection = new SqlConnection(Helper.GetConnectionString());

                connection.Open();

                SqlCommand command = new SqlCommand(sb.ToString(), connection);
                command.ExecuteNonQuery();

                SyncWithLocalFightersId();

                return true;

            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
        }
        public static bool SyncWithLocalFightersId()
        {
            //truncate table HemaRatings

            //insert into HemaRatings select a.Id as IdAtleta, h.Id as IdHemaRatings
            //from Atleti a join HemaRatingsFighters h on (a.Nome + ' ' + a.Cognome) = h.Name

            return true;
        }
        public static bool InsertClubsIntoDB(List<HemaRatingsClubEntity> hemaClubs)
        {
            if (!DeleteClubs())
                return false;

            StringBuilder sb = new StringBuilder();

            foreach (var c in hemaClubs)
            {
                //l'inserimento deve essere in delta
                sb.AppendLine("IF NOT EXISTS (SELECT * FROM HemaRatingsClub WHERE Name = '" + c.Name + "')");
                sb.AppendLine("INSERT INTO HemaRatingsClub VALUES (" +
                    c.Id.ToString() + ", '" + c.Name + "', '" + c.Country + "', '" + c.State + "', '" + c.City + "')");
                sb.AppendLine("");
            }

            SqlConnection connection = null;

            try
            {

                connection = new SqlConnection(Helper.GetConnectionString());

                connection.Open();

                SqlCommand command = new SqlCommand(sb.ToString(), connection);
                command.ExecuteNonQuery();

                return true;

            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
        }
        public static bool InsertClubsIntoDB(HemaRatingsClubEntity hemaClub)
        {

            StringBuilder sb = new StringBuilder();

            //l'inserimento deve essere in delta
            sb.AppendLine("IF NOT EXISTS (SELECT * FROM HemaRatingsClub WHERE Name = '" + hemaClub.Name + "')");
            sb.AppendLine("INSERT INTO HemaRatingsClub VALUES (" +
                hemaClub.Id.ToString() + ", '" + hemaClub.Name + "', '" + hemaClub.Country + "', '" + hemaClub.State + "', '" + hemaClub.City + "')");
            sb.AppendLine("");

            SqlConnection connection = null;

            try
            {

                connection = new SqlConnection(Helper.GetConnectionString());

                connection.Open();

                SqlCommand command = new SqlCommand(sb.ToString(), connection);
                command.ExecuteNonQuery();

                return true;

            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
        }
        public static bool DeleteFighters()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("TRUNCATE TABLE HemaRatingsFighters");

            SqlConnection connection = null;

            try
            {

                connection = new SqlConnection(Helper.GetConnectionString());

                connection.Open();

                SqlCommand command = new SqlCommand(sb.ToString(), connection);
                command.ExecuteNonQuery();

                return true;

            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
        }
        public static bool DeleteClubs()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("TRUNCATE TABLE HemaRatingsClub");

            SqlConnection connection = null;

            try
            {

                connection = new SqlConnection(Helper.GetConnectionString());

                connection.Open();

                SqlCommand command = new SqlCommand(sb.ToString(), connection);
                command.ExecuteNonQuery();

                return true;

            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
        }

        public static List<HemaRatingsClubEntity> GetClubsInvolved(int idTorneo, int idDisciplina)
        {
            List<HemaRatingsClubEntity> result = new List<HemaRatingsClubEntity>();

            String SqlText = "select distinct asd.Nome_ASD as Name " +
                            "from Atleti a join AtletiVsTorneoVsDiscipline atd on a.Id = atd.IdAtleta " +
                            "join TorneoVsDiscipline td on td.Id = atd.IdTorneoVsDiscipline " +
                            "join ASD asd on a.IdASD = asd.Id " +
                            "left outer join HemaRatings hr on hr.IdAtleta = a.Id " +
                            "where td.IdTorneo = " + idTorneo + " " +
                            "and td.IdDisciplina = " + idDisciplina + " " +
                            "order by Name ";

            SqlConnection connection = null;

            try
            {

                connection = new SqlConnection(Helper.GetConnectionString());

                connection.Open();

                SqlCommand command = new SqlCommand(SqlText, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                    result.Add(new HemaRatingsClubEntity()
                    {
                        Name = reader[0].ToString(),
                        Country = "",
                        State = "",
                        City = ""
                    });
                        

            }
            catch (Exception e)
            {
            }
            finally
            {
                connection.Close();
            }

            return result;

        }

        public static List<HemaRatingsFighterMatchEntity> GetFightersInvolved(int idTorneo, int idDisciplina)
        {
            List<HemaRatingsFighterMatchEntity> result = new List<HemaRatingsFighterMatchEntity>();

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("select(a.Nome + ' ' + a.Cognome) as Name, asd.Nome_ASD as Club, 'IT' as Nationality, a.Sesso as Gender, hr.IdHemaRatings as HemaRatingsId");
            sb.AppendLine("from Atleti a join AtletiVsTorneoVsDiscipline atd on a.Id = atd.IdAtleta");
            sb.AppendLine("join TorneoVsDiscipline td on td.Id = atd.IdTorneoVsDiscipline");
            sb.AppendLine("join ASD asd on a.IdASD = asd.Id");
            sb.AppendLine("left outer join HemaRatings hr on hr.IdAtleta = a.Id");
            sb.AppendLine("where td.IdTorneo = " + idTorneo + "");
            sb.AppendLine("and td.IdDisciplina = " + idDisciplina + "");
            sb.AppendLine("order by a.Cognome");

            SqlConnection connection = null;

            try
            {

                connection = new SqlConnection(Helper.GetConnectionString());

                connection.Open();

                SqlCommand command = new SqlCommand(sb.ToString(), connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                    result.Add(new HemaRatingsFighterMatchEntity()
                    {
                        Name = reader["Name"].ToString(),
                        Club = reader["Nome_ASD"].ToString(),
                        Nationality = reader["Nationality"].ToString(),
                        Gender = reader["Gender"].ToString(),
                        HemaRatingsId = Convert.ToInt32(reader["HemaRatingsId"])
                    });


            }
            catch (Exception e)
            {
            }
            finally
            {
                connection.Close();
            }


            return result;
        }
    }
}
