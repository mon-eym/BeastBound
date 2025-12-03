using System;

namespace Beastbound.Utils
{
    public static class TextFx
    {
        public static void TypeLineCentered(string text, int y, ConsoleColor color, int delayMs = 12)
        {
            int x = Math.Max(0, (Console.BufferWidth - text.Length) / 2);
            var old = Console.ForegroundColor;
            Console.ForegroundColor = color;
            for (int i = 0; i < text.Length; i++)
            {
                try
                {
                    Console.SetCursorPosition(x + i, y);
                    Console.Write(text[i]);
                }
                catch { }
                System.Threading.Thread.Sleep(delayMs);
            }
            Console.ForegroundColor = old;
        }

        public static void BlinkCentered(string text, int y, ConsoleColor color, int times, int intervalMs)
        {
            int x = Math.Max(0, (Console.BufferWidth - text.Length) / 2);
            for (int i = 0; i < times; i++)
            {
                ConsoleUI.WriteAt(x, y, text, color);
                System.Threading.Thread.Sleep(intervalMs);
                ConsoleUI.WriteAt(x, y, new string(' ', text.Length), ConsoleColor.Black);
                System.Threading.Thread.Sleep(intervalMs);
            }
            ConsoleUI.WriteAt(x, y, text, color);
        }

        public static void Shimmer(int x, int y, int width, int bandWidth, int intervalMs)
        {
            var colors = new[] { ConsoleColor.Gray, ConsoleColor.White };
            for (int step = 0; step < width + bandWidth; step++)
            {
                for (int i = 0; i < width; i++)
                {
                    var col = (i >= step && i < step + bandWidth) ? colors[1] : colors[0];
                    ConsoleUI.WriteAt(x + i, y, "█", col);
                }
                System.Threading.Thread.Sleep(intervalMs);
            }
        }
    }
}