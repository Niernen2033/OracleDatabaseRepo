using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OracleDatabaseProject
{
    enum MarksItemsIndex
    {
        MARK_ID,
        STUDENT_ID,
        SUBJECT_ID,
        CREATE_DATE,
        MARK
    }

    class Marks : IInsertCommand, IIdentifyBaseItem
    {
        public int mark_id { get; set; }
        public int student_id { get; set; }
        public int subject_id { get; set; }
        public string create_date { get; set; }
        public int mark { get; set; }

        public Marks()
        {
            this.mark_id = 0;
            this.student_id = 0;
            this.subject_id = 0;
            this.create_date = string.Empty;
            this.mark = 0;
        }

        public override string ToString()
        {
            return this.mark_id + ";" + this.student_id + ";" + this.subject_id + ";" +
                this.create_date + ";" + this.mark;
        }

        public string GetInsertString()
        {
            return "INSERT INTO Marks VALUES(NULL," + this.student_id + ", " + this.subject_id + ", " +
                "TO_DATE('" + this.create_date + "', 'DD.MM.YYYY'), " + this.mark + ")";
        }

        public object GetItemBasedOnIndex(int index)
        {
            if (index < 0 || index >= Enum.GetValues(typeof(MarksItemsIndex)).Length)
            {
                return null;
            }
            MarksItemsIndex itemIndex = (MarksItemsIndex)index;
            object result = null;
            switch(itemIndex)
            {
                case MarksItemsIndex.CREATE_DATE:
                    result = this.create_date;
                    break;
                case MarksItemsIndex.MARK:
                    result = this.mark;
                    break;
                case MarksItemsIndex.MARK_ID:
                    result = this.mark_id;
                    break;
                case MarksItemsIndex.STUDENT_ID:
                    result = this.student_id;
                    break;
                case MarksItemsIndex.SUBJECT_ID:
                    result = this.subject_id;
                    break;
            }
            return result;
        }
    }
}
