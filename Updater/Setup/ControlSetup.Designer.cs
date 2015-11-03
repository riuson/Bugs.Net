namespace Updater.Setup
{
    partial class ControlSetup
    {
        /// <summary> 
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            this.BeforeDisposing();
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Обязательный метод для поддержки конструктора - не изменяйте 
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBoxSettings = new System.Windows.Forms.GroupBox();
            this.checkBoxCheckUpdatesOnStart = new System.Windows.Forms.CheckBox();
            this.panelMenu = new System.Windows.Forms.Panel();
            this.groupBoxSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxSettings
            // 
            this.groupBoxSettings.Controls.Add(this.checkBoxCheckUpdatesOnStart);
            this.groupBoxSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxSettings.Location = new System.Drawing.Point(0, 0);
            this.groupBoxSettings.Name = "groupBoxSettings";
            this.groupBoxSettings.Size = new System.Drawing.Size(459, 71);
            this.groupBoxSettings.TabIndex = 0;
            this.groupBoxSettings.TabStop = false;
            // 
            // checkBoxCheckUpdatesOnStart
            // 
            this.checkBoxCheckUpdatesOnStart.AutoSize = true;
            this.checkBoxCheckUpdatesOnStart.Location = new System.Drawing.Point(10, 19);
            this.checkBoxCheckUpdatesOnStart.Name = "checkBoxCheckUpdatesOnStart";
            this.checkBoxCheckUpdatesOnStart.Size = new System.Drawing.Size(190, 17);
            this.checkBoxCheckUpdatesOnStart.TabIndex = 0;
            this.checkBoxCheckUpdatesOnStart.Text = "Check updates on application start";
            this.checkBoxCheckUpdatesOnStart.UseVisualStyleBackColor = true;
            // 
            // panelMenu
            // 
            this.panelMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMenu.Location = new System.Drawing.Point(0, 71);
            this.panelMenu.Name = "panelMenu";
            this.panelMenu.Size = new System.Drawing.Size(459, 206);
            this.panelMenu.TabIndex = 1;
            // 
            // ControlSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelMenu);
            this.Controls.Add(this.groupBoxSettings);
            this.Name = "ControlSetup";
            this.Size = new System.Drawing.Size(459, 277);
            this.groupBoxSettings.ResumeLayout(false);
            this.groupBoxSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxSettings;
        private System.Windows.Forms.CheckBox checkBoxCheckUpdatesOnStart;
        private System.Windows.Forms.Panel panelMenu;
    }
}
