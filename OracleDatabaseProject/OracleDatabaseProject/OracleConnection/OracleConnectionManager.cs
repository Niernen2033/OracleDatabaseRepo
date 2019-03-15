using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Diagnostics;

namespace OracleDatabaseProject
{
    sealed class OracleConnectionManager : IDisposable
    {
        private OracleConnection m_connection;

        public bool IsOpen
        {
            get
            {
                if (this.m_connection.State == ConnectionState.Closed)
                {
                    return false;
                }
                return true;
            }
        }

        public bool IsBusy
        {
            get
            {
                if (this.m_connection.State == ConnectionState.Connecting
                    || this.m_connection.State == ConnectionState.Executing
                    || this.m_connection.State == ConnectionState.Fetching)
                {
                    return true;
                }
                return false;
            }
        }


        public OracleConnectionManager()
        {
            this.m_connection = new OracleConnection();
        }

        public bool OpenLongConnection(string userId, string userPassword, string host, ushort port, string serviceName)
        {
            if(this.m_connection.State != ConnectionState.Closed)
            {
                return false;
            }

            OracleConnectionStringBuilder ocsb = new OracleConnectionStringBuilder();
            ocsb.UserID = userId;
            ocsb.Password = userPassword;
            ocsb.DataSource = host + ":" + port.ToString() + "/" + serviceName;
            this.m_connection.ConnectionString = ocsb.ConnectionString;
            try
            {
                this.m_connection.Open();
            }
            catch(Exception exc)
            {
                DebugManager.Instance.Log(exc.Message);
                return false;
            }
            DebugManager.Instance.Log("Connection established (" + this.m_connection.ServerVersion + ")");
            return true;
        }

        public bool OpenLongConnection(OracleConnectionData oracleConnectionData)
        {
            return this.OpenLongConnection(oracleConnectionData.UserID, oracleConnectionData.Password, oracleConnectionData.Host, oracleConnectionData.Port, oracleConnectionData.ServiceName);
        }

        public bool CloseConnection(ushort waitTimeInSec = 0)
        {
            if(!this.IsOpen)
            {
                this.m_connection.Dispose();
                return true;
            }
            if(this.IsBusy && waitTimeInSec > 0)
            {
                Stopwatch timer = new Stopwatch();
                timer.Start();
                while (true)
                {
                    timer.Stop();
                    if(!this.IsBusy)
                    {
                        break;
                    }

                    if (timer.ElapsedMilliseconds > (waitTimeInSec*1000))
                    {
                        break;
                    }
                    else
                    {
                        timer.Start();
                    }
                }
            }
            this.m_connection.Close();
            this.m_connection.Dispose();
            return true;
        }

        public void Dispose()
        {
            if(this.m_connection.State != ConnectionState.Closed)
            {
                this.m_connection.Close();
            }
            this.m_connection.Dispose();
        }
    }
}
