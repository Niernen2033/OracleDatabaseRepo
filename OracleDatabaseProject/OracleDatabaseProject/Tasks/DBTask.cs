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
        public List<string> Jobs { get; set; }
        public int FreezeTime { get; set; }
        public int ChanceForRollback { get; set; }

        public DBTask(DBTask dBTask)
        {
            this.TaskOwner = dBTask.TaskOwner;
            this.Jobs = new List<string>(dBTask.Jobs);
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
            this.Jobs = new List<string>();
            this.FreezeTime = 0;
            this.TaskJobType = TaskJobType.NONE;
            this.ChanceForRollback = 0;
        }

        private void GenerateRadnomAdminTask()
        {
            BaseOperations baseOperations = new BaseOperations();
            Random random = new Random();
            this.FreezeTime = this.GetFreezeTimeInSec(random.Next(0, 30));
            this.ChanceForRollback = 0;
            List<TaskJobType> possibleJobs = new List<TaskJobType>()
            {
                TaskJobType.SELECT,
                TaskJobType.UPDATE,
                TaskJobType.INSERT
            };
            TaskJobType jobType = possibleJobs[random.Next(0, possibleJobs.Count)];
            this.TaskJobType = jobType;
            bool jobMode = this.GetJobModeBasedOnType(jobType);
            this.Jobs = baseOperations.GetRandomCommand(jobType, TaskOwner.ADMIN, jobMode);
        }

        private void GenerateRadnomStudentTask()
        {
            BaseOperations baseOperations = new BaseOperations();
            Random random = new Random();
            this.FreezeTime = this.GetFreezeTimeInSec(random.Next(0, 30));
            this.ChanceForRollback = 0;
            List<TaskJobType> possibleJobs = new List<TaskJobType>()
            {
                TaskJobType.SELECT
            };
            TaskJobType jobType = possibleJobs[random.Next(0, possibleJobs.Count)];
            this.TaskJobType = jobType;
            bool jobMode = this.GetJobModeBasedOnType(jobType);
            this.Jobs = baseOperations.GetRandomCommand(jobType, TaskOwner.STUDENT, jobMode);
        }

        private bool GetJobModeBasedOnType(TaskJobType jobType)
        {
            bool result = false;
            if(jobType == TaskJobType.INSERT)
            {
                result = true;
            }
            return result;
        }

        private void GenerateRadnomTeacherTask()
        {
            BaseOperations baseOperations = new BaseOperations();
            Random random = new Random();
            this.FreezeTime = this.GetFreezeTimeInSec(random.Next(0, 30));
            this.ChanceForRollback = 0;
            List<TaskJobType> possibleJobs = new List<TaskJobType>()
            {
                TaskJobType.SELECT,
                TaskJobType.UPDATE,
                TaskJobType.INSERT
            };
            TaskJobType jobType = possibleJobs[random.Next(0, possibleJobs.Count)];
            this.TaskJobType = jobType;
            bool jobMode = this.GetJobModeBasedOnType(jobType);
            this.Jobs = baseOperations.GetRandomCommand(jobType, TaskOwner.TEACHER, jobMode);
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
