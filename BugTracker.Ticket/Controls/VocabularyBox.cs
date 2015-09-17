﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BugTracker.DB.Interfaces;
using BugTracker.DB;
using BugTracker.DB.Dao;
using BugTracker.DB.Entities;
using BugTracker.Core.Interfaces;
using BugTracker.TicketEditor.Classes;
using BugTracker.DB.Events;

namespace BugTracker.TicketEditor.Controls
{
    public partial class VocabularyBox<T> : UserControl where T : class, new()
    {
        private ICollection<T> mEntityList;
        private BindingSource mBS;
        private IApplication mApp;

        public VocabularyBox(IApplication app)
        {
            InitializeComponent();
            this.mApp = app;
            this.mBS = new BindingSource();
            this.mBS.AllowNew = true;

            this.UpdateList();

            this.mApp.Messages.Subscribe(typeof(EntityEditedEventArgs<T>), this.VocabularyUpdated);
        }

        private void BeforeDisposing()
        {
            this.mApp.Messages.Unsubscribe(typeof(EntityEditedEventArgs<T>), this.VocabularyUpdated);
        }

        private void UpdateList()
        {
            using (ISession session = SessionManager.Instance.OpenSession(false))
            {
                Repository<T> repository = new Repository<T>(session);
                this.mEntityList = repository.List();
                this.mBS.DataSource = this.mEntityList;
                this.comboBox1.DataSource = this.mBS; ;
                this.comboBox1.DisplayMember = "Value";
            }
        }

        public T SelectedValue
        {
            get
            {
                T d = this.comboBox1.SelectedItem as T;

                if (d != null)
                {
                    return d;
                }

                return default(T);
            }
            set
            {
                T d = value;

                if (this.comboBox1.Items.Contains(d))
                {
                    this.comboBox1.SelectedItem = d;
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            this.MinimumSize = new Size(0, this.comboBox1.Height);
            this.MaximumSize = new Size(0, this.comboBox1.Height);
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;

            base.OnLoad(e);
        }

        private void VocabularyUpdated(object sender, MessageEventArgs e)
        {
            this.UpdateList();
        }

        private void linkLabelEdit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            T data = this.comboBox1.SelectedItem as T;

            if (data != null)
            {
                this.mApp.Messages.Send(this, new EntityShowEventArgs<T>(data));
            }
            else
            {
                this.mApp.Messages.Send(this, new EntityShowEventArgs<T>());
            }
        }
    }
}
