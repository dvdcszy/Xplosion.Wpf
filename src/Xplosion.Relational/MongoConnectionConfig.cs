using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using static System.Configuration.ConfigurationManager;

namespace Xplosion.Relational
{
    public class MongoConnectionConfig
    {
        public IMongoCollection<MlParameters> ConnectAndGetCollection()
        {
            var dbClient = new MongoClient(ConnectionStrings["mongoDbLocalhost"].ConnectionString);

            var database = dbClient.GetDatabase("Hrsg_Formula_Xplosion");

            var collection =  database.GetCollection<MlParameters>("Parameters");

            return collection;

            //collection.InsertMany(insertList);
        }

        public List<MlParameters> GetParametersCollection(double pcs_id)
        {
            var filter = Builders<MlParameters>.Filter.Eq("project_id_pcs", pcs_id);

            return ConnectAndGetCollection().Find(filter).ToList();
        }

        public MlParameters GetAttribute(double pcs_id, string param)
        {
            var builder = Builders<MlParameters>.Filter;
            var filter = Builders<MlParameters>.Filter.And(builder.Eq("project_id_pcs", pcs_id), builder.Eq("parameter", param));

            return ConnectAndGetCollection().Find(filter).Single<MlParameters>();
        }
    }
}
