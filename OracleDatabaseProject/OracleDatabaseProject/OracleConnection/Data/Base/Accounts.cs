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

    class Accounts : IInsertCommand, IIdentifyBaseItem
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

        public object GetItemBasedOnIndex(int index)
        {
            if (index < 0 || index >= Enum.GetValues(typeof(AccountsItemsIndex)).Length)
            {
                return null;
            }
            AccountsItemsIndex itemIndex = (AccountsItemsIndex)index;
            object result = null;
            switch (itemIndex)
            {
                case AccountsItemsIndex.ACCOUNT_ID:
                    result = this.account_id;
                    break;
                case AccountsItemsIndex.CREATE_DATE:
                    result = this.create_date;
                    break;
                case AccountsItemsIndex.EMAIL:
                    result = this.email;
                    break;
                case AccountsItemsIndex.IS_TEACHER:
                    result = this.is_teacher;
                    break;
                case AccountsItemsIndex.LOGIN:
                    result = this.login;
                    break;
                case AccountsItemsIndex.PASSWORD:
                    result = this.password;
                    break;
            }
            return result;
        }
    }
}
