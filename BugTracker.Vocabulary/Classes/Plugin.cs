using BugTracker.Core.Classes;
using BugTracker.Core.Interfaces;
using BugTracker.DB.Entities;
using BugTracker.Vocabulary.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                            this.ShowPriorityList();
                        };

                        IButton menuItemProblemTypeList = MenuPanelFabric.CreateMenuItem("Problem Type list", "Vocabulary editor");
                        menuItemProblemTypeList.Click += delegate(object sender, EventArgs ea)
                        {
                            this.ShowProblemTypeList();
                        };

                        IButton menuItemSolutionList = MenuPanelFabric.CreateMenuItem("Solution list", "Vocabulary editor");
                        menuItemSolutionList.Click += delegate(object sender, EventArgs ea)
                        {
                            this.ShowSolutionList();
                        };

                        IButton menuItemStatusList = MenuPanelFabric.CreateMenuItem("Status list", "Vocabulary editor");
                        menuItemStatusList.Click += delegate(object sender, EventArgs ea)
                        {
                            this.ShowStatusList();
                        };

                        return new IButton[] { menuItemPriorityList, menuItemProblemTypeList, menuItemSolutionList, menuItemStatusList };
                    }
                default:
                    return new IButton[] { };
            }
        }

        private void ShowPriorityList()
        {
            ControlVocabularyList<Priority> controlEditor = new ControlVocabularyList<Priority>(this.mApp);
            this.mApp.Controls.Show(controlEditor);
        }

        private void ShowProblemTypeList()
        {
            ControlVocabularyList<ProblemType> controlEditor = new ControlVocabularyList<ProblemType>(this.mApp);
            this.mApp.Controls.Show(controlEditor);
        }

        private void ShowSolutionList()
        {
            ControlVocabularyList<Solution> controlEditor = new ControlVocabularyList<Solution>(this.mApp);
            this.mApp.Controls.Show(controlEditor);
        }

        private void ShowStatusList()
        {
            ControlVocabularyList<Status> controlEditor = new ControlVocabularyList<Status>(this.mApp);
            this.mApp.Controls.Show(controlEditor);
        }
    }
}
