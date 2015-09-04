using BugTracker.Core.Classes;
using BugTracker.Core.Interfaces;
using BugTracker.DB.Entities;
using BugTracker.Vocabulary.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BugTracker.Vocabulary.Classes
{
    internal class Plugin : IPlugin
    {
        private IApplication mApp;

        public void Initialize(IApplication app)
        {
            this.mApp = app;
        }

        public IButton[] GetCommandLinks(string tag)
        {
            switch (tag)
            {
                case "settings":
                    {
                        IButton menuItemPriorityList = MenuPanelFabric.CreateMenuItem("Priority list", "Vocabulary editor");
                        menuItemPriorityList.Click += delegate(object sender, EventArgs ea)
                        {
                            this.ShowList(typeof(Priority));
                        };

                        IButton menuItemProblemTypeList = MenuPanelFabric.CreateMenuItem("Problem Type list", "Vocabulary editor");
                        menuItemProblemTypeList.Click += delegate(object sender, EventArgs ea)
                        {
                            this.ShowList(typeof(ProblemType));
                        };

                        IButton menuItemSolutionList = MenuPanelFabric.CreateMenuItem("Solution list", "Vocabulary editor");
                        menuItemSolutionList.Click += delegate(object sender, EventArgs ea)
                        {
                            this.ShowList(typeof(Solution));
                        };

                        IButton menuItemStatusList = MenuPanelFabric.CreateMenuItem("Status list", "Vocabulary editor");
                        menuItemStatusList.Click += delegate(object sender, EventArgs ea)
                        {
                            this.ShowList(typeof(Status));
                        };

                        return new IButton[] { menuItemPriorityList, menuItemProblemTypeList, menuItemSolutionList, menuItemStatusList };
                    }
                default:
                    return new IButton[] { };
            }
        }

        private void ShowList(Type type)
        {
            Type generic = typeof(ControlVocabularyList<>);
            Type[] typeArgs = { type };
            Type constructed = generic.MakeGenericType(typeArgs);

            object o = Activator.CreateInstance(constructed, new object[] { this.mApp });
            Control control = o as Control;

            if (control != null)
            {
                this.mApp.Controls.Show(control);
            }
        }
    }
}
