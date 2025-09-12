using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
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
                    int brightness = (int)((color.R + color.B + color.G) / 3);
                    Color new_color = Color.FromArgb(brightness, brightness, brightness);
                    image_greyscale.SetPixel(x, y, new_color);
                }
            }
            return image_greyscale;
        }
        public string asciify(Bitmap source,string charset, bool colour)
        {
            string output = "";
            var sb = new StringBuilder();
            if (colour)
            {

                for (int y = 0; y < source.Height; y++)
                {
                    for (int x = 0; x < source.Width; x++)
                    {
                        Color color = source.GetPixel(x, y);
                        int index = (int)((color.R / 255.0f) * (charset.Length - 1));
                        sb.Append( $"\u001b[38;2;{color.R};{color.G};{color.B}m{charset[index]}");

                    }
                    sb.AppendLine();
                }
                sb.Append("\u001b[38;2;255;255;255m");
                return sb.ToString();
            }
            else 
            {
                for (int y = 0; y < source.Height; y++)
                {
                    for (int x = 0; x < source.Width; x++)
                    {
                        Color color = source.GetPixel(x, y);
                        int index = (int)((color.R / 255.0f) * (charset.Length - 1));
                        sb.Append(charset[index]);

                    }
                    sb.AppendLine();
                }
                return sb.ToString();
            }   
        }
    }
}
