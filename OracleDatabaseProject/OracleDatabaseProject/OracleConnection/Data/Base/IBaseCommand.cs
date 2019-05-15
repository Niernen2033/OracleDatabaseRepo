using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OracleDatabaseProject
{
    interface IInsertCommand
    {
        string GetInsertString();
    }

    interface IUpdateCommand
    {
        string GetUpdateString(object newItem, params int[] changedItemsIndex);
        string GetVariableNameFromIndex(int variableIndex);
        string GetVariableValueFromIndex(int variableIndex, bool addSql = false);
    }
}
