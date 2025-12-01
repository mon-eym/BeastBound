using System;

namespace PokelikeConsole
{
    internal sealed class BattleEngine
    {
        private readonly Pokemon _player;
        private readonly Pokemon _enemy;
        private readonly Renderer _renderer;

        public BattleEngine(Pokemon player, Pokemon enemy, Renderer renderer)
        {
            _player = player;
            _enemy = enemy;
            _renderer = renderer;
        }

        public void StartBattle()
        {
            while (_player.HP > 0 && _enemy.HP > 0)
            {
                DrawBattleScreen();

                var key = Console.ReadKey(true).Key;
                Move chosenMove = null;

                switch (key)
                {
                    case ConsoleKey.D1:
                        if (_player.Moves.Length > 0) chosenMove = _player.Moves[0];
                        break;
                    case ConsoleKey.D2:
                        if (_player.Moves.Length > 1) chosenMove = _player.Moves[1];
                        break;
                    case ConsoleKey.D3:
                        if (_player.Moves.Length > 2) chosenMove = _player.Moves[2];
                        break;
                    case ConsoleKey.D4:
                        if (_player.Moves.Length > 3) chosenMove = _player.Moves[3];
                        break;
                    case ConsoleKey.D6:
                        _renderer.ShowDialog("You fled from the battle.");
                        return;
                }

                if (chosenMove == null)
                {
                    _renderer.ShowDialog("You hesitated...");
                }
                else
                {
                    UseMove(_player, _enemy, chosenMove);
                }

                if (_enemy.HP <= 0) break;

                // Simple enemy AI: always use first move if it exists
                if (_enemy.Moves != null && _enemy.Moves.Length > 0)
                    UseMove(_enemy, _player, _enemy.Moves[0]);
            }

            _renderer.ShowDialog(_player.HP > 0 ? "You won the battle!" : "You lost the battle...");
        }

        private void UseMove(Pokemon attacker, Pokemon target, Move move)
        {
            int damage = move.Power;
            target.HP = Math.Max(0, target.HP - damage);
            _renderer.ShowDialog($"{attacker.Name} used {move.Name}! It dealt {damage} damage.");
        }

        private void DrawBattleScreen()
        {
            Console.Clear();
            Console.WriteLine($"Enemy: {_enemy.Name} Lv{_enemy.Level}  HP: {HpBar(_enemy)} {_enemy.HP}/{_enemy.MaxHP}");
            Console.WriteLine();
            Console.WriteLine("               [ Enemy Pokémon ]");
            Console.WriteLine("                      VS");
            Console.WriteLine("               [ Your Pokémon ]");
            Console.WriteLine();
            Console.WriteLine($"You:   {_player.Name} Lv{_player.Level}  HP: {HpBar(_player)} {_player.HP}/{_player.MaxHP}");
            Console.WriteLine();
            Console.WriteLine("1-4: Use move   6: Flee");
        }

        private string HpBar(Pokemon p)
        {
            int barLength = 20;
            if (p.MaxHP <= 0) return new string(' ', barLength);

            int filled = (int)((double)p.HP / p.MaxHP * barLength);
            if (filled < 0) filled = 0;
            if (filled > barLength) filled = barLength;

            return new string('█', filled).PadRight(barLength);
        }
    }
}