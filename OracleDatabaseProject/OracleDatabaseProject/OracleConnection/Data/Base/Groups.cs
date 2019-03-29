using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OracleDatabaseProject
{
    class Groups : IInsertCommand
    {
        public int group_id { get; set; }
        public string name { get; set; }

        public Groups()
        {
            this.group_id = 0;
            this.name = string.Empty;
        }

        public override string ToString()
        {
            return this.group_id + ";" + this.name;
        }

        public string GetInsertString()
        {
            return "INSERT INTO Groups VALUES(NULL,'" + this.name + "')";
        }
    }
}
