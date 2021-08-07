using System;
using System.Windows.Forms;

namespace BizHawk.Client.EmuHawk
{
    // Due to some issues with SuspendLayout() and ResumeLayout() we can't use those two when resizing the window
    // So direct messaging with the windows API is used instead to prevent layouting and rendering while resizing
    // and repositioning all controls following a window resize.
    // This speeds up the time required after the window resize by more than ten times.
    public sealed partial class CustomMainForm : Form, IExternalToolForm
    {
        /// <summary>
        /// An application sends the WM_SETREDRAW message to a window to allow changes in
        /// that window to be redrawn or to prevent changes in that window from being redrawn.
        /// </summary>
        /// <source>
        /// http://pinvoke.net/default.aspx/Constants.WM
        /// </source>
        private const int WM_SETREDRAW = 0xB;

        private NativeWindow nativeWindow;

        /// <summary>
        /// Sends the <c>WM_SETREDRAW</c> messasge with a wParam of 0 which disables layouting and redrawing.
        /// </summary>
        private void BeginUpdate()
        {
            IntPtr wParam = IntPtr.Zero;
            SendWindowMessage(WM_SETREDRAW, wParam, IntPtr.Zero);
        }

        /// <summary>
        /// Sends the <c>WM_SETREDRAW</c> messasge with a wParam of 1 which (re-)enables layouting and redrawing.
        /// Also invalidates the form to trigger a redraw.
        /// </summary>
        private void EndUpdate()
        {
            IntPtr wParam = new IntPtr(1);
            SendWindowMessage(WM_SETREDRAW, wParam, IntPtr.Zero);
            Invalidate();
            Refresh();
        }

        /// <summary>
        /// Sends a window message.
        /// </summary>
        private void SendWindowMessage(int msg, IntPtr wParam, IntPtr lParam)
        {
            if (nativeWindow == null)
            {
                nativeWindow = NativeWindow.FromHandle(Handle);
            }
            Message message = Message.Create(Handle, msg, wParam, lParam);
            nativeWindow.DefWndProc(ref message);
        }
    }
}
