using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OracleDatabaseProject
{
    class Students : IInsertCommand
    {
        public int student_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public int student_index { get; set; }
        public int account_id { get; set; }

        public Students()
        {
            this.student_id = 0;
            this.first_name = string.Empty;
            this.last_name = string.Empty;
            this.student_index = 0;
            this.account_id = 0;
        }

        public override string ToString()
        {
            return this.student_id + ";" + this.first_name + ";" + this.last_name + ";" + 
                this.student_index + ";" + this.account_id;
        }

        public string GetInsertString()
        {
            return "INSERT INTO Students VALUES(NULL,'" + this.first_name + "', '" + this.last_name + "', "
                + this.student_index + ", " + this.account_id + ")";
        }
    }
}
