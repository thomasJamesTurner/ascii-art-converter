using System.Drawing;
using System.Drawing.Imaging;

namespace ascii_art_converter
{
    public class Program
    {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        public static void Main(string[] args)
        {
            var (charWidth, charHeight) = ConsoleFontHelper.GetConsoleFontSize();
            Console.WriteLine($"Character size: {charWidth}x{charHeight} pixels");
            float aspectRatio = (float)charHeight / charWidth;
            Console.WriteLine($"Aspect ratio: {aspectRatio:F2}");

            Console.WriteLine("input image path");
            string path = Console.ReadLine();

            ImageLoader imageLoader = new ImageLoader();
            Bitmap image = imageLoader.load(path);
            
            int width = Console.WindowWidth;
            int height = Console.WindowHeight - 1;
            //int height = (int)(Console.WindowHeight / aspectRatio);
            //int width = image.Width;
            //int height = (int)(image.Height / aspectRatio);
            ImageProcessor processor = new ImageProcessor();
            Bitmap image_resized = processor.resize(image, width, height);
            Bitmap image_greyscale = processor.greyscale(image_resized);
            image_greyscale = processor.invert(image_greyscale);
            image_greyscale.Save("temp.png", format: ImageFormat.Png);
            string asciiChars = "@#$S]%?!:+~-*'.  ";
            string output = "";
            
            for (int y = 0; y < image_greyscale.Height; y++)
            {
                for (int x = 0; x < image_greyscale.Width; x++)
                {
                    Color color = image_greyscale.GetPixel(x, y);
                    int index = (int)((color.R / 255.0f) * (asciiChars.Length - 1));
                    output += asciiChars[index];

                }
                output += "\n";
            }
            Console.WriteLine(output);


        }
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
    }
}
