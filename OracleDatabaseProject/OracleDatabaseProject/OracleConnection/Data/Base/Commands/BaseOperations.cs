using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OracleDatabaseProject
{
    enum BaseObjectClasses { Accounts, Groups, Marks, Students, Subjects, Subjects_Teachers, Teachers };
    class BaseOperations
    {
        private List<string> m_insertTemplates;
        private bool m_isInsertTemplatesImported;
        private Random m_random;
        private CommandBuilder m_commandBuilder;
        private DatabaseData m_realDatabaseData;
        private DatabaseManager m_databaseManager;
        private bool m_isRealDatabaseLoaded;

        public BaseOperations()
        {
            this.m_insertTemplates = new List<string>();
            this.m_isInsertTemplatesImported = false;
            this.m_databaseManager = new DatabaseManager();
            this.m_realDatabaseData = new DatabaseData();
            this.m_isRealDatabaseLoaded = false;


            this.m_random = new Random();
            this.m_commandBuilder = new CommandBuilder();
        }

        public object GetItemFromDatabase(CommandArgument commandArgument, bool random)
        {
            string argumentTypeIdentifier = this.m_commandBuilder.GetArgumentTypeIdentifier(commandArgument.ArgumentType);
            string clearCommandArguments = commandArgument.ArgumentName.Replace(argumentTypeIdentifier, "").Replace("{", "").Replace("}", "");
            string[] baseIdentifiers = clearCommandArguments.Split('.');
            BaseObjectClasses baseObjectClass;
            if(!Enum.TryParse(baseIdentifiers[0], true, out baseObjectClass))
            {
                return null;
            }

            if (random)
            {
                if (!this.m_databaseManager.GenerateDatabase(2, 3, false, false))
                {
                    return null;
                }
            }

            int itemIndex = -1;
            IIdentifyBaseItem baseItem = null;
            switch (baseObjectClass)
            {
                case BaseObjectClasses.Accounts:
                    {
                        AccountsItemsIndex accountsIndex;
                        if (!Enum.TryParse(baseIdentifiers[1], true, out accountsIndex))
                        {
                            return null;
                        }
                        itemIndex = (int)accountsIndex;
                        if (random)
                        {
                            baseItem = this.m_databaseManager.DatabaseData.Accounts[0];
                        }
                        else
                        {
                            baseItem = this.m_realDatabaseData.Accounts[this.m_random.Next(0, this.m_realDatabaseData.Accounts.Count)];
                        }
                        break;
                    }
                case BaseObjectClasses.Groups:
                    {
                        GroupsItemsIndex groupsIndex;
                        if (!Enum.TryParse(baseIdentifiers[1], true, out groupsIndex))
                        {
                            return null;
                        }
                        itemIndex = (int)groupsIndex;
                        if (random)
                        {
                            baseItem = this.m_databaseManager.DatabaseData.Groups[0];
                        }
                        else
                        {
                            baseItem = this.m_realDatabaseData.Groups[this.m_random.Next(0, this.m_realDatabaseData.Groups.Count)];
                        }
                        break;
                    }
                case BaseObjectClasses.Marks:
                    {
                        MarksItemsIndex marksIndex;
                        if (!Enum.TryParse(baseIdentifiers[1], true, out marksIndex))
                        {
                            return null;
                        }
                        itemIndex = (int)marksIndex;
                        if (random)
                        {
                            baseItem = this.m_databaseManager.DatabaseData.Marks[0];
                        }
                        else
                        {
                            baseItem = this.m_realDatabaseData.Marks[this.m_random.Next(0, this.m_realDatabaseData.Marks.Count)];
                        }
                        break;
                    }
                case BaseObjectClasses.Students:
                    {
                        StudentsItemsIndex studentsIndex;
                        if (!Enum.TryParse(baseIdentifiers[1], true, out studentsIndex))
                        {
                            return null;
                        }
                        itemIndex = (int)studentsIndex;
                        if (random)
                        {
                            baseItem = this.m_databaseManager.DatabaseData.Students[0];
                        }
                        else
                        {
                            baseItem = this.m_realDatabaseData.Students[this.m_random.Next(0, this.m_realDatabaseData.Students.Count)];
                        }
                        break;
                    }
                case BaseObjectClasses.Subjects:
                    {
                        SubjectsItemsIndex subjectsIndex;
                        if (!Enum.TryParse(baseIdentifiers[1], true, out subjectsIndex))
                        {
                            return null;
                        }
                        itemIndex = (int)subjectsIndex;
                        if (random)
                        {
                            baseItem = this.m_databaseManager.DatabaseData.Subjects[0];
                        }
                        else
                        {
                            baseItem = this.m_realDatabaseData.Subjects[this.m_random.Next(0, this.m_realDatabaseData.Subjects.Count)];
                        }
                        break;
                    }
                case BaseObjectClasses.Subjects_Teachers:
                    {
                        Subjects_TeachersItemsIndex subjectsTeachersIndex;
                        if (!Enum.TryParse(baseIdentifiers[1], true, out subjectsTeachersIndex))
                        {
                            return null;
                        }
                        itemIndex = (int)subjectsTeachersIndex;
                        if (random)
                        {
                            baseItem = this.m_databaseManager.DatabaseData.Subjects_Teachers[0];
                        }
                        else
                        {
                            baseItem = this.m_realDatabaseData.Subjects_Teachers[this.m_random.Next(0, this.m_realDatabaseData.Subjects_Teachers.Count)];
                        }
                        break;
                    }
                case BaseObjectClasses.Teachers:
                    {
                        TeachersItemsIndex teachersIndex;
                        if (!Enum.TryParse(baseIdentifiers[1], true, out teachersIndex))
                        {
                            return null;
                        }
                        itemIndex = (int)teachersIndex;
                        if (random)
                        {
                            baseItem = this.m_databaseManager.DatabaseData.Teachers[0];
                        }
                        else
                        {
                            baseItem = this.m_realDatabaseData.Teachers[this.m_random.Next(0, this.m_realDatabaseData.Teachers.Count)];
                        }
                        break;
                    }
            }

            object result = null;
            if(baseItem != null && itemIndex != -1)
            {
                result = baseItem.GetItemBasedOnIndex(itemIndex);
            }

            return result;
        }

        public string GetRandomSelectCommand(TaskOwner taskType)
        {
            List<string> commands = this.GetSpecyficSelectTemplates(taskType);
            if(commands.Count == 0)
            {
                return string.Empty;
            }
            if(!this.m_isRealDatabaseLoaded)
            {
                if(!this.m_databaseManager.LoadDatabaseFromFiles())
                {
                    this.m_databaseManager.Clear();
                    return string.Empty;
                }
                this.m_realDatabaseData = new DatabaseData(this.m_databaseManager.DatabaseData);
                this.m_databaseManager.Clear();
                this.m_isRealDatabaseLoaded = true;
            }

            int randomCommandIndex = this.m_random.Next(0, commands.Count);
            string[] command_arguments = commands[randomCommandIndex].Split(':');
            this.m_commandBuilder.SetRawCommand(command_arguments[0], command_arguments[1]);
            for (int i = 0; i < this.m_commandBuilder.ArgumentsCount; i++)
            {
                int tempArgumentId = this.m_commandBuilder.CommandArguments[i].ArgumentId;
                object tempArgumentValue = this.GetItemFromDatabase(this.m_commandBuilder.CommandArguments[i], true);
                if(!this.m_commandBuilder.SetArgument(tempArgumentId, tempArgumentValue))
                {
                    return string.Empty;
                }
            }

            return this.m_commandBuilder.GetCommand();
        }

        private bool ImportAllSelectTemplates()
        {
            this.m_insertTemplates.Clear();
            List<string> tempInsertTemplates = new List<string>();
            if(!DataManager.Load(GlobalVariables.DatabaseCommandDirectory + "Select.txt", out tempInsertTemplates))
            {
                return false;
            }
            for (int i = 0; i < tempInsertTemplates.Count; i++)
            {
                if (!tempInsertTemplates[i].Contains("#") && tempInsertTemplates[i] != string.Empty)
                {
                    this.m_insertTemplates.Add(tempInsertTemplates[i]);
                }
            }
            this.m_isInsertTemplatesImported = true;
            return true;
        }

        private List<string> GetSpecyficSelectTemplates(TaskOwner taskType, bool reImport = false)
        {
            if(this.m_insertTemplates == null)
            {
                return new List<string>();
            }
            if(!this.m_isInsertTemplatesImported || reImport)
            {
                if(!this.ImportAllSelectTemplates())
                {
                    return new List<string>();
                }
            }

            List<string> result = new List<string>();
            string specyficType = Enum.GetName(typeof(TaskOwner), taskType).ToLower();
            for(int i=0; i<this.m_insertTemplates.Count; i++)
            {
                string[] templateData = this.m_insertTemplates[i].Split(':');
                if(templateData.Length > 0)
                {
                    if(templateData[0] == specyficType)
                    {
                        result.Add(templateData[1] + ":" + templateData[2]);
                    }
                }
            }
            return result;
        }
    }
}
