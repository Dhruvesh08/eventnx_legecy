using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class MongodbHelper
    {
        string connectionString = ConfigurationManager.AppSettings["mongoserver"].ToString();
        string dbName = ConfigurationManager.AppSettings["mongodb"].ToString();

        public void SaveData(string collection,string data)
        {
            var client = new MongoClient(connectionString);
            var db = client.GetDatabase(dbName);
            var mongoCollection = db.GetCollection<BsonDocument>(collection);
            var document = BsonSerializer.Deserialize<BsonDocument>(data);
            mongoCollection.InsertOne(document);
        }

        public void SaveMultipleData(string collection, List<string> data)
        {
            var client = new MongoClient(connectionString);
            var db = client.GetDatabase(dbName);
            var mongoCollection = db.GetCollection<BsonDocument>(collection);
            List<BsonDocument> documentList = new List<BsonDocument>();
            foreach(var d in data)
            {
                var document = BsonSerializer.Deserialize<BsonDocument>(d);
                documentList.Add(document);
            }
            mongoCollection.InsertMany(documentList);
        }

        public List<BsonDocument> FindDocuments(string collection, BsonDocument query)
        {
            var client = new MongoClient(connectionString);
            var db = client.GetDatabase(dbName);
            var mongoCollection = db.GetCollection<BsonDocument>(collection);
            return mongoCollection.Find(query).ToList();
        }

        public List<BsonDocument> GetAllRegistrationByEventId(string EventKey)
        {
            var filter = new BsonDocument("data.EventKey", EventKey);
            var documents = FindDocuments("EventRegistration", filter);
            return documents;
        }

        public BsonDocument GetUserDetailByEmail(string email,string EventKey)
        {
            Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
            keyValuePairs.Add("data.email", email);
            keyValuePairs.Add("data.EventKey", EventKey);
            var filter = new BsonDocument(keyValuePairs);
            var documents = FindDocuments("EventRegistration", filter).FirstOrDefault();
            return documents;
        }

        public string ToJson(BsonDocument bson)
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new BsonBinaryWriter(stream))
                {
                    BsonSerializer.Serialize(writer, typeof(BsonDocument), bson);
                }
                stream.Seek(0, SeekOrigin.Begin);
                using (var reader = new Newtonsoft.Json.Bson.BsonReader(stream))
                {
                    var sb = new StringBuilder();
                    var sw = new StringWriter(sb);
                    using (var jWriter = new JsonTextWriter(sw))
                    {
                        jWriter.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                        jWriter.WriteToken(reader);
                    }
                    return sb.ToString();
                }
            }
        }
    }
}
