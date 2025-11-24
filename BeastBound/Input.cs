using System;

namespace PokelikeConsole
{
    internal sealed class Input
    {
        // Non-blocking key read
        public bool TryReadKey(out ConsoleKey key)
        {
            key = default;
            if (!Console.KeyAvailable) return false;
            var info = Console.ReadKey(intercept: true);
            key = info.Key;
            return true;
        }
    }
}