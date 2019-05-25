using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OracleDatabaseProject
{
    enum CommArgumentType { NONE = -1, INTEGER, STRING, DATE }

    class CommandBuilder
    {
        public string RawCommand { get; private set; }
        public string RawArguments { get; private set; }

        private Dictionary<CommArgumentType, string> m_argumentTypeIdentifiers;
        private List<CommandArgument> m_commandArguments;

        public int ArgumentsCount { get { return this.m_commandArguments.Count; } }
        public ReadOnlyCollection<CommandArgument> CommandArguments { get { return this.m_commandArguments.AsReadOnly(); } }

        public CommandBuilder()
        {
            this.RawCommand = string.Empty;
            this.RawArguments = string.Empty;
            this.m_commandArguments = new List<CommandArgument>();
            this.m_argumentTypeIdentifiers = new Dictionary<CommArgumentType, string>();
            this.SetAllDefaultArgumentTypeIdentifiers();
        }

        private void SetAllDefaultArgumentTypeIdentifiers()
        {
            this.m_argumentTypeIdentifiers.Add(CommArgumentType.NONE, "non_");
            this.m_argumentTypeIdentifiers.Add(CommArgumentType.INTEGER, "int_");
            this.m_argumentTypeIdentifiers.Add(CommArgumentType.STRING, "str_");
            this.m_argumentTypeIdentifiers.Add(CommArgumentType.DATE, "dat_");
        }

        public void SetArgumentTypeIdentifier(CommArgumentType argumentType, string identifier)
        {
            this.m_argumentTypeIdentifiers[argumentType] = identifier;
        }

        public string GetArgumentTypeIdentifier(CommArgumentType argumentType)
        {
            return string.Copy(this.m_argumentTypeIdentifiers[argumentType]);
        }

        private CommArgumentType DetermineArgumentType(string argument)
        {
            CommArgumentType result = CommArgumentType.NONE;
            if (argument.Contains(this.m_argumentTypeIdentifiers[CommArgumentType.INTEGER]))
            {
                result = CommArgumentType.INTEGER;
            }
            else if (argument.Contains(this.m_argumentTypeIdentifiers[CommArgumentType.STRING]))
            {
                result = CommArgumentType.STRING;
            }
            else if (argument.Contains(this.m_argumentTypeIdentifiers[CommArgumentType.DATE]))
            {
                result = CommArgumentType.DATE;
            }
            return result;
        }

        private void AnaliseRawData()
        {
            this.m_commandArguments.Clear();
            string[] tempArguments = this.RawArguments.Split(';');
            for (int i = 0; i < tempArguments.Length; i++)
            {
                string subArgument = string.Copy(tempArguments[i]);
                CommandArgument argument = new CommandArgument(i);
                argument.SetArgumentInfo(subArgument, this.DetermineArgumentType(subArgument));
                this.m_commandArguments.Add(argument);
            }
        }

        public string GetCommand()
        {
            if(!this.IsAllArgumentsSeted())
            {
                return string.Empty;
            }

            string command = string.Copy(this.RawCommand);

            for (int i = 0; i < this.m_commandArguments.Count; i++)
            {
                command = command.Replace(this.m_commandArguments[i].ArgumentName, this.m_commandArguments[i].ArgumentValue);
            }

            return command;
        }

        public bool SetArgument(int argumentId, object argumentValue)
        {
            if(argumentId < 0 || argumentId >= this.m_commandArguments.Count || argumentValue == null)
            {
                return false;
            }
            bool status = false;
            for (int i = 0; i < this.m_commandArguments.Count; i++)
            {
                if(this.m_commandArguments[i].ArgumentId == argumentId)
                {
                    status = this.m_commandArguments[i].SetArgument(argumentValue);
                    break;
                }
            }
            return status;
        }

        public bool SetArgument(string argumentName, object argumentValue)
        {
            bool status = false;
            for (int i = 0; i < this.m_commandArguments.Count; i++)
            {
                if (this.m_commandArguments[i].ArgumentName == argumentName)
                {
                    status = this.m_commandArguments[i].SetArgument(argumentValue);
                    break;
                }
            }
            return status;
        }

        public bool IsAllArgumentsSeted()
        {
            bool status = true;
            for (int i = 0; i < this.m_commandArguments.Count; i++)
            {
                if(!this.m_commandArguments[i].ArgumentSeted)
                {
                    status = false;
                    break;
                }
            }
            return status;
        }

        public void SetRawCommand(string rawCommand, string rawArguments)
        {
            this.RawCommand = rawCommand;
            this.RawArguments = rawArguments;
            this.AnaliseRawData();
        }

        public CommandBuilder(string rawCommand, string rawArguments)
        {
            this.RawCommand = rawCommand;
            this.RawArguments = rawArguments;
            this.m_commandArguments = new List<CommandArgument>();
            this.SetAllDefaultArgumentTypeIdentifiers();
            this.AnaliseRawData();
        }

        public CommandBuilder(CommandBuilder commandBuilder)
        {
            this.RawCommand = string.Empty;
            this.RawArguments = string.Empty;
            if(commandBuilder.RawCommand != null)
            {
                this.RawCommand = string.Copy(commandBuilder.RawCommand);
            }
            if (commandBuilder.RawArguments != null)
            {
                this.RawArguments = string.Copy(commandBuilder.RawArguments);
            }
            this.m_commandArguments = new List<CommandArgument>(commandBuilder.m_commandArguments);
            this.m_argumentTypeIdentifiers = new Dictionary<CommArgumentType, string>();
            this.SetAllDefaultArgumentTypeIdentifiers();
        }
    }


    class CommandArgument
    {
        public int ArgumentId { get; private set; }
        public CommArgumentType ArgumentType { get; private set; }
        public string ArgumentName { get; private set; }
        public string ArgumentValue { get; private set; }
        public bool ArgumentSeted { get; private set; }

        public void SetArgumentInfo(string argumentName, CommArgumentType argumentType = CommArgumentType.NONE)
        {
            this.ArgumentType = argumentType;
            this.ArgumentName = argumentName;
        }

        public bool SetArgument(object argumentValue)
        {
            if(this.ArgumentType == CommArgumentType.NONE || this.ArgumentName == string.Empty)
            {
                return false;
            }
            switch (this.ArgumentType)
            {
                case CommArgumentType.DATE:
                    this.ArgumentValue = "TO_DATE('" + argumentValue.ToString() + "', 'DD.MM.YYYY')";
                    break;
                case CommArgumentType.INTEGER:
                    this.ArgumentValue = argumentValue.ToString();
                    break;
                case CommArgumentType.STRING:
                    this.ArgumentValue = "'" + argumentValue.ToString() + "'";
                    break;
            }
            this.ArgumentSeted = true;
            return true;
        }

        public void ClearArgument()
        {
            this.ArgumentName = string.Empty;
            this.ArgumentType = CommArgumentType.NONE;
            this.ArgumentValue = string.Empty;
            this.ArgumentSeted = false;
        }

        public CommandArgument()
        {
            this.ArgumentId = 0;
            this.ArgumentType = CommArgumentType.NONE;
            this.ArgumentValue = string.Empty;
            this.ArgumentName = string.Empty;
            this.ArgumentSeted = false;
        }

        public CommandArgument(int argumentId)
        {
            this.ArgumentId = argumentId;
            this.ArgumentType = CommArgumentType.NONE;
            this.ArgumentValue = string.Empty;
            this.ArgumentName = string.Empty;
            this.ArgumentSeted = false;
        }

        public CommandArgument(CommandArgument commandArgument)
        {
            this.ArgumentId = commandArgument.ArgumentId;
            this.ArgumentType = commandArgument.ArgumentType;
            this.ArgumentSeted = commandArgument.ArgumentSeted;
            this.ArgumentValue = string.Empty;
            this.ArgumentName = string.Empty;
            if (commandArgument.ArgumentValue != null)
            {
                this.ArgumentValue = string.Copy(commandArgument.ArgumentValue);
            }
            if (commandArgument.ArgumentName != null)
            {
                this.ArgumentName = string.Copy(commandArgument.ArgumentName);
            }
        }
    }
}
