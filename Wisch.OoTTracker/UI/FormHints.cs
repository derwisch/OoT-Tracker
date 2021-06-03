using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Wisch.OoTTracker.UI
{
    class FormHints : Form
    {
        private readonly TextBox textBox;
        private readonly ItemSlot toggle;

        public FormHints(string hints, ItemSlot toggle)
        {
            this.toggle = toggle;
            textBox = new TextBox();
            SuspendLayout();
            textBox.Dock = DockStyle.Fill;
            textBox.Location = new Point(0, 0);
            textBox.Multiline = true;
            textBox.Name = "textBox";
            textBox.Size = new Size(240, 217);
            textBox.TabIndex = 0;
            textBox.Text = hints;
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(240, 217);
            Controls.Add(textBox);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "FormHints";
            Text = "Hints";
            ResumeLayout(false);
            PerformLayout();
            Icon = Resources.Instance.GetIcon("tracker_icon");
        }

        public string Hints => textBox.Text;

        protected override void OnClosing(CancelEventArgs e)
        {
            toggle.Hints = Hints;
            base.OnClosing(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Dispose();
        }
    }
}