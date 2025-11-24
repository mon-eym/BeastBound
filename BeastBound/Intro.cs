using System;
using System.Threading;

namespace PokelikeConsole
{
    internal static class Intro
    {
        public static void Show()
        {
            Console.Clear();
            Console.CursorVisible = false;

            string[] logo = {
                "██████╗ ███████╗ █████╗ ███████╗████████╗",
                "██╔══██╗██╔════╝██╔══██╗██╔════╝╚══██╔══╝",
                "██████╔╝█████╗  ███████║███████╗   ██║   ",
                "██╔═══╝ ██╔══╝  ██╔══██║╚════██║   ██║   ",
                "█████ ╝ ███████╗██║  ██║███████║   ██║   ",
                "╚════╝  ╚══════╝╚═╝  ╚═╝╚══════╝   ╚═╝   ",
                "         BeastBound Console Edition       "
            };

            foreach (var line in logo)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(line);
                Thread.Sleep(150); // animate line by line
            }

            Console.ResetColor();
            Console.WriteLine("\nPress any key to begin your adventure...");
            Console.ReadKey(true);
            Console.Clear();
        }
    }
}