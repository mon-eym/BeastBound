using System;
using System.Collections.Generic;

namespace Beastbound.Utils
{
    public static class ConsoleUI
    {
        private static readonly int FrameMargin = 2;
        private static List<string> LogBuffer = new();

        public static void Init()
        {
            try
            {
                Console.OutputEncoding = System.Text.Encoding.UTF8;
            }
            catch { /* fallback silently */ }
        }

        public static void DrawFrame(string title)
        {
            Console.Clear();
            int w = Console.BufferWidth;
            int h = Console.BufferHeight;

            // Top border
            WriteAt(0, 0, new string('═', w), ConsoleColor.DarkGray);
            WriteAt(2, 0, $" {title} ", ConsoleColor.White);

            // Bottom border
            WriteAt(0, h - 1, new string('═', w), ConsoleColor.DarkGray);

            // Side bars
            for (int y = 1; y < h - 1; y++)
            {
                WriteAt(0, y, "║", ConsoleColor.DarkGray);
                WriteAt(w - 1, y, "║", ConsoleColor.DarkGray);
            }
        }

        public static void DrawGradientBackground(int intensity)
        {
            int w = Console.BufferWidth;
            int h = Console.BufferHeight;
            var shades = new[] { ConsoleColor.Black, ConsoleColor.DarkBlue, ConsoleColor.DarkCyan, ConsoleColor.DarkGray };
            var color = shades[Math.Min(shades.Length - 1, intensity / 6)];

            for (int y = 0; y < h; y++)
            {
                WriteAt(0, y, new string(' ', w), color);
            }
        }

        public static void WriteAt(int x, int y, string text, ConsoleColor color)
        {
            var old = Console.ForegroundColor;
            Console.ForegroundColor = color;
            try
            {
                Console.SetCursorPosition(Math.Max(0, x), Math.Max(0, y));
                Console.Write(text);
            }
            catch { /* ignore if out of bounds */ }
            finally
            {
                Console.ForegroundColor = old;
            }
        }

        public static void WriteCentered(string text, int y, ConsoleColor color)
        {
            int x = Math.Max(0, (Console.BufferWidth - text.Length) / 2);
            WriteAt(x, y, text, color);
        }

        public static int Menu(string[] options, int startY = 8, ConsoleColor highlightColor = ConsoleColor.White, int startX = 6)
        {
            int selected = 0;

            ConsoleKey key;
            do
            {
                for (int i = 0; i < options.Length; i++)
                {
                    var color = i == selected ? highlightColor : ConsoleColor.Gray;
                    WriteAt(startX, startY + i * 2, (i == selected ? "▶ " : "  ") + options[i], color);
                }

                key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.UpArrow)
                    selected = (selected - 1 + options.Length) % options.Length;
                else if (key == ConsoleKey.DownArrow)
                    selected = (selected + 1) % options.Length;

            } while (key != ConsoleKey.Enter);

            return selected;
        }

        public static void WriteLog(string message, ConsoleColor color)
        {
            int areaTop = Console.BufferHeight - 8;
            int areaLeft = 4;
            int areaWidth = Console.BufferWidth - 8;

            LogBuffer.Add(message);
            if (LogBuffer.Count > 5)
                LogBuffer.RemoveAt(0);

            // Erase region
            for (int y = areaTop; y < areaTop + 6; y++)
                WriteAt(areaLeft, y, new string(' ', areaWidth), ConsoleColor.Black);

            WriteAt(areaLeft, areaTop - 1, "─ Battle Log ─", ConsoleColor.DarkGray);

            for (int i = 0; i < LogBuffer.Count; i++)
            {
                WriteAt(areaLeft, areaTop + i, LogBuffer[i], color);
            }
        }

        public static void PressToContinue()
        {
            var y = Console.BufferHeight - 2;
            WriteCentered("[ Press any key to continue ]", y, ConsoleColor.DarkGray);
            Console.ReadKey(true);
        }

        public static void CleanExit()
        {
            Console.Clear();
            WriteCentered("Thanks for playing Beastbound!", Console.BufferHeight / 2, ConsoleColor.White);
            WriteCentered("See you again.", Console.BufferHeight / 2 + 2, ConsoleColor.DarkGray);
            System.Threading.Thread.Sleep(600);
        }

        public static void DrawPanel(string title)
        {
            int width = Console.WindowWidth;
            Console.Clear();

            // Top border
            string border = new string('═', width);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(border);

            // Centered title
            int titleX = (width - title.Length) / 2;
            Console.SetCursorPosition(titleX, 1);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(title);

            // Spacer line
            Console.SetCursorPosition(0, 2);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(border);

            Console.ResetColor();
        }

        public static void DrawBox(int x, int y, int width, int height, ConsoleColor borderColor)
        {
            for (int i = 0; i < width; i++)
            {
                WriteAt(x + i, y, "═", borderColor);
                WriteAt(x + i, y + height - 1, "═", borderColor);
            }
            for (int i = 0; i < height; i++)
            {
                WriteAt(x, y + i, "║", borderColor);
                WriteAt(x + width - 1, y + i, "║", borderColor);
            }
            WriteAt(x, y, "╔", borderColor);
            WriteAt(x + width - 1, y, "╗", borderColor);
            WriteAt(x, y + height - 1, "╚", borderColor);
            WriteAt(x + width - 1, y + height - 1, "╝", borderColor);
        }

    }
}