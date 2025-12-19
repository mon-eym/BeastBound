using Beastbound;
using Beastbound.Audio;
using Beastbound.Menu;
using Beastbound.Models;
using Beastbound.Utils;
using System;
using Beastbound.Battle;
using System.Numerics;
using Beastbound.Ascii;

namespace Beastbound.Battle
{
    public static class BattleEngine
    {
        private static readonly Random Rng = new Random();

        public static bool StartBattle(string selectedName, int stageNumber)
        {
            // Create player
            Creature player = selectedName switch
            {
                "Charizard" => PokemonFactory.CreateCharizard(),
                "Blastoise" => PokemonFactory.CreateBlastoise(),
                "Venusaur" => PokemonFactory.CreateVenusaur(),
                _ => PokemonFactory.CreateCharizard()
            };

            // Create enemy
            Creature enemy = Factory.CreateBoss(stageNumber, selectedName);
            Creature.PlayerPrimaryType = player.PrimaryType;

            // Initial draw
            BattleUI.DrawBackdrop();
            BattleUI.DrawBattleHeader(player, enemy, $"Stage {stageNumber}", 2, stageNumber);
            BattleUI.DrawPanels(player, enemy, stageNumber);
            BattleUI.DrawMoveBox(player);

            // ✅ Main battle loop
            while (player.CurrentHP > 0 && enemy.CurrentHP > 0)
            {
                // --- Player turn ---
                int moveIndex = GetPlayerMoveIndex(player.Moves);
                Move playerMove = player.Moves[moveIndex];
                ExecuteTurnStep(player, enemy, playerMove, stageNumber);

                if (enemy.IsFainted) break;

                // --- Enemy turn ---
                Move enemyMove = ChooseEnemyMove(enemy);
                ExecuteTurnStep(enemy, player, enemyMove, stageNumber);

                // Redraw UI
                BattleUI.DrawBattleHeader(player, enemy, $"Stage {stageNumber}", 2, stageNumber);
                BattleUI.DrawPanels(player, enemy, stageNumber);
                BattleUI.DrawMoveBox(player);
            }

            // ✅ Show result screen
            if (enemy.CurrentHP <= 0)
            {
                BattleResultScreen.Show(true, stageNumber);
                return true;
            }
            else if (player.CurrentHP <= 0)
            {
                BattleResultScreen.Show(false, stageNumber);
                return false;
            }

            return false; // fallback
        }

        // --- Input handling ---
        private static int GetPlayerMoveIndex(IReadOnlyList<Move> moves)
        {
            while (true)
            {
                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1: return 0;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2: return 1;
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3: return 2;
                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4: return 3;
                    default:
                        ConsoleUI.WriteCentered("Select 1-4 to choose a move.", 24, ConsoleColor.DarkGray);
                        break;
                }
            }
        }

        // --- Turn execution ---
        private static bool ExecuteTurnStep(Creature user, Creature target, Move move, int stageNumber)
        {
            if (user.IsFainted || target.IsFainted) return target.IsFainted;

            if (!RollHit(move.Accuracy))
            {
                ConsoleUI.WriteLog($"{user.Name}'s {move.Name} missed!", ConsoleColor.DarkYellow);
                return target.IsFainted;
            }

            int damage = CalculateDamage(move, user, target);
            target.CurrentHP = Math.Max(0, target.CurrentHP - damage);

            ConsoleUI.WriteLog($"{user.Name} used {move.Name}! (-{damage} HP)", ConsoleColor.White);

            BattleUI.DrawPanels(user, target, stageNumber);
            return target.IsFainted;
        }

        // --- Accuracy roll ---
        private static bool RollHit(int accuracyPercent)
        {
            int acc = Math.Max(0, Math.Min(accuracyPercent, 100));
            int roll = Rng.Next(1, 101);
            return roll <= acc;
        }

        // --- Damage formula ---
        private static int CalculateDamage(Move move, Creature attacker, Creature defender)
        {
            int level = attacker.Level;
            double baseDamage =
                (((2.0 * level / 5.0 + 2) * move.Power * (attacker.Attack / (double)Math.Max(1, defender.Defense))) / 50.0) + 2.0;

            double typeEff = TypeChart.Effectiveness(move.Type, defender.PrimaryType);
            if (defender.SecondaryType != Type.None)
                typeEff *= TypeChart.Effectiveness(move.Type, defender.SecondaryType);

            double stab = (move.Type == attacker.PrimaryType || move.Type == attacker.SecondaryType) ? 1.5 : 1.0;
            bool crit = Rng.Next(0, 16) == 0;
            double critMod = crit ? 1.5 : 1.0;
            double rand = (Rng.Next(85, 101)) / 100.0;

            int damage = (int)Math.Max(1, Math.Round(baseDamage * typeEff * stab * critMod * rand));

            if (crit) ConsoleUI.WriteLog("A critical hit!", ConsoleColor.Magenta);
            if (typeEff > 1.0) ConsoleUI.WriteLog("It's super effective!", ConsoleColor.Green);
            else if (typeEff < 1.0 && typeEff > 0) ConsoleUI.WriteLog("It's not very effective...", ConsoleColor.DarkGray);
            else if (typeEff == 0) ConsoleUI.WriteLog("It had no effect...", ConsoleColor.DarkGray);

            return damage;
        }

        // --- Enemy AI ---
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

        // --- Difficulty scaling ---
        public static int CalculateDifficulty(int defeatedOpponents, string bossType)
        {
            int baseDifficulty = 1 + defeatedOpponents * 3;

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