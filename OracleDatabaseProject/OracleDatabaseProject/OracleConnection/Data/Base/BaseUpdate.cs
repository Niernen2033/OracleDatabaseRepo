using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OracleDatabaseProject
{
    enum BaseObjectClasses { Accounts, Groups, Marks, Students, Subjects, Subjects_Teachers, Teachers };
    class BaseUpdate
    {
        private int m_itemToUpdateIndex;
        private BaseObjectClasses m_baseObjectClassType;
        private List<string> m_updateSetString;

        public BaseUpdate(BaseObjectClasses objectType)
        {
            this.m_itemToUpdateIndex = -1;
            this.m_baseObjectClassType = objectType;
            this.m_updateSetString = new List<string>();
        }

        public void SetItemIndexToUpdate(int updateItemIndex)
        {
            this.m_itemToUpdateIndex = updateItemIndex;
        }

        private string GetBaseObjectIdName()
        {
            switch (this.m_baseObjectClassType)
            {
                case BaseObjectClasses.Accounts:
                    return Enum.GetNames(typeof(AccountsItemsIndex))[(int)AccountsItemsIndex.ACCOUNT_ID];
                case BaseObjectClasses.Groups:
                    return Enum.GetNames(typeof(GroupsItemsIndex))[(int)GroupsItemsIndex.GROUP_ID];
                case BaseObjectClasses.Marks:
                    return Enum.GetNames(typeof(MarksItemsIndex))[(int)MarksItemsIndex.MARK_ID];
                case BaseObjectClasses.Students:
                    return Enum.GetNames(typeof(StudentsItemsIndex))[(int)StudentsItemsIndex.STUDENT_ID];
                case BaseObjectClasses.Subjects:
                    return Enum.GetNames(typeof(SubjectsItemsIndex))[(int)SubjectsItemsIndex.SUBJECT_ID];
                case BaseObjectClasses.Subjects_Teachers:
                    return Enum.GetNames(typeof(Subjects_TeachersItemsIndex))[(int)Subjects_TeachersItemsIndex.SUBJECT_ID];
                case BaseObjectClasses.Teachers:
                    return Enum.GetNames(typeof(TeachersItemsIndex))[(int)TeachersItemsIndex.TEACHER_ID];
            }
            return string.Empty;
        }

        public string GetUpdateString()
        {
            if(this.m_updateSetString.Count == 0 || this.m_itemToUpdateIndex < 0)
            {
                return string.Empty;
            }
            string resultString = "UPDATE " + Enum.GetNames(typeof(BaseObjectClasses))[(int)this.m_baseObjectClassType] + " SET ";
            for (int i = 0; i < this.m_updateSetString.Count; i++)
            {
                resultString += this.m_updateSetString[i];
            }
            resultString += " WHERE " + this.GetBaseObjectIdName() +  " = " + this.m_itemToUpdateIndex.ToString();
            return resultString;
        }

        public bool ChangeField(int fieldId, object fieldValue)
        {
            if (fieldValue == null || fieldId < 0)
            {
                return false;
            }

            string setString = string.Empty;
            switch (this.m_baseObjectClassType)
            {
                case BaseObjectClasses.Accounts:
                    {
                        AccountsItemsIndex itemIndex = (AccountsItemsIndex)fieldId;
                        string[] variableNames = Enum.GetNames(typeof(AccountsItemsIndex));
                        setString = variableNames[(int)itemIndex].ToLower() + " = " + fieldValue.ToString();
                        switch (itemIndex)
                        {
                            case AccountsItemsIndex.CREATE_DATE:
                                //TO_DATE('10.11.2013', 'DD.MM.YYYY')
                                setString = variableNames[(int)itemIndex].ToLower() + " = TO_DATE('" + fieldValue.ToString() + "', 'DD.MM.YYYY')";
                                break;
                            case AccountsItemsIndex.EMAIL:
                                setString = variableNames[(int)itemIndex].ToLower() + " = '" + fieldValue.ToString() + "'";
                                break;
                            case AccountsItemsIndex.IS_TEACHER:
                                setString = variableNames[(int)itemIndex].ToLower() + " = " + fieldValue.ToString();
                                break;
                            case AccountsItemsIndex.LOGIN:
                                setString = variableNames[(int)itemIndex].ToLower() + " = '" + fieldValue.ToString() + "'";
                                break;
                            case AccountsItemsIndex.PASSWORD:
                                setString = variableNames[(int)itemIndex].ToLower() + " = '" + fieldValue.ToString() + "'";
                                break;
                            default:
                                break;
                        }
                        break;
                    }
                case BaseObjectClasses.Groups:
                    break;
                case BaseObjectClasses.Marks:
                    break;
                case BaseObjectClasses.Students:
                    break;
                case BaseObjectClasses.Subjects:
                    break;
                case BaseObjectClasses.Subjects_Teachers:
                    break;
                case BaseObjectClasses.Teachers:
                    break;
            }

            if (setString != string.Empty)
            {
                if (this.m_updateSetString.Count != 0)
                {
                    setString = ", " + setString;
                }
                this.m_updateSetString.Add(setString);
                return true;
            }
            return false;
        }

    }
}
