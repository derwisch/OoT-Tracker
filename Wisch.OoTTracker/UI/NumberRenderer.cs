using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wisch.OoTTracker.UI
{
    class NumberRenderer
    {
        private readonly Image numberAtlas;
        private Dictionary<char, Image> numberAtlasParts;

        private const int DIGIT_WIDTH = 8;
        private const int DIGIT_HEIGHT = 14;

        public NumberRenderer()
        {
            numberAtlas = Resources.Instance["number-sheet"];

            numberAtlasParts = new Dictionary<char, Image>()
            {
                { '0', CreatePartImage(0) },
                { '1', CreatePartImage(8) },
                { '2', CreatePartImage(16) },
                { '3', CreatePartImage(24) },
                { '4', CreatePartImage(32) },
                { '5', CreatePartImage(40) },
                { '6', CreatePartImage(48) },
                { '7', CreatePartImage(56) },
                { '8', CreatePartImage(64) },
                { '9', CreatePartImage(72) }
            };


        }

        private Image CreatePartImage(int x)
        {
            Bitmap result = new Bitmap(DIGIT_WIDTH, DIGIT_HEIGHT);

            using (Graphics g = Graphics.FromImage(result))
            {
                g.DrawImage(numberAtlas,
                    new Rectangle(0, 0, DIGIT_WIDTH, DIGIT_HEIGHT),
                    new Rectangle(x, 0, DIGIT_WIDTH, DIGIT_HEIGHT),
                    GraphicsUnit.Pixel);
            }

            return result;
        }

        public void Draw(int x, int y, char digit, float fontSize)
        {
            if (!numberAtlasParts.ContainsKey(digit))
                return;

            int width = (int)(fontSize / DIGIT_HEIGHT * DIGIT_WIDTH);
            int height = (int)fontSize;

            GuiApi.Instance.DrawImage(numberAtlasParts[digit], x, y, width, height);
        }

        public void Draw(int x, int y, string numberString, float fontSize)
        {
            int offset = 0;

            foreach (char c in numberString)
            {
                if (c == '-')
                    continue;

                offset += (int)(fontSize / DIGIT_HEIGHT * DIGIT_WIDTH);
                Draw(x + offset, y, c, fontSize);
            }
        }

        public void Draw(int x, int y, int number, float fontSize)
        {
            string numberString = number.ToString("000");
            Draw(x, y, numberString, fontSize);
        }

        public void Render(int x, int y, int number, float fontSize)
        {
            GuiApi.Instance.DrawNew("native", true);

            Draw(x, y, number, fontSize);

            GuiApi.Instance.DrawFinish();
        }
    }
}
