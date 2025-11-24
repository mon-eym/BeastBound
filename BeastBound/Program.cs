using System;
using System.Threading;
using WindowsInput;
using WindowsInput.Native;

namespace PokelikeConsole
{
    internal static class Program
    {
        static void Main()
        {
            Console.Title = "Pokémon-style Console Map (C#)";
            Console.CursorVisible = false;

            // Optional: improve line drawing
            try { Console.OutputEncoding = System.Text.Encoding.UTF8; } catch { }

            // Wait briefly to ensure console is initialized
            Thread.Sleep(500);

            // Simulate Alt+Enter to toggle fullscreen
            var sim = new InputSimulator();
            sim.Keyboard.ModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.RETURN);

            // Show intro AFTER fullscreen
            Intro.Show();

            Game game = null;
            try
            {
                game = new Game();
                game.Run();
            }
            finally
            {
                if (game != null)
                    game.Dispose();
            }
        }
    }
}