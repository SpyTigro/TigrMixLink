namespace TigrMixLink
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            BTNremoveLast = new Button();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI Black", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.Location = new Point(12, 12);
            button1.Name = "button1";
            button1.Size = new Size(25, 25);
            button1.TabIndex = 1;
            button1.Text = "+";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // BTNremoveLast
            // 
            BTNremoveLast.BackColor = Color.White;
            BTNremoveLast.FlatAppearance.BorderColor = Color.Red;
            BTNremoveLast.Font = new Font("Segoe UI Black", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BTNremoveLast.ForeColor = SystemColors.ActiveCaptionText;
            BTNremoveLast.ImageAlign = ContentAlignment.TopLeft;
            BTNremoveLast.Location = new Point(43, 12);
            BTNremoveLast.Name = "BTNremoveLast";
            BTNremoveLast.Size = new Size(25, 25);
            BTNremoveLast.TabIndex = 3;
            BTNremoveLast.Text = "-";
            BTNremoveLast.UseVisualStyleBackColor = false;
            BTNremoveLast.Click += BTNremoveLast_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(BTNremoveLast);
            Controls.Add(button1);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "TigrMixLink Config";
            Load += Form1_Load;
            ResumeLayout(false);
        }

        #endregion
        private Button button1;
        private Button BTNremoveLast;
    }
}