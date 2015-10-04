namespace BugTracker.Translator.Controls
{
    partial class ControlTranslate
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
            System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
            this.dgvList = new System.Windows.Forms.DataGridView();
            this.listBoxModules = new System.Windows.Forms.ListBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageSource = new System.Windows.Forms.TabPage();
            this.richTextBoxSource = new System.Windows.Forms.RichTextBox();
            this.tabPageTranslated = new System.Windows.Forms.TabPage();
            this.richTextBoxTranslated = new System.Windows.Forms.RichTextBox();
            this.columnMethod = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnLineNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnSource = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnTranslated = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnComment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnChanged = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPageSource.SuspendLayout();
            this.tabPageTranslated.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel1.Controls.Add(this.dgvList, 1, 0);
            tableLayoutPanel1.Controls.Add(this.listBoxModules, 0, 0);
            tableLayoutPanel1.Controls.Add(this.tabControl1, 1, 3);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new System.Drawing.Size(627, 347);
            tableLayoutPanel1.TabIndex = 3;
            // 
            // dgvList
            // 
            this.dgvList.AllowUserToAddRows = false;
            this.dgvList.AllowUserToDeleteRows = false;
            this.dgvList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvList.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnMethod,
            this.columnLineNumber,
            this.columnId,
            this.columnSource,
            this.columnTranslated,
            this.columnComment,
            this.columnChanged});
            this.dgvList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvList.Location = new System.Drawing.Point(128, 3);
            this.dgvList.Name = "dgvList";
            tableLayoutPanel1.SetRowSpan(this.dgvList, 3);
            this.dgvList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvList.Size = new System.Drawing.Size(495, 252);
            this.dgvList.TabIndex = 5;
            this.dgvList.SelectionChanged += new System.EventHandler(this.dgvList_SelectionChanged);
            // 
            // listBoxModules
            // 
            this.listBoxModules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxModules.FormattingEnabled = true;
            this.listBoxModules.Location = new System.Drawing.Point(3, 3);
            this.listBoxModules.Name = "listBoxModules";
            tableLayoutPanel1.SetRowSpan(this.listBoxModules, 4);
            this.listBoxModules.Size = new System.Drawing.Size(119, 341);
            this.listBoxModules.TabIndex = 6;
            this.listBoxModules.SelectedIndexChanged += new System.EventHandler(this.listBoxModules_SelectedIndexChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageSource);
            this.tabControl1.Controls.Add(this.tabPageTranslated);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(128, 261);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(495, 83);
            this.tabControl1.TabIndex = 7;
            // 
            // tabPageSource
            // 
            this.tabPageSource.Controls.Add(this.richTextBoxSource);
            this.tabPageSource.Location = new System.Drawing.Point(4, 22);
            this.tabPageSource.Name = "tabPageSource";
            this.tabPageSource.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSource.Size = new System.Drawing.Size(487, 57);
            this.tabPageSource.TabIndex = 0;
            this.tabPageSource.Text = "Source";
            this.tabPageSource.UseVisualStyleBackColor = true;
            // 
            // richTextBoxSource
            // 
            this.richTextBoxSource.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBoxSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxSource.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxSource.Name = "richTextBoxSource";
            this.richTextBoxSource.Size = new System.Drawing.Size(481, 51);
            this.richTextBoxSource.TabIndex = 0;
            this.richTextBoxSource.Text = "";
            // 
            // tabPageTranslated
            // 
            this.tabPageTranslated.Controls.Add(this.richTextBoxTranslated);
            this.tabPageTranslated.Location = new System.Drawing.Point(4, 22);
            this.tabPageTranslated.Name = "tabPageTranslated";
            this.tabPageTranslated.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTranslated.Size = new System.Drawing.Size(487, 58);
            this.tabPageTranslated.TabIndex = 1;
            this.tabPageTranslated.Text = "Translated";
            this.tabPageTranslated.UseVisualStyleBackColor = true;
            // 
            // richTextBoxTranslated
            // 
            this.richTextBoxTranslated.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBoxTranslated.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxTranslated.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxTranslated.Name = "richTextBoxTranslated";
            this.richTextBoxTranslated.Size = new System.Drawing.Size(481, 52);
            this.richTextBoxTranslated.TabIndex = 1;
            this.richTextBoxTranslated.Text = "";
            this.richTextBoxTranslated.Leave += new System.EventHandler(this.richTextBoxTranslated_Leave);
            // 
            // columnMethod
            // 
            this.columnMethod.DataPropertyName = "Method";
            this.columnMethod.HeaderText = "Method";
            this.columnMethod.Name = "columnMethod";
            this.columnMethod.ReadOnly = true;
            // 
            // columnLineNumber
            // 
            this.columnLineNumber.DataPropertyName = "SourceLineNumber";
            this.columnLineNumber.HeaderText = "Line";
            this.columnLineNumber.Name = "columnLineNumber";
            this.columnLineNumber.ReadOnly = true;
            // 
            // columnId
            // 
            this.columnId.DataPropertyName = "Id";
            this.columnId.HeaderText = "Id";
            this.columnId.Name = "columnId";
            this.columnId.ReadOnly = true;
            // 
            // columnSource
            // 
            this.columnSource.DataPropertyName = "SourceString";
            this.columnSource.HeaderText = "Source";
            this.columnSource.Name = "columnSource";
            this.columnSource.ReadOnly = true;
            // 
            // columnTranslated
            // 
            this.columnTranslated.DataPropertyName = "TranslatedString";
            this.columnTranslated.HeaderText = "Translated";
            this.columnTranslated.Name = "columnTranslated";
            this.columnTranslated.ReadOnly = true;
            // 
            // columnComment
            // 
            this.columnComment.DataPropertyName = "Comment";
            this.columnComment.HeaderText = "Comment";
            this.columnComment.Name = "columnComment";
            // 
            // columnChanged
            // 
            this.columnChanged.DataPropertyName = "Changed";
            this.columnChanged.HeaderText = "Changed";
            this.columnChanged.Name = "columnChanged";
            this.columnChanged.ReadOnly = true;
            this.columnChanged.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.columnChanged.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // ControlTranslate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(tableLayoutPanel1);
            this.Name = "ControlTranslate";
            this.Size = new System.Drawing.Size(627, 347);
            tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPageSource.ResumeLayout(false);
            this.tabPageTranslated.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvList;
        private System.Windows.Forms.ListBox listBoxModules;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageSource;
        private System.Windows.Forms.TabPage tabPageTranslated;
        private System.Windows.Forms.RichTextBox richTextBoxSource;
        private System.Windows.Forms.RichTextBox richTextBoxTranslated;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnMethod;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLineNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnId;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnTranslated;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnComment;
        private System.Windows.Forms.DataGridViewCheckBoxColumn columnChanged;
    }
}
