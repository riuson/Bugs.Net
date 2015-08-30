namespace BugTracker.Core.Forms
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelControls = new System.Windows.Forms.Panel();
            this.navigationBar = new BugTracker.Core.Controls.NavigationBar();
            this.SuspendLayout();
            // 
            // panelControls
            // 
            this.panelControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControls.Location = new System.Drawing.Point(5, 10);
            this.panelControls.Name = "panelControls";
            this.panelControls.Size = new System.Drawing.Size(714, 404);
            this.panelControls.TabIndex = 0;
            // 
            // navigationBar
            // 
            this.navigationBar.AutoSize = true;
            this.navigationBar.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.navigationBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.navigationBar.Location = new System.Drawing.Point(5, 5);
            this.navigationBar.Name = "navigationBar";
            this.navigationBar.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.navigationBar.Size = new System.Drawing.Size(714, 5);
            this.navigationBar.TabIndex = 1;
            this.navigationBar.Navigate += new System.EventHandler<BugTracker.Core.Controls.NavigationBar.NavigateEventArgs>(this.navigationBar_Navigate);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(724, 419);
            this.Controls.Add(this.panelControls);
            this.Controls.Add(this.navigationBar);
            this.Name = "MainWindow";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Text = "MainWindow";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelControls;
        private Controls.NavigationBar navigationBar;
    }
}