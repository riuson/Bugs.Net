using BugTracker.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.Core.Controls
{
    internal class MenuListView : ListView, IMenuPanel
    {
        private ImageList mImages;

        public MenuListView()
        {
            this.View = System.Windows.Forms.View.Tile;
            this.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.mImages = new ImageList();
            this.mImages.ImageSize = new Size(48, 48);
            this.LargeImageList = this.mImages;
            this.MultiSelect = false;
            //this.Activation = ItemActivation.OneClick;
            this.HotTracking = true;
            //this.HoverSelection = true;

            this.HandleCreated += this.MenuListView_HandleCreated;
            this.ItemActivate += this.MenuListView_ItemActivate;
        }

        private void MenuListView_HandleCreated(object sender, EventArgs e)
        {
            try
            {
                SetWindowTheme(this.Handle, "explorer", null);
            }
            catch
            {
                // Dll not found
            }
        }

        private void MenuListView_ItemActivate(object sender, EventArgs e)
        {
            if (this.SelectedItems.Count == 1)
            {
                MenuListViewItem item = this.SelectedItems[0] as MenuListViewItem;

                if (item != null)
                {
                    item.CallClick();
                }
            }
        }

        public void Add(IButton button)
        {
            MenuListViewItem item = button as MenuListViewItem;

            if (item != null)
            {
                MenuListViewItemData data = item.Tag as MenuListViewItemData;

                if (data != null & data.Image != null)
                {
                    this.mImages.Images.Add(data.Image);
                    item.ImageIndex = this.mImages.Images.Count - 1;
                }

                this.Items.Add(item);
            }
        }

        public void Add(IApplication app, string tag)
        {
            IButton[] btns = app.Plugins.CollectCommandLinks(tag);

            foreach (var btn in btns)
            {
                this.Add(btn);
            }
        }

        public Control AsControl
        {
            get { return this; }
        }

        public static IButton CreateMenuItem(string text, string note = "", Image image = null)
        {
            MenuListViewItem result = new MenuListViewItem();
            result.Text = text;
            result.SubItems.Add(note);
            result.Tag = new MenuListViewItemData(image);

            return result;
        }

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        public static extern int SetWindowTheme(IntPtr hWnd, String pszSubAppName, String pszSubIdList);

        private class MenuListViewItem : ListViewItem, IButton
        {
            public event EventHandler Click;

            public void CallClick()
            {
                if (this.Click != null)
                {
                    this.Click(this, EventArgs.Empty);
                }
            }
        }

        private class MenuListViewItemData
        {
            public Image Image { get; private set; }

            public MenuListViewItemData(Image image)
            {
                this.Image = image;
            }
        }
    }
}
