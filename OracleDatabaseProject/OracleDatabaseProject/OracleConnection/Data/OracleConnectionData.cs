﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OracleDatabaseProject
{
    public class OracleConnectionData
    {
        public string UserID { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public ushort Port { get; set; }
        public string ServiceName { get; set; }

        public OracleConnectionData()
        {
            this.UserID = string.Empty;
            this.Password = string.Empty;
            this.Host = string.Empty;
            this.Port = 0;
            this.ServiceName = string.Empty;
        }

        public OracleConnectionData(ref OracleConnectionData other)
        {
            this.UserID = string.Copy(other.UserID);
            this.Password = string.Copy(other.Password);
            this.Host = string.Copy(other.Host);
            this.Port = other.Port;
            this.ServiceName = string.Copy(other.ServiceName);
        }
    }
}
