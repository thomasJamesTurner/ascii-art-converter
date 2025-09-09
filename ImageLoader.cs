using System.Drawing;
namespace ascii_art_converter
{
    public class ImageLoader
    {
        Bitmap Current_Image;

        public Bitmap load(string path)
        {
            Bitmap bmp = null;
            try
            {
                bmp = new Bitmap(Image.FromFile(path));
                Console.WriteLine("loaded image");
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
            return bmp;
        }
    }
}
