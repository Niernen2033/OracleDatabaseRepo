using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OracleDatabaseProject
{
    class DBTaskManager
    {
        public DBTaskManager()
        {

        }

        public void GetJob(DBTask task, OracleConnectionData oracleConnectionData)
        {
            Random random = new Random();
            int freezeBefore = random.Next(0, 2);
            OracleConnectionManager connectionManager = new OracleConnectionManager();
            connectionManager.SetConnectionData(oracleConnectionData);

            if (!connectionManager.OpenConnection())
            {
                return;
            }
            if (freezeBefore == 0)
            {
                Thread.Sleep(task.FreezeTime);
            }
            bool status = connectionManager.ExecuteCommand(task.Job);
            DebugManager.Instance.AddLog(status.ToString(), this, true);
            if (freezeBefore == 1)
            {
                Thread.Sleep(task.FreezeTime);
            }
            connectionManager.CloseConnection();
        }
    }
}
