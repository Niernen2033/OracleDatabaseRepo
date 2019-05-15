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
        public TaskOwner TaskOwner { get; set; }
        public string Job { get; set; }
        public int FreezeTime { get; set; }

        public DBTask(DBTask dBTask)
        {
            this.TaskOwner = dBTask.TaskOwner;
            this.Job = string.Copy(dBTask.Job);
            this.FreezeTime = dBTask.FreezeTime;
        }

        public int GetFreezeTimeInSec(int sec)
        {
            int targetSec = 0;
            if(sec > (int.MaxValue / 1000))
            {
                targetSec = int.MaxValue;
            }
            else
            {
                targetSec = sec * 1000;
            }
            return targetSec;
        }

        public DBTask(TaskOwner taskOwner)
        {
            this.TaskOwner = taskOwner;
            this.Job = string.Empty;
            this.FreezeTime = 0;
        }
    }
}
