using BusinessEntity.DAO;
using BusinessEntity.Entity;
using Resources;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Resources
{
    public static class SqlDal_HemaSite
    {
        static string _hemaConnectionString = ConfigurationManager.AppSettings["HEMASITEDataSource"].ToString();

        static bool _hemaSiteActivated = Convert.ToBoolean(ConfigurationManager.AppSettings["HEMASITE"]);

        public static void TruncateAllTables()
        {
            SqlConnection c = null;

            List<string> tables = new List<string>()
            {
                "TOURNAMENT",
                "POOLS_STATS",
                "POOLS_MATCHES"

            };

            foreach (var table in tables)
            {
                try
                {
                    string commandText = "TRUNCATE TABLE " + table;
                    c = new SqlConnection(_hemaConnectionString);

                    c.Open();

                    SqlCommand command = new SqlCommand(commandText, c);
                    command.ExecuteNonQuery();

                }
                catch (Exception e)
                {

                }
                finally
                {
                    c.Close();
                }
            }

        }

        public static void ClearAllTable(int idTorneo)
        {
            if (!_hemaSiteActivated)
                return;

            ClearStatisticsValue(idTorneo);
            ClearPoolsMatchs(idTorneo);
            ClearTournamentDescValue(idTorneo);
        }

        public static void ClearStatisticsValue(int idTorneo)
        {
            SqlConnection c = null;

            try
            {
                string commandText = "DELETE [POOLS_STATS] WHERE IdTorneo = " + idTorneo;
                c = new SqlConnection(_hemaConnectionString);

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                command.ExecuteNonQuery();

            }
            catch (Exception e)
            {

            }
            finally
            {
                c.Close();
            }
        }

        public static void ClearStatisticsValue(int idTorneo, int idGirone)
        {
            SqlConnection c = null;

            try
            {
                string commandText = "DELETE [POOLS_STATS] WHERE IdTorneo = " + idTorneo +" AND IdGirone = " + idGirone;
                c = new SqlConnection(_hemaConnectionString);

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                command.ExecuteNonQuery();

            }
            catch (Exception e)
            {

            }
            finally
            {
                c.Close();
            }
        }

        public static void ClearTournamentDescValue(int idTorneo)
        {
            SqlConnection c = null;

            try
            {
                string commandText = "DELETE [TOURNAMENT] WHERE IdTorneo = " + idTorneo;
                c = new SqlConnection(_hemaConnectionString);

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                command.ExecuteNonQuery();

            }
            catch (Exception e)
            {

            }
            finally
            {
                c.Close();
            }
        }

        public static void ClearPoolsMatchs(int idTorneo, int idGirone)
        {
            SqlConnection c = null;

            try
            {
                string commandText = "DELETE [POOLS_MATCHES] WHERE IdTorneo = " + idTorneo + " AND IdGirone = " + idGirone;
                c = new SqlConnection(_hemaConnectionString); 

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                command.ExecuteNonQuery();

            }
            catch (Exception e)
            {

            }
            finally
            {
                c.Close();
            }
        }

        public static void ClearPoolsMatchs(int idTorneo)
        {
            SqlConnection c = null;

            try
            {
                string commandText = "DELETE [POOLS_MATCHES] WHERE IdTorneo = " + idTorneo;
                c = new SqlConnection(_hemaConnectionString);

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                command.ExecuteNonQuery();

            }
            catch (Exception e)
            {

            }
            finally
            {
                c.Close();
            }
        }

        public static void UpdateStatistics(int idTorneo, int idDisciplina, int idGirone)
        {
            if (!_hemaSiteActivated)
                return;

            ClearStatisticsValue(idTorneo, idGirone);

            //prendo tutti i valori post gironi...anche se non ho finito
            //TODO forse è da fare per girone....
            List<GironiConclusi> gironiConclusi = SqlDal_Pools.GetClassificaGironi(idTorneo, idDisciplina).Where(x => x.IdGirone == idGirone).ToList();

            DataTable dataTable = ToDataTable(gironiConclusi);

            using (SqlConnection connection = new SqlConnection(_hemaConnectionString))
            {
                connection.Open();
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = "POOLS_STATS";
                    bulkCopy.WriteToServer(dataTable);
                }
            }
        }

        public static void UpdatePoolsMatchs(int idTorneo, int idGirone, DataGrid dataGridPool)
        {
            if (!_hemaSiteActivated)
                return;

            ClearPoolsMatchs(idTorneo, idGirone);

            List<MatchEntityPoolsMatches> matchList = new List<MatchEntityPoolsMatches>();

            foreach (MatchEntity match in dataGridPool.Items)
                matchList.Add(new MatchEntityPoolsMatches(match, idTorneo, idGirone));

            DataTable dataTable = ToDataTable(matchList);

            using (SqlConnection connection = new SqlConnection(_hemaConnectionString))
            {
                connection.Open();
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = "POOLS_MATCHES";
                    bulkCopy.WriteToServer(dataTable);
                }
            }

        }

        public static void UpdateTournamentDescription(int idTorneo, string desc, int pools)
        {
            if (!_hemaSiteActivated)
                return;

            ClearTournamentDescValue(idTorneo);

            SqlConnection c = null;

            try
            {
                string commandText = "INSERT INTO [TOURNAMENT] VALUES(" + idTorneo + ", '" + desc + "', " + pools + ")";
                c = new SqlConnection(_hemaConnectionString);

                c.Open();

                SqlCommand command = new SqlCommand(commandText, c);
                command.ExecuteNonQuery();

            }
            catch (Exception e)
            {

            }
            finally
            {
                c.Close();
            }
        }

        static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            // Ottieni tutte le proprietà pubbliche della classe
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Aggiungi le colonne al DataTable
            foreach (PropertyInfo property in properties)
            {
                dataTable.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
            }

            // Aggiungi le righe al DataTable
            foreach (T item in items)
            {
                DataRow row = dataTable.NewRow();
                foreach (PropertyInfo property in properties)
                {
                    row[property.Name] = property.GetValue(item) ?? DBNull.Value;
                }
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

        
    }
}
