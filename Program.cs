using System.Drawing;
using System.Drawing.Imaging;
using System.IO.Pipelines;
using System.Runtime.InteropServices;

namespace ascii_art_converter
{
    public class Program
    {

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out int lpMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, int dwMode);

        private const int STD_OUTPUT_HANDLE = -11;
        private const int ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;

        public static void EnableVirtualTerminal()
        {
            var handle = GetStdHandle(STD_OUTPUT_HANDLE);

            if (!GetConsoleMode(handle, out int mode))
            {
                return;
            }

            SetConsoleMode(handle, mode | ENABLE_VIRTUAL_TERMINAL_PROCESSING);
        }
        public static void Main(string[] args)
        {
            EnableVirtualTerminal();
            
            string charSet = """ !"£$%^&*()_+-={}[]:@;'~#<>?,./\|`¬¦ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890""";
            var sorted = ConsoleFontHelper.SortByPixelCount(charSet);

            var filtered = ConsoleFontHelper.RemoveNearDuplicates(sorted, 2);

            string finalCharset = new string(filtered.Select(c => c.character).ToArray());
            Console.WriteLine($"\nFinal charset: {finalCharset}");
            string asciiChars = finalCharset;
            while (true)
            {
                var (charWidth, charHeight) = ConsoleFontHelper.GetConsoleFontSize();
                Console.WriteLine($"Character size: {charWidth}x{charHeight} pixels");
                float font_aspect_ratio = (float)charHeight / charWidth;
                Console.WriteLine($"Font aspect ratio: {font_aspect_ratio:F2}");
                Bitmap image = null;
                ImageLoader imageLoader = new ImageLoader();
                while (true)
                {
                    Console.WriteLine("input image path");
                    string path = Console.ReadLine();
                    image = imageLoader.load(path);
                    if(image != null )
                    {
                        break;
                    }
                    else 
                    {
                        Console.WriteLine("Could not load image");
                    }
                }
                
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
                    
                    Console.WriteLine(processor.asciify(image_resized, asciiChars, true));
                }
                else if(colour =="monochrome")
                {
                    Bitmap image_resized = processor.resize(image, width, height);
                    Bitmap image_greyscale = processor.greyscale(image_resized);
                    image_greyscale.Save("temp.png", format: ImageFormat.Png);

              
                    Console.WriteLine(processor.asciify(image_greyscale, asciiChars, true));
                }
                else
                {

                    Bitmap image_resized = processor.resize(image, width, height);
                    Bitmap image_greyscale = processor.greyscale(image_resized);
                    image_greyscale.Save("temp.png", format: ImageFormat.Png);
                    
                    Console.WriteLine(processor.asciify(image_greyscale, asciiChars, false));
                }

            }


        }
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
    }
}
