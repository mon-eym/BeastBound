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
            Console.Title = "Beastbound";
            Console.CursorVisible = false;
            ConsoleUI.Init();

            // Simulate Alt + Enter to go fullscreen
            var sim = new InputSimulator();
            sim.Keyboard.ModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.RETURN);


            // Play intro asynchronously (doesn't freeze the game)
            Play("Audio\\intro.wav");

            // Run your intro animation / logic
            IntroRunner.Play();

            // Switch to menu music after intro
            Stop(); // stop intro before starting menu
            PlayLoop("Audio\\menu.wav");
            MainMenu.Show();

            // Switch to boss music when battle starts
            Stop(); // stop menu before starting boss
            PlayLoop("Audio\\boss.wav");

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