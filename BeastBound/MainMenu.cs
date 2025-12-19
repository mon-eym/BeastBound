using System;
using Beastbound.Battle;
using Beastbound.Utils;
using Beastbound;
using System.Threading;
using System.IO;
using BeastBound;
using System.Media;

namespace Beastbound.Menu
{
    public static class MainMenu
    {
        public static string Show()
        {
            while (true)
            {
                Console.Clear();
                ConsoleUI.DrawFrame("Main Menu");

                var options = new[]
                {
            "Start Battle",
            "How to play",
            "Credits",
            "Exit"
        };

                int choice = ConsoleUI.Menu(options, highlightColor: ConsoleColor.Cyan);

                if (choice == 0)
                {
                    Console.Clear();
                    ConsoleUI.DrawPanel("Starter Selection");
                    PokemonMenu.Show();

                    Console.Clear();
                    ConsoleUI.DrawPanel("Battle Begins");
                    ConsoleUI.WriteCentered($"You chose {PokemonMenu.SelectedPokemon}!", 6, ConsoleColor.White);
                    ConsoleUI.WriteCentered("Prepare to face your first opponent...", 8, ConsoleColor.DarkGray);
                    Console.ReadKey(true);

                    return "start";
                }
                else if (choice == 1)
                {
                    ShowHowToPlay();
                }
                else if (choice == 2)
                {
                    ShowCredits();
                }
                else
                {
                    ConsoleUI.CleanExit();
                    return "exit";
                }
            }
        }


        private static void ShowCredits()
        {
            Console.Clear();
            ConsoleUI.DrawFrame("Credits");
            ConsoleUI.WriteCentered("Beastbound — a turn-based console battle experience", 6, ConsoleColor.White);
            ConsoleUI.WriteCentered("Inspired by classic monster battles and B2W2 pacing", 8, ConsoleColor.DarkGray);
            ConsoleUI.WriteCentered("Design & Code: John Eymard A. Ricafort & James Sumugat", 10, ConsoleColor.Gray);
            ConsoleUI.WriteCentered("Press any key to return", 14, ConsoleColor.DarkGray);
            Console.ReadKey(true);
        }

        private static void ShowHowToPlay()
        {
            Console.Clear();
            ConsoleUI.DrawFrame("How to Play");

            int line = 6;
            ConsoleUI.WriteCentered("🎮 Welcome to BeastBound!", line++, ConsoleColor.White);
            ConsoleUI.WriteCentered("Your goal is to defeat 3 elemental bosses in a retro-style turn-based battle.", line++, ConsoleColor.Gray);
            line++;

            ConsoleUI.WriteCentered("🔥 Choose your Pokémon:", line++, ConsoleColor.Yellow);
            ConsoleUI.WriteCentered("- Charizard (Fire) → strong vs Grass", line++, ConsoleColor.DarkRed);
            ConsoleUI.WriteCentered("- Blastoise (Water) → strong vs Fire", line++, ConsoleColor.Blue);
            ConsoleUI.WriteCentered("- Venusaur (Grass) → strong vs Water", line++, ConsoleColor.Green);
            line++;

            ConsoleUI.WriteCentered("⚔️ Battle Controls:", line++, ConsoleColor.Yellow);
            ConsoleUI.WriteCentered("- Press 1–4 to choose a move", line++, ConsoleColor.Gray);
            ConsoleUI.WriteCentered("- Watch for critical hits and type effectiveness", line++, ConsoleColor.Gray);
            line++;

            ConsoleUI.WriteCentered("🏆 Stage Progression:", line++, ConsoleColor.Yellow);
            ConsoleUI.WriteCentered("- Defeat Thornox (Grass), Pyronox (Fire), and Aquarion (Water)", line++, ConsoleColor.Gray);
            ConsoleUI.WriteCentered("- After each win, you can switch Pokémon for the next stage", line++, ConsoleColor.Gray);
            line++;

            ConsoleUI.WriteCentered("💡 Tip: Match your Pokémon's type to the boss's weakness!", line++, ConsoleColor.Cyan);
            line++;

            ConsoleUI.WriteCentered("Press any key to return to the main menu.", line++, ConsoleColor.DarkGray);
            Console.ReadKey(true);
        }   
    }
}