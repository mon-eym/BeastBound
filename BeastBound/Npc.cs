using System;

namespace PokelikeConsole
{
    internal sealed class Npc
    {
        private static readonly Random Rng = new Random();

        public string Name { get; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public string Dialog { get; }

        private int _wanderCooldown = 0;

        public Npc(string name, int x, int y, string dialog)
        {
            Name = name;
            X = x;
            Y = y;
            Dialog = dialog;

        }

        public void UpdateWander(Map map)
        {
            if (map == null) return; // prevent crash

            if (_wanderCooldown > 0)
            {
                _wanderCooldown--;
                return;
            }

            if (_wanderCooldown > 0)
            {
                _wanderCooldown--;
                return;
            }

            // 25% chance to attempt a step
            if (Rng.NextDouble() < 0.25)
            {
                int dx = Rng.Next(-1, 2);
                int dy = Rng.Next(-1, 2);
                int nx = X + dx;
                int ny = Y + dy;

                if (map.InBounds(nx, ny) && map.Get(nx, ny).IsWalkable)
                {
                    X = nx; Y = ny;
                }
            }

            _wanderCooldown = Rng.Next(3, 10);
        }
    }
}