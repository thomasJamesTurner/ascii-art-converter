namespace ascii_art_converter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            int width = Console.WindowWidth;
            int height = Console.WindowHeight;

            Console.WriteLine("input image path");
            string path = Console.ReadLine();
            ImageLoader imageLoader = new ImageLoader();
            imageLoader.load(path);
        }
    }
}
