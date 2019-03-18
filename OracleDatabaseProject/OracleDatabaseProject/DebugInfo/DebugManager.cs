using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OracleDatabaseProject
{
    sealed class DebugManager
    {
        //singleton variables
        private static DebugManager m_instance = null;
        private static readonly object m_padlock = new object();

        //normal variables
        private bool m_available;
        private ListBox m_info_panel;
        private List<object> m_debug_disabled_objects;

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
            this.m_available = false;
            this.m_info_panel = null;
#if DEBUG_MANAGER
            this.m_debug_disabled_objects = new List<object>();
#else //DEBUG_MANAGER
            this.m_debug_disabled_objects = null;
#endif //DEBUG_MANAGER
        }

        public void Enable(ref ListBox infoPanel)
        {
            this.m_available = true;
            this.m_info_panel = infoPanel;
        }

        public void Enable()
        {
            this.m_available = true;
        }

        public void SetInfoPanel(ref ListBox infoPanel)
        {
            this.m_info_panel = infoPanel;
        }

        public void RegisterToDisabledList(object source)
        {
#if DEBUG_MANAGER
            if (source == null || this.m_debug_disabled_objects.Contains(source))
            {
                return;
            }
            this.m_debug_disabled_objects.Add(source);
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
#endif //DEBUG_MANAGER
        }

        public void Print(string info, object source)
        {
#if DEBUG_MANAGER
            if(this.m_debug_disabled_objects.Count > 0)
            {
                if(!this.m_debug_disabled_objects.Contains(source))
                {
                    if (this.CanIPrintDebugInfo())
                    {
                        //this.m_info_panel.Items.Add(info);
                        this.m_info_panel.Invoke(new Action(delegate ()
                        {
                            this.m_info_panel.Items.Add(info);
                        }));
                    }
                }
            }
            else
            {
                if (this.CanIPrintDebugInfo())
                {
                    //this.m_info_panel.Items.Add(info);
                    this.m_info_panel.Invoke(new Action(delegate ()
                    {
                        this.m_info_panel.Items.Add(info);
                    }));
                }
            }
#endif //DEBUG_MANAGER
        }

        public void Disable()
        {
            this.m_available = false;
        }

        private bool CanIPrintDebugInfo()
        {
            if(this.m_available && this.m_info_panel != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
