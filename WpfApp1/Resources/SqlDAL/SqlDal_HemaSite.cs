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

        public static void ClearAllTable(int idTorneo, int idDisciplina)
        {
            if (!_hemaSiteActivated)
                return;

            ClearStatisticsValue(idTorneo, idDisciplina);
            ClearPoolsMatchs(idTorneo, idDisciplina);
            ClearTournamentDescValue(idTorneo);
        }

        /// <summary>
        /// OK
        /// </summary>
        /// <param name="idTorneo"></param>
        /// <param name="idDisciplina"></param>
        public static void ClearStatisticsValue(int idTorneo, int idDisciplina)
        {
            SqlConnection c = null;

            try
            {
                string commandText = "DELETE [POOLS_STATS] WHERE IdTorneo = " + idTorneo + " AND IdDisciplina = " + idDisciplina;
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

        /// <summary>
        /// ok
        /// </summary>
        /// <param name="idTorneo"></param>
        /// <param name="idGirone"></param>
        /// <param name="idDisciplina"></param>
        public static void ClearStatisticsValue(int idTorneo, int idGirone, int idDisciplina)
        {
            SqlConnection c = null;

            try
            {
                string commandText = "DELETE [POOLS_STATS] WHERE IdTorneo = " + idTorneo + " AND IdGirone = " + idGirone + " AND IdDisciplina = " + idDisciplina;
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

        public static void ClearPoolsMatchs(int idTorneo, int idGirone, int idDisciplina)
        {
            SqlConnection c = null;

            try
            {
                string commandText = "DELETE [POOLS_MATCHES] WHERE IdTorneo = " + idTorneo + " AND IdGirone = " + idGirone + " AND IdDisciplina = " + idDisciplina;
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

        public static void ClearPoolsMatchs(int idTorneo, int idDisciplina)
        {
            SqlConnection c = null;

            try
            {
                string commandText = "DELETE [POOLS_MATCHES] WHERE IdTorneo = " + idTorneo + " AND IdDisciplina = " + idDisciplina;
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

            ClearStatisticsValue(idTorneo, idGirone, idDisciplina);

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

        public static void UpdatePoolsMatchs(int idTorneo, int idGirone, DataGrid dataGridPool, int idDisciplina)
        {
            if (!_hemaSiteActivated)
                return;

            ClearPoolsMatchs(idTorneo, idGirone, idDisciplina);

            List<MatchEntityPoolsMatches> matchList = new List<MatchEntityPoolsMatches>();

            foreach (MatchEntity match in dataGridPool.Items)
                matchList.Add(new MatchEntityPoolsMatches(match, idTorneo, idGirone, idDisciplina));

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
        /// <summary>
        /// ok
        /// </summary>
        /// <param name="idTorneo"></param>
        public static void UpdateTournamentDescription(int idTorneo)
        {
            if (!_hemaSiteActivated)
                return;

            var tournament = SqlDal_Tournaments.GetTorneoById(idTorneo);

            if (tournament == null)
                return;

            ClearTournamentDescValue(idTorneo);

            SqlConnection c = null;

            using (SqlConnection connection = new SqlConnection(_hemaConnectionString))
            {
                string query = @"INSERT INTO TOURNAMENT (IdTorneo, NomeTorneo, Luogo, DataInizio, DataFine)
                             VALUES (@IdTorneo, @NomeTorneo, @Luogo, @DataInizio, @DataFine)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Configura i parametri con i relativi tipi
                    command.Parameters.Add(new SqlParameter("@IdTorneo", SqlDbType.Int) { Value = idTorneo });
                    command.Parameters.Add(new SqlParameter("@NomeTorneo", SqlDbType.NVarChar, 50) { Value = tournament.Name });
                    command.Parameters.Add(new SqlParameter("@Luogo", SqlDbType.NVarChar, 50) { Value = tournament.Place ?? ""});
                    command.Parameters.Add(new SqlParameter("@DataInizio", SqlDbType.Date) { Value = tournament.StartDate });
                    command.Parameters.Add(new SqlParameter("@DataFine", SqlDbType.Date) { Value = tournament.EndDate });

                    // Apri la connessione e esegui il comando
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    Console.WriteLine($"{rowsAffected} riga(e) inserita(e).");
                }
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