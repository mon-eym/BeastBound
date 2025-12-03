using System;
using Beastbound.Models;
using Beastbound.Utils;
using Beastbound;

namespace Beastbound.Battle
{
    public static class BattleEngine
    {
        private static readonly Random Rng = new Random();

        public static void RunSingleBattle()
        {
            Console.Clear();
            ConsoleUI.DrawFrame("Battle");

            // Create creatures
            var (player, enemy) = Factory.CreateDemoDuel();

            BattleUI.DrawBackdrop();
            BattleUI.DrawPanels(player, enemy);

            ConsoleUI.WriteLog("A wild " + enemy.Name + " appeared!", ConsoleColor.White);
            ConsoleUI.PressToContinue();

            bool battleOver = false;
            while (!battleOver)
            {
                BattleUI.DrawPanels(player, enemy);
                int moveIndex = BattleUI.SelectMove(player);

                var playerMove = player.Moves[moveIndex];
                ConsoleUI.WriteLog($"{player.Name} used {playerMove.Name}!", ConsoleColor.Cyan);

                // Determine turn order (Speed)
                var first = player.Speed >= enemy.Speed ? (player, playerMove) : (enemy, ChooseEnemyMove(enemy));
                var second = player.Speed >= enemy.Speed ? (enemy, ChooseEnemyMove(enemy)) : (player, playerMove);

                // Execute first
                battleOver = ExecuteTurnStep(first.Item1, second.Item1, first.Item2);
                if (battleOver) break;

                // Execute second (if target still alive)
                battleOver = ExecuteTurnStep(second.Item1, first.Item1, second.Item2);
            }

            // End
            if (player.IsFainted)
            {
                ConsoleUI.WriteLog($"{player.Name} fainted... You lost.", ConsoleColor.Red);
            }
            else
            {
                ConsoleUI.WriteLog($"{enemy.Name} fainted! You won!", ConsoleColor.Green);
            }
            ConsoleUI.PressToContinue();
        }

        private static bool ExecuteTurnStep(Creature user, Creature target, Move move)
        {
            if (user.IsFainted || target.IsFainted) return user.IsFainted || target.IsFainted;

            // Miss chance
            if (Rng.Next(0, 100) >= move.Accuracy)
            {
                ConsoleUI.WriteLog($"{user.Name}'s {move.Name} missed!", ConsoleColor.DarkYellow);
                return target.IsFainted;
            }

            // Basic damage formula (B2W2-inspired, simplified)
            // Damage ~ (((2*Level/5+2) * Power * Atk/Def)/50 + 2) * Modifiers
            int level = user.Level;
            double baseDamage =
                (((2.0 * level / 5.0 + 2) * move.Power * (user.Attack / (double)Math.Max(1, target.Defense))) / 50.0) + 2.0;

            // Type effectiveness
            double typeEff = TypeChart.Effectiveness(move.Type, target.PrimaryType);
            if (target.SecondaryType != Type.None)
                typeEff *= TypeChart.Effectiveness(move.Type, target.SecondaryType);

            // STAB
            double stab = (move.Type == user.PrimaryType || move.Type == user.SecondaryType) ? 1.5 : 1.0;

            // Critical (simplified ~6.25%)
            bool crit = Rng.Next(0, 16) == 0;
            double critMod = crit ? 1.5 : 1.0;

            // Random factor [85%..100%]
            double rand = (Rng.Next(85, 101)) / 100.0;

            int damage = (int)Math.Max(1, Math.Round(baseDamage * typeEff * stab * critMod * rand));

            target.CurrentHP = Math.Max(0, target.CurrentHP - damage);

            // Feedback messaging in B2W2 style
            if (crit) ConsoleUI.WriteLog("A critical hit!", ConsoleColor.Magenta);
            if (typeEff > 1.0) ConsoleUI.WriteLog("It's super effective!", ConsoleColor.Green);
            else if (typeEff < 1.0 && typeEff > 0) ConsoleUI.WriteLog("It's not very effective...", ConsoleColor.DarkGray);
            else if (typeEff == 0) ConsoleUI.WriteLog("It had no effect...", ConsoleColor.DarkGray);

            BattleUI.AnimateHit(target);
            BattleUI.DrawPanels(user, target);

            return target.IsFainted;
        }

        private static Move ChooseEnemyMove(Creature enemy)
        {
            // Simple AI: prefer super-effective moves if any, else strongest
            Move best = enemy.Moves[0];
            double bestScore = ScoreMove(best);

            foreach (var m in enemy.Moves)
            {
                double s = ScoreMove(m);
                if (s > bestScore)
                {
                    best = m;
                    bestScore = s;
                }
            }
            return best;

            double ScoreMove(Move m)
            {
                double eff = TypeChart.Effectiveness(m.Type, Factory.PlayerPrimaryType);
                return m.Power * eff * (m.Accuracy / 100.0);
            }
        }
    }
}