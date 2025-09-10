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
            int height = (int)(Console.WindowHeight / aspectRatio);
            //int width = image.Width;
            //int height = (int)(image.Height / aspectRatio);
            ImageProcessor processor = new ImageProcessor();
            Bitmap image_resized = processor.resize(image, width, height);
            Bitmap image_greyscale = processor.greyscale(image_resized);
            image_greyscale.Save("temp.png", format: ImageFormat.Png);


        }
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
    }
}
