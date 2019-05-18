using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OracleDatabaseProject
{
    static class GlobalVariables
    {
        public static readonly string ProjectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "/";
        public static readonly string DefaultConnectionsDirectory = ProjectDirectory + "defaultConnections/";
        public static readonly string DebugInfoLogsDirectory = ProjectDirectory + "debugInfoLogs/";
        public static readonly string DatabaseCreateDirectory = ProjectDirectory + "databaseCreate/";
        public static readonly string DatabaseCommandDirectory = ProjectDirectory + "databaseCommands/";
        public static readonly string DataToGenerateDatabaseDirectory = DatabaseCreateDirectory + "dataToGenerate/";
        public static readonly string GeneratedDataDatabaseDirectory = DatabaseCreateDirectory + "generatedData/";
    }
}
