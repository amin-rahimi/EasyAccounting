namespace EasyAccounting.form
{
    partial class AddEditContractForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.radTitleBar1 = new Telerik.WinControls.UI.RadTitleBar();
            this.visualStudio2012LightTheme1 = new Telerik.WinControls.Themes.VisualStudio2012LightTheme();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cIsFinished = new System.Windows.Forms.CheckBox();
            this.tDescription = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tNextAppointmentDate = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tNextAppointmentTime = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.tStartDate = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tPayment = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bSave = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.radTitleBar1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // radTitleBar1
            // 
            this.radTitleBar1.AllowResize = false;
            this.radTitleBar1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.radTitleBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.radTitleBar1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radTitleBar1.Location = new System.Drawing.Point(3, 3);
            this.radTitleBar1.Name = "radTitleBar1";
            this.radTitleBar1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.radTitleBar1.Size = new System.Drawing.Size(424, 41);
            this.radTitleBar1.TabIndex = 9;
            this.radTitleBar1.TabStop = false;
            this.radTitleBar1.Text = "قرار داد";
            this.radTitleBar1.ThemeName = "VisualStudio2012Light";
            ((Telerik.WinControls.UI.RadTitleBarElement)(this.radTitleBar1.GetChildAt(0))).Text = "قرار داد";
            ((Telerik.WinControls.UI.RadTitleBarElement)(this.radTitleBar1.GetChildAt(0))).ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(120)))));
            ((Telerik.WinControls.UI.RadTitleBarElement)(this.radTitleBar1.GetChildAt(0))).Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            ((Telerik.WinControls.UI.RadTitleBarElement)(this.radTitleBar1.GetChildAt(0))).Opacity = 1D;
            ((Telerik.WinControls.UI.RadImageButtonElement)(this.radTitleBar1.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.bCancel);
            this.panel1.Controls.Add(this.bSave);
            this.panel1.Controls.Add(this.cIsFinished);
            this.panel1.Controls.Add(this.tDescription);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.tNextAppointmentDate);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.tNextAppointmentTime);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.tStartDate);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.tPayment);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 44);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(10);
            this.panel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.panel1.Size = new System.Drawing.Size(424, 525);
            this.panel1.TabIndex = 10;
            // 
            // cIsFinished
            // 
            this.cIsFinished.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cIsFinished.AutoSize = true;
            this.cIsFinished.Location = new System.Drawing.Point(321, 442);
            this.cIsFinished.Name = "cIsFinished";
            this.cIsFinished.Size = new System.Drawing.Size(82, 17);
            this.cIsFinished.TabIndex = 10;
            this.cIsFinished.Text = "اتمام قرارداد";
            this.cIsFinished.UseVisualStyleBackColor = true;
            // 
            // tDescription
            // 
            this.tDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tDescription.Location = new System.Drawing.Point(13, 280);
            this.tDescription.Multiline = true;
            this.tDescription.Name = "tDescription";
            this.tDescription.Size = new System.Drawing.Size(390, 141);
            this.tDescription.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.Location = new System.Drawing.Point(205, 262);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(202, 14);
            this.label5.TabIndex = 8;
            this.label5.Text = "توضیحات";
            // 
            // tNextAppointmentDate
            // 
            this.tNextAppointmentDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tNextAppointmentDate.Location = new System.Drawing.Point(13, 153);
            this.tNextAppointmentDate.Name = "tNextAppointmentDate";
            this.tNextAppointmentDate.Size = new System.Drawing.Size(391, 21);
            this.tNextAppointmentDate.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.Location = new System.Drawing.Point(206, 135);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(202, 14);
            this.label4.TabIndex = 6;
            this.label4.Text = "تاریخ جلسه بعد";
            // 
            // tNextAppointmentTime
            // 
            this.tNextAppointmentTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tNextAppointmentTime.Location = new System.Drawing.Point(13, 216);
            this.tNextAppointmentTime.Name = "tNextAppointmentTime";
            this.tNextAppointmentTime.RightToLeftLayout = true;
            this.tNextAppointmentTime.Size = new System.Drawing.Size(391, 21);
            this.tNextAppointmentTime.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Location = new System.Drawing.Point(206, 198);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(202, 14);
            this.label3.TabIndex = 4;
            this.label3.Text = "زمان جلسه بعد";
            // 
            // tStartDate
            // 
            this.tStartDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tStartDate.Location = new System.Drawing.Point(13, 95);
            this.tStartDate.Name = "tStartDate";
            this.tStartDate.Size = new System.Drawing.Size(393, 21);
            this.tStartDate.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(208, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(202, 14);
            this.label2.TabIndex = 2;
            this.label2.Text = "تاریخ شروع قرارداد";
            // 
            // tPayment
            // 
            this.tPayment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tPayment.Location = new System.Drawing.Point(13, 37);
            this.tPayment.Name = "tPayment";
            this.tPayment.Size = new System.Drawing.Size(394, 21);
            this.tPayment.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(209, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(202, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "مبلغ قرارداد";
            // 
            // bSave
            // 
            this.bSave.Location = new System.Drawing.Point(13, 487);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(75, 23);
            this.bSave.TabIndex = 11;
            this.bSave.Text = "ذخیره";
            this.bSave.UseVisualStyleBackColor = true;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // bCancel
            // 
            this.bCancel.Location = new System.Drawing.Point(95, 487);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 12;
            this.bCancel.Text = "لغو";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // AddEditContractForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(430, 572);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.radTitleBar1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AddEditContractForm";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.RightToLeftLayout = true;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "قرار داد";
            this.Load += new System.EventHandler(this.AddEditContractForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radTitleBar1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadTitleBar radTitleBar1;
        private Telerik.WinControls.Themes.VisualStudio2012LightTheme visualStudio2012LightTheme1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tPayment;
        private System.Windows.Forms.DateTimePicker tNextAppointmentTime;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tStartDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cIsFinished;
        private System.Windows.Forms.TextBox tDescription;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tNextAppointmentDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bSave;
    }
}