namespace BugTracker.Members.Controls
{
    partial class ControlLogin
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
            System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
            this.labelMember = new System.Windows.Forms.Label();
            this.labelPassword = new System.Windows.Forms.Label();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.comboBoxMember = new System.Windows.Forms.ComboBox();
            this.maskedTextBoxPassword = new System.Windows.Forms.MaskedTextBox();
            this.buttonMembersList = new System.Windows.Forms.Button();
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
            tableLayoutPanel1.Controls.Add(this.labelMember, 1, 3);
            tableLayoutPanel1.Controls.Add(this.labelPassword, 1, 4);
            tableLayoutPanel1.Controls.Add(this.buttonOk, 4, 0);
            tableLayoutPanel1.Controls.Add(this.buttonCancel, 4, 1);
            tableLayoutPanel1.Controls.Add(this.comboBoxMember, 2, 3);
            tableLayoutPanel1.Controls.Add(this.maskedTextBoxPassword, 2, 4);
            tableLayoutPanel1.Controls.Add(this.buttonMembersList, 4, 2);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 6;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new System.Drawing.Size(829, 405);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // labelMember
            // 
            this.labelMember.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelMember.AutoSize = true;
            this.labelMember.Location = new System.Drawing.Point(216, 94);
            this.labelMember.Name = "labelMember";
            this.labelMember.Size = new System.Drawing.Size(48, 13);
            this.labelMember.TabIndex = 0;
            this.labelMember.Text = "Member:";
            // 
            // labelPassword
            // 
            this.labelPassword.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelPassword.AutoSize = true;
            this.labelPassword.Enabled = false;
            this.labelPassword.Location = new System.Drawing.Point(208, 120);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(56, 13);
            this.labelPassword.TabIndex = 0;
            this.labelPassword.Text = "Password:";
            this.labelPassword.Visible = false;
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(750, 3);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 3;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(750, 32);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // comboBoxMember
            // 
            this.comboBoxMember.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxMember.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMember.FormattingEnabled = true;
            this.comboBoxMember.Location = new System.Drawing.Point(270, 90);
            this.comboBoxMember.Name = "comboBoxMember";
            this.comboBoxMember.Size = new System.Drawing.Size(337, 21);
            this.comboBoxMember.TabIndex = 5;
            // 
            // maskedTextBoxPassword
            // 
            this.maskedTextBoxPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.maskedTextBoxPassword.Enabled = false;
            this.maskedTextBoxPassword.Location = new System.Drawing.Point(270, 117);
            this.maskedTextBoxPassword.Name = "maskedTextBoxPassword";
            this.maskedTextBoxPassword.Size = new System.Drawing.Size(337, 20);
            this.maskedTextBoxPassword.TabIndex = 6;
            this.maskedTextBoxPassword.Visible = false;
            // 
            // buttonMembersList
            // 
            this.buttonMembersList.Location = new System.Drawing.Point(750, 61);
            this.buttonMembersList.Name = "buttonMembersList";
            this.buttonMembersList.Size = new System.Drawing.Size(75, 23);
            this.buttonMembersList.TabIndex = 4;
            this.buttonMembersList.Text = "Members...";
            this.buttonMembersList.UseVisualStyleBackColor = true;
            this.buttonMembersList.Click += new System.EventHandler(this.buttonMembersList_Click);
            // 
            // ControlLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(tableLayoutPanel1);
            this.Name = "ControlLogin";
            this.Size = new System.Drawing.Size(829, 405);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxMember;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxPassword;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Label labelMember;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonMembersList;
    }
}
