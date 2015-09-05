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

            object[] attributesRevision = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyGitRevisionAttribute), false);
            object[] attributesAuthorDate = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyGitCommitAuthorDateAttribute), false);

            this.labelRevision.Text = (attributesRevision[0] as AssemblyGitRevisionAttribute).RevisionHash;
            this.labelCommitAuthorDate.Text = (attributesAuthorDate[0] as AssemblyGitCommitAuthorDateAttribute).CommitAuthorDate.ToLongDateString();
        }
    }
}
