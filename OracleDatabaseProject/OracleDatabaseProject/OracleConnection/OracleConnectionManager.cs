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
    enum OracleTransactionEndStatus { O_ROLLBACK, O_COMMIT };

    sealed class OracleConnectionManager : IDisposable
    {
        private OracleConnection m_connection;
        private OracleConnectionData m_connectionData;
        private OracleTransaction m_transaction;
        public int LastCommandResult { get; private set; }

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


        public OracleConnectionManager(bool trySetDefaultConn = false)
        {
            this.m_connection = new OracleConnection();
            this.m_connectionData = null;
            this.m_transaction = null;
            this.LastCommandResult = 0;
            //DebugManager.Instance.RegisterToDisabledList(this);
            if(trySetDefaultConn)
            {
                if (!XmlManager.Load<OracleConnectionData>(GlobalVariables.DefaultConnectionsDirectory + "pg_connection.xml", out this.m_connectionData))
                {
                    this.m_connectionData = null;
                }
            }
        }

        public Task<bool> OpenConnectionAsync(string userId, string userPassword, string host, ushort port, string serviceName)
        {
            return Task.Run(() => this.OpenConnection(userId, userPassword, host, port, serviceName));
        }

        public Task<bool> OpenConnectionAsync(OracleConnectionData oracleConnectionData)
        {
            return Task.Run(() => this.OpenConnection(oracleConnectionData));
        }

        public Task<bool> OpenConnectionAsync()
        {
            if(this.m_connectionData == null)
            {
                return Task.FromResult<bool>(false);
            }
            return Task.Run(() => this.OpenConnection(this.m_connectionData));
        }

        public void SetConnectionData(OracleConnectionData oracleConnectionData)
        {
            this.m_connectionData = new OracleConnectionData(ref oracleConnectionData);
        }

        public bool OpenConnection()
        {
            if(this.m_connectionData == null)
            {
                return false;
            }
            return this.OpenConnection(this.m_connectionData);
        }

        public bool BeginTransaction()
        {
            if(this.m_connection == null || !this.IsOpen || this.m_transaction != null)
            {
                return false;
            }
            this.m_transaction = this.m_connection.BeginTransaction();
            return true;
        }

        public bool ExecuteCommandInTransaction(string comm, TaskJobType commType, ushort maxExecuteTimeInSec = 0)
        {
            if(this.m_transaction == null || this.m_connection == null)
            {
                return false;
            }

            this.LastCommandResult = 0;
            if (this.IsBusy)
            {
                this.BusyStatusWait(maxExecuteTimeInSec);
                if (this.IsBusy)
                {
                    return false;
                }
            }

            using (OracleCommand cmd = this.m_connection.CreateCommand())
            {
                this.LastCommandResult = 0;
                try
                {
                    cmd.Connection = this.m_connection;
                    cmd.Transaction = this.m_transaction;
                    DebugManager.Instance.AddLog("Executing: " + comm, this);
                    cmd.CommandText = comm;
                    if(commType == TaskJobType.NONE)
                    {
                        this.LastCommandResult = cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        OracleDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                this.LastCommandResult++;
                            }
                        }
                    }
                }
                catch (Exception exc)
                {
                    DebugManager.Instance.AddLog(exc.Message, this);
                    return false;
                }
                DebugManager.Instance.AddLog("Command result: " + this.LastCommandResult, this, true);
            }

            return true;
        }

        public bool EndTransaction(OracleTransactionEndStatus endStatus)
        {
            bool resultStatus = true;
            if (endStatus == OracleTransactionEndStatus.O_COMMIT)
            {
                try
                {
                    DebugManager.Instance.AddLog("Commit", this);
                    this.m_transaction.Commit();
                }
                catch(Exception excCommit)
                {
                    try
                    {
                        DebugManager.Instance.AddLog(excCommit.Message, this);
                        DebugManager.Instance.AddLog("Commit error | Rollback", this);
                        this.m_transaction.Rollback();
                    }
                    catch(Exception excRollback)
                    {
                        DebugManager.Instance.AddLog(excRollback.Message, this);
                        resultStatus = false;
                    }
                }
            }
            else if(endStatus == OracleTransactionEndStatus.O_ROLLBACK)
            {
                try
                {
                    DebugManager.Instance.AddLog("Rollback", this);
                    this.m_transaction.Rollback();
                }
                catch (Exception excRollback)
                {
                    DebugManager.Instance.AddLog(excRollback.Message, this);
                    resultStatus = false;
                }
            }
            if (this.m_transaction != null)
            {
                this.m_transaction.Dispose();
            }
            this.m_transaction = null;
            return resultStatus;
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
                DebugManager.Instance.AddLog(exc.Message, this);
                return false;
            }
            DebugManager.Instance.AddLog("Connection established (" + this.m_connection.ServerVersion + ")", this, true);
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
                this.BusyStatusWait(waitTimeInSec);
            }
            this.m_connection.Close();
            DebugManager.Instance.AddLog("Connection closed", this, true);
            return true;
        }

        private void BusyStatusWait(ushort waitTimeInSec)
        {
            if(waitTimeInSec == 0)
            {
                return;
            }
            Stopwatch timer = new Stopwatch();
            timer.Start();
            while (this.IsBusy)
            {
                timer.Stop();
                if (timer.ElapsedMilliseconds > (waitTimeInSec * 1000))
                {
                    break;
                }
                else
                {
                    timer.Start();
                }
            }
        }

        public bool ExecuteCommand(string comm, TaskJobType commType, ushort maxExecuteTimeInSec = 0)
        {
            bool isThisFunctionOpenConnection = false;
            this.LastCommandResult = 0;
            if (!this.IsOpen)
            {
                if(this.m_connectionData == null)
                {
                    return false;
                }
                if(!this.OpenConnection(this.m_connectionData))
                {
                    return false;
                }
                else
                {
                    DebugManager.Instance.AddLog("ExecuteCommand connection is open", this);
                    isThisFunctionOpenConnection = true;
                }
            }

            if (this.IsBusy)
            {
                this.BusyStatusWait(maxExecuteTimeInSec);
                if (this.IsBusy)
                {
                    if (isThisFunctionOpenConnection)
                    {
                        this.CloseConnection();
                    }
                    return false;
                }
            }

            DebugManager.Instance.AddLog("Executing: " + comm, this);
            try
            {
                using (OracleCommand cmd = new OracleCommand(comm, this.m_connection))
                {
                    if (commType == TaskJobType.NONE)
                    {
                        this.LastCommandResult = cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        OracleDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                this.LastCommandResult++;
                            }
                        }
                    }
                }
            }
            catch(Exception exc)
            {
                DebugManager.Instance.AddLog(exc.Message, this);
                if (isThisFunctionOpenConnection)
                {
                    this.CloseConnection();
                }
                return false;
            }
            DebugManager.Instance.AddLog("Command result: " + this.LastCommandResult, this, true);
            if (isThisFunctionOpenConnection)
            {
                DebugManager.Instance.AddLog("ExecuteCommand : close connection", this);
                this.CloseConnection();
            }
            
            return true;
        }

        public Task<bool> ExecuteCommandAsync(string comm, TaskJobType commType, ushort maxExecuteTimeInSec = 0)
        {
            return Task.Run(() => this.ExecuteCommand(comm, commType, maxExecuteTimeInSec));
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
