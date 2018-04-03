using Newtonsoft.Json;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.IO;

namespace WebApplication1.Models
{
    public class Connection : BaseEntity
    {
        private Connection()
        {
        }

        public string ConnectionId { get; set; }
        public string Code { get; set; }

        private static LiteCollection<Connection> OpenStore()
        {
            string fileName = "C:/Users/Gary/Desktop/LiteDB.db";
            LiteDatabase db = new LiteDatabase(fileName);
            return db.GetCollection<Connection>("connection");
        }

        public static Connection Get()
        {
            var collection = OpenStore();
            IEnumerable<Connection> connections = collection.Find(x => true);
            try
            {
                return connections.First();
            }
            catch
            {
                Connection connection = new Connection();
                connection.ConnectionId = "";
                connection.Code = "";
                collection.Insert(connection);
                return connection;
            }
        }

        public void Store()
        {
            var collection = OpenStore();
            collection.Update(this);
        }
    }
}