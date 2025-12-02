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
        private const int TargetFps = 500;

        public Game()
        {
            _world = new World();
            _renderer = new Renderer(Console.WindowWidth, Console.WindowHeight - 3, useAsciiOnly: false);
            _input = new Input();

            _npcs = new[]
            {
                // Overworld NPCs (coordinates are in the Overworld map)
                new Npc("Youngster", 75, 38, "The hub makes it easy to get around!"),
                new Npc("Professor", 80, 32, "The town is just above the hub."),
                new Npc("Fisherman", 80, 68, "The beach is my favorite spot."),
                new Npc("Hiker", 125, 38, "The cave ahead gets tougher!"),

                // Battle House NPCs (BattleHouse map coordinates)
                new Npc("Lass",       6,  6, "Welcome to the Battle House! Try the easy battle first."),
                new Npc("Ace Trainer",15, 6, "Think you're ready for a real challenge?"),
                new Npc("Champion",   24, 6, "Only the strongest can defeat me.")
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

            // Handle doors first (non-walkable)
            if (!tile.IsWalkable)
            {
                if (tile.Type == TileType.DoorClosed || tile.Type == TileType.DoorOpen || tile.Type == TileType.DoorLocked)
                {
                    if (_world.CurrentMapName == "Overworld")
                    {
                        // Cave entrance door (in the cave room)
                        if (nx == 125 && ny == 38)
                        {
                            _world.SwitchMap("Cave", 20, 10);
                        }
                        // Battle House door (near center)
                        else if (nx == 80 && ny == 53)
                        {
                            _world.SwitchMap("BattleHouse", 15, 14);
                        }
                        // You can add more building doors here later
                    }
                    else if (_world.CurrentMapName == "Cave")
                    {
                        // Exit cave back to overworld
                        _world.SwitchMap("Overworld", 125, 39);
                    }
                    else if (_world.CurrentMapName == "BattleHouse")
                    {
                        // Exit Battle House back to overworld (door at bottom center of BattleHouse map)
                        _world.SwitchMap("Overworld", 80, 54);
                    }

                    return;
                }

                // Other non-walkable tiles: just block movement
                return;
            }

            // Walkable tile: handle step delay based on grass
            if (tile.Type == TileType.GrassLight || tile.Type == TileType.GrassMedium)
                player.StepDelayTicks = 1;
            else
                player.StepDelayTicks = 0;

            if (!player.CanStep()) return;

            // Avoid colliding with NPCs
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
                    {
                        // You can refine this by coordinates later
                        _renderer.ShowDialog("Sign: Welcome to the BeastBound Hub!");
                    }
                    else if (_world.CurrentMapName == "BattleHouse")
                    {
                        _renderer.ShowDialog("Sign: Choose your opponent: Easy, Medium, Hard.");
                    }
                    else
                    {
                        _renderer.ShowDialog("It's a weathered sign.");
                    }
                    break;

                case TileType.DoorClosed:
                case TileType.DoorLocked:
                    _renderer.ShowDialog("The door is locked.");
                    break;

                default:
                    _renderer.ShowDialog("Nothing interesting here.");
                    break;
            }
        }

        private void HandleBattleHouseInteraction(Npc npc)
        {
            // Prepare a simple starter Pokémon for now
            var playerMon = new Pokemon
            {
                Name = "TrucVert",
                Level = 50,
                HP = 61,
                MaxHP = 61,
                Moves = new[]
                {
                    new Move { Name = "Headbutt", Power = 20 },
                    new Move { Name = "Flash",    Power = 0  },
                    new Move { Name = "Mud-Slap", Power = 15 },
                    new Move { Name = "Tackle",   Power = 10 }
                }
            };

            Pokemon enemyMon;
            string intro;
            string winMessage;

            if (npc.Name == "Lass")
            {
                // Easy battle
                enemyMon = new Pokemon
                {
                    Name = "Pidgey",
                    Level = 10,
                    HP = 30,
                    MaxHP = 30,
                    Moves = new[]
                    {
                        new Move { Name = "Gust", Power = 10 }
                    }
                };
                intro = "Lass: Let's have an easy warm-up battle!";
                winMessage = "You win! That was the easy battle.";
            }
            else if (npc.Name == "Ace Trainer")
            {
                // Medium battle
                enemyMon = new Pokemon
                {
                    Name = "Growlithe",
                    Level = 25,
                    HP = 50,
                    MaxHP = 50,
                    Moves = new[]
                    {
                        new Move { Name = "Ember", Power = 15 }
                    }
                };
                intro = "Ace Trainer: I'll test your real strength!";
                winMessage = "That was tough, but you won the medium battle!";
            }
            else if (npc.Name == "Champion")
            {
                // Hard battle
                enemyMon = new Pokemon
                {
                    Name = "Mr-Mime",
                    Level = 50,
                    HP = 56,
                    MaxHP = 56,
                    Moves = new[]
                    {
                        new Move { Name = "Psybeam", Power = 20 }
                    }
                };
                intro = "Champion: Only true champions can win this.";
                winMessage = "Incredible! You defeated the Champion in the hard battle!";
            }
            else
            {
                _renderer.ShowDialog($"{npc.Name}: Welcome to the Battle House.");
                return;
            }

            _renderer.ShowDialog(intro);

            var battle = new BattleEngine(playerMon, enemyMon, _renderer);
            battle.StartBattle();

            if (playerMon.HP > 0)
            {
                _renderer.ShowDialog(winMessage);
            }
            else
            {
                _renderer.ShowDialog("You were defeated... but you can always try again.");
            }
        }

        public void Dispose()
        {
            Console.CursorVisible = true;
            _renderer?.Dispose();
        }
    }
}