namespace Norm_kurs
{
    partial class AddPayment
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
            this.AddButton = new System.Windows.Forms.Button();
            this.ExitButton = new System.Windows.Forms.Button();
            this.PayementdateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.PayementTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // AddButton
            // 
            this.AddButton.Location = new System.Drawing.Point(6, 58);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(75, 23);
            this.AddButton.TabIndex = 0;
            this.AddButton.Text = "Добавить";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // ExitButton
            // 
            this.ExitButton.Location = new System.Drawing.Point(118, 58);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(75, 23);
            this.ExitButton.TabIndex = 1;
            this.ExitButton.Text = "Отмена";
            this.ExitButton.UseVisualStyleBackColor = true;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // PayementdateTimePicker
            // 
            this.PayementdateTimePicker.Location = new System.Drawing.Point(6, 32);
            this.PayementdateTimePicker.Name = "PayementdateTimePicker";
            this.PayementdateTimePicker.Size = new System.Drawing.Size(187, 20);
            this.PayementdateTimePicker.TabIndex = 2;
            // 
            // PayementTextBox
            // 
            this.PayementTextBox.Location = new System.Drawing.Point(93, 6);
            this.PayementTextBox.Name = "PayementTextBox";
            this.PayementTextBox.Size = new System.Drawing.Size(100, 20);
            this.PayementTextBox.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Зарплата в час";
            // 
            // AddPayment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(206, 91);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.PayementTextBox);
            this.Controls.Add(this.PayementdateTimePicker);
            this.Controls.Add(this.ExitButton);
            this.Controls.Add(this.AddButton);
            this.Name = "AddPayment";
            this.Text = "AddPayment";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.DateTimePicker PayementdateTimePicker;
        private System.Windows.Forms.TextBox PayementTextBox;
        private System.Windows.Forms.Label label1;
    }
}