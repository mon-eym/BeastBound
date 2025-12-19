using Beastbound.Audio;
using Beastbound.Battle;
using Beastbound.Intro;
using Beastbound.Menu;
using Beastbound.Utils;
using System.Media;
using WindowsInput;
using WindowsInput.Native;

namespace Beastbound
{
    internal class Program
    {
        private static SoundPlayer soundPlayer;

        static void Main(string[] args)
        {
            Console.Title = "Beatdown";
            Console.CursorVisible = false;
            ConsoleUI.Init();

            // Simulate Alt + Enter for fullscreen
            var sim = new InputSimulator();
            sim.Keyboard.ModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.RETURN);

            // Play intro music and animation
            Play("Audio/intro.wav");
            IntroRunner.Play(); // Make sure this method returns cleanly

            // Switch to menu music
            Stop();
            PlayLoop("Audio/menu.wav");

            // Wait before showing menu
            Thread.Sleep(100);

            while (true)
            {
                string menuResult = MainMenu.Show();

                if (menuResult == "start")
                {
                    int currentStage = 1;
                    string selected = PokemonMenu.SelectedPokemon;

                    while (currentStage <= 3) // ✅ only 3 stages
                    {
                        Stop();
                        PlayLoop("Audio/boss.wav");

                        bool playerWon = BattleEngine.StartBattle(selected, currentStage);

                        if (!playerWon)
                        {
                            Console.Clear();
                            ConsoleUI.WriteCentered("Game Over! Returning to Main Menu...", 10, ConsoleColor.DarkGray);
                            Thread.Sleep(1500);
                            break;
                        }

                        // ✅ Victory: allow Pokémon switch before next stage
                        if (currentStage < 3)
                        {
                            Console.Clear();
                            ConsoleUI.WriteCentered("You won! Do you want to switch Pokémon for the next stage?", 10, ConsoleColor.White);
                            ConsoleUI.WriteCentered("Press Y to switch, any other key to continue.", 12, ConsoleColor.DarkGray);

                            var key = Console.ReadKey(true).Key;
                            if (key == ConsoleKey.Y)
                            {
                                PokemonMenu.Show();
                                selected = PokemonMenu.SelectedPokemon;
                            }
                        }

                        currentStage++;
                    }

                    // After defeat or finishing all stages, restart menu music
                    Stop();
                    PlayLoop("Audio/menu.wav");
                } 
                else if (menuResult == "exit")
                {
                    ConsoleUI.CleanExit();
                    return;
                }
            }

        }



        private static void Play(string relativePath)
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + relativePath;
            soundPlayer = new SoundPlayer(fullPath);
            soundPlayer.Load();
            soundPlayer.Play(); // non-blocking playback
        }

        private static void PlayLoop(string relativePath)
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + relativePath;
            soundPlayer = new SoundPlayer(fullPath);
            soundPlayer.Load();
            soundPlayer.PlayLooping(); // continuous loop
        }

        private static void Stop()
        {
            if (soundPlayer != null)
            {
                soundPlayer.Stop(); // stops current playback
            }
        }
    }

}