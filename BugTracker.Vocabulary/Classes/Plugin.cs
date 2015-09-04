using BugTracker.Core.Classes;
using BugTracker.Core.Interfaces;
using BugTracker.DB.Entities;
using BugTracker.Vocabulary.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
                        List<IButton> result = new List<IButton>();

                        Assembly assembly = typeof(IVocabulary).Assembly;
                        Type[] types = assembly.GetTypes();

                        foreach (var type in types)
                        {
                            Type ti = type.GetInterface(typeof(IVocabulary).FullName);

                            if (ti != null && !type.IsAbstract && !type.IsInterface)
                            {
                                IButton menuItemTypeList = MenuPanelFabric.CreateMenuItem(String.Format("{0} list", type.Name), "Vocabulary editor");
                                menuItemTypeList.Click += delegate(object sender, EventArgs ea)
                                {
                                    this.ShowList(type);
                                };
                                result.Add(menuItemTypeList);
                            }
                        }

                        return result.ToArray();
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
