using AppCore;
using AppCore.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace About.Controls
{
    internal partial class ControlAbout : UserControl
    {
        public ControlAbout(IApplication app)
        {
            InitializeComponent();
            this.Text = "About".Tr();
            this.labelTitle.Text = this.labelTitle.Text.Tr();
            this.labelAuthorTitle.Text = this.labelAuthorTitle.Text.Tr();
            this.labelDescription.Text = this.labelDescription.Text.Tr();
            this.labelSourcesTitle.Text = this.labelSourcesTitle.Text.Tr();

            AssemblyGitRevisionAttribute attributeRevision = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyGitRevisionAttribute>();
            AssemblyGitCommitAuthorDateAttribute attributeAuthorDate = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyGitCommitAuthorDateAttribute>();

            string commitDate = attributeAuthorDate.CommitAuthorDate.ToString("yyyy-MM-dd HH:mm:ss zzz");
            string githubLink = "https://github.com/riuson/Bugs.Net";
            string revisionShort = attributeRevision.RevisionHash.Substring(0, 7);

            this.linkLabelSources.Text = String.Format("{0}, rev.{1} from {2}".Tr(), githubLink, revisionShort, commitDate);
            this.linkLabelSources.Links.Add(this.linkLabelSources.Text.IndexOf(githubLink), githubLink.Length, githubLink);
            this.linkLabelSources.Links.Add(this.linkLabelSources.Text.IndexOf(revisionShort), revisionShort.Length, String.Format("{0}/commit/{1}", githubLink, revisionShort));

            string email = "mailto:riuson@gmail.com";
            this.linkLabelAuthor.Text = String.Format("Vladimir {0}", email);
            this.linkLabelAuthor.Links.Add(this.linkLabelAuthor.Text.IndexOf(email), email.Length, email);
        }

        private void linkLabelSources_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.linkLabelSources.LinkVisited = true;
            System.Diagnostics.Process.Start(Convert.ToString(e.Link.LinkData));
        }
    }
}
