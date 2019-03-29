using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OracleDatabaseProject
{
    class Subjects_Teachers : IInsertCommand
    {
        public int subject_id { get; set; }
        public int teacher_id { get; set; }
        public int group_id { get; set; }

        public Subjects_Teachers()
        {
            this.subject_id = 0;
            this.teacher_id = 0;
            this.group_id = 0;
        }

        public override string ToString()
        {
            return this.subject_id + ";" + this.teacher_id + ";" + this.group_id;
        }

        public string GetInsertString()
        {
            return "INSERT INTO Subjects_Teachers VALUES(" + this.subject_id + ", " + this.teacher_id + ", " +
                + this.group_id + ")";
        }
    }
}
