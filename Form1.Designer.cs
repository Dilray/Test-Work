using System.Windows.Forms;

namespace Test_Work
{
    partial class Form1
    {
        // this.myButton.Click += button1_Click;
        private void InitializeComponent()
        {
            this.label = new System.Windows.Forms.Label();
            this.myButton = new System.Windows.Forms.Button();
            this.listBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.label.Location = new System.Drawing.Point(743, 22);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(288, 19);
            this.label.TabIndex = 2;
            this.label.Text = "Effective Mobile тестовое задание 1";
            // 
            // myButton
            // 
            this.myButton.Location = new System.Drawing.Point(762, 615);
            this.myButton.Name = "myButton";
            this.myButton.Size = new System.Drawing.Size(236, 38);
            this.myButton.TabIndex = 0;
            this.myButton.Text = "Загрузить файл с данными";
            this.myButton.Click += button1_Click;
            // 
            // listBox
            // 
            this.listBox.Location = new System.Drawing.Point(12, 12);
            this.listBox.Name = "listBox";
            this.listBox.Size = new System.Drawing.Size(620, 641);
            this.listBox.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1172, 684);
            this.Controls.Add(this.myButton);
            this.Controls.Add(this.listBox);
            this.Controls.Add(this.label);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();
            this.FormClosing += new FormClosingEventHandler(MainForm_FormClosing);
        }
    }
}