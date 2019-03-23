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
            DebugManager.Instance.Save(GlobalVariables.DebugInfoLogsDirectory + "debugLog.txt");
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

        private async void button1_Click(object sender, EventArgs e)
        {
            string command = this.richTextBox1.Text;
            DebugManager.Instance.AddLog(command, this);
            bool status = await this.connectionManager.ExecuteCommandAsync(command);
            DebugManager.Instance.AddLog(status.ToString(), this);
        }
    }
}
