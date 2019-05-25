using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OracleDatabaseProject
{
    enum Subjects_TeachersItemsIndex
    {
        SUBJECT_ID,
        TEACHER_ID,
        GROUP_ID
    }

    class Subjects_Teachers : IInsertCommand, IIdentifyBaseItem
    {
        public int subject_id { get; set; }
        public int teacher_id { get; set; }
        public int group_id { get; set; }

        public Subjects_Teachers()
        {
            this.subject_id = 0;
            this.teacher_id = 0;
            this.group_id = 0;
        }

        public static string GetSelectString()
        {
            return "SELECT * FROM Subjects_Teachers";
        }

        public override string ToString()
        {
            return this.subject_id + ";" + this.teacher_id + ";" + this.group_id;
        }

        public string GetInsertString()
        {
            return "INSERT INTO Subjects_Teachers VALUES(" + this.subject_id + ", " + this.teacher_id + ", " +
                + this.group_id + ")";
        }

        public object GetItemBasedOnIndex(int index)
        {
            if (index < 0 || index >= Enum.GetValues(typeof(Subjects_TeachersItemsIndex)).Length)
            {
                return null;
            }
            Subjects_TeachersItemsIndex itemIndex = (Subjects_TeachersItemsIndex)index;
            object result = null;
            switch (itemIndex)
            {
                case Subjects_TeachersItemsIndex.GROUP_ID:
                    result = this.group_id;
                    break;
                case Subjects_TeachersItemsIndex.SUBJECT_ID:
                    result = this.subject_id;
                    break;
                case Subjects_TeachersItemsIndex.TEACHER_ID:
                    result = this.teacher_id;
                    break;
            }
            return result;
        }
    }
}
