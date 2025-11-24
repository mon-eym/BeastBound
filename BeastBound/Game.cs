using System;
using System.Threading;

namespace PokelikeConsole
{
    internal sealed class Game : IDisposable
    {
        private readonly World _world;
        private readonly Renderer _renderer;
        private readonly Input _input;
        private readonly Npc[] _npcs;

        private bool _running = true;
        private const int TargetFps = 30;

        public Game()
        {
            _world = new World();
            _renderer = new Renderer(Console.WindowWidth, Console.WindowHeight - 3, useAsciiOnly: false);
            _input = new Input();

            _npcs = new[]
            {
                new Npc("Youngster", 15, 15, "I like shorts!"),
                new Npc("Professor", 20, 12, "Technology is incredible!"),
                new Npc("Swimmer", 30, 65, "The water is so refreshing!"),
                new Npc("Hiker", 125, 55, "Caves are full of mystery."),
                new Npc("Bug Catcher", 110, 15, "I saw a rare Caterpie!"),
                new Npc("Lass", 18, 22, "Do you like cute Pokémon?"),
                new Npc("Fisherman", 40, 70, "The ocean hides many secrets."),
                new Npc("Guard", 126, 54, "Only trainers with a badge may enter."),
                new Npc("Old Man", 80, 39, "Route 1 leads to Viridian City.")
            };
        }

        public void Run()
        {
            var frameDuration = TimeSpan.FromMilliseconds(1000.0 / TargetFps);

            while (_running)
            {
                var frameStart = DateTime.UtcNow;

                HandleInput();

                foreach (var npc in _npcs)
                    npc.UpdateWander(_world.CurrentMap);

                _renderer.Draw(_world.CurrentMap, _world.Player, _npcs);

                var elapsed = DateTime.UtcNow - frameStart;
                var sleep = frameDuration - elapsed;
                if (sleep.TotalMilliseconds > 0) Thread.Sleep(sleep);
            }
        }

        private void HandleInput()
        {
            while (_input.TryReadKey(out var key))
            {
                switch (key)
                {
                    case ConsoleKey.Q:
                        _running = false;
                        return;

                    case ConsoleKey.Tab:
                        _renderer.ToggleAsciiMode();
                        break;

                    case ConsoleKey.A:
                        Interact();
                        break;

                    case ConsoleKey.UpArrow:
                        TryMove(0, -1);
                        break;
                    case ConsoleKey.DownArrow:
                        TryMove(0, 1);
                        break;
                    case ConsoleKey.LeftArrow:
                        TryMove(-1, 0);
                        break;
                    case ConsoleKey.RightArrow:
                        TryMove(1, 0);
                        break;
                }
            }
        }

        private void TryMove(int dx, int dy)
        {
            var map = _world.CurrentMap;
            var player = _world.Player;

            var nx = player.X + dx;
            var ny = player.Y + dy;

            if (!map.InBounds(nx, ny)) return;

            var tile = map.Get(nx, ny);
            if (!tile.IsWalkable)
            {
                // Handle map transitions
                if (tile.Type == TileType.Door)
                {
                    if (_world.CurrentMapName == "Overworld" && nx > 120)
                        _world.SwitchMap("Cave", 20, 10);
                    else if (_world.CurrentMapName == "Overworld" && nx < 30)
                        _world.SwitchMap("House1", 10, 6);
                    else if (_world.CurrentMapName == "Cave")
                        _world.SwitchMap("Overworld", 127, 55);
                    else if (_world.CurrentMapName == "House1")
                        _world.SwitchMap("Overworld", 12, 22);
                }
                return;
            }

            player.StepDelayTicks = tile.Type == TileType.GrassTall ? 1 : 0;
            if (!player.CanStep()) return;

            foreach (var npc in _npcs)
                if (npc.X == nx && npc.Y == ny)
                    return;

            player.MoveTo(nx, ny);
        }

        private void Interact()
        {
            var map = _world.CurrentMap;
            var player = _world.Player;

            foreach (var npc in _npcs)
            {
                if (Math.Abs(npc.X - player.X) + Math.Abs(npc.Y - player.Y) == 1)
                {
                    _renderer.ShowDialog($"{npc.Name}: {npc.Dialog}");
                    return;
                }
            }

            var tile = map.Get(player.X, player.Y);
            switch (tile.Type)
            {
                case TileType.Sign:
                    _renderer.ShowDialog("Sign: Welcome to BeastBound!");
                    break;
                case TileType.Door:
                    _renderer.ShowDialog("The door is locked.");
                    break;
                case TileType.GrassTall:
                    _renderer.ShowDialog("The grass rustles...");
                    break;
                default:
                    _renderer.ShowDialog("Nothing interesting here.");
                    break;
            }
        }

        public void Dispose()
        {
            Console.CursorVisible = true;
            _renderer?.Dispose();
        }
    }
}