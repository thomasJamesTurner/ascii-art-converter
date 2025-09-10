using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ascii_art_converter
{
    class ImageProcessor
    {
        public Bitmap resize(Bitmap source, int width, int height)
        {
            return new Bitmap(source, new Size(width, height));
        }

        public Bitmap greyscale(Bitmap source)
        {
            Bitmap image_greyscale = new Bitmap(source.Width, source.Height);
            for (int x = 0; x < source.Width; x++)
            {
                for (int y = 0; y < source.Height; y++)
                {
                    Color color = source.GetPixel(x, y);
                    Color new_color = Color.FromArgb(color.R, color.R, color.R);
                    image_greyscale.SetPixel(x, y, new_color);
                }
            }
            return image_greyscale;
        }
    }
}
