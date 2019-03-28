using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
            DebugManager.Instance.EnableLogsSaving(GlobalVariables.DebugInfoLogsDirectory + "debugLog.txt", 1);

            this.connectionManager = new OracleConnectionManager();

            this.FormClosing += Form1_FormClosing;
            DebugManager.Instance.InfoLogAdded += Instance_InfoLogAdded;

            //temp
            OracleConnectionData oData = new OracleConnectionData();
            if (XmlManager.Load<OracleConnectionData>(GlobalVariables.DefaultConnectionsDirectory + "pg_connection.xml", out oData))
            {
                this.connectionManager.SetConnectionData(oData);
            }
        }

        private void Instance_InfoLogAdded(object sender, DebugEventArgs e)
        {
            this.listBox1.Invoke(new Action(delegate ()
            {
                this.listBox1.Items.Add(e.Log);
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
                DebugManager.Instance.AddLog(status.ToString(), this);
            }
            this.EnableButtons(this.button1);
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            this.DisableButtons(this.button2);
            DatabaseManager databaseManager = new DatabaseManager();
            bool status = databaseManager.LoadDatabaseFromFiles();
            DebugManager.Instance.AddLog(status.ToString(), this);
            if(status)
            {
                foreach(Accounts gr in databaseManager.GetAccounts)
                {
                    string comm = gr.GetInsertString();
                    bool stat = await this.connectionManager.ExecuteCommandAsync(comm);
                    if (!stat)
                    {
                        DebugManager.Instance.AddLog("exc command fail", this);
                        break;
                    }
                }
            }
            this.EnableButtons(this.button2);
        }
    }
}
