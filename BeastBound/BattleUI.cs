using System;
using Beastbound.Models;
using Beastbound.Utils;
using Beastbound;

namespace Beastbound.Battle
{
    public static class BattleUI
    {
        public static void DrawBackdrop()
        {
            Console.Clear();
            ConsoleUI.DrawFrame("Battle");
            // Minimal scene
            ConsoleUI.WriteAt(6, 4, "┌───────────────────────────────────────────────────────────┐", ConsoleColor.DarkGray);
            ConsoleUI.WriteAt(6, 5, "│                    Wild Grass Clearing                    │", ConsoleColor.DarkGray);
            ConsoleUI.WriteAt(6, 6, "└───────────────────────────────────────────────────────────┘", ConsoleColor.DarkGray);

            // Platforms
            ConsoleUI.WriteAt(18, 16, "     _____      ", ConsoleColor.Green);
            ConsoleUI.WriteAt(60, 10, "     _____      ", ConsoleColor.DarkGreen);
        }

        public static void DrawPanels(Creature player, Creature enemy)
        {
            // Enemy panel (top right)
            DrawPanel(52, 2, enemy.Name, enemy.Level, enemy.CurrentHP, enemy.MaxHP, ConsoleColor.Magenta);
            // Player panel (bottom left)
            DrawPanel(4, 18, player.Name, player.Level, player.CurrentHP, player.MaxHP, ConsoleColor.Cyan);

            // Sprites (ASCII)
            ConsoleUI.WriteAt(62, 12, "ᕦ(✧∇✧)ᕤ", ConsoleColor.Magenta); // Enemy
            ConsoleUI.WriteAt(22, 15, "(ง•̀_•́)ง", ConsoleColor.Cyan);     // Player

        }

        public static void creatureModels()
        {
            Console.WriteLine(@"

             _P!""^^""!q_
           a'         `A_
          8b            ""q
        .88)        d8b  ""p
        A8P        (888)  l
      ,~            98P   l
      (  *                I
      \.      *      __   q                    ,g.
       \Ww.,_____,.wWWW)  l                   dWWB
        \WWWWWWWWWWWWWW    L._,.+'"""".       IWWWWWA
 .""""'+.__.^WWWWWWWWWW`               )      SW~""JWWW
(               __,.+.,__        __.+'      l  7WWWP
 `+.__       .+'        '+.__.+'             K   `W
      `+.__ .I            I. o               \   /
          `.I'            'I o                PW7
           cI              I  8              j'7
           CI.            .I   9            J"" 7
          C 'I.          .I'   9           J   I
          c.+.`+.,_  _.,+'   _  6     __,+^`  I'
         J    `   `~~'    .+^ `;`-""^'""      ,7'
        8:              .J      9        _,'/
         6      I`-,_,-'I       T:::.+'""`,9^
        _.9    .9       6.      ,""`---""~^`
      .'       (  ilmk   ).    .(
     (       _.9        (        )
      `+=====""'          `+.__.+'
");
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
            ConsoleUI.WriteAt(4, 22, "Choose a move:", ConsoleColor.White);

            var options = player.Moves.Select(m => $"{m.Name}  ({m.Type})  Pow {m.Power} / Acc {m.Accuracy}%").ToArray();
            return ConsoleUI.Menu(options, 24, highlightColor: ConsoleColor.Cyan, startX: 4);
        }

        private static void DrawPanel(int x, int y, string name, int level, int hp, int maxHp, ConsoleColor color)
        {
            ConsoleUI.WriteAt(x, y, $"┌──────────────────────────────┐", ConsoleColor.DarkGray);
            ConsoleUI.WriteAt(x, y + 1, $"│ {name,-18} Lv.{level,2}       │", color);
            ConsoleUI.WriteAt(x, y + 2, $"│ HP: {hp,3}/{maxHp,3}            │", ConsoleColor.White);
            ConsoleUI.WriteAt(x, y + 3, $"│ [{HealthBar(hp, maxHp, 20)}] │", ConsoleColor.White);
            ConsoleUI.WriteAt(x, y + 4, $"└──────────────────────────────┘", ConsoleColor.DarkGray);
        }

        private static string HealthBar(int hp, int maxHp, int width)
        {
            int filled = (int)Math.Round((hp / (double)maxHp) * width);
            filled = Math.Clamp(filled, 0, width);
            return new string('█', filled) + new string(' ', width - filled);
        }
    }
}