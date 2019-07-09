using System;
using System.Runtime.InteropServices;

namespace CodeMagic.UI.Console
{
    public static class ConsoleFontHelper
    {
        private const string FontName = "Lucida Console";
        private const short FontSize = 16;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private unsafe struct CONSOLE_FONT_INFO_EX
        {
            internal uint cbSize;
            internal uint nFont;
            internal COORD dwFontSize;
            internal int FontFamily;
            internal int FontWeight;
            internal fixed char FaceName[LF_FACESIZE];
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct COORD
        {
            internal short X;
            internal short Y;

            internal COORD(short x, short y)
            {
                X = x;
                Y = y;
            }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetCurrentConsoleFontEx(
            IntPtr consoleOutput,
            bool maximumWindow,
            ref CONSOLE_FONT_INFO_EX consoleCurrentFontEx);

        private const int STD_OUTPUT_HANDLE = -11;
        private const int TMPF_TRUETYPE = 4;
        private const int LF_FACESIZE = 32;
        private static IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        public static void SetConsoleFont()
        {
            unsafe
            {
                IntPtr hnd = GetStdHandle(STD_OUTPUT_HANDLE);
                if (hnd == INVALID_HANDLE_VALUE)
                    return;

                CONSOLE_FONT_INFO_EX info = new CONSOLE_FONT_INFO_EX();
                info.cbSize = (uint)Marshal.SizeOf(info);

                // Set console font.
                CONSOLE_FONT_INFO_EX newInfo = new CONSOLE_FONT_INFO_EX();
                newInfo.cbSize = (uint)Marshal.SizeOf(newInfo);
                newInfo.FontFamily = TMPF_TRUETYPE;
                IntPtr ptr = new IntPtr(newInfo.FaceName);
                Marshal.Copy(FontName.ToCharArray(), 0, ptr, FontName.Length);

                // Get some settings from current font.
                newInfo.dwFontSize = new COORD(info.dwFontSize.X, FontSize);
                newInfo.FontWeight = info.FontWeight;
                SetCurrentConsoleFontEx(hnd, false, ref newInfo);
            }
        }
    }
}