using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OracleDatabaseProject
{
    enum GroupsItemsIndex
    {
        GROUP_ID,
        NAME
    }

    class Groups : IInsertCommand, IIdentifyBaseItem
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

        public object GetItemBasedOnIndex(int index)
        {
            if (index < 0 || index >= Enum.GetValues(typeof(GroupsItemsIndex)).Length)
            {
                return null;
            }
            GroupsItemsIndex itemIndex = (GroupsItemsIndex)index;
            object result = null;
            switch (itemIndex)
            {
                case GroupsItemsIndex.GROUP_ID:
                    result = this.group_id;
                    break;
                case GroupsItemsIndex.NAME:
                    result = this.name;
                    break;
            }
            return result;
        }

        public static string GetSelectString()
        {
            return "SELECT * FROM Groups";
        }
    }
}
