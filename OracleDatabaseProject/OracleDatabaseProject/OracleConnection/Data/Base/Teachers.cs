using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OracleDatabaseProject
{
    class Teachers : IInsertCommand
    {
        public int teacher_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public int professionally_active { get; set; }
        public int account_id { get; set; }

        public Teachers()
        {
            this.teacher_id = 0;
            this.first_name = string.Empty;
            this.last_name = string.Empty;
            this.professionally_active = 0;
            this.account_id = 0;
        }

        public override string ToString()
        {
            return this.teacher_id + ";" + this.first_name + ";" + this.last_name + ";" +
                this.professionally_active + ";" + this.account_id;
        }

        public string GetInsertString()
        {
            return "INSERT INTO Teachers VALUES(NULL,'" + this.first_name + "', '" + this.last_name + 
                "'," + + this.professionally_active + "," + account_id + ")";
        }
    }
}
