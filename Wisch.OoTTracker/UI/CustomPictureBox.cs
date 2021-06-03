using System.Windows.Forms;

namespace Wisch.OoTTracker.UI
{
    internal class CustomPictureBox : PictureBox
    {
        protected override void OnPaint(PaintEventArgs pe)
        {
            pe.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            base.OnPaint(pe);
        }
    }
}