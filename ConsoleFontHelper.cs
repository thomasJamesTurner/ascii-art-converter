using System;
using System.Drawing;
using System.Runtime.InteropServices;

class ConsoleFontHelper
{
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetStdHandle(int nStdHandle);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool GetCurrentConsoleFontEx(IntPtr hConsoleOutput, bool bMaximumWindow, ref CONSOLE_FONT_INFOEX lpConsoleCurrentFontEx);

    private const int STD_OUTPUT_HANDLE = -11;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    private struct COORD
    {
        public short X;
        public short Y;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    private struct CONSOLE_FONT_INFOEX
    {
        public uint cbSize;
        public uint nFont;
        public COORD dwFontSize;
        public int FontFamily;
        public int FontWeight;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string FaceName;
    }

    public static (int width, int height) GetConsoleFontSize()
    {
        IntPtr hnd = GetStdHandle(STD_OUTPUT_HANDLE);
        CONSOLE_FONT_INFOEX info = new CONSOLE_FONT_INFOEX();
        info.cbSize = (uint)Marshal.SizeOf(info);

        if (GetCurrentConsoleFontEx(hnd, false, ref info))
        {
            return (info.dwFontSize.X, info.dwFontSize.Y); // Width, Height in pixels
        }
        else
        {
            throw new InvalidOperationException("Could not get console font info.");
        }
    }
    public static List<(char character, int pixelCount)> SortByPixelCount(string charset, string fontName = "Consolas", int fontSize = 16)
    {
        var results = new List<(char character, int pixelCount)>();

        using var font = new System.Drawing.Font(fontName, fontSize, GraphicsUnit.Pixel);

        foreach (char c in charset)
        {
            int pixelCount = GetFilledPixelCount(c, font);
            results.Add((c, pixelCount));
        }

        // Sort ascending by pixel count
        return results.OrderBy(r => r.pixelCount).ToList();
    }
    private static int GetFilledPixelCount(char character, System.Drawing.Font font)
    {
        int width = (int)Math.Ceiling(font.Size * 1.2); // approximate width
        int height = (int)Math.Ceiling(font.Size * 2);   // approximate height

        using var bmp = new Bitmap(width, height);
        using var g = Graphics.FromImage(bmp);
        g.Clear(Color.Black); // background

        // Draw character in white
        g.DrawString(character.ToString(), font, Brushes.White, 0, 0);

        // Count white pixels
        int count = 0;
        for (int y = 0; y < bmp.Height; y++)
        {
            for (int x = 0; x < bmp.Width; x++)
            {
                Color pixel = bmp.GetPixel(x, y);
                if (pixel.R > 128 || pixel.G > 128 || pixel.B > 128)
                {
                    count++;
                }
            }
        }

        return count;
    }
    public static List<(char character, int pixelCount)> RemoveNearDuplicates(List<(char character, int pixelCount)> sortedList, int minDifference = 4)
    {
        if (sortedList.Count == 0)
            return new List<(char, int)>();

        var filtered = new List<(char character, int pixelCount)>
    {
        sortedList[0] // Always include the first (smallest pixel count)
    };

        for (int i = 1; i < sortedList.Count; i++)
        {
            var current = sortedList[i];
            var last = filtered.Last();

            // Only add if pixel count difference >= minDifference
            if (Math.Abs(current.pixelCount - last.pixelCount) >= minDifference)
            {
                filtered.Add(current);
            }
        }

        return filtered;
    }
}
