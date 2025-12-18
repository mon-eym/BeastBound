using Beastbound.Utils;
using System;

namespace Beastbound.Menu
{
    public static class PokemonMenu
    {
        public static string SelectedPokemon { get; private set; } = "";

        public static void Show()
        {
            Console.Clear();
            ConsoleUI.DrawPanel("Choose Your Pokémon Starter");

            int startY = 5;
            ConsoleUI.WriteCentered("Select wisely — your choice affects battle difficulty!", startY, ConsoleColor.DarkGray);

            // Decorative arrows
            ConsoleUI.WriteCentered("⇨⇨⇨⇨⇨⇨⇨⇨⇨⇨⇨⇨⇨⇨⇨⇨⇨⇨⇨⇨⇨⇨⇨⇨⇨⇨⇨⇨⇨⇨⇨⇨⇨", startY + 1, ConsoleColor.Gray);

            // Pokémon options
            ConsoleUI.WriteCentered("[1] Charizard 🔥", startY + 3, ConsoleColor.Red);
            ConsoleUI.WriteCentered("[2] Blastoise 🌊", startY + 5, ConsoleColor.Blue);
            ConsoleUI.WriteCentered("[3] Venusaur 🌱", startY + 7, ConsoleColor.Green);

            ConsoleUI.WriteCentered("Press 1, 2, or 3 to choose", startY + 9, ConsoleColor.White);

            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    SelectedPokemon = "Charizard";
                    break;
                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    SelectedPokemon = "Blastoise";
                    break;
                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    SelectedPokemon = "Venusaur";
                    break;
                default:
                    SelectedPokemon = "Charizard"; // fallback
                    break;
            }

            Console.Clear();
            ConsoleUI.DrawPanel("Starter Confirmed");
            ConsoleUI.WriteCentered($"You chose {SelectedPokemon}!", 6, ConsoleColor.White);
            ConsoleUI.WriteCentered("Prepare to face your first opponent...", 8, ConsoleColor.DarkGray);
            Console.ReadKey(true);
        }


        // Difficulty scaling based on defeated opponents
        public static int CalculateDifficulty(int defeatedOpponents, string bossType)
        {
            int baseDifficulty = defeatedOpponents * 2;

            // Advantage if Pokémon type matches boss weakness
            if ((SelectedPokemon == "Charizard" && bossType == "Grass") ||
                (SelectedPokemon == "Blastoise" && bossType == "Fire") ||
                (SelectedPokemon == "Venusaur" && bossType == "Water"))
            {
                baseDifficulty -= 2; // easier if correct choice
            }
            else
            {
                baseDifficulty += 2; // harder if wrong choice
            }

            return Math.Max(1, baseDifficulty); // ensure difficulty ≥ 1
        }
    }
}