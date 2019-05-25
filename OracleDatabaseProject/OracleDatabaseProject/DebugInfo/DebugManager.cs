using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using System.Threading;

namespace OracleDatabaseProject
{
    sealed class DebugLog
    {
        public string Log { get; private set; }
        public bool Sensitive { get; private set; }

        public DebugLog(string log, bool isSensitive)
        {
            this.Log = log;
            this.Sensitive = isSensitive;
        }

        public DebugLog(DebugLog other)
        {
            if (other != null)
            {
                this.Log = string.Copy(other.Log);
                this.Sensitive = other.Sensitive;
            }
            else
            {
                this.Log = string.Empty;
                this.Sensitive = false;
            }
        }
    }

    sealed class DebugEventArgs : EventArgs
    {
        public DebugLog DebugLog { get; private set; }

        public DebugEventArgs(DebugLog log)
        {
            this.DebugLog = new DebugLog(log);
        }
    }

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
        private List<DebugLog> m_info_data;
        private List<object> m_debug_disabled_objects;
#endif //DEBUG_MANAGER

        public event EventHandler<DebugEventArgs> InfoLogAdded;

#if DEBUG_MANAGER
        public ReadOnlyCollection<DebugLog> GetInfoData { get { return this.m_info_data.AsReadOnly(); } }
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
            this.m_info_data = new List<DebugLog>();
#endif //DEBUG_MANAGER
        }

        private void OnInfoLogAdded(DebugEventArgs e)
        {
#if DEBUG_MANAGER
            this.InfoLogAdded?.Invoke(this, e);
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

        public void AddLog(string info, object source, bool sensitive=false)
        {
#if DEBUG_MANAGER
            if (!this.m_debug_disabled_objects.Contains(source))
            {
                if (this.CanIStoreDebugInfo())
                {
                    string sourceInfo = "";
                    if(source != null)
                    {
                        sourceInfo += source.ToString().Replace("OracleDatabaseProject.", "");
                    }
                    if(Thread.CurrentThread.Name != string.Empty)
                    {
                        sourceInfo += "[" + Thread.CurrentThread.Name + "]";
                    }
                    this.m_info_data.Add(new DebugLog(sourceInfo + "[" + DateTime.Now.ToLongTimeString() + "] => " + info, sensitive));

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

        public void EnableAndSetLogsSaving(string savePath, ushort logsCountPerSave = 1)
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

        public void EnableSaving()
        {
#if DEBUG_MANAGER
            this.m_is_saving_enabled = true;
#else //DEBUG_MANAGER
            return;
#endif //DEBUG_MANAGER
        }

        public void DisableSaving()
        {
#if DEBUG_MANAGER
            this.m_is_saving_enabled = false;
#else //DEBUG_MANAGER
            return;
#endif //DEBUG_MANAGER
        }

        public void DisableAndClearLogsSaving()
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
                    for (int i = 0; i < this.m_info_data.Count; i++)
                    {
                        try
                        {
                            writer.WriteLine(this.m_info_data[i].Log);
                        }
                        catch (Exception exc)
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
}
