using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.Configuration;

namespace rct_lmis
{
    public class MongoDBConnection 
    {
        private static MongoDBConnection _instance;
        private static readonly object _lock = new object();
        private IMongoDatabase _database;

        private MongoDBConnection() 
        {
            var c = ConfigurationManager.AppSettings["mongodbcs"];
            var db = ConfigurationManager.AppSettings["dblmis"];
            var client = new MongoClient(c);
            _database = client.GetDatabase(db);
        }

        public static MongoDBConnection Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new MongoDBConnection();
                        }
                    }
                }
                return _instance;
            }
        }

        public IMongoDatabase Database
        {
            get { return _database; }
        }
    }
}
