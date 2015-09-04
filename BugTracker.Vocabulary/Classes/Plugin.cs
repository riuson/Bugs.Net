using BugTracker.Core.Classes;
using BugTracker.Core.Interfaces;
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
                        IButton menuItemSettings = MenuPanelFabric.CreateMenuItem("Vocabulary", "Configure database");
                        menuItemSettings.Click += delegate(object sender, EventArgs ea)
                        {
                            this.ShowVocabulary();
                        };

                        return new IButton[] { menuItemSettings };
                    }
                default:
                    return new IButton[] { };
            }
        }

        private void ShowVocabulary()
        {
        }
    }
}
