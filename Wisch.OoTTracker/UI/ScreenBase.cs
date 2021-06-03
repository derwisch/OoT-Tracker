using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Wisch.OoTTracker.UI
{
    abstract class ScreenBase
    {
        /// <summary>
        /// The controls of this screen. 
        /// </summary>
        public IEnumerable<Control> Controls => controls;
        private readonly List<Control> controls = new List<Control>();

        protected readonly Bookkeeping bookkeeping;
        protected readonly int offsetX;
        protected readonly int offsetY;

        public ScreenBase(Bookkeeping bookkeeping, int offsetX, int offsetY)
        {
            this.bookkeeping = bookkeeping;
            this.offsetX = offsetX;
            this.offsetY = offsetY;
            this.bookkeeping.DiscrepancyFound += DiscrepancyFound;
            CreateControls();
        }

        protected abstract void DiscrepancyFound(object sender, Bookkeeping.DiscrepancyEventArgs args);

        /// <summary>
        /// Method in which all controls must be created.
        /// </summary>
        protected abstract void CreateControls();

        /// <summary>
        /// Builds a label and adds it to the control list.
        /// </summary>
        /// <param name="x">X coordinate of the control on this screen. The screen automatically offsets the control.</param>
        /// <param name="y">Y coordinate of the control on this screen. The screen automatically offsets the control.</param>
        protected Label BuildLabel(int x, int y,
                                   int width, int height,
                                   string initialText,
                                   Color color,
                                   string fontFamily = "Microsoft Sans Serif",
                                   float fontSize = 10F,
                                   bool bold = false,
                                   ContentAlignment alignment = ContentAlignment.TopLeft)
        {
            var result = new Label()
            {
                Location = new Point(offsetX + x, offsetY + y),
                Size = new Size(width, height),
                Font = new Font(fontFamily, fontSize, bold ? FontStyle.Bold : FontStyle.Regular),
                ForeColor = color,
                BackColor = Color.Transparent,
                BorderStyle = BorderStyle.None,
                TextAlign = alignment,
                Text = initialText
            };
            controls.Add(result);
            return result;
        }

        /// <summary>
        /// Builds an item slot and adds it to the control list.<br />
        /// <c>width</c> and <c>height</c> default to 48.
        /// </summary>
        /// <param name="x">X coordinate of the control on this screen. The screen automatically offsets the control.</param>
        /// <param name="y">Y coordinate of the control on this screen. The screen automatically offsets the control.</param>
        /// <param name="alternativeTemplates">
        /// An array of string pairs consisting of name followed by the image key for the given alternative. <br />
        /// Must contain at least one pair of <c>"name", "image_key"</c>
        /// </param>
        protected ItemSlot BuildItemSlot(int x, int y, params string[] alternativeTemplates)
        {
            return BuildItemSlot(x, y, 48, 48, alternativeTemplates);
        }

        /// <summary>
        /// Builds an item slot and adds it to the control list.
        /// </summary>
        /// <param name="x">X coordinate of the control on this screen. The screen automatically offsets the control.</param>
        /// <param name="y">Y coordinate of the control on this screen. The screen automatically offsets the control.</param>
        /// <param name="alternativeTemplates">
        /// An array of string pairs consisting of name followed by the image key for the given alternative. <br />
        /// Must contain at least one pair of <c>"name", "image_key"</c>
        /// </param>
        protected ItemSlot BuildItemSlot(int x, int y, int width, int height, params string[] alternativeTemplates)
        {
            ItemSlot.Alternative[] alternatives = new ItemSlot.Alternative[alternativeTemplates.Length / 2];

            for (int i = 0; i < alternatives.Length; ++i)
            {
                alternatives[i] = new ItemSlot.Alternative(alternativeTemplates[2 * i], Resources.Instance[alternativeTemplates[2 * i + 1]]);
            }

            ItemSlot slot = new ItemSlot()
            {
                Icon = alternatives[0].Icon,
                Location = new Point(offsetX + x, offsetY + y),
                Size = new Size(width, height),
                Text = alternatives[0].Name,
                Alternatives = alternatives,
                CanToggle = false // since this tracker now updates items itself manual toggles are not required
            };
            controls.Add(slot);

            SaveData.Loaded += (sender, e) =>
            {
                string hints = SaveData.GetData<string>($"{SaveData.ITEM_SLOT_PREFIX}{alternatives[0].Name}");
                slot.Hints = hints.Replace("{LINEBREAK}", Environment.NewLine);
            };

            SaveData.Saving += (sender, e) =>
            {
                if (!String.IsNullOrWhiteSpace(slot.Hints))
                {
                    string hints = slot.Hints.Replace(Environment.NewLine, "{LINEBREAK}");
                    SaveData.SetData($"{SaveData.ITEM_SLOT_PREFIX}{alternatives[0].Name}", hints);
                }
            };

            return slot;
        }

        /// <summary>
        /// Builds a counter toggle and adds it to the control list.
        /// </summary>
        /// <param name="x">X coordinate of the control on this screen. The screen automatically offsets the control.</param>
        /// <param name="y">Y coordinate of the control on this screen. The screen automatically offsets the control.</param>
        /// <param name="label">Label that will be updated by the counter</param>
        protected CounterToggle BuildCounterToggle(int x, int y, int width, int height, string imageKey, Label label)
        {
            CounterToggle toggle = new CounterToggle(label)
            {
                Icon = Resources.Instance[imageKey],
                Location = new Point(offsetX + x, offsetY + y),
                Size = new Size(width, height)
            };
            controls.Add(toggle);
            return toggle;
        }
    }
}