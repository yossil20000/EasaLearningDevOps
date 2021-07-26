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
        public DataBaseInfo DataBaseInfo { get; set; }
    }
    public sealed class DataBaseInfo
    {
        public DataBaseType DataBaseType { get; set; } = DataBaseType.UseSqlite;
        public string ConnectionString { get; set; }
        public string Source { get; set; } = "LearningQAContext.db";
    }
}
