namespace OracleDatabaseProject
{
    partial class Form1
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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button_ODBconnect = new System.Windows.Forms.Button();
            this.button_ODBdisconnect = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 25);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(687, 186);
            this.listBox1.TabIndex = 0;
            // 
            // button_ODBconnect
            // 
            this.button_ODBconnect.Location = new System.Drawing.Point(543, 263);
            this.button_ODBconnect.Name = "button_ODBconnect";
            this.button_ODBconnect.Size = new System.Drawing.Size(124, 40);
            this.button_ODBconnect.TabIndex = 1;
            this.button_ODBconnect.Text = "button1";
            this.button_ODBconnect.UseVisualStyleBackColor = true;
            this.button_ODBconnect.Click += new System.EventHandler(this.button_ODBconnect_Click);
            // 
            // button_ODBdisconnect
            // 
            this.button_ODBdisconnect.Location = new System.Drawing.Point(370, 263);
            this.button_ODBdisconnect.Name = "button_ODBdisconnect";
            this.button_ODBdisconnect.Size = new System.Drawing.Size(75, 39);
            this.button_ODBdisconnect.TabIndex = 2;
            this.button_ODBdisconnect.Text = "button1";
            this.button_ODBdisconnect.UseVisualStyleBackColor = true;
            this.button_ODBdisconnect.Click += new System.EventHandler(this.button_ODBdisconnect_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 246);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(726, 331);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button_ODBdisconnect);
            this.Controls.Add(this.button_ODBconnect);
            this.Controls.Add(this.listBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button button_ODBconnect;
        private System.Windows.Forms.Button button_ODBdisconnect;
        private System.Windows.Forms.Button button1;
    }
}

