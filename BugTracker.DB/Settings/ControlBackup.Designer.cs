﻿namespace BugTracker.DB.Settings
{
    partial class ControlBackup
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
            this.labelDaysRepeat = new System.Windows.Forms.Label();
            this.labelDaysRemove = new System.Windows.Forms.Label();
            this.labelDirectory = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonCheckBackup = new System.Windows.Forms.Button();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.numericUpDownRepeat = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownRemove = new System.Windows.Forms.NumericUpDown();
            this.textBoxBackupDirectory = new System.Windows.Forms.TextBox();
            this.buttonBrowseBackupDirectory = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRepeat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRemove)).BeginInit();
            this.SuspendLayout();
            // 
            // labelDaysRepeat
            // 
            this.labelDaysRepeat.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelDaysRepeat.AutoSize = true;
            this.labelDaysRepeat.Location = new System.Drawing.Point(3, 35);
            this.labelDaysRepeat.Name = "labelDaysRepeat";
            this.labelDaysRepeat.Size = new System.Drawing.Size(139, 13);
            this.labelDaysRepeat.TabIndex = 0;
            this.labelDaysRepeat.Text = "Days before repeat backup:";
            // 
            // labelDaysRemove
            // 
            this.labelDaysRemove.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelDaysRemove.AutoSize = true;
            this.labelDaysRemove.Location = new System.Drawing.Point(3, 61);
            this.labelDaysRemove.Name = "labelDaysRemove";
            this.labelDaysRemove.Size = new System.Drawing.Size(144, 13);
            this.labelDaysRemove.TabIndex = 0;
            this.labelDaysRemove.Text = "Days before remove backup:";
            // 
            // labelDirectory
            // 
            this.labelDirectory.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelDirectory.AutoSize = true;
            this.labelDirectory.Location = new System.Drawing.Point(3, 8);
            this.labelDirectory.Name = "labelDirectory";
            this.labelDirectory.Size = new System.Drawing.Size(111, 13);
            this.labelDirectory.TabIndex = 0;
            this.labelDirectory.Text = "Directory for backups:";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.labelDaysRepeat, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonCheckBackup, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.richTextBoxLog, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.labelDaysRemove, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDownRepeat, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDownRemove, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelDirectory, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBoxBackupDirectory, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonBrowseBackupDirectory, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(484, 217);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // buttonCheckBackup
            // 
            this.buttonCheckBackup.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.buttonCheckBackup, 2);
            this.buttonCheckBackup.Location = new System.Drawing.Point(3, 84);
            this.buttonCheckBackup.Name = "buttonCheckBackup";
            this.buttonCheckBackup.Size = new System.Drawing.Size(104, 23);
            this.buttonCheckBackup.TabIndex = 3;
            this.buttonCheckBackup.Text = "Check backup";
            this.buttonCheckBackup.UseVisualStyleBackColor = true;
            this.buttonCheckBackup.Click += new System.EventHandler(this.buttonCheckBackup_Click);
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanel1.SetColumnSpan(this.richTextBoxLog, 3);
            this.richTextBoxLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxLog.Location = new System.Drawing.Point(3, 113);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.Size = new System.Drawing.Size(478, 101);
            this.richTextBoxLog.TabIndex = 4;
            this.richTextBoxLog.Text = "";
            // 
            // numericUpDownRepeat
            // 
            this.numericUpDownRepeat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownRepeat.Location = new System.Drawing.Point(153, 32);
            this.numericUpDownRepeat.Name = "numericUpDownRepeat";
            this.numericUpDownRepeat.Size = new System.Drawing.Size(296, 20);
            this.numericUpDownRepeat.TabIndex = 5;
            // 
            // numericUpDownRemove
            // 
            this.numericUpDownRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownRemove.Location = new System.Drawing.Point(153, 58);
            this.numericUpDownRemove.Name = "numericUpDownRemove";
            this.numericUpDownRemove.Size = new System.Drawing.Size(296, 20);
            this.numericUpDownRemove.TabIndex = 5;
            // 
            // textBoxBackupDirectory
            // 
            this.textBoxBackupDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBackupDirectory.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.textBoxBackupDirectory.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.textBoxBackupDirectory.Location = new System.Drawing.Point(153, 4);
            this.textBoxBackupDirectory.Name = "textBoxBackupDirectory";
            this.textBoxBackupDirectory.Size = new System.Drawing.Size(296, 20);
            this.textBoxBackupDirectory.TabIndex = 6;
            // 
            // buttonBrowseBackupDirectory
            // 
            this.buttonBrowseBackupDirectory.AutoSize = true;
            this.buttonBrowseBackupDirectory.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonBrowseBackupDirectory.Location = new System.Drawing.Point(455, 3);
            this.buttonBrowseBackupDirectory.Name = "buttonBrowseBackupDirectory";
            this.buttonBrowseBackupDirectory.Size = new System.Drawing.Size(26, 23);
            this.buttonBrowseBackupDirectory.TabIndex = 7;
            this.buttonBrowseBackupDirectory.Text = "...";
            this.buttonBrowseBackupDirectory.UseVisualStyleBackColor = true;
            this.buttonBrowseBackupDirectory.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // ControlBackup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ControlBackup";
            this.Size = new System.Drawing.Size(484, 217);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRepeat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRemove)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button buttonCheckBackup;
        private System.Windows.Forms.RichTextBox richTextBoxLog;
        private System.Windows.Forms.NumericUpDown numericUpDownRepeat;
        private System.Windows.Forms.NumericUpDown numericUpDownRemove;
        private System.Windows.Forms.TextBox textBoxBackupDirectory;
        private System.Windows.Forms.Button buttonBrowseBackupDirectory;
        private System.Windows.Forms.Label labelDaysRepeat;
        private System.Windows.Forms.Label labelDaysRemove;
        private System.Windows.Forms.Label labelDirectory;
    }
}
