using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OracleDatabaseProject
{
    enum StudentsItemsIndex
    {
        STUDENT_ID,
        FIRST_NAME,
        LAST_NAME,
        STUDENT_INDEX,
        ACCOUNT_ID
    }

    class Students : IInsertCommand, IIdentifyBaseItem
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

        public object GetItemBasedOnIndex(int index)
        {
            if (index < 0 || index >= Enum.GetValues(typeof(StudentsItemsIndex)).Length)
            {
                return null;
            }
            StudentsItemsIndex itemIndex = (StudentsItemsIndex)index;
            object result = null;
            switch (itemIndex)
            {
                case StudentsItemsIndex.ACCOUNT_ID:
                    result = this.account_id;
                    break;
                case StudentsItemsIndex.FIRST_NAME:
                    result = this.first_name;
                    break;
                case StudentsItemsIndex.LAST_NAME:
                    result = this.last_name;
                    break;
                case StudentsItemsIndex.STUDENT_ID:
                    result = this.student_id;
                    break;
                case StudentsItemsIndex.STUDENT_INDEX:
                    result = this.student_index;
                    break;
            }
            return result;
        }
    }
}
