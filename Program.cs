using System.Drawing;
using System.Drawing.Imaging;
using System.IO.Pipelines;

namespace ascii_art_converter
{
    public class Program
    {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        public static void Main(string[] args)
        {
            while (true)
            {
                var (charWidth, charHeight) = ConsoleFontHelper.GetConsoleFontSize();
                Console.WriteLine($"Character size: {charWidth}x{charHeight} pixels");
                float font_aspect_ratio = (float)charHeight / charWidth;
                Console.WriteLine($"Font aspect ratio: {font_aspect_ratio:F2}");

                Console.WriteLine("input image path");
                string path = Console.ReadLine();

                ImageLoader imageLoader = new ImageLoader();
                Bitmap image = imageLoader.load(path);
                float image_aspect_ratio = (float)image.Width/ image.Height;

                Console.WriteLine("type colour for the image to be in colour");
                string colour = Console.ReadLine();
                int width = Console.WindowWidth-1;
                int height = (int)(Console.WindowWidth /(image_aspect_ratio * font_aspect_ratio));

                ImageProcessor processor = new ImageProcessor();
                if (colour == "colour")
                {
                    
                    Bitmap image_resized = processor.resize(image, width, height);
                   
                    image_resized.Save("temp.png", format: ImageFormat.Png);
                    
                    string asciiChars = " .'-*~+:!?%[S$#@";
                    Console.WriteLine(processor.asciify(image_resized, asciiChars, true));
                }
                else
                {
                    
                    Bitmap image_resized = processor.resize(image, width, height);
                    Bitmap image_greyscale = processor.greyscale(image_resized);
                    image_greyscale.Save("temp.png", format: ImageFormat.Png);
                    string asciiChars = " .'-*~+:!?%[S$#@";
                    Console.WriteLine(processor.asciify(image_greyscale, asciiChars, false));
                }

            }


        }
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
    }
}
