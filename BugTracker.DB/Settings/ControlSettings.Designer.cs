namespace BugTracker.DB.Settings
{
    partial class ControlSettings
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
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Button buttonBrowse;
            this.textBoxFilename = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonCheckConnection = new System.Windows.Forms.Button();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            label1 = new System.Windows.Forms.Label();
            buttonBrowse = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(3, 8);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(26, 13);
            label1.TabIndex = 0;
            label1.Text = "File:";
            // 
            // buttonBrowse
            // 
            buttonBrowse.Anchor = System.Windows.Forms.AnchorStyles.Left;
            buttonBrowse.AutoSize = true;
            buttonBrowse.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            buttonBrowse.Location = new System.Drawing.Point(455, 3);
            buttonBrowse.Name = "buttonBrowse";
            buttonBrowse.Size = new System.Drawing.Size(26, 23);
            buttonBrowse.TabIndex = 2;
            buttonBrowse.Text = "...";
            buttonBrowse.UseVisualStyleBackColor = true;
            buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // textBoxFilename
            // 
            this.textBoxFilename.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFilename.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.textBoxFilename.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.textBoxFilename.Location = new System.Drawing.Point(35, 4);
            this.textBoxFilename.Name = "textBoxFilename";
            this.textBoxFilename.Size = new System.Drawing.Size(414, 20);
            this.textBoxFilename.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(buttonBrowse, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBoxFilename, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonCheckConnection, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.richTextBoxLog, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(484, 217);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // buttonCheckConnection
            // 
            this.buttonCheckConnection.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.buttonCheckConnection, 3);
            this.buttonCheckConnection.Location = new System.Drawing.Point(3, 32);
            this.buttonCheckConnection.Name = "buttonCheckConnection";
            this.buttonCheckConnection.Size = new System.Drawing.Size(104, 23);
            this.buttonCheckConnection.TabIndex = 3;
            this.buttonCheckConnection.Text = "Check connection";
            this.buttonCheckConnection.UseVisualStyleBackColor = true;
            this.buttonCheckConnection.Click += new System.EventHandler(this.buttonCheckConnection_Click);
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanel1.SetColumnSpan(this.richTextBoxLog, 3);
            this.richTextBoxLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxLog.Location = new System.Drawing.Point(3, 61);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.Size = new System.Drawing.Size(478, 153);
            this.richTextBoxLog.TabIndex = 4;
            this.richTextBoxLog.Text = "";
            // 
            // ControlSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ControlSettings";
            this.Size = new System.Drawing.Size(484, 217);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxFilename;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button buttonCheckConnection;
        private System.Windows.Forms.RichTextBox richTextBoxLog;
    }
}
