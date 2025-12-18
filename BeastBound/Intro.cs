using Beastbound.Utils;
using System.Linq;
using System.Threading;
using System.Media;
using Beastbound.Audio;

namespace Beastbound.Intro
{
    public static class IntroRunner
    {

        public static void Play()
        {
            // Ensure fullscreen (focus + Alt+Enter + fallback maximize)
            FullscreenHelper.EnsureFullscreen();

            // Re-fetch updated dimensions and prep console
            Console.Clear();
            Console.CursorVisible = false;
            int width = Console.WindowWidth;
            int height = Console.WindowHeight;

            // Optional: polished frame
            DrawFrame(width, height);

            // Logo ASCII (make sure lines have no trailing spaces)
            string[] logo = new[]
            {
                @"██████╗ ███████╗ █████╗ ███████╗████████╗██████╗  ██████╗ ██╗   ██╗███╗   ██╗██████╗ ",
                @"██╔══██╗██╔════╝██╔══██╗██╔════╝╚══██╔══╝██╔══██╗██╔═══██╗██║   ██║████╗  ██║██╔══██╗",
                @"██████╔╝█████╗  ███████║███████╗   ██║   ██████╔╝██║   ██║██║   ██║██╔██╗ ██║██║  ██║",
                @"██╔══██╗██╔══╝  ██╔══██║╚════██║   ██║   ██╔══██╗██║   ██║██║   ██║██║╚██╗██║██║  ██║",
                @"██████╔╝███████╗██║  ██║███████║   ██║   ██████╔╝╚██████╔╝╚██████╔╝██║ ╚████║██████╔╝",
                @"╚═════╝ ╚══════╝╚═╝  ╚═╝╚══════╝   ╚═╝   ╚═════╝  ╚═════╝  ╚═════╝ ╚═╝  ╚═══╝╚═════╝ ",
                "",
                "                            BEASTBOUND: CONSOLE EDITION"
            };

            int logoWidth = logo.Max(line => line.Length);
            int logoStartY = height / 2 - logo.Length - 2;
            int logoStartX = (width - logoWidth) / 2;

            // Animate logo
            for (int i = 0; i < logo.Length; i++)
            {
                ConsoleUI.WriteAt(logoStartX, logoStartY + i, logo[i], ConsoleColor.White);
                Thread.Sleep(35);
            }

            // Tagline
            string tagline = "Bound by instincts. Forged in battle.";
            TextFx.TypeLineCentered(tagline, logoStartY + logo.Length + 1, ConsoleColor.DarkGray, 6);

          
            // Prompt
            TextFx.BlinkCentered("Press any key to begin", logoStartY + logo.Length + 4, ConsoleColor.White, 3, 70);
            Console.ReadKey(true);
        }
        
        private static void DrawFrame(int width, int height)
        {
            for (int x = 0; x < width; x++)
            {
                ConsoleUI.WriteAt(x, 0, "═", ConsoleColor.DarkGray);
                ConsoleUI.WriteAt(x, height - 1, "═", ConsoleColor.DarkGray);
            }
            for (int y = 0; y < height; y++)
            {
                ConsoleUI.WriteAt(0, y, "║", ConsoleColor.DarkGray);
                ConsoleUI.WriteAt(width - 1, y, "║", ConsoleColor.DarkGray);
            }
            ConsoleUI.WriteAt(0, 0, "╔", ConsoleColor.DarkGray);
            ConsoleUI.WriteAt(width - 1, 0, "╗", ConsoleColor.DarkGray);
            ConsoleUI.WriteAt(0, height - 1, "╚", ConsoleColor.DarkGray);
            ConsoleUI.WriteAt(width - 1, height - 1, "╝", ConsoleColor.DarkGray);
        }
    }
}