using BugTracker.Core.Classes;
using BugTracker.Core.Interfaces;
using BugTracker.DB.Entities;
using BugTracker.DB.Events;
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
        private List<Type> mVocabularyTypes;

        public void Initialize(IApplication app)
        {
            this.mApp = app;
            this.mVocabularyTypes = this.CollectTypes();

            foreach (var type in this.mVocabularyTypes)
            {
                Type generic = typeof(EntityShowEventArgs<>);
                Type[] typeArgs = { type };
                Type constructed = generic.MakeGenericType(typeArgs);

                //object o = Activator.CreateInstance(constructed, new object[] { null });
                this.mApp.Messages.Subscribe(constructed, this.ShowList);
            }
        }

        public IButton[] GetCommandLinks(string tag)
        {
            switch (tag)
            {
                case "settings":
                    {
                        List<IButton> result = new List<IButton>();

                        foreach (var type in this.mVocabularyTypes)
                        {
                            IButton menuItemTypeList = MenuPanelFabric.CreateMenuItem(
                                String.Format("{0} list", type.Name),
                                "Vocabulary editor",
                                BugTracker.Vocabulary.Properties.Resources.icon_list_ul_a41e35_48);
                            menuItemTypeList.Click += delegate(object sender, EventArgs ea)
                            {
                                Type generic = typeof(EntityShowEventArgs<>);
                                Type[] typeArgs = { type };
                                Type constructed = generic.MakeGenericType(typeArgs);
                                object o = Activator.CreateInstance(constructed);
                                this.ShowList(this, o as MessageEventArgs);
                            };
                            result.Add(menuItemTypeList);
                        }

                        return result.ToArray();
                    }
                default:
                    return new IButton[] { };
            }
        }

        private List<Type> CollectTypes()
        {
            List<Type> result = new List<Type>();

            Assembly assembly = typeof(IVocabulary).Assembly;
            Type[] types = assembly.GetTypes();

            foreach (var type in types)
            {
                Type ti = type.GetInterface(typeof(IVocabulary).FullName);

                if (ti != null && !type.IsAbstract && !type.IsInterface)
                {
                    result.Add(type);
                }
            }

            return result;
        }

        private void ShowList(object sender, MessageEventArgs e)
        {
            foreach (var type in this.mVocabularyTypes)
            {
                Type genericMessageType = typeof(EntityShowEventArgs<>);
                Type[] messageTypeArgs = { type };
                Type constructedMessageType = genericMessageType.MakeGenericType(messageTypeArgs);

                if (e.GetType() == constructedMessageType)
                {
                    Type genericEditorType = typeof(ControlVocabularyList<>);
                    Type[] editorTypeArgs = { type };
                    Type constructedEditorType = genericEditorType.MakeGenericType(editorTypeArgs);

                    object o = Activator.CreateInstance(constructedEditorType, new object[] { this.mApp });
                    Control control = o as Control;
                    control.Disposed += control_Disposed;

                    if (control != null)
                    {
                        this.mApp.Controls.Show(control);
                    }

                    break;
                }
            }
        }

        private void control_Disposed(object sender, EventArgs e)
        {
            foreach (var type in this.mVocabularyTypes)
            {
                Type genericEditorType = typeof(ControlVocabularyList<>);
                Type[] editorTypeArgs = { type };
                Type constructedEditorType = genericEditorType.MakeGenericType(editorTypeArgs);

                if (sender.GetType() == constructedEditorType)
                {
                    Type genericMessageType = typeof(EntityEditedEventArgs<>);
                    Type[] messageTypeArgs = { type };
                    Type constructedMessageType = genericMessageType.MakeGenericType(messageTypeArgs);
                    object o = Activator.CreateInstance(constructedMessageType, new object[] { });
                    this.mApp.Messages.Send(this, (MessageEventArgs)o);

                    break;
                }
            }
        }
    }
}
