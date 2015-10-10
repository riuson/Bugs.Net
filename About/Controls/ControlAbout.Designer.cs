namespace BugTracker.About.Controls
{
    partial class ControlAbout
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
            this.labelTitle = new System.Windows.Forms.Label();
            this.labelSourcesTitle = new System.Windows.Forms.Label();
            this.labelDescription = new System.Windows.Forms.Label();
            this.labelAuthorTitle = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.linkLabelSources = new System.Windows.Forms.LinkLabel();
            this.linkLabelAuthor = new System.Windows.Forms.LinkLabel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelTitle.Location = new System.Drawing.Point(3, 0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.labelTitle.Size = new System.Drawing.Size(107, 27);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "About Bugs.Net";
            // 
            // labelSourcesTitle
            // 
            this.labelSourcesTitle.AutoSize = true;
            this.labelSourcesTitle.Location = new System.Drawing.Point(3, 67);
            this.labelSourcesTitle.Name = "labelSourcesTitle";
            this.labelSourcesTitle.Size = new System.Drawing.Size(49, 13);
            this.labelSourcesTitle.TabIndex = 0;
            this.labelSourcesTitle.Text = "Sources:";
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.labelDescription, 2);
            this.labelDescription.Location = new System.Drawing.Point(3, 27);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(275, 13);
            this.labelDescription.TabIndex = 0;
            this.labelDescription.Text = "Simple bug tracking application with database in local file";
            // 
            // labelAuthorTitle
            // 
            this.labelAuthorTitle.AutoSize = true;
            this.labelAuthorTitle.Location = new System.Drawing.Point(3, 47);
            this.labelAuthorTitle.Name = "labelAuthorTitle";
            this.labelAuthorTitle.Size = new System.Drawing.Size(41, 13);
            this.labelAuthorTitle.TabIndex = 0;
            this.labelAuthorTitle.Text = "Author:";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.labelTitle, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelDescription, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelAuthorTitle, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelSourcesTitle, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.linkLabelSources, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.linkLabelAuthor, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(5, 5);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(597, 335);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // linkLabelSources
            // 
            this.linkLabelSources.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.linkLabelSources.AutoSize = true;
            this.linkLabelSources.Location = new System.Drawing.Point(116, 70);
            this.linkLabelSources.Name = "linkLabelSources";
            this.linkLabelSources.Size = new System.Drawing.Size(55, 13);
            this.linkLabelSources.TabIndex = 1;
            this.linkLabelSources.TabStop = true;
            this.linkLabelSources.Text = "linkLabel1";
            this.linkLabelSources.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelSources_LinkClicked);
            // 
            // linkLabelAuthor
            // 
            this.linkLabelAuthor.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.linkLabelAuthor.AutoSize = true;
            this.linkLabelAuthor.Location = new System.Drawing.Point(116, 50);
            this.linkLabelAuthor.Name = "linkLabelAuthor";
            this.linkLabelAuthor.Size = new System.Drawing.Size(93, 13);
            this.linkLabelAuthor.TabIndex = 1;
            this.linkLabelAuthor.TabStop = true;
            this.linkLabelAuthor.Text = "riuson@gmail.com";
            this.linkLabelAuthor.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelSources_LinkClicked);
            // 
            // ControlAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ControlAbout";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(607, 345);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.LinkLabel linkLabelSources;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.LinkLabel linkLabelAuthor;
        private System.Windows.Forms.Label labelSourcesTitle;
        private System.Windows.Forms.Label labelAuthorTitle;
    }
}
