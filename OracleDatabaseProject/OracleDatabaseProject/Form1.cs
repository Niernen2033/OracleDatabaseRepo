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
        public Form1()
        {
            InitializeComponent();

            DebugManager.Instance.Enable();
            DebugManager.Instance.SetInfoList(ref this.listBox1);

            OracleConnectionData oData = new OracleConnectionData();
            if(XmlManager.Load<OracleConnectionData>(GlobalVariables.ProjectDirectory + "/defaultConnections/my_connection.xml", out oData))
            {
                OracleConnectionManager connectionManager = new OracleConnectionManager();
                DebugManager.Instance.Log(connectionManager.IsOpen.ToString());
                connectionManager.OpenLongConnection(oData);
                DebugManager.Instance.Log(connectionManager.IsOpen.ToString());
                connectionManager.CloseConnection();
                DebugManager.Instance.Log(connectionManager.IsOpen.ToString());
            }
        }
    }
}
