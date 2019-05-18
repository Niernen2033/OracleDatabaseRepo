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
        private DatabaseData m_databaseData;
        private bool m_isDatabaseLoaded;

        public BaseOperations()
        {
            this.m_insertTemplates = new List<string>();
            this.m_isInsertTemplatesImported = false;
            this.m_databaseData = new DatabaseData();
            this.m_isDatabaseLoaded = false;


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

            object result = null;
            switch(baseObjectClass)
            {
                case BaseObjectClasses.Accounts:
                    {
                        break;
                    }
                case BaseObjectClasses.Groups:
                    {
                        break;
                    }
                case BaseObjectClasses.Marks:
                    {
                        MarksItemsIndex itemIndex;
                        if (!Enum.TryParse(baseIdentifiers[1], true, out itemIndex))
                        {
                            return null;
                        }
                        if(random)
                        {

                        }
                        else
                        {
                            int markIndex = this.m_random.Next(0, this.m_databaseData.Marks.Count);
                            Marks mark = this.m_databaseData.Marks[markIndex];
                            result = mark.GetItemBasedOnIndex((int)itemIndex);
                        }
                        break;
                    }
                case BaseObjectClasses.Students:
                    {
                        break;
                    }
                case BaseObjectClasses.Subjects:
                    {
                        SubjectsItemsIndex itemIndex;
                        if (!Enum.TryParse(baseIdentifiers[1], true, out itemIndex))
                        {
                            return null;
                        }
                        if (random)
                        {

                        }
                        else
                        {
                            int subjectIndex = this.m_random.Next(0, this.m_databaseData.Subjects.Count);
                            Subjects subject = this.m_databaseData.Subjects[subjectIndex];
                            result = subject.GetItemBasedOnIndex((int)itemIndex);
                        }
                        break;
                    }
                case BaseObjectClasses.Subjects_Teachers:
                    {
                        break;
                    }
                case BaseObjectClasses.Teachers:
                    {
                        break;
                    }
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
            if(!this.m_isDatabaseLoaded)
            {
                DatabaseManager databaseManager = new DatabaseManager();
                if(!databaseManager.LoadDatabaseFromFiles())
                {
                    return string.Empty;
                }
                this.m_databaseData = new DatabaseData(databaseManager.DatabaseData);
                this.m_isDatabaseLoaded = true;
            }

            int randomCommandIndex = this.m_random.Next(0, commands.Count);
            string[] command_arguments = commands[randomCommandIndex].Split(':');
            this.m_commandBuilder.SetRawCommand(command_arguments[0], command_arguments[1]);
            for (int i = 0; i < this.m_commandBuilder.ArgumentsCount; i++)
            {
                int tempArgumentId = this.m_commandBuilder.CommandArguments[i].ArgumentId;
                object tempArgumentValue = this.GetItemFromDatabase(this.m_commandBuilder.CommandArguments[i], false);
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
                if (!tempInsertTemplates[i].Contains("#"))
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
