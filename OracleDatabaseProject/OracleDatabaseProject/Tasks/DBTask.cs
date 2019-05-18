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
        STUDENT,
        TEACHER,
        ALL,
    }

    enum TaskJobType
    {
        NONE = -1,
        SELECT,
        INSERT,
        UPDATE,
    }

    class DBTask
    {
        public TaskOwner TaskOwner { get; set; }
        public TaskJobType TaskJobType { get; set; }
        public string Job { get; set; }
        public int FreezeTime { get; set; }
        public int ChanceForRollback { get; set; }

        public DBTask(DBTask dBTask)
        {
            this.TaskOwner = dBTask.TaskOwner;
            this.Job = string.Copy(dBTask.Job);
            this.FreezeTime = dBTask.FreezeTime;
            this.TaskJobType = dBTask.TaskJobType;
            this.ChanceForRollback = dBTask.ChanceForRollback;
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
            this.TaskJobType = TaskJobType.NONE;
            this.ChanceForRollback = 0;
        }

        private void GenerateRadnomAdminTask()
        {

        }

        private void GenerateRadnomStudentTask()
        {
            Random random = new Random();
            this.TaskJobType = TaskJobType.SELECT;
            this.FreezeTime = this.GetFreezeTimeInSec(random.Next(0, 30));
            this.ChanceForRollback = 0;
            List<string> possibleJobs = new List<string>()
            {
                "SELECT * FROM Marks WHERE ",
                "SELECT"
            };
            this.Job = possibleJobs[random.Next(0, possibleJobs.Count)];
        }

        private void GenerateRadnomTeacherTask()
        {

        }

        public void GenerateRandomTask()
        {
            switch(this.TaskOwner)
            {
                case TaskOwner.ADMIN:
                    this.GenerateRadnomAdminTask();
                    break;
                case TaskOwner.STUDENT:
                    this.GenerateRadnomStudentTask();
                    break;
                case TaskOwner.TEACHER:
                    this.GenerateRadnomTeacherTask();
                    break;
            }
        }
    }
}
