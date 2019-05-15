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

    class Accounts : IInsertCommand, IUpdateCommand
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

        public string GetUpdateString(object newItem, params int[] changedItemsIndex)
        {
            if(newItem == null)
            {
                return string.Empty;
            }
            if(!(newItem is Accounts))
            {
                return string.Empty;
            }
            Accounts account = (Accounts)newItem;
            string resultString = "UPDATE Accounts SET ";
            int updatedItemsCount = 0;
            for (int i = 0; i < changedItemsIndex.Length; i++)
            {
                string variableName = this.GetVariableNameFromIndex(changedItemsIndex[i]);
                if(variableName == string.Empty)
                {
                    continue;
                }
                string variableValue = this.GetVariableValueFromIndex(changedItemsIndex[i], true);
                if(variableValue == string.Empty)
                {
                    continue;
                }
                if(i != changedItemsIndex.Length - 1 && i != 0)
                {
                    resultString += ", ";
                }
                resultString += variableName + " = " + variableValue;
                updatedItemsCount++;
            }

            resultString += " WHERE account_id = " + account.account_id.ToString();

            if (updatedItemsCount == 0)
            {
                resultString = string.Empty;
            }

            return resultString;
        }

        public string GetVariableNameFromIndex(int variableIndex)
        {
            string[] variableNames = Enum.GetNames(typeof(AccountsItemsIndex));
            if (variableIndex >= variableNames.Length || variableIndex < 0)
            {
                return string.Empty;
            }
            return variableNames[variableIndex].ToLower();
        }

        public string GetVariableValueFromIndex(int variableIndex, bool addSql = false)
        {
            if (variableIndex >= Enum.GetNames(typeof(AccountsItemsIndex)).Length || variableIndex < 0)
            {
                return string.Empty;
            }
            AccountsItemsIndex itemIndex = (AccountsItemsIndex)variableIndex;
            string resultValue = string.Empty;
            bool resetValueSqlNeeded = false;
            switch (itemIndex)
            {
                case AccountsItemsIndex.ACCOUNT_ID:
                    resultValue = this.account_id.ToString();
                    break;
                case AccountsItemsIndex.CREATE_DATE:
                    resetValueSqlNeeded = true;
                    resultValue = this.create_date;
                    break;
                case AccountsItemsIndex.EMAIL:
                    resetValueSqlNeeded = true;
                    resultValue = this.email;
                    break;
                case AccountsItemsIndex.IS_TEACHER:
                    resultValue = this.is_teacher.ToString();
                    break;
                case AccountsItemsIndex.LOGIN:
                    resetValueSqlNeeded = true;
                    resultValue = this.login;
                    break;
                case AccountsItemsIndex.PASSWORD:
                    resetValueSqlNeeded = true;
                    resultValue = this.password;
                    break;
            }

            if(resetValueSqlNeeded && addSql)
            {
                resultValue = "'" + resultValue + "'";
            }

            return resultValue;
        }
    }
}
