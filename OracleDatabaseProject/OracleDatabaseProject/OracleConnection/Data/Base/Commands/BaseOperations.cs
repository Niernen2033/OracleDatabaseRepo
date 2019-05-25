using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace OracleDatabaseProject
{
    enum BaseObjectClasses { Accounts, Groups, Marks, Students, Subjects, Subjects_Teachers, Teachers };
    class BaseOperations
    {
        private CommandTemplate m_insertTemplates;
        private CommandTemplate m_updateTemplates;
        private CommandTemplate m_selectTemplates;

        private Random m_random;
        private CommandBuilder m_commandBuilder;
        private DatabaseData m_realDatabaseData;
        private DatabaseManager m_databaseManager;
        private OracleConnectionManager m_connectionManager;
        private bool m_isRealDatabaseLoaded;

        public BaseOperations()
        {
            this.m_insertTemplates = new CommandTemplate(TaskJobType.INSERT);
            this.m_updateTemplates = new CommandTemplate(TaskJobType.UPDATE);
            this.m_selectTemplates = new CommandTemplate(TaskJobType.SELECT);
            this.m_databaseManager = new DatabaseManager();
            this.m_realDatabaseData = new DatabaseData();
            this.m_connectionManager = new OracleConnectionManager(true);
            this.m_isRealDatabaseLoaded = false;


            this.m_random = new Random();
            this.m_commandBuilder = new CommandBuilder();
        }

        private object GetItemFromDatabase(CommandArgument commandArgument, TaskJobType jobType, bool random)
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
                while (true)
                {
                    if (!this.m_databaseManager.GenerateDatabase(20, 30, false, false))
                    {
                        return null;
                    }
                    if(this.m_databaseManager.DatabaseData.Teachers.Count != 0)
                    {
                        break;
                    }
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
                            Accounts item = (Accounts)baseItem;
                            if (jobType == TaskJobType.INSERT)
                            {
                                if(this.m_connectionManager.ExecuteCommand(Accounts.GetSelectString(), jobType))
                                {
                                    item.account_id = this.m_connectionManager.LastCommandResult + 1;
                                }
                            }
                            else
                            {
                                if (this.m_connectionManager.ExecuteCommand(Accounts.GetSelectString(), jobType))
                                {
                                    item.account_id = this.m_random.Next(0, this.m_connectionManager.LastCommandResult);
                                }
                            }
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
                            if (jobType == TaskJobType.INSERT)
                            {
                                Groups item = (Groups)baseItem;
                                if (this.m_connectionManager.ExecuteCommand(Groups.GetSelectString(), jobType))
                                {
                                    item.group_id = this.m_connectionManager.LastCommandResult + 1;
                                }
                            }
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
                            Marks item = (Marks)baseItem;
                            if (jobType == TaskJobType.INSERT)
                            {
                                if (this.m_connectionManager.ExecuteCommand(Marks.GetSelectString(), jobType))
                                {
                                    item.mark_id = this.m_connectionManager.LastCommandResult + 1;
                                    if (this.m_connectionManager.ExecuteCommand(Students.GetSelectString(), jobType))
                                    {
                                        item.student_id = this.m_random.Next(0, this.m_connectionManager.LastCommandResult);
                                    }
                                }
                            }
                            else
                            {
                                if (this.m_connectionManager.ExecuteCommand(Marks.GetSelectString(), jobType))
                                {
                                    item.mark_id = this.m_random.Next(0, this.m_connectionManager.LastCommandResult);
                                }
                            }
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
                            if (jobType == TaskJobType.INSERT)
                            {
                                Students item = (Students)baseItem;
                                if (this.m_connectionManager.ExecuteCommand(Students.GetSelectString(), jobType))
                                {
                                    item.student_id = this.m_connectionManager.LastCommandResult + 1;
                                }
                            }
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
                            if (jobType == TaskJobType.INSERT)
                            {
                                Subjects item = (Subjects)baseItem;
                                if (this.m_connectionManager.ExecuteCommand(Subjects.GetSelectString(), jobType))
                                {
                                    item.subject_id = this.m_connectionManager.LastCommandResult + 1;
                                }
                            }
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
                            if (jobType == TaskJobType.INSERT)
                            {
                                Teachers item = (Teachers)baseItem;
                                if (this.m_connectionManager.ExecuteCommand(Teachers.GetSelectString(), jobType))
                                {
                                    item.teacher_id = this.m_connectionManager.LastCommandResult + 1;
                                }
                            }
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

        public List<string> GetRandomCommand(TaskJobType jobType, TaskOwner taskOwnerType, bool random = false)
        {
            List<string> commands = this.GetSpecyficTemplates(jobType, taskOwnerType);
            if (commands.Count == 0)
            {
                return null;
            }
            if (!this.m_isRealDatabaseLoaded)
            {
                if (!this.m_databaseManager.LoadDatabaseFromFiles())
                {
                    this.m_databaseManager.Clear();
                    return null;
                }
                this.m_realDatabaseData = new DatabaseData(this.m_databaseManager.DatabaseData);
                this.m_databaseManager.Clear();
                this.m_isRealDatabaseLoaded = true;
            }

            List<string> result = new List<string>();
            int randomCommandIndex = this.m_random.Next(0, commands.Count);
            string[] multipleCommands = commands[randomCommandIndex].Split('&');
            CommandBuilder saveMultipleCommand = null;
            for (int i = 0; i < multipleCommands.Length; i++)
            {
                string[] command_arguments = multipleCommands[i].Split(':');
                this.m_commandBuilder.SetRawCommand(command_arguments[0], command_arguments[1]);
                bool commandStatus = true;
                for (int j = 0; j < this.m_commandBuilder.ArgumentsCount; j++)
                {
                    int tempArgumentId = this.m_commandBuilder.CommandArguments[j].ArgumentId;
                    object tempArgumentValue = null;
                    if (i != 0 && saveMultipleCommand != null)
                    {
                        for (int k = 0; k < saveMultipleCommand.ArgumentsCount; k++)
                        {
                            if (this.m_commandBuilder.CommandArguments[j].ArgumentName == saveMultipleCommand.CommandArguments[k].ArgumentName)
                            {
                                tempArgumentValue = saveMultipleCommand.CommandArguments[k].ArgumentValue;
                                break;
                            }
                        }
                    }
                    if(tempArgumentValue == null)
                    {
                        tempArgumentValue = this.GetItemFromDatabase(this.m_commandBuilder.CommandArguments[j], jobType, random);
                    }
                    if (!this.m_commandBuilder.SetArgument(tempArgumentId, tempArgumentValue))
                    {
                        commandStatus = false;
                        break;
                    }
                }
                if (commandStatus)
                {
                    saveMultipleCommand = new CommandBuilder(this.m_commandBuilder);
                    result.Add(this.m_commandBuilder.GetCommand());
                }
            }

            return result;
        }

        private bool ImportTemplates(TaskJobType jobType)
        {
            this.ClearTemplate(jobType);
            List<string> tempData = new List<string>();
            switch (jobType)
            {
                case TaskJobType.INSERT:
                    {
                        if (!DataManager.Load(GlobalVariables.DatabaseCommandDirectory + "Insert.txt", out tempData))
                        {
                            return false;
                        }
                        break;
                    }
                case TaskJobType.SELECT:
                    {
                        if (!DataManager.Load(GlobalVariables.DatabaseCommandDirectory + "Select.txt", out tempData))
                        {
                            return false;
                        }
                        break;
                    }
                case TaskJobType.UPDATE:
                    {
                        if (!DataManager.Load(GlobalVariables.DatabaseCommandDirectory + "Update.txt", out tempData))
                        {
                            return false;
                        }
                        break;
                    }
                default:
                    return false;
            }
            CommandTemplate commandTemplate = this.GetTemplateBasedOnType(jobType);
            if(commandTemplate == null)
            {
                return false;
            }

            List<string> tempTemplate = new List<string>();
            for (int i = 0; i < tempData.Count; i++)
            {
                if (!tempData[i].Contains("#") && tempData[i] != string.Empty)
                {
                    tempTemplate.Add(tempData[i]);
                }
            }
            commandTemplate.SetTemplates(tempTemplate);
            return true;
        }

        private void ClearTemplate(TaskJobType jobType)
        {
            switch (jobType)
            {
                case TaskJobType.INSERT:
                    this.m_insertTemplates.Clear();
                    break;
                case TaskJobType.SELECT:
                    this.m_selectTemplates.Clear();
                    break;
                case TaskJobType.UPDATE:
                    this.m_updateTemplates.Clear();
                    break;
            }
        }

        private void ClearAllTemplates()
        {
            this.ClearTemplate(TaskJobType.INSERT);
            this.ClearTemplate(TaskJobType.SELECT);
            this.ClearTemplate(TaskJobType.UPDATE);
        }

        private bool ImportAllTemplates()
        {
            this.ClearAllTemplates();
            bool insertSatus = this.ImportTemplates(TaskJobType.INSERT);
            bool selectSatus = this.ImportTemplates(TaskJobType.SELECT);
            bool updateSatus = this.ImportTemplates(TaskJobType.UPDATE);
            return (insertSatus && selectSatus && updateSatus);
        }

        private CommandTemplate GetTemplateBasedOnType(TaskJobType jobType)
        {
            CommandTemplate result = null;
            switch (jobType)
            {
                case TaskJobType.INSERT:
                    result = this.m_insertTemplates;
                    break;
                case TaskJobType.SELECT:
                    result = this.m_selectTemplates;
                    break;
                case TaskJobType.UPDATE:
                    result = this.m_updateTemplates;
                    break;
            }
            return result;
        }

        private List<string> GetSpecyficTemplates(TaskJobType jobType, TaskOwner taskType, bool reImport = false)
        {
            if(this.m_insertTemplates == null)
            {
                return new List<string>();
            }

            CommandTemplate commandTemplate = this.GetTemplateBasedOnType(jobType);
            if(commandTemplate == null)
            {
                return new List<string>();
            }

            if (!commandTemplate.IsLoaded || reImport)
            {
                if (!this.ImportTemplates(commandTemplate.TemplateType))
                {
                    return new List<string>();
                }
            }

            List<string> result = new List<string>();
            string specyficType = Enum.GetName(typeof(TaskOwner), taskType).ToLower();

            for (int i = 0; i < commandTemplate.Template.Count; i++)
            {
                string[] templateData = commandTemplate.Template[i].Split(':');
                if (templateData.Length > 0)
                {
                    if (templateData[0] == specyficType)
                    {
                        result.Add(commandTemplate.Template[i].Replace(templateData[0] + ":", ""));
                    }
                }
            }
            return result;
        }
    }
}
