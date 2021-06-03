using System;
using System.Drawing;
using System.Windows.Forms;

namespace Wisch.OoTTracker.UI
{
    /// <summary>
    /// Control that represents an item slot.
    /// </summary>
    public partial class ItemSlot : Control
    {
        /// <summary>
        /// Item alternative for the item slot. 
        /// </summary>
        public class Alternative
        {
            public string Name { get; set; }
            public Image Icon { get; set; }

            public Alternative()
            {
                Name = "";
                Icon = null;
            }

            public Alternative(string name, Image icon)
            {
                Name = name;
                Icon = icon;
            }
        }

        public ItemSlot()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            TabStop = false;
            CanToggle = true;
            BackColor = Color.Transparent;
            Margin = new Padding(8);
            pictureBox = new CustomPictureBox()
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom
            };
            pictureBox.Click += (sender, e) =>
            {
                if (CanToggle)
                    IsFound = !IsFound;
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

            ContextMenuStrip = new ContextMenuStrip();
            AddEditHintsItem();
            alternatives = new Alternative[0];
        }

        private void AddEditHintsItem()
        {
            var editHintsItem = ContextMenuStrip.Items.Add("Edit Hints");

            editHintsItem.Click += (sender, e) =>
            {
                var form = new FormHints(Hints, this);
                form.Show();
                form.Text = $"{Text} - Hints";
                Hints = form.Hints;
            };
        }

        public override bool AutoSize => true;

        private Alternative[] alternatives;

        public string Hints
        {
            get => hints;
            set
            {
                hints = value;
            }
        }

        public event EventHandler<EventArgs> SelectedAlternativeChanged;

        private Alternative selectedAlternative;
        public Alternative SelectedAlternative
        {
            get => selectedAlternative;
            set
            {
                if (value != null)
                {
                    Icon = value.Icon;
                    Text = value.Name;

                    foreach (ToolStripMenuItem entry in ContextMenuStrip.Items)
                    {
                        entry.Checked = entry.Text == value.Name;
                    }
                }
                selectedAlternative = value;
                SelectedAlternativeChanged?.Invoke(this, EventArgs.Empty);
            }

        }

        public Alternative[] Alternatives
        {
            get
            {
                if ((alternatives == null || alternatives.Length == 0) && !String.IsNullOrWhiteSpace(Text) && baseIcon != null)
                {
                    alternatives = new Alternative[] { new Alternative(Text, baseIcon) };
                }
                return alternatives;
            }
            set
            {
                alternatives = value;
                ContextMenuStrip.Dispose();
                ContextMenuStrip = new ContextMenuStrip();
                AddEditHintsItem();
                if (alternatives.Length == 0)
                {
                    return;
                }
                if (alternatives.Length > 1)
                {
                    bool firstItem = true;

                    foreach (var item in alternatives)
                    {
                        var entry = ContextMenuStrip.Items.Add(item.Name) as ToolStripMenuItem;
                        if (firstItem)
                        {
                            entry.Checked = true;
                            firstItem = false;
                        }
                        entry.Click += (sender, e) =>
                        {
                            SelectedAlternative = item;
                            foreach (ToolStripMenuItem menuEntry in ContextMenuStrip.Items)
                            {
                                menuEntry.Checked = false;
                            }
                            entry.Checked = true;
                        };
                    }
                }
                SelectedAlternative = alternatives[0];
            }
        }

        public bool CanToggle { get; set; }

        public event EventHandler<EventArgs> FoundChanged;

        public bool IsFound
        {
            get => isFound;
            set
            {
                if (isFound != value)
                {
                    isFound = value;
                    pictureBox.Image = isFound ? baseIcon : disabledIcon;
                    Invalidate();
                    pictureBox.Invalidate();
                    FoundChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        bool isFound = false;

        private readonly PictureBox pictureBox;

        private Image baseIcon;
        private Image disabledIcon;
        private string hints;

        public Image Icon
        {
            get => baseIcon;
            set
            {
                baseIcon = value;
                disabledIcon = ToolStripRenderer.CreateDisabledImage(value);
                pictureBox.Image = isFound ? baseIcon : disabledIcon;
            }
        }

        protected override void OnClick(EventArgs e)
        {
            if (CanToggle)
                IsFound = !IsFound;
        }
    }
}