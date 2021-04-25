
namespace Scrap_Scramble_Final_Version
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
            this.ButtonBotStart = new System.Windows.Forms.Button();
            this.ListBoxLogging = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.ListBoxTesting = new System.Windows.Forms.ListBox();
            this.ButtonGetUserStates = new System.Windows.Forms.Button();
            this.ButtonGetSetBreakdown = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ButtonBotStart
            // 
            this.ButtonBotStart.Location = new System.Drawing.Point(516, 33);
            this.ButtonBotStart.Name = "ButtonBotStart";
            this.ButtonBotStart.Size = new System.Drawing.Size(75, 23);
            this.ButtonBotStart.TabIndex = 0;
            this.ButtonBotStart.Text = "Start Bot";
            this.ButtonBotStart.UseVisualStyleBackColor = true;
            this.ButtonBotStart.Click += new System.EventHandler(this.ButtonBotStart_click);
            // 
            // ListBoxLogging
            // 
            this.ListBoxLogging.FormattingEnabled = true;
            this.ListBoxLogging.Location = new System.Drawing.Point(516, 77);
            this.ListBoxLogging.Name = "ListBoxLogging";
            this.ListBoxLogging.ScrollAlwaysVisible = true;
            this.ListBoxLogging.Size = new System.Drawing.Size(756, 459);
            this.ListBoxLogging.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(881, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Console";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(368, 515);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Test Button";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ListBoxTesting
            // 
            this.ListBoxTesting.FormattingEnabled = true;
            this.ListBoxTesting.Location = new System.Drawing.Point(90, 77);
            this.ListBoxTesting.Name = "ListBoxTesting";
            this.ListBoxTesting.Size = new System.Drawing.Size(362, 225);
            this.ListBoxTesting.TabIndex = 4;
            // 
            // ButtonGetUserStates
            // 
            this.ButtonGetUserStates.Location = new System.Drawing.Point(90, 327);
            this.ButtonGetUserStates.Name = "ButtonGetUserStates";
            this.ButtonGetUserStates.Size = new System.Drawing.Size(118, 23);
            this.ButtonGetUserStates.TabIndex = 5;
            this.ButtonGetUserStates.Text = "Get User States";
            this.ButtonGetUserStates.UseVisualStyleBackColor = true;
            this.ButtonGetUserStates.Click += new System.EventHandler(this.ButtonGetUserStates_Click);
            // 
            // ButtonGetSetBreakdown
            // 
            this.ButtonGetSetBreakdown.Location = new System.Drawing.Point(214, 327);
            this.ButtonGetSetBreakdown.Name = "ButtonGetSetBreakdown";
            this.ButtonGetSetBreakdown.Size = new System.Drawing.Size(114, 23);
            this.ButtonGetSetBreakdown.TabIndex = 6;
            this.ButtonGetSetBreakdown.Text = "Get Set Breakdowns";
            this.ButtonGetSetBreakdown.UseVisualStyleBackColor = true;
            this.ButtonGetSetBreakdown.Click += new System.EventHandler(this.ButtonGetSetBreakdown_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(334, 327);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(118, 23);
            this.button3.TabIndex = 7;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1300, 582);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.ButtonGetSetBreakdown);
            this.Controls.Add(this.ButtonGetUserStates);
            this.Controls.Add(this.ListBoxTesting);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ListBoxLogging);
            this.Controls.Add(this.ButtonBotStart);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ButtonBotStart;
        public System.Windows.Forms.ListBox ListBoxLogging;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox ListBoxTesting;
        private System.Windows.Forms.Button ButtonGetUserStates;
        private System.Windows.Forms.Button ButtonGetSetBreakdown;
        private System.Windows.Forms.Button button3;
    }
}

