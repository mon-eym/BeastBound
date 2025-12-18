using Beastbound;
using Beastbound.Audio;
using Beastbound.Menu;
using Beastbound.Models;
using Beastbound.Utils;
using System;
using Beastbound.Battle;
using System.Numerics;

namespace Beastbound.Battle
{
    public static class BattleEngine
    {
        private static readonly Random Rng = new Random();

        public static void RunSingleBattle()
        {
            Console.Clear();
            ConsoleUI.DrawFrame("Battle");

            // Start boss battle...


            // Create player and enemy
            var (player, enemy) = Factory.CreateDemoDuel();

            BattleUI.DrawBackdrop();
            

            ConsoleUI.WriteLog("A wild " + enemy.Name + " appeared!", ConsoleColor.White);
            ConsoleUI.PressToContinue();

            int defeatedOpponents = 0;
            string bossType = "Fire"; // example type

            int difficulty = CalculateDifficulty(defeatedOpponents, bossType);
            ApplyDifficultyScaling(enemy, difficulty);
            BattleUI.DrawBattleHeader(player, enemy, "Wild Grass Clearing", difficulty);

            bool battleOver = false;
            while (!battleOver)
            {
                BattleUI.DrawPanels(player, enemy);

                int moveIndex = BattleUI.SelectMove(player); // fixed: only after player is declared
                Move playerMove = player.Moves[moveIndex];

                ConsoleUI.WriteLog($"{player.Name} used {playerMove.Name}!", ConsoleColor.Cyan);

                // Turn order
                var first = player.Speed >= enemy.Speed ? (player, playerMove) : (enemy, ChooseEnemyMove(enemy));
                var second = player.Speed >= enemy.Speed ? (enemy, ChooseEnemyMove(enemy)) : (player, playerMove);

                // Execute turns
                battleOver = ExecuteTurnStep(first.Item1, second.Item1, first.Item2);
                if (battleOver) break;

                battleOver = ExecuteTurnStep(second.Item1, first.Item1, second.Item2);
            }

            // Outcome
            if (player.IsFainted)
                ConsoleUI.WriteLog($"{player.Name} fainted... You lost.", ConsoleColor.Red);
            else
                ConsoleUI.WriteLog($"{enemy.Name} fainted! You won!", ConsoleColor.Green);

            ConsoleUI.PressToContinue();
        }

        private static void ApplyDifficultyScaling(Creature enemy, int difficulty)
        {
            difficulty = Math.Max(1, difficulty);
            enemy.MaxHP += difficulty * 10;
            enemy.Attack += difficulty * 2;
            enemy.Defense += difficulty * 2;
            enemy.Speed += difficulty;
            enemy.CurrentHP = enemy.MaxHP;
        }

        private static bool ExecuteTurnStep(Creature user, Creature target, Move move)
        {
            if (user.IsFainted || target.IsFainted) return user.IsFainted || target.IsFainted;

            if (Rng.Next(0, 100) >= move.Accuracy)
            {
                ConsoleUI.WriteLog($"{user.Name}'s {move.Name} missed!", ConsoleColor.DarkYellow);
                return target.IsFainted;
            }

            int level = user.Level;
            double baseDamage =
                (((2.0 * level / 5.0 + 2) * move.Power * (user.Attack / (double)Math.Max(1, target.Defense))) / 50.0) + 2.0;

            double typeEff = TypeChart.Effectiveness(move.Type, target.PrimaryType);
            if (target.SecondaryType != Type.None)
                typeEff *= TypeChart.Effectiveness(move.Type, target.SecondaryType);

            double stab = (move.Type == user.PrimaryType || move.Type == user.SecondaryType) ? 1.5 : 1.0;
            bool crit = Rng.Next(0, 16) == 0;
            double critMod = crit ? 1.5 : 1.0;
            double rand = (Rng.Next(85, 101)) / 100.0;

            int damage = (int)Math.Max(1, Math.Round(baseDamage * typeEff * stab * critMod * rand));
            target.CurrentHP = Math.Max(0, target.CurrentHP - damage);

            if (crit) ConsoleUI.WriteLog("A critical hit!", ConsoleColor.Magenta);
            if (typeEff > 1.0) ConsoleUI.WriteLog("It's super effective!", ConsoleColor.Green);
            else if (typeEff < 1.0 && typeEff > 0) ConsoleUI.WriteLog("It's not very effective...", ConsoleColor.DarkGray);
            else if (typeEff == 0) ConsoleUI.WriteLog("It had no effect...", ConsoleColor.DarkGray);

            BattleUI.AnimateHit(target);
            BattleUI.DrawPanels(user, target);

            return target.IsFainted;
        }

        public static void StartBattle()
        {
            var (player, enemy) = Factory.CreateDemoDuel();
            Creature.PlayerPrimaryType = player.PrimaryType;

            BattleUI.DrawBackdrop();
            BattleUI.DrawBattleHeader(player, enemy, "Forest Shrine", 2);
            BattleUI.DrawPanels(player, enemy);
            BattleUI.DrawMoveBox(player);
        }

        private static Move ChooseEnemyMove(Creature enemy)
        {
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
                double eff = TypeChart.Effectiveness(m.Type, Creature.PlayerPrimaryType);
                return m.Power * eff * (m.Accuracy / 100.0);
            }
        }



        public static int CalculateDifficulty(int defeatedOpponents, string bossType)
        {
            int baseDifficulty = 1 + defeatedOpponents * 2;

            if ((PokemonMenu.SelectedPokemon == "Charizard" && bossType == "Grass") ||
                (PokemonMenu.SelectedPokemon == "Blastoise" && bossType == "Fire") ||
                (PokemonMenu.SelectedPokemon == "Venusaur" && bossType == "Water"))
            {
                baseDifficulty -= 2;
            }
            else
            {
                baseDifficulty += 2;
            }

            return Math.Max(1, baseDifficulty);
        }
    }
}