using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OracleDatabaseProject
{
    enum TaskOwner
    {
        ADMIN,
        USER
    }

    class DBTask
    {
        public TaskOwner TaskOwner { get; private set; }
        public string Job { get; private set; }
        public int FreezeTime { get; private set; }

        public DBTask(TaskOwner taskOwner)
        {
            this.TaskOwner = taskOwner;
            this.Job = string.Empty;
            this.FreezeTime = 0;
        }

        public void SetJob(string job)
        {
            this.Job = job;
        }

        public void SetFreezeTime(int freezeTime)
        {
            this.FreezeTime = freezeTime;
        }
    }
}
