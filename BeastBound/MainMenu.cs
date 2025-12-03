using System;
using Beastbound.Battle;
using Beastbound.Utils;
using Beastbound;

namespace Beastbound.Menu
{
    public static class MainMenu
    {
        public static void Show()
        {
            while (true)
            {
                Console.Clear();
                ConsoleUI.DrawFrame("Main Menu");
                var options = new[]
                {
                    "Start Battle",
                    "Credits",
                    "Exit"
                };

                int choice = ConsoleUI.Menu(options, highlightColor: ConsoleColor.Cyan);

                if (choice == 0)
                {
                    BattleEngine.RunSingleBattle();
                }
                else if (choice == 1)
                {
                    ShowCredits();
                }
                else
                {
                    ConsoleUI.CleanExit();
                    return;
                }
            }
        }

        private static void ShowCredits()
        {
            Console.Clear();
            ConsoleUI.DrawFrame("Credits");
            ConsoleUI.WriteCentered("Beastbound — a turn-based console battle experience", 6, ConsoleColor.White);
            ConsoleUI.WriteCentered("Inspired by classic monster battles and B2W2 pacing", 8, ConsoleColor.DarkGray);
            ConsoleUI.WriteCentered("Design & Code: You + Copilot", 10, ConsoleColor.Gray);
            ConsoleUI.WriteCentered("Press any key to return", 14, ConsoleColor.DarkGray);
            Console.ReadKey(true);
        }
    }
}