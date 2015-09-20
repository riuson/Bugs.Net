namespace BugTracker.Translator.Controls
{
    partial class ControlLanguage
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
            this.labelLanguageTitle = new System.Windows.Forms.Label();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.comboBoxLangauge = new System.Windows.Forms.ComboBox();
            this.labelRestartNote = new System.Windows.Forms.Label();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 5;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel1.Controls.Add(this.labelLanguageTitle, 1, 3);
            tableLayoutPanel1.Controls.Add(this.buttonOk, 4, 0);
            tableLayoutPanel1.Controls.Add(this.buttonCancel, 4, 1);
            tableLayoutPanel1.Controls.Add(this.comboBoxLangauge, 2, 3);
            tableLayoutPanel1.Controls.Add(this.labelRestartNote, 2, 4);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 5;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 75F));
            tableLayoutPanel1.Size = new System.Drawing.Size(595, 344);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // labelLanguageTitle
            // 
            this.labelLanguageTitle.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelLanguageTitle.AutoSize = true;
            this.labelLanguageTitle.Location = new System.Drawing.Point(138, 129);
            this.labelLanguageTitle.Name = "labelLanguageTitle";
            this.labelLanguageTitle.Size = new System.Drawing.Size(58, 13);
            this.labelLanguageTitle.TabIndex = 0;
            this.labelLanguageTitle.Text = "Langauge:";
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(517, 3);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 3;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(517, 32);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // comboBoxLangauge
            // 
            this.comboBoxLangauge.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxLangauge.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLangauge.FormattingEnabled = true;
            this.comboBoxLangauge.Location = new System.Drawing.Point(202, 125);
            this.comboBoxLangauge.Name = "comboBoxLangauge";
            this.comboBoxLangauge.Size = new System.Drawing.Size(219, 21);
            this.comboBoxLangauge.TabIndex = 5;
            // 
            // labelRestartNote
            // 
            this.labelRestartNote.AutoSize = true;
            this.labelRestartNote.Location = new System.Drawing.Point(204, 154);
            this.labelRestartNote.Margin = new System.Windows.Forms.Padding(5);
            this.labelRestartNote.Name = "labelRestartNote";
            this.labelRestartNote.Size = new System.Drawing.Size(182, 13);
            this.labelRestartNote.TabIndex = 6;
            this.labelRestartNote.Text = "Restart application to apply changes.";
            // 
            // ControlLanguage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(tableLayoutPanel1);
            this.Name = "ControlLanguage";
            this.Size = new System.Drawing.Size(595, 344);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelLanguageTitle;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.ComboBox comboBoxLangauge;
        private System.Windows.Forms.Label labelRestartNote;

    }
}
