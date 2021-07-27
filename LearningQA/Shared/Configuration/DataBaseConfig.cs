using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearningQA.Shared.Configuration
{
    public enum DataBaseType
    {
        UseSqlite,
        UseSqlServer
    }
    
    public sealed class DataBaseConfig
    {
        public static string ConfigSection = "DbConfig";
        public DataBaseType Provider { get; set; } = DataBaseType.UseSqlite;
        public ConnectionString ConnectionString { get; set; }
    }
    public sealed class ConnectionString
    {
        public string SqliteConnectionString { get; set; } = @"Data Source=.\LearningQAContext.db;Cache=Shared";
        public string SqlServerConnectionString { get; set; } = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=LearningQAContext;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
    }
}
