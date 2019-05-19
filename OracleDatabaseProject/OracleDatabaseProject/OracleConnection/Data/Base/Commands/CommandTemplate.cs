using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace OracleDatabaseProject
{
    class CommandTemplate
    {
        public TaskJobType TemplateType { get; private set; }
        private List<string> m_templates;
        public bool IsLoaded { get; private set; }
        public ReadOnlyCollection<string> Template { get { return this.m_templates.AsReadOnly(); } }

        public CommandTemplate(TaskJobType templateType)
        {
            this.m_templates = new List<string>();
            this.IsLoaded = false;
            this.TemplateType = templateType;
        }

        public CommandTemplate(List<string> templates, TaskJobType templateType)
        {
            this.m_templates = new List<string>(templates);
            this.IsLoaded = true;
            this.TemplateType = templateType;
        }

        public CommandTemplate(CommandTemplate commandTemplate)
        {
            this.m_templates = new List<string>(commandTemplate.m_templates);
            this.IsLoaded = commandTemplate.IsLoaded;
            this.TemplateType = commandTemplate.TemplateType;
        }

        public void SetTemplates(List<string> template)
        {
            this.m_templates = new List<string>(template);
            this.IsLoaded = true;
        }

        public void Clear()
        {
            this.m_templates.Clear();
            this.IsLoaded = false;
        }
    }
}
