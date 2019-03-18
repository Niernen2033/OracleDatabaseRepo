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
            //DebugManager.Instance.RegisterToDisabledList(this);
        }

        public Task<bool> OpenConnectionAsync(string userId, string userPassword, string host, ushort port, string serviceName)
        {
            return Task.Run(() => this.OpenConnection(userId, userPassword, host, port, serviceName));
        }

        public Task<bool> OpenConnectionAsync(OracleConnectionData oracleConnectionData)
        {
            return Task.Run(() => this.OpenConnection(oracleConnectionData));
        }

        public bool OpenConnection(string userId, string userPassword, string host, ushort port, string serviceName)
        {
            if(this.m_connection.State != ConnectionState.Closed)
            {
                return true;
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
                DebugManager.Instance.Print(exc.Message, this);
                return false;
            }
            DebugManager.Instance.Print("Connection established (" + this.m_connection.ServerVersion + ")", this);
            return true;
        }

        public bool OpenConnection(OracleConnectionData oracleConnectionData)
        {
            return this.OpenConnection(oracleConnectionData.UserID, oracleConnectionData.Password, oracleConnectionData.Host, oracleConnectionData.Port, oracleConnectionData.ServiceName);
        }

        public bool CloseConnection(ushort waitTimeInSec = 0)
        {
            if(!this.IsOpen)
            {
                return true;
            }
            if(this.IsBusy && waitTimeInSec > 0)
            {
                Stopwatch timer = new Stopwatch();
                timer.Start();
                while (this.IsBusy)
                {
                    timer.Stop();
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
            return true;
        }

        public bool CreateTable()
        {
            if(!this.IsOpen)
            {
                return false;
            }
            using (OracleCommand cmd = new OracleCommand("create table footable(foocolum number)", this.m_connection))
            {
                int result = cmd.ExecuteNonQuery();
                DebugManager.Instance.Print(result.ToString(), this);
            }
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
