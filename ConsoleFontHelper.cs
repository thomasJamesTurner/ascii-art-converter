using System;
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
}
