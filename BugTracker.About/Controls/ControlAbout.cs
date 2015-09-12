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

            string commitDate = (attributesAuthorDate[0] as AssemblyGitCommitAuthorDateAttribute).CommitAuthorDate.ToString("yyyy-MM-dd HH:mm:ss zzz");
            string githubLink = "https://github.com/riuson/Bugs.Net";
            string revisionShort = (attributesRevision[0] as AssemblyGitRevisionAttribute).RevisionHash.Substring(0, 7);
            this.linkLabelSources.Text = String.Format("{0}, rev.{1} from {2}", githubLink, revisionShort, commitDate);
            this.linkLabelSources.Links.Add(this.linkLabelSources.Text.IndexOf(githubLink), githubLink.Length, githubLink);
            this.linkLabelSources.Links.Add(this.linkLabelSources.Text.IndexOf(revisionShort), revisionShort.Length, String.Format("{0}/commit/{1}", githubLink, revisionShort));
        }

        private void linkLabelSources_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.linkLabelSources.LinkVisited = true;
            System.Diagnostics.Process.Start(Convert.ToString(e.Link.LinkData));
        }
    }
}
