using Beastbound.Utils;
using System;

namespace Beastbound.Battle
{
    public static class BattleResultScreen
    {
        public static void Show(bool playerWon, int stageNumber)
        {
            Console.Clear();

            string[] banner = playerWon
                ? new[]
                {
                    "░██    ░██ ░██████  ░██████  ░██████████  ░██████   ░█████████  ░██     ░██ ░██",
                    "░██    ░██   ░██   ░██   ░██     ░██     ░██   ░██  ░██     ░██  ░██   ░██  ░██ ",
                    "░██    ░██   ░██  ░██            ░██    ░██     ░██ ░██     ░██   ░██ ░██   ░██ ",
                    "░██    ░██   ░██  ░██            ░██    ░██     ░██ ░█████████     ░████    ░██ ",
                    " ░██  ░██    ░██  ░██            ░██    ░██     ░██ ░██   ░██       ░██     ░██",
                    "  ░██░██     ░██   ░██   ░██     ░██     ░██   ░██  ░██    ░██      ░██         ",
                    "   ░███    ░██████  ░██████      ░██      ░██████   ░██     ░██     ░██     ░██ "
                }
                : new[]
                {
                    "░██     ░██   ░██████   ░██     ░██    ░██           ░██████     ░██████   ░██████████░██ ",
                    " ░██   ░██   ░██   ░██  ░██     ░██    ░██          ░██   ░██   ░██   ░██      ░██    ░██ ",
                    "  ░██ ░██   ░██     ░██ ░██     ░██    ░██         ░██     ░██ ░██             ░██    ░██",
                    "   ░████    ░██     ░██ ░██     ░██    ░██         ░██     ░██  ░████████      ░██    ░██ ",
                    "    ░██     ░██     ░██ ░██     ░██    ░██         ░██     ░██         ░██     ░██    ░██ ",
                    "    ░██      ░██   ░██   ░██   ░██     ░██          ░██   ░██   ░██   ░██      ░██        ",
                    "    ░██       ░██████     ░██████      ░██████████   ░██████     ░██████       ░██    ░██ "
                };

            Console.ForegroundColor = playerWon ? ConsoleColor.Green : ConsoleColor.Red;
            int startY = 4;
            for (int i = 0; i < banner.Length; i++)
            {
                ConsoleUI.WriteCentered(banner[i], startY + i, playerWon ? ConsoleColor.Green : ConsoleColor.Red);
            }
            Console.ResetColor();

            ConsoleUI.WriteCentered(playerWon ? "You defeated the boss!" : "Your Pokémon fainted...",
                startY + banner.Length + 2,
                playerWon ? ConsoleColor.Green : ConsoleColor.Red);

            ConsoleUI.WriteCentered(playerWon ? $"Prepare for Stage {stageNumber + 1}..." : "Game Over. Try again!",
                startY + banner.Length + 4,
                ConsoleColor.Yellow);

            ConsoleUI.WriteCentered("Press any key to continue",
                startY + banner.Length + 6,
                ConsoleColor.White);

            Console.ReadKey(true);
        }
    }
}