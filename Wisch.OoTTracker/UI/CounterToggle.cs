using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wisch.OoTTracker.UI
{
    /// <summary>
    /// Control that counts a labels text up or down.
    /// </summary>
    class CounterToggle : Control
    {
        private readonly PictureBox pictureBox;
        private readonly Label label;

        public Image Icon
        {
            get => pictureBox.Image;
            set => pictureBox.Image = value;
        }

        private int value;
        public int Value
        {
            get => value;
            set
            {
                this.value = value;
                label.Text = Value.ToString();
            }
        }

        public bool CanGoNegative { get; set; } = false;

        public CounterToggle(Label label)
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.label = label;
            TabStop = false;
            BackColor = Color.Transparent;
            Margin = new Padding(8);
            pictureBox = new CustomPictureBox()
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom
            };
            pictureBox.MouseUp += (sender, e) =>
            {
                switch (e.Button)
                {
                    case MouseButtons.Left:
                        Value++;
                        break;
                    case MouseButtons.Right:
                        if (CanGoNegative || Value > 0)
                        {
                            Value--;
                        }
                        break;
                }
            };

            pictureBox.MouseEnter += (sender, e) =>
            {
                OnMouseEnter(e);
            };

            pictureBox.MouseLeave += (sender, e) =>
            {
                OnMouseLeave(e);
            };

            Controls.Add(pictureBox);
        }

    }
}
