using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OracleDatabaseProject
{
    class Subjects
    {
        public int subject_id { get; set; }
        public string title { get; set; }

        public Subjects()
        {
            this.subject_id = 0;
            this.title = string.Empty;
        }

        public override string ToString()
        {
            return this.subject_id + ";" + this.title;
        }
    }
}
