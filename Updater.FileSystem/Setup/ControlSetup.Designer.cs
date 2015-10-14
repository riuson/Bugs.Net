namespace Updater.FileSystem.Setup
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelSourceDirectory = new System.Windows.Forms.Label();
            this.buttonBrowseDirectory = new System.Windows.Forms.Button();
            this.textBoxSourceDirectory = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.labelSourceDirectory, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonBrowseDirectory, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBoxSourceDirectory, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(459, 277);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // labelSourceDirectory
            // 
            this.labelSourceDirectory.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelSourceDirectory.AutoSize = true;
            this.labelSourceDirectory.Location = new System.Drawing.Point(3, 8);
            this.labelSourceDirectory.Name = "labelSourceDirectory";
            this.labelSourceDirectory.Size = new System.Drawing.Size(87, 13);
            this.labelSourceDirectory.TabIndex = 0;
            this.labelSourceDirectory.Text = "Source directory:";
            // 
            // buttonBrowseDirectory
            // 
            this.buttonBrowseDirectory.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.buttonBrowseDirectory.AutoSize = true;
            this.buttonBrowseDirectory.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonBrowseDirectory.Location = new System.Drawing.Point(430, 3);
            this.buttonBrowseDirectory.Name = "buttonBrowseDirectory";
            this.buttonBrowseDirectory.Size = new System.Drawing.Size(26, 23);
            this.buttonBrowseDirectory.TabIndex = 1;
            this.buttonBrowseDirectory.Text = "...";
            this.buttonBrowseDirectory.UseVisualStyleBackColor = true;
            this.buttonBrowseDirectory.Click += new System.EventHandler(this.buttonBrowseDirectory_Click);
            // 
            // textBoxSourceDirectory
            // 
            this.textBoxSourceDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSourceDirectory.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.textBoxSourceDirectory.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.textBoxSourceDirectory.Location = new System.Drawing.Point(96, 4);
            this.textBoxSourceDirectory.Name = "textBoxSourceDirectory";
            this.textBoxSourceDirectory.Size = new System.Drawing.Size(328, 20);
            this.textBoxSourceDirectory.TabIndex = 2;
            // 
            // ControlSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ControlSetup";
            this.Size = new System.Drawing.Size(459, 277);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label labelSourceDirectory;
        private System.Windows.Forms.Button buttonBrowseDirectory;
        private System.Windows.Forms.TextBox textBoxSourceDirectory;
    }
}
