using Beastbound.Audio;
using Beastbound.Battle;
using Beastbound.Intro;
using Beastbound.Menu;
using Beastbound.Utils;
using WindowsInput;
using WindowsInput.Native;

namespace Beastbound
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Beastbound";
            Console.CursorVisible = false;
            ConsoleUI.Init();

            // Simulate Alt + Enter to go fullscreen
            var sim = new InputSimulator();
            sim.Keyboard.ModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.RETURN);

            // Play intro music once
            AudioManager.PlayOnce(AudioPaths.Intro);
            IntroRunner.Play(); // Your intro animation method
            AudioManager.Stop();

            // Loop menu music
            AudioManager.PlayLoop(AudioPaths.Menu);
            MainMenu.Show(); // Your actual menu method
            AudioManager.Stop();

            // Start boss battle music
            AudioManager.PlayLoop(AudioPaths.Boss);
            BattleEngine.RunSingleBattle(); // Replace with your actual battle method
            AudioManager.Stop();
        }
    }
}