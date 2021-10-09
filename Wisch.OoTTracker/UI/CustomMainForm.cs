using BizHawk.Emulation.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Wisch.OoTTracker;
using Wisch.OoTTracker.UI;

namespace BizHawk.Client.EmuHawk
{
    public sealed partial class CustomMainForm : Form, IExternalToolForm
    {
        [RequiredService]
        public IMemoryDomains MemoryDomains { get; set; }

        [RequiredService]
        public IVideoProvider VideoProvider { get; set; }

        private readonly Bookkeeping bookkeeping;

        public CustomMainForm()
        {
            bookkeeping = new Bookkeeping(this);
            Text = "Item Tracker";
            SuspendLayout();
            InitControls();
            Icon = Resources.Instance.GetIcon("tracker_icon");
            ResumeLayout();
            Invalidate();

            SaveData.SetBookkeeping(bookkeeping);
            SaveData.Load();
        }

        ScreenEquipment equipmentScreen;
        ScreenItems itemsScreen;
        ScreenQuest questScreen;

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            SaveData.Save();
            base.OnFormClosing(e);
        }

        private void InitControls()
        {
            this.ClientSize = new Size(480, 960);

            equipmentScreen = new ScreenEquipment(bookkeeping, 0, 0);
            itemsScreen = new ScreenItems(bookkeeping, 0, 320);
            questScreen = new ScreenQuest(this, bookkeeping, 0, 640);

            foreach (Control control in equipmentScreen.Controls)
            {
                Controls.Add(control);

                if (baseLocations.ContainsKey(control.Name))
                {
                    throw new InvalidOperationException($"Key '{control.Name}' already in baseLocations");
                }
                if (baseScale.ContainsKey(control.Name))
                {
                    throw new InvalidOperationException($"Key '{control.Name}' already in baseScale");
                }

                baseLocations.Add(control.Name, control.Location);
                baseScale.Add(control.Name, control.Size);

                if (control is Label)
                {
                    baseFontSize.Add(control.Name, control.Font.Size);
                }
            }

            foreach (Control control in itemsScreen.Controls)
            {
                Controls.Add(control);

                if (baseLocations.ContainsKey(control.Name))
                {
                    throw new InvalidOperationException($"Key '{control.Name}' already in baseLocations");
                }
                if (baseScale.ContainsKey(control.Name))
                {
                    throw new InvalidOperationException($"Key '{control.Name}' already in baseScale");
                }

                baseLocations.Add(control.Name, control.Location);
                baseScale.Add(control.Name, control.Size);

                if (control is Label)
                {
                    baseFontSize.Add(control.Name, control.Font.Size);
                }
            }

            foreach (Control control in questScreen.Controls)
            {
                Controls.Add(control);

                if (baseLocations.ContainsKey(control.Name))
                {
                    throw new InvalidOperationException($"Key '{control.Name}' already in baseLocations");
                }
                if (baseScale.ContainsKey(control.Name))
                {
                    throw new InvalidOperationException($"Key '{control.Name}' already in baseScale");
                }

                baseLocations.Add(control.Name, control.Location);
                baseScale.Add(control.Name, control.Size);

                if (control is Label)
                {
                    baseFontSize.Add(control.Name, control.Font.Size);
                }
            }
        }

        private readonly Dictionary<string, Point> baseLocations = new Dictionary<string, Point>();
        private readonly Dictionary<string, Size> baseScale = new Dictionary<string, Size>();
        private readonly Dictionary<string, float> baseFontSize = new Dictionary<string, float>(); 

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //y offset is scaled in DrawScreen() and thus can be constant
            const int Y_OFFSET = 320;

            e.Graphics.Clear(Color.DimGray);

            float yOffset = 0;
            var bgImg = Resources.Instance["screen_eqipment_3"];
            DrawScreen(e.Graphics, yOffset, bgImg);

            yOffset += Y_OFFSET;
            bgImg = Resources.Instance["screen_items"];
            DrawScreen(e.Graphics, yOffset, bgImg);

            yOffset += Y_OFFSET;
            bgImg = Resources.Instance["screen_quest_4"];
            DrawScreen(e.Graphics, yOffset, bgImg);
        }

        private void DrawScreen(Graphics g, float yOffset, Image bgImg)
        {
            const int X_STEP = 32;
            const int Y_STEP = 32;

            for (int y = 0; y <= (bgImg.Height / Y_STEP); ++y)
            {
                for (int x = 0; x <= (bgImg.Width / X_STEP); ++x)
                {
                    var srcRect = new Rectangle(x * X_STEP, y * Y_STEP, X_STEP, Y_STEP);
                    var destRect = new RectangleF(
                        2 * x * X_STEP * ratioWidth,
                        ((2 * y * Y_STEP) + yOffset) * ratioHeight,
                        2 * X_STEP * ratioWidth,
                        2 * Y_STEP * ratioHeight
                    );
                    g.DrawImage(bgImg, destRect, srcRect, GraphicsUnit.Pixel);
                }
            }
        }

        private float ratioWidth = 1;
        private float ratioHeight = 1;

        protected override void OnResizeEnd(EventArgs e)
        {
            base.OnResizeEnd(e);

            float baseWidth = 480;
            float baseHeight = 960;

            float currentWidth = ClientSize.Width;
            float currentHeight = ClientSize.Height;

            ratioWidth = currentWidth / baseWidth;
            ratioHeight = currentHeight / baseHeight;

            ResizeControls();
        }

        private void ResizeControls()
        {
            BeginUpdate();

            foreach (Control control in Controls)
            {
                float x = baseLocations[control.Name].X;
                float y = baseLocations[control.Name].Y;
                float width = baseScale[control.Name].Width;
                float height = baseScale[control.Name].Height;

                x *= ratioWidth;
                y *= ratioHeight;
                width *= ratioWidth;
                height *= ratioHeight;

                if (control is Label)
                {
                    control.Font = new Font(control.Font.FontFamily, baseFontSize[control.Name] * ratioHeight);
                }

                control.Location = new Point((int)x, (int)y);
                control.Size = new Size((int)width, (int)height);
            }

            EndUpdate();
        }

        public bool UpdateBefore { get; }

        public bool AskSaveChanges() => true;

        public void FastUpdate() { }

        public void NewUpdate(ToolFormUpdateType type) { }

        public void Restart() { }

        public void UpdateValues()
        {
            SuspendLayout();
            bookkeeping.Update();
            ResumeLayout();

            questScreen.RedrawSkulltulas();
        }
    }
}
