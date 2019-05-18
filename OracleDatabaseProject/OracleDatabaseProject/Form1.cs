using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OracleDatabaseProject
{
    public partial class Form1 : Form
    {
        private OracleConnectionManager connectionManager;

        public Form1()
        {
            InitializeComponent();
            DebugManager.Instance.Enable();
            DebugManager.Instance.EnableLogsSaving(GlobalVariables.DebugInfoLogsDirectory 
                + "debugLog[" + DateTime.Today.ToLongDateString() + "][" 
                + DateTime.Now.ToLongTimeString().Replace(":","-") + "].txt", 1);

            this.connectionManager = new OracleConnectionManager(true);

            this.FormClosing += Form1_FormClosing;
            DebugManager.Instance.InfoLogAdded += Instance_InfoLogAdded;

            /*
            Dictionary<int, int> numbers = new Dictionary<int, int>();
            Random a = new Random();
            RandomGauss b = new RandomGauss();
            for(int i=0; i<10000; i++)
            {
                int numb = (int)(b.Next());
                if(!numbers.ContainsKey(numb))
                {
                    numbers.Add(numb, 1);
                }
                else
                {
                    numbers[numb]++;
                }
            }
            DataManager.Save<int, int>(@"C:\Users\Michal\Desktop\rozklad.txt", numbers);
            this.listBox1.Items.Add("DONE");
            */
        }

        private void Instance_InfoLogAdded(object sender, DebugEventArgs e)
        {
            this.listBox1.Invoke(new Action(delegate ()
            {
                if(e.DebugLog.Sensitive) this.listBox1.Items.Add(this.listBox1.Items.Count +"|" + e.DebugLog.Log);
            }));
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.connectionManager.Dispose();
        }

        private void DisableButtons(params Button[] buttons)
        {
            foreach(Button button in buttons)
            {
                button.Enabled = false;
            }
        }

        private void EnableButtons(params Button[] buttons)
        {
            foreach (Button button in buttons)
            {
                button.Enabled = true;
            }
        }

        private async void button_ODBconnect_Click(object sender, EventArgs e)
        {
            this.button_ODBconnect.Enabled = false;
            this.DisableButtons(this.button_ODBconnect, this.button_ODBdisconnect);
            bool openConn = await this.connectionManager.OpenConnectionAsync();
            DebugManager.Instance.AddLog(this.connectionManager.IsOpen.ToString(), this);
            this.EnableButtons(this.button_ODBconnect, this.button_ODBdisconnect);
        }

        private void button_ODBdisconnect_Click(object sender, EventArgs e)
        {
            this.DisableButtons(this.button_ODBconnect, this.button_ODBdisconnect);
            bool closeConn = connectionManager.CloseConnection(0);
            DebugManager.Instance.AddLog(this.connectionManager.IsOpen.ToString(), this);
            this.EnableButtons(this.button_ODBconnect, this.button_ODBdisconnect);
        }

        private bool IsStringContainCommand(string data)
        {
            if (data.Contains("CREATE") || data.Contains("DROP")
                || data.Contains("__COMMNADS_END__"))
            {
                return true;
            }
            return false;
        }

        private List<string> ReadCommands(string data)
        {
            List<string> result = new List<string>();
            List<string> allLines = data.Split('\n').ToList();
            allLines.Add("__COMMNADS_END__");

            string command = string.Empty;
            for (int i = 0; i < allLines.Count; i++)
            {
                if (this.IsStringContainCommand(command) && this.IsStringContainCommand(allLines[i]))
                {
                    if (!command.Contains("END;"))
                    {
                        if (command.Length > 5)
                        {
                            int semiIndex = 0;
                            while (true)
                            {
                                semiIndex++;
                                string commandEnd = command.Substring(command.Length - semiIndex, semiIndex);
                                if (commandEnd.Contains(";"))
                                {
                                    break;
                                }
                            }
                            command = command.Substring(0, command.Length - semiIndex);
                        }
                    }
                    result.Add(command);
                    command = string.Empty;
                }
                command += allLines[i] + " ";
            }

            return result;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            List<string> commands = this.ReadCommands(this.richTextBox1.Text);
            this.DisableButtons(this.button1);
            foreach (string command in commands)
            {
                bool status = await this.connectionManager.ExecuteCommandAsync(command);
                DebugManager.Instance.AddLog(status.ToString(), this, true);
            }
            this.EnableButtons(this.button1);
        }

        private async void SendDatabase()
        {
            DatabaseManager databaseManager = new DatabaseManager();
            bool commandStatus = true;
            string command = string.Empty;
            bool databaseStatus = databaseManager.LoadDatabaseFromFiles();
            if (!databaseStatus)
            {
                return;
            }

            foreach (Groups item in databaseManager.DatabaseData.Groups)
            {
                string comm = item.GetInsertString();
                commandStatus = await this.connectionManager.ExecuteCommandAsync(comm);
                if (!commandStatus)
                {
                    DebugManager.Instance.AddLog("exc command fail", this);
                    break;
                }
            }
            DebugManager.Instance.AddLog("Result for Groups: " + commandStatus, this, true);
            if (!commandStatus) return;
            foreach (Subjects item in databaseManager.DatabaseData.Subjects)
            {
                string comm = item.GetInsertString();
                commandStatus = await this.connectionManager.ExecuteCommandAsync(comm);
                if (!commandStatus)
                {
                    DebugManager.Instance.AddLog("exc command fail", this);
                    break;
                }
            }
            DebugManager.Instance.AddLog("Result for Subjects: " + commandStatus, this, true);
            if (!commandStatus) return;
            foreach (Accounts item in databaseManager.DatabaseData.Accounts)
            {
                string comm = item.GetInsertString();
                commandStatus = await this.connectionManager.ExecuteCommandAsync(comm);
                if (!commandStatus)
                {
                    DebugManager.Instance.AddLog("exc command fail", this);
                    break;
                }
            }
            DebugManager.Instance.AddLog("Result for Accounts: " + commandStatus, this, true);
            if (!commandStatus) return;
            foreach (Teachers item in databaseManager.DatabaseData.Teachers)
            {
                string comm = item.GetInsertString();
                commandStatus = await this.connectionManager.ExecuteCommandAsync(comm);
                if (!commandStatus)
                {
                    DebugManager.Instance.AddLog("exc command fail", this);
                    break;
                }
            }
            DebugManager.Instance.AddLog("Result for Teachers: " + commandStatus, this, true);
            if (!commandStatus) return;
            foreach (Students item in databaseManager.DatabaseData.Students)
            {
                string comm = item.GetInsertString();
                commandStatus = await this.connectionManager.ExecuteCommandAsync(comm);
                if (!commandStatus)
                {
                    DebugManager.Instance.AddLog("exc command fail", this);
                    break;
                }
            }
            DebugManager.Instance.AddLog("Result for Students: " + commandStatus, this, true);
            if (!commandStatus) return;
            foreach (Marks item in databaseManager.DatabaseData.Marks)
            {
                string comm = item.GetInsertString();
                commandStatus = await this.connectionManager.ExecuteCommandAsync(comm);
                if (!commandStatus)
                {
                    DebugManager.Instance.AddLog("exc command fail", this);
                    break;
                }
            }
            DebugManager.Instance.AddLog("Result for Marks: " + commandStatus, this, true);
            if (!commandStatus) return;
            foreach (Subjects_Teachers item in databaseManager.DatabaseData.Subjects_Teachers)
            {
                string comm = item.GetInsertString();
                commandStatus = await this.connectionManager.ExecuteCommandAsync(comm);
                if (!commandStatus)
                {
                    DebugManager.Instance.AddLog("exc command fail", this);
                    break;
                }
            }
            DebugManager.Instance.AddLog("Result for Subjects_Teachers: " + commandStatus, this, true);
            if (!commandStatus) return;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DisableButtons(this.button2);
            //DatabaseManager databaseManager = new DatabaseManager();
            //bool status = databaseManager.GenerateDatabase(4000, 7000, true, true);
            //DebugManager.Instance.AddLog(status.ToString(), this, true);
            //this.SendDatabase();
            BaseOperations baseOperations = new BaseOperations();
            string a = baseOperations.GetRandomSelectCommand(TaskOwner.STUDENT);
            DebugManager.Instance.AddLog(a, this);
            this.listBox1.Items.Clear();
            this.EnableButtons(this.button2);
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            this.DisableButtons(this.button3);
            DBTaskManager dBTaskManager = new DBTaskManager();
            OracleConnectionData connectionData = new OracleConnectionData();
            if (!XmlManager.Load<OracleConnectionData>(GlobalVariables.DefaultConnectionsDirectory + "pg_connection.xml", out connectionData))
            {
                return;
            }
            else
            {
                dBTaskManager.LoadConnectionData(connectionData);
            }

            DatabaseManager databaseManager = new DatabaseManager();
            for (int i = 0; i < 50; i++)
            {
                DBTask dBTask = new DBTask(TaskOwner.STUDENT);
                dBTask.GenerateRandomTask();
                dBTaskManager.AddTask(dBTask);
            }

            await dBTaskManager.StartAllTasksAsync();

            this.EnableButtons(this.button3);
        }
    }
}
