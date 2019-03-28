using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OracleDatabaseProject
{
    class Marks
    {
        public int mark_id { get; set; }
        public int student_id { get; set; }
        public int subject_id { get; set; }
        public string create_date { get; set; }
        public int mark { get; set; }

        public Marks()
        {
            this.mark_id = 0;
            this.student_id = 0;
            this.subject_id = 0;
            this.create_date = string.Empty;
            this.mark = 0;
        }

        public override string ToString()
        {
            return this.mark_id + ";" + this.student_id + ";" + this.subject_id + ";" +
                this.create_date + ";" + this.mark;
        }
    }
}
