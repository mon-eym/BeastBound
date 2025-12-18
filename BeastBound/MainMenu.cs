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
        public static void Show()
        {
            while (true)
            {
                Console.Clear();
                ConsoleUI.DrawFrame("Main Menu");


                // Render menu options...

            

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
                    Beastbound.Menu.PokemonMenu.Show(); // Show Pokémon starter menu

                    // Optional: show selected Pokémon before battle
                    Console.Clear();
                    ConsoleUI.DrawPanel("Battle Begins");
                    ConsoleUI.WriteCentered($"You chose {PokemonMenu.SelectedPokemon}!", 6, ConsoleColor.White);
                    ConsoleUI.WriteCentered("Prepare to face your first opponent...", 8, ConsoleColor.DarkGray);
                    Console.ReadKey(true);

                    BattleEngine.RunSingleBattle(); // Start the battle
                }
                else if (choice == 2)
                {
                    ShowCredits();
                }
                else if (choice == 1)
                {
                    ShowHowToPlay();
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

        private static void ShowHowToPlay()
        {
            Console.Clear();
            ConsoleUI.DrawFrame("How to Play");
            ConsoleUI.WriteCentered("Instructions on how to play the game will go here.", 6, ConsoleColor.White);
            ConsoleUI.WriteCentered("Press any key to return", 10, ConsoleColor.DarkGray);
            Console.ReadKey(true);
        }
    }
}