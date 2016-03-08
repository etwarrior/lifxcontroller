namespace LIFXController
{
    partial class newEventForm
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
            this.newEventColorListBox = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.newEventOkButton = new System.Windows.Forms.Button();
            this.newEventCancelButton = new System.Windows.Forms.Button();
            this.newEventNameTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.newEventBrightnessListBox = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.newEventTransitionTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // newEventColorListBox
            // 
            this.newEventColorListBox.FormattingEnabled = true;
            this.newEventColorListBox.Items.AddRange(new object[] {
            "White",
            "Red",
            "Orange",
            "Yellow",
            "Green",
            "Blue",
            "Indigo",
            "Violet"});
            this.newEventColorListBox.Location = new System.Drawing.Point(13, 36);
            this.newEventColorListBox.Name = "newEventColorListBox";
            this.newEventColorListBox.Size = new System.Drawing.Size(72, 121);
            this.newEventColorListBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Color";
            // 
            // newEventOkButton
            // 
            this.newEventOkButton.Location = new System.Drawing.Point(19, 343);
            this.newEventOkButton.Name = "newEventOkButton";
            this.newEventOkButton.Size = new System.Drawing.Size(85, 32);
            this.newEventOkButton.TabIndex = 2;
            this.newEventOkButton.Text = "OK";
            this.newEventOkButton.UseVisualStyleBackColor = true;
            this.newEventOkButton.Click += new System.EventHandler(this.newEventOkButton_Click);
            // 
            // newEventCancelButton
            // 
            this.newEventCancelButton.Location = new System.Drawing.Point(161, 343);
            this.newEventCancelButton.Name = "newEventCancelButton";
            this.newEventCancelButton.Size = new System.Drawing.Size(85, 32);
            this.newEventCancelButton.TabIndex = 3;
            this.newEventCancelButton.Text = "Cancel";
            this.newEventCancelButton.UseVisualStyleBackColor = true;
            // 
            // newEventNameTextBox
            // 
            this.newEventNameTextBox.Location = new System.Drawing.Point(58, 317);
            this.newEventNameTextBox.Name = "newEventNameTextBox";
            this.newEventNameTextBox.Size = new System.Drawing.Size(152, 20);
            this.newEventNameTextBox.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(92, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Effect";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Items.AddRange(new object[] {
            "On",
            "Dim",
            "Flash",
            "Flicker",
            "Transition"});
            this.listBox1.Location = new System.Drawing.Point(92, 36);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(72, 121);
            this.listBox1.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(171, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Brightness";
            // 
            // newEventBrightnessListBox
            // 
            this.newEventBrightnessListBox.FormattingEnabled = true;
            this.newEventBrightnessListBox.Items.AddRange(new object[] {
            "90%",
            "80%",
            "70%",
            "60%",
            "50%",
            "40%",
            "30%",
            "20%",
            "10%"});
            this.newEventBrightnessListBox.Location = new System.Drawing.Point(171, 36);
            this.newEventBrightnessListBox.Name = "newEventBrightnessListBox";
            this.newEventBrightnessListBox.Size = new System.Drawing.Size(72, 121);
            this.newEventBrightnessListBox.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(46, 178);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Transition Time (ms)";
            // 
            // newEventTransitionTextBox
            // 
            this.newEventTransitionTextBox.Location = new System.Drawing.Point(49, 194);
            this.newEventTransitionTextBox.Name = "newEventTransitionTextBox";
            this.newEventTransitionTextBox.Size = new System.Drawing.Size(152, 20);
            this.newEventTransitionTextBox.TabIndex = 10;
            // 
            // newEventForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(259, 396);
            this.Controls.Add(this.newEventTransitionTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.newEventBrightnessListBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.newEventNameTextBox);
            this.Controls.Add(this.newEventCancelButton);
            this.Controls.Add(this.newEventOkButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.newEventColorListBox);
            this.Name = "newEventForm";
            this.Text = "New Event";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox newEventColorListBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button newEventOkButton;
        private System.Windows.Forms.Button newEventCancelButton;
        private System.Windows.Forms.TextBox newEventNameTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox newEventBrightnessListBox;
        private System.Windows.Forms.TextBox newEventTransitionTextBox;
        private System.Windows.Forms.Label label4;
    }
}