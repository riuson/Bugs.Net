namespace Updater.Archive
{
    partial class ControlArchive
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
            this.buttonSaveArchive = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dgvFiles = new System.Windows.Forms.DataGridView();
            this.columnIncluded = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.columnPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textBoxFilter = new System.Windows.Forms.TextBox();
            this.checkBoxSelecteed = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFiles)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonSaveArchive
            // 
            this.buttonSaveArchive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSaveArchive.AutoSize = true;
            this.buttonSaveArchive.Location = new System.Drawing.Point(305, 29);
            this.buttonSaveArchive.Name = "buttonSaveArchive";
            this.buttonSaveArchive.Size = new System.Drawing.Size(89, 23);
            this.buttonSaveArchive.TabIndex = 0;
            this.buttonSaveArchive.Text = "Save archive...";
            this.buttonSaveArchive.UseVisualStyleBackColor = true;
            this.buttonSaveArchive.Click += new System.EventHandler(this.buttonSaveArchive_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.dgvFiles, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBoxFilter, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonSaveArchive, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.checkBoxSelecteed, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(397, 268);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // dgvFiles
            // 
            this.dgvFiles.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvFiles.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFiles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnIncluded,
            this.columnPath});
            this.dgvFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFiles.Location = new System.Drawing.Point(3, 29);
            this.dgvFiles.Name = "dgvFiles";
            this.dgvFiles.Size = new System.Drawing.Size(296, 236);
            this.dgvFiles.TabIndex = 1;
            this.dgvFiles.SelectionChanged += new System.EventHandler(this.dgvFiles_SelectionChanged);
            // 
            // columnIncluded
            // 
            this.columnIncluded.DataPropertyName = "Included";
            this.columnIncluded.HeaderText = "Included";
            this.columnIncluded.Name = "columnIncluded";
            // 
            // columnPath
            // 
            this.columnPath.DataPropertyName = "RelativePath";
            this.columnPath.HeaderText = "Path";
            this.columnPath.Name = "columnPath";
            // 
            // textBoxFilter
            // 
            this.textBoxFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFilter.Location = new System.Drawing.Point(3, 3);
            this.textBoxFilter.Name = "textBoxFilter";
            this.textBoxFilter.Size = new System.Drawing.Size(296, 20);
            this.textBoxFilter.TabIndex = 2;
            this.textBoxFilter.TextChanged += new System.EventHandler(this.textBoxFilter_TextChanged);
            // 
            // checkBoxSelecteed
            // 
            this.checkBoxSelecteed.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.checkBoxSelecteed.AutoSize = true;
            this.checkBoxSelecteed.Location = new System.Drawing.Point(305, 4);
            this.checkBoxSelecteed.Name = "checkBoxSelecteed";
            this.checkBoxSelecteed.Size = new System.Drawing.Size(68, 17);
            this.checkBoxSelecteed.TabIndex = 3;
            this.checkBoxSelecteed.Text = "Selected";
            this.checkBoxSelecteed.UseVisualStyleBackColor = true;
            this.checkBoxSelecteed.CheckStateChanged += new System.EventHandler(this.checkBoxSelecteed_CheckStateChanged);
            // 
            // ControlArchive
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ControlArchive";
            this.Size = new System.Drawing.Size(397, 268);
            this.Load += new System.EventHandler(this.ControlArchive_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFiles)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonSaveArchive;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView dgvFiles;
        private System.Windows.Forms.DataGridViewCheckBoxColumn columnIncluded;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnPath;
        private System.Windows.Forms.TextBox textBoxFilter;
        private System.Windows.Forms.CheckBox checkBoxSelecteed;
    }
}
