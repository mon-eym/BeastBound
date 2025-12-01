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
    // Overworld NPCs
    new Npc("Youngster", 75, 38, "The hub makes it easy to get around!"),
    new Npc("Professor", 80, 32, "The town is just above the hub."),
    new Npc("Fisherman", 80, 68, "The beach is my favorite spot."),
    new Npc("Hiker", 125, 38, "The cave ahead gets tougher!"),

    // Battle House NPCs (inside BattleHouse map coordinates)
    // Easy battle trainer
    new Npc("Lass", 6, 6, "Welcome to the Battle House! Try the easy battle first."),
    // Medium battle trainer
    new Npc("Ace Trainer", 15, 6, "Think you're ready for a real challenge?"),
    // Hard battle trainer
    new Npc("Champion", 24, 6, "Only the strongest can defeat me.")
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
                // Door transitions
                if (tile.Type == TileType.Door)
                {
                    if (_world.CurrentMapName == "Overworld")
                    {
                        // Cave entrance door
                        if (nx == 125 && ny == 38)
                            _world.SwitchMap("Cave", 20, 10);

                        // Battle House door (overworld position 78,47 from earlier)
                        else if (nx == 78 && ny == 47)
                            _world.SwitchMap("BattleHouse", 15, 14);

                        // Other house entries if you want
                    }
                    else if (_world.CurrentMapName == "Cave")
                    {
                        _world.SwitchMap("Overworld", 125, 39); // exit cave near entrance
                    }
                    else if (_world.CurrentMapName == "BattleHouse")
                    {
                        // Exit back to overworld from BattleHouse bottom door
                        _world.SwitchMap("Overworld", 78, 48);
                    }

                    return;
                }

                return;
            }

            // Walkable tile: handle movement penalties etc.
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

            // Check NPCs adjacent
            foreach (var npc in _npcs)
            {
                if (Math.Abs(npc.X - player.X) + Math.Abs(npc.Y - player.Y) == 1)
                {
                    // If we are inside BattleHouse, treat some NPCs as battle trainers
                    if (_world.CurrentMapName == "BattleHouse")
                    {
                        HandleBattleHouseInteraction(npc);
                        return;
                    }

                    _renderer.ShowDialog($"{npc.Name}: {npc.Dialog}");
                    return;
                }
            }

            // Tile interaction fallback
            var tile = map.Get(player.X, player.Y);
            switch (tile.Type)
            {
                case TileType.Sign:
                    if (_world.CurrentMapName == "Overworld")
                        _renderer.ShowDialog("Sign: Welcome to the BeastBound Hub!");
                    else if (_world.CurrentMapName == "BattleHouse")
                        _renderer.ShowDialog("Sign: Choose your opponent: Easy, Medium, Hard.");
                    else
                        _renderer.ShowDialog("It's a weathered sign.");
                    break;

                case TileType.Door:
                    _renderer.ShowDialog("The door is locked.");
                    break;

                default:
                    _renderer.ShowDialog("Nothing interesting here.");
                    break;
            }
        }

        private void HandleBattleHouseInteraction(Npc npc)
        {
            if (npc.Name == "Lass")
            {
                // Easy battle
                _renderer.ShowDialog("Lass: Let's have an easy warm-up battle!");
                _renderer.ShowDialog("You win! That was the easy battle.");
            }
            else if (npc.Name == "Ace Trainer")
            {
                // Medium battle
                _renderer.ShowDialog("Ace Trainer: I'll test your real strength!");
                _renderer.ShowDialog("That was tough, but you won the medium battle!");
            }
            else if (npc.Name == "Champion")
            {
                // Hard battle
                _renderer.ShowDialog("Champion: Only true champions can win this.");
                _renderer.ShowDialog("Incredible! You defeated the Champion in the hard battle!");
            }
            else
            {
                _renderer.ShowDialog($"{npc.Name}: Welcome to the Battle House.");
            }
        }

        public void Dispose()
        {
            Console.CursorVisible = true;
            _renderer?.Dispose();
        }
    }
}