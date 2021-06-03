using BizHawk.Emulation.Common;
using System.Drawing;
using System.Windows.Forms;
using Wisch.OoTTracker;
using Wisch.OoTTracker.UI;

namespace BizHawk.Client.EmuHawk
{
    public sealed class CustomMainForm : Form, IExternalToolForm
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
            }

            foreach (Control control in itemsScreen.Controls)
            {
                Controls.Add(control);
            }

            foreach (Control control in questScreen.Controls)
            {
                Controls.Add(control);
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            const int X_STEP = 32;
            const int Y_STEP = 32;

            e.Graphics.Clear(Color.DimGray);

            int yOffset = 0;

            var bgImg = Resources.Instance["screen_eqipment_3"];
            for (int y = 0; y <= (bgImg.Height / Y_STEP); ++y)
                for (int x = 0; x <= (bgImg.Width / X_STEP); ++x)
                {
                    var srcRect = new Rectangle(x * X_STEP, y * Y_STEP, X_STEP, Y_STEP);
                    var destRect = new Rectangle(2 * x * X_STEP, (2 * y * Y_STEP) + yOffset, 2 * X_STEP, 2 * Y_STEP);
                    e.Graphics.DrawImage(bgImg, destRect, srcRect, GraphicsUnit.Pixel);
                }

            yOffset += 320;

            bgImg = Resources.Instance["screen_items"];
            for (int y = 0; y <= (bgImg.Height / Y_STEP); ++y)
                for (int x = 0; x <= (bgImg.Width / X_STEP); ++x)
                {
                    var srcRect = new Rectangle(x * X_STEP, y * Y_STEP, X_STEP, Y_STEP);
                    var destRect = new Rectangle(2 * x * X_STEP, (2 * y * Y_STEP) + yOffset, 2 * X_STEP, 2 * Y_STEP);
                    e.Graphics.DrawImage(bgImg, destRect, srcRect, GraphicsUnit.Pixel);
                }

            yOffset += 320;

            bgImg = Resources.Instance["screen_quest_4"];
            for (int y = 0; y <= (bgImg.Height / Y_STEP); ++y)
                for (int x = 0; x <= (bgImg.Width / X_STEP); ++x)
                {
                    var srcRect = new Rectangle(x * X_STEP, y * Y_STEP, X_STEP, Y_STEP);
                    var destRect = new Rectangle(2 * x * X_STEP, (2 * y * Y_STEP) + yOffset, 2 * X_STEP, 2 * Y_STEP);
                    e.Graphics.DrawImage(bgImg, destRect, srcRect, GraphicsUnit.Pixel);
                }
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
