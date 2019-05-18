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

    interface IIdentifyBaseItem
    {
        object GetItemBasedOnIndex(int index);
    }
}
