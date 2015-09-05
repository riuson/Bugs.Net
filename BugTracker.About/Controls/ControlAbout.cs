using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BugTracker.Core.Interfaces;
using System.Reflection;

namespace BugTracker.About.Controls
{
    internal partial class ControlAbout : UserControl
    {
        public ControlAbout(IApplication app)
        {
            InitializeComponent();
            this.Text = "About";

            var attributeRevision = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyGitRevisionAttribute>();
            var attributeAuthorDate = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyGitCommitAuthorDateAttribute>();

            this.labelRevision.Text = attributeRevision.RevisionHash;
            this.labelCommitAuthorDate.Text = attributeAuthorDate.CommitAuthorDate.ToLongDateString();
        }
    }
}
