using BizHawk.Client.EmuHawk;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;

namespace Wisch.OoTTracker.UI
{
    /// <summary>
    /// This class contains the parts of the BizHawk 2.4.1 GuiApi class that are relevant here. <br />
    /// Some things had to be changed because .NET Framework 4.6.1 does not support C# 8 features.
    /// </summary>
    class GuiApi
	{
		public static GuiApi Instance { get; } = new GuiApi();

		private readonly Dictionary<Color, SolidBrush> _solidBrushes = new Dictionary<Color, SolidBrush>();

		private SolidBrush GetBrush(Color color) => _solidBrushes.TryGetValue(color, out var b) ? b : (_solidBrushes[color] = new SolidBrush(color));

		private DisplaySurface _GUISurface;
		private readonly Bitmap _nullGraphicsBitmap = new Bitmap(1, 1);

		private readonly ImageAttributes _attributes = new ImageAttributes();
		private readonly CompositingMode _compositingMode = CompositingMode.SourceOver;

		private readonly Color _defaultForeground = Color.White;

		/// <summary>
		/// Draws a string on screen. Requires an active surface through <see cref="DrawNew(string, bool)"/>.
		/// </summary>
		/// <param name="x">X coordinate in pixels on the emulator viewport.</param>
		/// <param name="y">Y coordinate in pixels on the emulator viewport.</param>
		/// <param name="message">The string to draw</param>
		/// <param name="fontstyle">valid values are "bold", "italic", "strikethrough" and "underline"</param>
		/// <param name="horizalign">valid values are "left", "center", "middle" and "right"</param>
		/// <param name="vertalign">valid values are "top", "center", "middle" and "bottom"</param>
		public void DrawString(int x, int y, string message, Color? forecolor = null, Color? backcolor = null, int? fontsize = null, string fontfamily = null, string fontstyle = null, string horizalign = null, string vertalign = null)
		{
			try
			{
				var family = fontfamily != null ? new FontFamily(fontfamily) : FontFamily.GenericMonospace;

				FontStyle fstyle;

				switch (fontstyle?.ToLower())
				{
					case "bold":
						fstyle = FontStyle.Bold;
						break;
					case "italic":
						fstyle = FontStyle.Italic;
						break;
					case "strikethrough":
						fstyle = FontStyle.Strikeout;
						break;
					case "underline":
						fstyle = FontStyle.Underline;
						break;
					default:
						fstyle = FontStyle.Regular;
						break;
				};

				using (var g = GetGraphics())
				{
					// The text isn't written out using GenericTypographic, so measuring it using GenericTypographic seemed to make it worse.
					// And writing it out with GenericTypographic just made it uglier.
					var font = new Font(family, fontsize ?? 12, fstyle, GraphicsUnit.Pixel);
					var sizeOfText = g.MeasureString(message, font, 0, new StringFormat(StringFormat.GenericDefault)).ToSize();

					switch (horizalign?.ToLower())
					{
						default:
						case "left":
							break;
						case "center":
						case "middle":
							x -= sizeOfText.Width / 2;
							break;
						case "right":
							x -= sizeOfText.Width;
							break;
					}

					switch (vertalign?.ToLower())
					{
						default:
						case "top":
							break;
						case "center":
						case "middle":
							y -= sizeOfText.Height / 2;
							break;
						case "bottom":
							y -= sizeOfText.Height;
							break;
					}

					var bg = backcolor;
					if (bg != null)
					{
						var brush = GetBrush(bg.Value);
						for (var xd = -1; xd <= 1; xd++) for (var yd = -1; yd <= 1; yd++)
							{
								g.DrawString(message, font, brush, x + xd, y + yd);
							}
					}
					g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
					g.DrawString(message, font, GetBrush(forecolor ?? _defaultForeground), x, y);
				}
			}
			catch (Exception) { /* ignored */ }
		}

		/// <summary>
		/// Creates a new drawing for the given surface.
		/// </summary>
		/// <param name="name">Name of the surface for which a new drawing should be created. Main viewport is "<c>native</c>"</param>
		/// <param name="clear">Whether the draw surface should be cleared or not.</param>
		public void DrawNew(string name, bool clear)
		{
			try
			{
				DrawFinish();
				_GUISurface = GlobalWin.DisplayManager.LockLuaSurface(name, clear);
			}
			catch (InvalidOperationException) { /* ignored */ }
		}

		/// <summary>
		/// Finishes the drawing and unlocks the surface.
		/// </summary>
		public void DrawFinish()
		{
			if (_GUISurface != null) GlobalWin.DisplayManager.UnlockLuaSurface(_GUISurface);
			_GUISurface = null;
		}

		/// <summary>
		/// Creates a graphics object from the current drawing surface.
		/// </summary>
		private Graphics GetGraphics()
		{
			var g = _GUISurface?.GetGraphics() ?? Graphics.FromImage(_nullGraphicsBitmap);

			int tx = BizHawk.Client.Common.Global.Emulator.CoreComm.ScreenLogicalOffsetX;
			int ty = BizHawk.Client.Common.Global.Emulator.CoreComm.ScreenLogicalOffsetY;

			if (tx != 0 || ty != 0)
			{
				var transform = g.Transform;
				transform.Translate(-tx, -ty);
				g.Transform = transform;
			}

			return g;
		}

		/// <summary>
		/// Draws a string on screen. Requires an active surface through <see cref="DrawNew(string, bool)"/>.
		/// </summary>
		/// <param name="img">The image to draw.</param>
		/// <param name="x">X coordinate in pixels on the emulator viewport.</param>
		/// <param name="y">Y coordinate in pixels on the emulator viewport.</param>
		/// <param name="width">The target width of the drawn image (or source width if null)</param>
		/// <param name="height">The target height of the drawn image (or source height if null)</param>
		public void DrawImage(Image img, int x, int y, int? width = null, int? height = null)
		{
			using (var g = GetGraphics())
			{
				g.CompositingMode = _compositingMode;
				g.DrawImage(
					img,
					new Rectangle(x, y, width ?? img.Width, height ?? img.Height),
					0,
					0,
					img.Width,
					img.Height,
					GraphicsUnit.Pixel,
					_attributes
				);
			}
		}

	}
}
