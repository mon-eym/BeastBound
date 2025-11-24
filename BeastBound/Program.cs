using System;

namespace PokelikeConsole
{
    internal static class Program
    {
        static void Main()
        {
            Console.Title = "Pokémon-style Console Map (C#)";
            Console.CursorVisible = false;

            // Optional: improve line drawing on Windows terminals
            try
            {
                Console.OutputEncoding = System.Text.Encoding.UTF8;
            }
            catch { /* fallback silently */ }

            // Replace using declaration with using statement for C# 7.3 compatibility
            using (var game = new Game())
            {
                game.Run();
            }
        }
    }
}