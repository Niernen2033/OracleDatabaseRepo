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
        private List<DBTask> m_tasks;
        private OracleConnectionData m_connectionData;

        public DBTaskManager()
        {
            this.m_tasks = new List<DBTask>();
            this.m_connectionData = new OracleConnectionData();
        }

        public void AddTask(DBTask task)
        {
            this.m_tasks.Add(task);
        }

        public void ClearTasks()
        {
            this.m_tasks.Clear();
        }

        public void LoadConnectionData(OracleConnectionData oracleConnectionData)
        {
            this.m_connectionData = new OracleConnectionData(ref oracleConnectionData);
        }

        public Task StartAllTasksAsync()
        {
            return Task.Run(() => this.StartAllTasks());
        }

        public void StartAllTasks()
        {
            if (this.m_tasks.Count == 0 || this.m_tasks == null)
            {
                return;
            }
            Thread[] threads = new Thread[this.m_tasks.Count];
            for (int i = 0; i < this.m_tasks.Count; i++)
            {
                DBTask task = new DBTask(this.m_tasks[i]);
                threads[i] = new Thread(() => this.GetJob(task, this.m_connectionData));
                threads[i].Name = "Thread[" + i + "]";
            }
            for (int i = 0; i < this.m_tasks.Count; i++)
            {
                threads[i].Start();
            }
            int deadThreadsCount = 1;
            while (true)
            {
                for (int i = 0; i < threads.Length; i++)
                {
                    if (!threads[i].IsAlive)
                    {
                        deadThreadsCount++;
                    }
                }
                if (deadThreadsCount == threads.Length)
                {
                    return;
                }
                else
                {
                    deadThreadsCount = 1;
                }
            }
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
            DebugManager.Instance.AddLog(status.ToString(), null, true);
            if (freezeBefore == 1)
            {
                Thread.Sleep(task.FreezeTime);
            }
            connectionManager.CloseConnection();
        }
    }
}
