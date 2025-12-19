using Beastbound;
using Beastbound.Ascii;
using Beastbound.Models;
using Beastbound.Utils;
using System;

namespace Beastbound.Battle
{
    public static class BattleUI
    {
        public static void DrawBackdrop()
        {
            Console.Clear();
            ConsoleUI.DrawFrame("Battle");

            ConsoleUI.WriteCentered("────────────────────────────────────────────────────────────", 6, ConsoleColor.DarkGray);
            // No scene box, no green lines
            // You can optionally add a subtle divider or leave it clean
        }
        public static void DrawBattleHeader(Creature player, Creature enemy, string stageLabel, int difficulty, int stageNumber)
        {
            Console.SetCursorPosition(0, 1);
            string difficultyIcons = string.Concat(Enumerable.Repeat("🟢", difficulty));
            ConsoleUI.WriteCentered($"🟢 {stageLabel} 🟢 Difficulty: {difficultyIcons}", 1, ConsoleColor.Yellow);

            // Player Info
            string playerLine = $"{player.Name} 🔥 HP: {player.CurrentHP}/{player.MaxHP}";
            string playerBar = GetHPBar(player.CurrentHP, player.MaxHP, 30);
            ConsoleUI.WriteCentered(playerLine, 3, ConsoleColor.Cyan);
            ConsoleUI.WriteCentered(playerBar, 4, ConsoleColor.Green);

            // Enemy Info
            string enemyLine = $"{enemy.Name} 🌐 HP: {enemy.CurrentHP}/{enemy.MaxHP}";
            string enemyBar = GetHPBar(enemy.CurrentHP, enemy.MaxHP, 30);
            ConsoleUI.WriteCentered(enemyLine, 6, ConsoleColor.Magenta);
            ConsoleUI.WriteCentered(enemyBar, 7, ConsoleColor.Red);
        }


        private static string GetHPBar(int current, int max, int width)
        {
            if (max <= 0) max = 1; // prevent divide-by-zero
            current = Math.Max(0, Math.Min(current, max)); // clamp between 0 and max

            int filled = (int)((double)current / max * width);
            int empty = Math.Max(0, width - filled); // ensure non-negative

            return "[" + new string('█', filled) + new string(' ', empty) + "]";
        }

        public static void DrawPanels(Creature player, Creature enemy, int stageNumber)
        {
            int width = Console.WindowWidth;
            int height = Console.WindowHeight;
            int spriteY = height / 2 - 10; // Adjust for art height

            // 🧠 Draw HP bars at the top
            string playerHP = $"{player.Name} Lv.{player.Level} HP: {player.CurrentHP}/{player.MaxHP}";
            string enemyHP = $"{enemy.Name} Lv.{enemy.Level} HP: {enemy.CurrentHP}/{enemy.MaxHP}";

            ConsoleUI.WriteAt(2, 2, playerHP, ConsoleColor.Cyan);
            ConsoleUI.WriteAt(width - enemyHP.Length - 2, 2, enemyHP, ConsoleColor.Magenta);

            // 🎨 Draw player ASCII art (left side, centered vertically)
            string playerArt = player.Name switch
            {
                "Charizard" => AsciiArtLibrary.CharizardArt,
                "Blastoise" => AsciiArtLibrary.BlastoiseArt,
                "Venusaur" => AsciiArtLibrary.VenusaurArt,
                _ => AsciiArtLibrary.CharizardArt
            };
            DrawAsciiCreature(playerArt, 5, spriteY, ConsoleColor.Cyan);

            // 🎨 Draw enemy ASCII art (right side, centered vertically)
            string enemyArt = stageNumber switch
            {
                1 => AsciiArtLibrary.GhostBoss,
                2 => AsciiArtLibrary.SecondBoss,
                3 => AsciiArtLibrary.FinalBoss,
                _ => AsciiArtLibrary.GhostBoss
            };
            DrawAsciiCreature(enemyArt, width - 55, spriteY, ConsoleColor.Magenta);


        }

        private static void DrawAsciiCreature(string art, int x, int y, ConsoleColor color)
        {
            var lines = art.Split('\n');
            int maxY = Console.BufferHeight - 1;
            int maxX = Console.BufferWidth - 1;

            for (int i = 0; i < lines.Length; i++)
            {
                int drawY = y + i;
                if (drawY < 0 || drawY > maxY) continue;

                string line = lines[i];
                int drawX = Math.Min(x, maxX - line.Length);
                if (drawX < 0) continue;

                Console.SetCursorPosition(drawX, drawY);
                Console.ForegroundColor = color;
                Console.WriteLine(line);
            }

            Console.ResetColor();
        }

        public static void DrawMoveBox(Creature player)
        {
            int width = Console.WindowWidth;
            int boxX = width / 2 - 28;
            int boxY = Console.WindowHeight * 2 / 10;

            ConsoleUI.DrawBox(boxX, boxY, 56, 7, ConsoleColor.DarkGray);
            ConsoleUI.WriteAt(boxX + 2, boxY + 1, "Choose a move:", ConsoleColor.White);

            for (int i = 0; i < player.Moves.Count && i < 4; i++)
            {
                var move = player.Moves[i];
                string label = $"{i + 1}. {move.Name} - {move.Type} Pow: {move.Power} Acc: {move.Accuracy}%";
                ConsoleUI.WriteAt(boxX + 2, boxY + 2 + i, label, ConsoleColor.Gray);
            }
        }

        public static void DrawMoveBar(Creature player)
        {
            int width = Console.WindowWidth;
            int boxWidth = width - 20;
            int boxX = 10;
            int boxY = Console.WindowHeight / 2;

            // Draw outer box
            ConsoleUI.DrawBox(boxX, boxY, boxWidth, 7, ConsoleColor.DarkGray);

            // Title
            ConsoleUI.WriteAt(boxX + 2, boxY + 1, "Choose a move:", ConsoleColor.White);

            // Move options (ordered vertically inside the box)
            var moves = player.Moves.Take(4).ToList(); // limit to 4
            for (int i = 0; i < moves.Count; i++)
            {
                var move = moves[i];
                string label = $"[{i + 1}] {move.Name,-14} ({move.Type})  Pow {move.Power,-3} / Acc {move.Accuracy}%";
                ConsoleUI.WriteAt(boxX + 4, boxY + 2 + i, label, ConsoleColor.Cyan);
            }


        }

 

        public static int SelectMove(Creature player, int stageNumber)
        {
            Console.Clear();
            DrawBackdrop();
            var dummyEnemy = new Creature { Name = "???", Level = 50, CurrentHP = 100, MaxHP = 100 };
            DrawPanels(player, dummyEnemy, stageNumber);
            DrawMoveBar(player);

            ConsoleUI.WriteCentered("Press 1–4 to choose your move", Console.WindowHeight / 2 + 8, ConsoleColor.White);

            while (true)
            {
                var key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.D1 || key == ConsoleKey.NumPad1) return 0;
                if (key == ConsoleKey.D2 || key == ConsoleKey.NumPad2) return 1;
                if (key == ConsoleKey.D3 || key == ConsoleKey.NumPad3) return 2;
                if (key == ConsoleKey.D4 || key == ConsoleKey.NumPad4) return 3;
            }
        }


        private static string HealthBar(int hp, int maxHp, int width)
        {
            int filled = (int)Math.Round((hp / (double)maxHp) * width);
            filled = Math.Clamp(filled, 0, width);
            return new string('█', filled) + new string(' ', width - filled);
        }
    }
}