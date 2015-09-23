namespace BugTracker.TicketEditor.Controls
{
    partial class ControlTicketEdit
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
            this.buttonCancel = new System.Windows.Forms.Button();
            this.labelTitle = new System.Windows.Forms.Label();
            this.labelMemberTitle = new System.Windows.Forms.Label();
            this.labelCreatedTitle = new System.Windows.Forms.Label();
            this.labelTypeTitle = new System.Windows.Forms.Label();
            this.labelPriorityTitle = new System.Windows.Forms.Label();
            this.labelStatusTitle = new System.Windows.Forms.Label();
            this.labelSolutionTitle = new System.Windows.Forms.Label();
            this.buttonOk = new System.Windows.Forms.Button();
            this.labelMember = new System.Windows.Forms.Label();
            this.labelCreated = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxTitle = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageChangelog = new System.Windows.Forms.TabPage();
            this.tabPageAttachments = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(727, 32);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // labelTitle
            // 
            this.labelTitle.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelTitle.AutoSize = true;
            this.labelTitle.Location = new System.Drawing.Point(21, 8);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(30, 13);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "Title:";
            // 
            // labelMemberTitle
            // 
            this.labelMemberTitle.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelMemberTitle.AutoSize = true;
            this.labelMemberTitle.Location = new System.Drawing.Point(3, 37);
            this.labelMemberTitle.Name = "labelMemberTitle";
            this.labelMemberTitle.Size = new System.Drawing.Size(48, 13);
            this.labelMemberTitle.TabIndex = 0;
            this.labelMemberTitle.Text = "Member:";
            // 
            // labelCreatedTitle
            // 
            this.labelCreatedTitle.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelCreatedTitle.AutoSize = true;
            this.labelCreatedTitle.Location = new System.Drawing.Point(4, 58);
            this.labelCreatedTitle.Name = "labelCreatedTitle";
            this.labelCreatedTitle.Size = new System.Drawing.Size(47, 13);
            this.labelCreatedTitle.TabIndex = 0;
            this.labelCreatedTitle.Text = "Created:";
            // 
            // labelTypeTitle
            // 
            this.labelTypeTitle.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelTypeTitle.AutoSize = true;
            this.labelTypeTitle.Location = new System.Drawing.Point(17, 71);
            this.labelTypeTitle.Name = "labelTypeTitle";
            this.labelTypeTitle.Size = new System.Drawing.Size(34, 13);
            this.labelTypeTitle.TabIndex = 0;
            this.labelTypeTitle.Text = "Type:";
            // 
            // labelPriorityTitle
            // 
            this.labelPriorityTitle.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelPriorityTitle.AutoSize = true;
            this.labelPriorityTitle.Location = new System.Drawing.Point(10, 84);
            this.labelPriorityTitle.Name = "labelPriorityTitle";
            this.labelPriorityTitle.Size = new System.Drawing.Size(41, 13);
            this.labelPriorityTitle.TabIndex = 0;
            this.labelPriorityTitle.Text = "Priority:";
            // 
            // labelStatusTitle
            // 
            this.labelStatusTitle.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelStatusTitle.AutoSize = true;
            this.labelStatusTitle.Location = new System.Drawing.Point(11, 97);
            this.labelStatusTitle.Name = "labelStatusTitle";
            this.labelStatusTitle.Size = new System.Drawing.Size(40, 13);
            this.labelStatusTitle.TabIndex = 0;
            this.labelStatusTitle.Text = "Status:";
            // 
            // labelSolutionTitle
            // 
            this.labelSolutionTitle.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelSolutionTitle.AutoSize = true;
            this.labelSolutionTitle.Location = new System.Drawing.Point(3, 110);
            this.labelSolutionTitle.Name = "labelSolutionTitle";
            this.labelSolutionTitle.Size = new System.Drawing.Size(48, 13);
            this.labelSolutionTitle.TabIndex = 0;
            this.labelSolutionTitle.Text = "Solution:";
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(727, 3);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 3;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // labelMember
            // 
            this.labelMember.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.labelMember.AutoSize = true;
            this.labelMember.Location = new System.Drawing.Point(57, 37);
            this.labelMember.Name = "labelMember";
            this.labelMember.Size = new System.Drawing.Size(161, 13);
            this.labelMember.TabIndex = 0;
            this.labelMember.Text = "Member";
            // 
            // labelCreated
            // 
            this.labelCreated.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCreated.AutoSize = true;
            this.labelCreated.Location = new System.Drawing.Point(57, 58);
            this.labelCreated.Name = "labelCreated";
            this.labelCreated.Size = new System.Drawing.Size(161, 13);
            this.labelCreated.TabIndex = 0;
            this.labelCreated.Text = "Date";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.buttonOk, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonCancel, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelTitle, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelMemberTitle, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelCreatedTitle, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelTypeTitle, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.labelPriorityTitle, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.labelStatusTitle, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.textBoxTitle, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelSolutionTitle, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.labelMember, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelCreated, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 2, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 8;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(806, 324);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // textBoxTitle
            // 
            this.textBoxTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.textBoxTitle, 2);
            this.textBoxTitle.Location = new System.Drawing.Point(57, 4);
            this.textBoxTitle.Name = "textBoxTitle";
            this.textBoxTitle.Size = new System.Drawing.Size(664, 20);
            this.textBoxTitle.TabIndex = 0;
            this.textBoxTitle.TextChanged += new System.EventHandler(this.OnDataChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageChangelog);
            this.tabControl1.Controls.Add(this.tabPageAttachments);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(224, 32);
            this.tabControl1.Name = "tabControl1";
            this.tableLayoutPanel1.SetRowSpan(this.tabControl1, 7);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(497, 289);
            this.tabControl1.TabIndex = 5;
            // 
            // tabPageChangelog
            // 
            this.tabPageChangelog.Location = new System.Drawing.Point(4, 22);
            this.tabPageChangelog.Name = "tabPageChangelog";
            this.tabPageChangelog.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageChangelog.Size = new System.Drawing.Size(489, 263);
            this.tabPageChangelog.TabIndex = 0;
            this.tabPageChangelog.Text = "Changes";
            this.tabPageChangelog.UseVisualStyleBackColor = true;
            // 
            // tabPageAttachments
            // 
            this.tabPageAttachments.Location = new System.Drawing.Point(4, 22);
            this.tabPageAttachments.Name = "tabPageAttachments";
            this.tabPageAttachments.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAttachments.Size = new System.Drawing.Size(489, 263);
            this.tabPageAttachments.TabIndex = 1;
            this.tabPageAttachments.Text = "Attachments";
            this.tabPageAttachments.UseVisualStyleBackColor = true;
            // 
            // ControlTicketEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ControlTicketEdit";
            this.Size = new System.Drawing.Size(806, 324);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxTitle;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label labelMember;
        private System.Windows.Forms.Label labelCreated;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageChangelog;
        private System.Windows.Forms.TabPage tabPageAttachments;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelMemberTitle;
        private System.Windows.Forms.Label labelCreatedTitle;
        private System.Windows.Forms.Label labelTypeTitle;
        private System.Windows.Forms.Label labelPriorityTitle;
        private System.Windows.Forms.Label labelStatusTitle;
        private System.Windows.Forms.Label labelSolutionTitle;

    }
}
