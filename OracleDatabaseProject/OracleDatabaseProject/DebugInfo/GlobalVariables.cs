﻿using System;
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
    }
}
