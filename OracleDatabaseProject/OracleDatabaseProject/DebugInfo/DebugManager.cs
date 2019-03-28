using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Collections.ObjectModel;

namespace OracleDatabaseProject
{
    sealed class DebugManager
    {
        //singleton variables
        private static DebugManager m_instance = null;
        private static readonly object m_padlock = new object();

        //normal variables
#if DEBUG_MANAGER
        private bool m_available;
        private bool m_is_saving_enabled;
        private ushort m_save_logs_count_needed;
        private ushort m_save_logs_count;
        private string m_save_logs_path;
        private List<string> m_info_data;
        private List<object> m_debug_disabled_objects;
#endif //DEBUG_MANAGER

        public event EventHandler<DebugEventArgs> InfoLogAdded;

#if DEBUG_MANAGER
        public ReadOnlyCollection<string> GetInfoData { get { return this.m_info_data.AsReadOnly(); } }
#else //DEBUG_MANAGER
        public List<string> GetInfoData { get { return new List<string>(); } }
#endif //DEBUG_MANAGER

        public static DebugManager Instance
        {
            get
            {
                if (m_instance == null)
                {
                    lock (m_padlock)
                    {
                        if (m_instance == null)
                        {
                            m_instance = new DebugManager();
                        }
                    }
                }
                return m_instance;
            }
        }

        private DebugManager()
        {
#if DEBUG_MANAGER
            this.m_is_saving_enabled = false;
            this.m_save_logs_path = string.Empty;
            this.m_save_logs_count_needed = 1;
            this.m_save_logs_count = 0;
            this.m_available = false;
            this.m_debug_disabled_objects = new List<object>();
            this.m_info_data = new List<string>();
#endif //DEBUG_MANAGER
        }

        private void OnInfoLogAdded(DebugEventArgs e)
        {
#if DEBUG_MANAGER
            this.InfoLogAdded.Invoke(this, e);
#else //DEBUG_MANAGER
            return;
#endif //DEBUG_MANAGER
        }

        public void Enable()
        {
#if DEBUG_MANAGER
            this.m_available = true;
#else //DEBUG_MANAGER
            return;
#endif //DEBUG_MANAGER
        }

        public void ClearInfoPanel()
        {
#if DEBUG_MANAGER
            if(this.m_info_data != null)
            {
                this.m_info_data.Clear();
            }
#else //DEBUG_MANAGER
            return;
#endif //DEBUG_MANAGER
        }

        public void RegisterToDisabledList(object source)
        {
#if DEBUG_MANAGER
            if (source == null || this.m_debug_disabled_objects.Contains(source))
            {
                return;
            }
            this.m_debug_disabled_objects.Add(source);
#else //DEBUG_MANAGER
            return;
#endif //DEBUG_MANAGER
        }

        public void UnregisterFromDisabledList(object source)
        {
#if DEBUG_MANAGER
            if (source == null)
            {
                return;
            }
            this.m_debug_disabled_objects.Remove(source);
#else //DEBUG_MANAGER
            return;
#endif //DEBUG_MANAGER
        }

        public void AddLog(string info, object source)
        {
#if DEBUG_MANAGER
            if (!this.m_debug_disabled_objects.Contains(source))
            {
                if (this.CanIStoreDebugInfo())
                {
                    string sourceInfo = "NONE";
                    if(source != null)
                    {
                        sourceInfo = source.ToString().Replace("OracleDatabaseProject.", "");
                    }
                    this.m_info_data.Add(sourceInfo + "[" + DateTime.Now.ToLongTimeString() + "] => " + info);

                    DebugEventArgs eventArgs = new DebugEventArgs(this.m_info_data[this.m_info_data.Count - 1]);
                    this.OnInfoLogAdded(eventArgs);

                    if (this.m_is_saving_enabled)
                    {
                        this.m_save_logs_count++;
                        if (this.m_save_logs_count == this.m_save_logs_count_needed)
                        {
                            this.SaveLogs();
                            this.m_save_logs_count = 0;
                        }
                    }
                }
            }
#else //DEBUG_MANAGER
            return;
#endif //DEBUG_MANAGER
        }

        public void Disable()
        {
#if DEBUG_MANAGER
            this.m_available = false;
#else //DEBUG_MANAGER
            return;
#endif //DEBUG_MANAGER
        }

        public void EnableLogsSaving(string savePath, ushort logsCountPerSave = 1)
        {
#if DEBUG_MANAGER
            this.m_is_saving_enabled = true;
            this.m_save_logs_count_needed = logsCountPerSave;
            if(this.m_save_logs_count_needed == 0)
            {
                this.m_save_logs_count_needed = 1;
            }
            this.m_save_logs_count = 0;
            this.m_save_logs_path = savePath;
#else //DEBUG_MANAGER
            return;
#endif //DEBUG_MANAGER
        }

        public void DisableLogsSaving()
        {
#if DEBUG_MANAGER
            this.m_is_saving_enabled = false;
            this.m_save_logs_count_needed = 1;
            this.m_save_logs_count = 0;
            this.m_save_logs_path = string.Empty;
#else //DEBUG_MANAGER
            return;
#endif //DEBUG_MANAGER
        }

        private void SaveLogs()
        {
#if DEBUG_MANAGER
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(this.m_save_logs_path)))
                {
                    try
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(this.m_save_logs_path));
                    }
                    catch(Exception exc)
                    {
                        MessageBox.Show(exc.Message);
                        return;
                    }
                }
            }
            catch(Exception exc)
            {
                MessageBox.Show(exc.Message);
                return;
            }

            try
            {
                using (StreamWriter writer = new StreamWriter(this.m_save_logs_path))
                {
                    foreach(string infoLog in this.m_info_data)
                    {
                        try
                        {
                            writer.WriteLine(infoLog);
                        }
                        catch(Exception exc)
                        {
                            MessageBox.Show(exc.Message);
                            return;
                        }
                    }
                }
            }
            catch(Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
#else //DEBUG_MANAGER
            return;
#endif //DEBUG_MANAGER
        }

        private bool CanIStoreDebugInfo()
        {
#if DEBUG_MANAGER
            if (this.m_available && this.m_info_data != null)
            {
                return true;
            }
            else
            {
                return false;
            }
#else //DEBUG_MANAGER
            return true;
#endif //DEBUG_MANAGER
        }
    }

    sealed class DebugEventArgs : EventArgs
    {
        public string Log { get; private set; }

        public DebugEventArgs(string log)
        {
            this.Log = log;
        }
    }
}
