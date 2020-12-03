using PluginInterface;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace BlackandwhiteTransform
{
    //[Version(1, 0)]
    public class BlackandwhiteTransform : IPlugin
    {
        public string Name
        {
            get
            {
                return "Насыщенность 0%";
            }
        }

        public string Author
        {
            get
            {
                return "khaldun_askara";
            }
        }

        public void Transform(Bitmap bitmap)
        {
            MessageBox.Show("Вжух!!");
            for (int i = 0; i < bitmap.Width; ++i)
                for (int j = 0; j < bitmap.Height; ++j)
                {
                    Color color = bitmap.GetPixel(i, j);
                    int average = (color.R + color.G + color.B) / 3;
                    bitmap.SetPixel(i, j, Color.FromArgb(average, average, average));
                }
            MessageBox.Show("Готово!");
        }
    }
}
