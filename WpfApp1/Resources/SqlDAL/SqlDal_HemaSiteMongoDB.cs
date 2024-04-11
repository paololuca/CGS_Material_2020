using BusinessEntity.DAO;
using BusinessEntity.Entity;
using MongoDB.Bson;
using MongoDB.Driver;
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

    public static class SqlDal_HemaSiteMongoDB
    {
        

        static bool _hemaSiteActivated = Convert.ToBoolean(ConfigurationManager.AppSettings["HEMASITE"]);
        static MongoClient client = new MongoClient(_hemaConnectionString);
        static IMongoDatabase _database = client.GetDatabase("hemasite");


        public static void DeleteAllDocuments(string collectionName, string databaseName)
        {
            var client = new MongoClient(_hemaConnectionString);
            _database = client.GetDatabase(databaseName);
            var collection = _database.GetCollection<BsonDocument>(collectionName);
            var filter = Builders<BsonDocument>.Filter.Empty; // Filtro vuoto per selezionare tutti i documenti
            collection.DeleteMany(filter);
        }

        public static void TruncateAllTables()
        {

            // Elimina tutti i documenti dalle collezioni desiderate
            DeleteAllDocuments("TOURNAMENT", "hemasite");
            DeleteAllDocuments("POOLS_STATS", "hemasite");
            DeleteAllDocuments("POOLS_MATCHES", "hemasite");

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
            var collection = _database.GetCollection<BsonDocument>("POOLS_STATS");
            var filter = Builders<BsonDocument>.Filter.Eq("IdTorneo", idTorneo);
            collection.DeleteMany(filter);
        }

        public static void ClearStatisticsValue(int idTorneo, int idGirone)
        {
            var collection = _database.GetCollection<BsonDocument>("POOLS_STATS");
            var filter = Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Eq("IdTorneo", idTorneo),
                Builders<BsonDocument>.Filter.Eq("IdGirone", idGirone)
            );
            collection.DeleteMany(filter);
        }

        public static void ClearTournamentDescValue(int idTorneo)
        {
            var collection = _database.GetCollection<BsonDocument>("TOURNAMENT");
            var filter = Builders<BsonDocument>.Filter.Eq("IdTorneo", idTorneo);
            collection.DeleteMany(filter);
                        
        }

        public static void ClearPoolsMatchs(int idTorneo, int idGirone)
        {
            var collection = _database.GetCollection<BsonDocument>("POOLS_MATCHES");
            var filter = Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Eq("IdTorneo", idTorneo),
                Builders<BsonDocument>.Filter.Eq("IdGirone", idGirone)
            );
            collection.DeleteMany(filter);
        }

        public static void ClearPoolsMatchs(int idTorneo)
        {
            var collection = _database.GetCollection<BsonDocument>("POOLS_MATCHES");
            var filter = Builders<BsonDocument>.Filter.Eq("IdTorneo", idTorneo);
            collection.DeleteMany(filter);
        }

        public static void UpdateStatistics(int idTorneo, int idDisciplina, int idGirone)
        {
            if (!_hemaSiteActivated)
                return;

            ClearStatisticsValue(idTorneo, idGirone);

            //prendo tutti i valori post gironi...anche se non ho finito
            //TODO forse è da fare per girone....
            List<GironiConclusi> gironiConclusi = SqlDal_Pools.GetClassificaGironi(idTorneo, idDisciplina).Where(x => x.IdGirone == idGirone).ToList();

            foreach (var g in gironiConclusi)
            {
                var collection = _database.GetCollection<BsonDocument>("POOLS_STATS");

                var document = new BsonDocument
                {
                    { "Qualificato", g.Qualificato },
                    { "IdTorneo", g.IdTorneo },
                    { "IdDisciplina", g.IdDisciplina },
                    { "IdGirone", g.IdGirone },
                    { "IdAtleta", g.IdAtleta },
                    { "Cognome", g.Cognome },
                    { "Nome", g.Nome },
                    { "Vittorie", g.Vittorie },
                    { "Sconfitte", g.Sconfitte },
                    { "PuntiFatti", g.PuntiFatti },
                    { "PuntiSubiti", g.PuntiSubiti },
                    { "Differenziale", g.Differenziale },
                    { "Posizionamento", g.Posizionamento },
                    { "Ranking", g.Ranking }
                };

                var options = new InsertOneOptions { BypassDocumentValidation = true };
                collection.InsertOne(document, options);
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


            foreach (var m in matchList)
            {
                var collection = _database.GetCollection<BsonDocument>("POOLS_MATCHES");

                var document = new BsonDocument
                {
                    { "IdTorneo", idTorneo },
                    { "IdGirone", idGirone },
                    { "NomeAtletaRosso", m.NomeAtletaRosso},
                    { "PuntiRosso", m.PuntiRosso },
                    { "NomeAtletaBlu", m.NomeAtletaBlu },
                    { "PuntiBlu", m.PuntiBlu },
                    { "DoppiaMorte", m.DoppiaMorte }
                };

                var options = new InsertOneOptions { BypassDocumentValidation = true };
                collection.InsertOne(document, options);
            }
            

        }

        public static void UpdateTournamentDescription(int idTorneo, string desc, int pools)
        {
            if (!_hemaSiteActivated)
                return;

            ClearTournamentDescValue(idTorneo);

            var collection = _database.GetCollection<BsonDocument>("TOURNAMENT");

            var document = new BsonDocument
            {
                { "IdTorneo", idTorneo },
                { "Desc", desc },
                { "Pools", pools }
            };

            var options = new InsertOneOptions { BypassDocumentValidation = true };
            collection.InsertOne(document, options);
        }

        
    }
}
