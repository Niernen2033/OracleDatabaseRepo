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
        private ListBox m_list_box;

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
            this.m_list_box = null;
        }

        public void Log(string info)
        {
            if (this.m_available)
            {
                if (this.m_list_box != null)
                {
                    this.m_list_box.Items.Add(info);
                }
            }
        }

        public void Enable()
        {
            this.m_available = true;
        }

        public void SetInfoList(ref ListBox listBox)
        {
            this.m_list_box = listBox;
        }

        public void Disable()
        {
            this.m_available = false;
        }
    }
}
