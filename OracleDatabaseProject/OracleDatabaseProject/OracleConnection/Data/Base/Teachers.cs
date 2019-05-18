using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OracleDatabaseProject
{
    enum TeachersItemsIndex
    {
        TEACHER_ID,
        FIRST_NAME,
        LAST_NAME,
        PROFESSIONALLY_ACTIVE,
        ACCOUNT_ID
    }

    class Teachers : IInsertCommand, IIdentifyBaseItem
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

        public object GetItemBasedOnIndex(int index)
        {
            if (index < 0 || index >= Enum.GetValues(typeof(TeachersItemsIndex)).Length)
            {
                return null;
            }
            TeachersItemsIndex itemIndex = (TeachersItemsIndex)index;
            object result = null;
            switch (itemIndex)
            {
                case TeachersItemsIndex.ACCOUNT_ID:
                    result = this.account_id;
                    break;
                case TeachersItemsIndex.FIRST_NAME:
                    result = this.first_name;
                    break;
                case TeachersItemsIndex.LAST_NAME:
                    result = this.last_name;
                    break;
                case TeachersItemsIndex.PROFESSIONALLY_ACTIVE:
                    result = this.professionally_active;
                    break;
                case TeachersItemsIndex.TEACHER_ID:
                    result = this.teacher_id;
                    break;
            }
            return result;
        }
    }
}
