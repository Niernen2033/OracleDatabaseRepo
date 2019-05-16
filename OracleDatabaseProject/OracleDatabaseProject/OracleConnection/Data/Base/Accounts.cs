using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OracleDatabaseProject
{
    enum AccountsItemsIndex
    {
        ACCOUNT_ID,
        LOGIN,
        PASSWORD,
        EMAIL,
        IS_TEACHER,
        CREATE_DATE
    }

    class Accounts : IInsertCommand
    {
        public int account_id { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public int is_teacher { get; set; }
        public string create_date { get; set; }

        public Accounts()
        {
            this.account_id = 0;
            this.login = string.Empty;
            this.password = string.Empty;
            this.email = string.Empty;
            this.is_teacher = 0;
            this.create_date = string.Empty;
        }

        public override string ToString()
        {
            return this.account_id + ";" + this.login + ";" + this.password + ";" + this.email + ";" 
                + this.is_teacher + ";" + this.create_date;
        }

        public string GetInsertString()
        {
            return "INSERT INTO Accounts VALUES(NULL,'" + this.login + "', '" + this.password + "','"
                + this.email + "'," + this.is_teacher + ",TO_DATE('" + this.create_date  + "', 'DD.MM.YYYY')" + ")";
        }
    }
}
