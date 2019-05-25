using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OracleDatabaseProject
{
    enum SubjectsItemsIndex
    {
        SUBJECT_ID,
        TITLE
    }

    class Subjects : IInsertCommand, IIdentifyBaseItem
    {
        public int subject_id { get; set; }
        public string title { get; set; }

        public Subjects()
        {
            this.subject_id = 0;
            this.title = string.Empty;
        }

        public static string GetSelectString()
        {
            return "SELECT * FROM Subjects";
        }

        public override string ToString()
        {
            return this.subject_id + ";" + this.title;
        }

        public string GetInsertString()
        {
            return "INSERT INTO Subjects VALUES(NULL,'" + this.title + "')";
        }

        public object GetItemBasedOnIndex(int index)
        {
            if (index < 0 || index >= Enum.GetValues(typeof(SubjectsItemsIndex)).Length)
            {
                return null;
            }
            SubjectsItemsIndex itemIndex = (SubjectsItemsIndex)index;
            object result = null;
            switch (itemIndex)
            {
                case SubjectsItemsIndex.SUBJECT_ID:
                    result = this.subject_id;
                    break;
                case SubjectsItemsIndex.TITLE:
                    result = this.title;
                    break;
            }
            return result;
        }
    }
}
