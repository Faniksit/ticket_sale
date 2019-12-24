namespace TicketSale
{
    partial class Report
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.startDate = new System.Windows.Forms.DateTimePicker();
            this.finishDate = new System.Windows.Forms.DateTimePicker();
            this.doneButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Italic | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(239, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Укажите начальную дату:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Italic | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(12, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(229, 24);
            this.label2.TabIndex = 1;
            this.label2.Text = "Укажите конечную дату:";
            // 
            // startDate
            // 
            this.startDate.Location = new System.Drawing.Point(264, 31);
            this.startDate.Name = "startDate";
            this.startDate.Size = new System.Drawing.Size(200, 20);
            this.startDate.TabIndex = 2;
            // 
            // finishDate
            // 
            this.finishDate.Location = new System.Drawing.Point(264, 106);
            this.finishDate.Name = "finishDate";
            this.finishDate.Size = new System.Drawing.Size(200, 20);
            this.finishDate.TabIndex = 3;
            // 
            // doneButton
            // 
            this.doneButton.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.doneButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.doneButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.doneButton.Location = new System.Drawing.Point(167, 150);
            this.doneButton.Name = "doneButton";
            this.doneButton.Size = new System.Drawing.Size(135, 37);
            this.doneButton.TabIndex = 4;
            this.doneButton.Text = "Подтвердить";
            this.doneButton.UseVisualStyleBackColor = false;
            // 
            // SetPeriod
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 199);
            this.Controls.Add(this.doneButton);
            this.Controls.Add(this.finishDate);
            this.Controls.Add(this.startDate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximumSize = new System.Drawing.Size(500, 238);
            this.MinimumSize = new System.Drawing.Size(500, 238);
            this.Name = "SetPeriod";
            this.Text = "Определение периода";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.DateTimePicker startDate;
        public System.Windows.Forms.DateTimePicker finishDate;
        public System.Windows.Forms.Button doneButton;
    }
}