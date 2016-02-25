namespace LIFXController
{
    partial class MainForm
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
            this.colorButton = new System.Windows.Forms.Button();
            this.OnButton = new System.Windows.Forms.Button();
            this.transitionTime = new System.Windows.Forms.TextBox();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.COLOR = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // colorButton
            // 
            this.colorButton.Location = new System.Drawing.Point(177, 228);
            this.colorButton.Name = "colorButton";
            this.colorButton.Size = new System.Drawing.Size(148, 53);
            this.colorButton.TabIndex = 0;
            this.colorButton.Text = "OFF";
            this.colorButton.UseVisualStyleBackColor = true;
            this.colorButton.Click += new System.EventHandler(this.OffButton_Click);
            // 
            // OnButton
            // 
            this.OnButton.Location = new System.Drawing.Point(177, 167);
            this.OnButton.Name = "OnButton";
            this.OnButton.Size = new System.Drawing.Size(148, 55);
            this.OnButton.TabIndex = 1;
            this.OnButton.Text = "ON";
            this.OnButton.UseVisualStyleBackColor = true;
            this.OnButton.Click += new System.EventHandler(this.OnButton_Click);
            // 
            // transitionTime
            // 
            this.transitionTime.Location = new System.Drawing.Point(177, 126);
            this.transitionTime.Name = "transitionTime";
            this.transitionTime.Size = new System.Drawing.Size(148, 20);
            this.transitionTime.TabIndex = 2;
            this.transitionTime.Text = "0";
            // 
            // COLOR
            // 
            this.COLOR.Location = new System.Drawing.Point(177, 62);
            this.COLOR.Name = "COLOR";
            this.COLOR.Size = new System.Drawing.Size(148, 42);
            this.COLOR.TabIndex = 3;
            this.COLOR.Text = "Color";
            this.COLOR.UseVisualStyleBackColor = true;
            this.COLOR.Click += new System.EventHandler(this.COLOR_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 313);
            this.Controls.Add(this.COLOR);
            this.Controls.Add(this.transitionTime);
            this.Controls.Add(this.OnButton);
            this.Controls.Add(this.colorButton);
            this.Name = "MainForm";
            this.Text = "LIFXController";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button colorButton;
        private System.Windows.Forms.Button OnButton;
        private System.Windows.Forms.TextBox transitionTime;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button COLOR;
    }
}

