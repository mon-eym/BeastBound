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
        public static void DrawBattleHeader(Creature player, Creature enemy, string stageName, int difficulty)
        {
            int width = Console.WindowWidth;

            // Player info (left)
            string playerInfo = $"{player.Name} 🔥 Lv.{player.Level}  HP: {player.CurrentHP}/{player.MaxHP}";
            ConsoleUI.WriteAt(2, 1, playerInfo, ConsoleColor.Cyan);

            // Enemy info (right)
            string enemyInfo = $"{enemy.Name} 🌀 Lv.{enemy.Level}  HP: {enemy.CurrentHP}/{enemy.MaxHP}";
            int enemyX = width - enemyInfo.Length - 2;
            ConsoleUI.WriteAt(enemyX, 1, enemyInfo, ConsoleColor.Magenta);

            // Stage + difficulty (centered)
            string flames = string.Concat(Enumerable.Repeat("🔥", difficulty));
            string centerInfo = $"🌿 {stageName} 🌿   Difficulty: {flames}";
            int centerX = (width - centerInfo.Length) / 2;
            ConsoleUI.WriteAt(centerX, 3, centerInfo, ConsoleColor.Green);
        }

        public static void DrawPanels(Creature player, Creature enemy)
        {
            int width = Console.WindowWidth;
            int spriteY = Console.WindowHeight / 2 - 10; // Adjust based on art height

            // Player panel (top left)
            DrawPanel(2, 5, player.Name, player.Level, player.CurrentHP, player.MaxHP, ConsoleColor.Cyan);

            // Enemy panel (top right)
            int enemyPanelX = width - 30;
            DrawPanel(enemyPanelX, 5, enemy.Name, enemy.Level, enemy.CurrentHP, enemy.MaxHP, ConsoleColor.Magenta);

            // ASCII art models
            if (player.Name.Contains("Pyrodon"))
                DrawAsciiCreature(AsciiArtLibrary.Pyrodon, 5, spriteY, ConsoleColor.Cyan); // left side

            if (enemy.Name.Contains("???"))
                DrawAsciiCreature(AsciiArtLibrary.GhostBoss, width - 55, spriteY, ConsoleColor.Magenta); // right side
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

        private static void DrawAsciiCreature(string[] art, int x, int y, ConsoleColor color)
        {
            for (int i = 0; i < art.Length; i++)
            {
                ConsoleUI.WriteAt(x, y + i, art[i], color);
            }
        }


        public static void AnimateHit(Creature target)
        {
            // Small shake
            int x = target.IsPlayer ? 22 : 62;
            int y = target.IsPlayer ? 15 : 12;
            for (int i = 0; i < 6; i++)
            {
                ConsoleUI.WriteAt(x + (i % 2 == 0 ? 1 : -1), y, target.IsPlayer ? "(ง•̀_•́)ง" : "ᕦ(✧∇✧)ᕤ",
                    target.IsPlayer ? ConsoleColor.Cyan : ConsoleColor.Magenta);
                System.Threading.Thread.Sleep(20);
                ConsoleUI.WriteAt(x, y, target.IsPlayer ? "(ง•̀_•́)ง" : "ᕦ(✧∇✧)ᕤ",
                    target.IsPlayer ? ConsoleColor.Cyan : ConsoleColor.Magenta);
            }
        }

        public static int SelectMove(Creature player)
        {
            Console.Clear();
            DrawBackdrop();
            var dummyEnemy = new Creature { Name = "???", Level = 50, CurrentHP = 100, MaxHP = 100 };
            DrawPanels(player, dummyEnemy);
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

        private static void DrawPanel(int x, int y, string name, int level, int hp, int maxHp, ConsoleColor color)
        {
            string typeIcon = name.Contains("Pyro") ? "🔥" : name.Contains("Tempest") ? "🌪️" : "🐾";
            string label = $"{typeIcon} {name} Lv.{level}";
            string hpText = $"HP: {hp}/{maxHp}";
            string bar = HealthBar(hp, maxHp, 20);

            // Top line: name + level
            ConsoleUI.WriteAt(x, y, label, color);

            // Second line: HP text
            ConsoleUI.WriteAt(x, y + 1, hpText, ConsoleColor.White);

            // Third line: HP bar
            ConsoleUI.WriteAt(x, y + 2, $"[{bar}]", ConsoleColor.White);
        }

        private static string HealthBar(int hp, int maxHp, int width)
        {
            int filled = (int)Math.Round((hp / (double)maxHp) * width);
            filled = Math.Clamp(filled, 0, width);
            return new string('█', filled) + new string(' ', width - filled);
        }
    }
}