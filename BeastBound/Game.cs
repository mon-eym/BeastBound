using System;
using System.Threading;

namespace PokelikeConsole
{
    internal sealed class Game : IDisposable
    {
        private readonly Map _map;
        private readonly Player _player;
        private readonly Renderer _renderer;
        private readonly Input _input;
        private readonly Npc[] _npcs;

        private bool _running = true;
        private const int TargetFps = 30;

        public Game()
        {
            _map = DemoMaps.BuildPalletTown();
            _player = new Player(_map.SpawnX, _map.SpawnY);
            _renderer = new Renderer(viewWidth: 32, viewHeight: 18, useAsciiOnly: false);
            _input = new Input();

            _npcs = new[]
            {
                new Npc("Youngster", 13, 9, "I like shorts! They're comfy and easy to wear."),
                new Npc("Professor", 6, 6, "Technology is incredible!")
            };
        }

        public void Run()
        {
            var frameDuration = TimeSpan.FromMilliseconds(1000.0 / TargetFps);

            while (_running)
            {
                var frameStart = DateTime.UtcNow;

                // Input
                HandleInput();

                // Update
                foreach (var npc in _npcs)
                    npc.UpdateWander(_map);

                // Render
                _renderer.Draw(_map, _player, _npcs);

                // Frame pacing
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
            var nx = _player.X + dx;
            var ny = _player.Y + dy;

            if (!_map.InBounds(nx, ny)) return;

            var tile = _map.Get(nx, ny);
            if (!tile.IsWalkable) return;

            // Simple movement penalty on tall grass
            if (tile.Type == TileType.GrassTall)
                _player.StepDelayTicks = 1;
            else
                _player.StepDelayTicks = 0;

            // Smooth step limiter
            if (!_player.CanStep()) return;

            // Avoid walking through NPCs
            foreach (var npc in _npcs)
                if (npc.X == nx && npc.Y == ny)
                    return;

            _player.MoveTo(nx, ny);
        }

        private void Interact()
        {
            // Interact with adjacent NPC
            foreach (var npc in _npcs)
            {
                if (Math.Abs(npc.X - _player.X) + Math.Abs(npc.Y - _player.Y) == 1)
                {
                    _renderer.ShowDialog($"{npc.Name}: {npc.Dialog}");
                    return;
                }
            }

            var tile = _map.Get(_player.X, _player.Y);
            switch (tile.Type)
            {
                case TileType.Sign:
                    _renderer.ShowDialog("Sign: Welcome to Pallet Town!");
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