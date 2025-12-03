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

            // Simulate Alt + Enter to go full screen
            var sim = new InputSimulator();
            sim.Keyboard.ModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.RETURN);

            IntroRunner.Play();
            MainMenu.Show();
        }
    }
}