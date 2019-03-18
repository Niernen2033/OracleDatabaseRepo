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
            DebugManager.Instance.Enable(ref this.listBox1);

            this.connectionManager = new OracleConnectionManager();
        }

        private async void button_ODBconnect_Click(object sender, EventArgs e)
        {
            this.button_ODBconnect.Enabled = false;
            OracleConnectionData oData = new OracleConnectionData();
            if (XmlManager.Load<OracleConnectionData>(GlobalVariables.ProjectDirectory + "/defaultConnections/pg_connection.xml", out oData))
            {
                bool openConn = await this.connectionManager.OpenConnectionAsync(oData);
                DebugManager.Instance.Print(this.connectionManager.IsOpen.ToString(), this);
            }
            this.button_ODBconnect.Enabled = true;
        }

        private void button_ODBdisconnect_Click(object sender, EventArgs e)
        {
            this.button_ODBdisconnect.Enabled = false;
            bool closeConn = connectionManager.CloseConnection(0);
            DebugManager.Instance.Print(this.connectionManager.IsOpen.ToString(), this);
            this.button_ODBdisconnect.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool status = this.connectionManager.CreateTable();
            DebugManager.Instance.Print(status.ToString(), this);
        }
    }
}
