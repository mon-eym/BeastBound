using System;



public class BattleEngine
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

            ConsoleKey key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.D1: UseMove(_player, _enemy, _player.Moves[0]); break;
                case ConsoleKey.D2: UseMove(_player, _enemy, _player.Moves[1]); break;
                case ConsoleKey.D3: UseMove(_player, _enemy, _player.Moves[2]); break;
                case ConsoleKey.D4: UseMove(_player, _enemy, _player.Moves[3]); break;
                case ConsoleKey.D5: _renderer.ShowDialog("You switched Pokémon!"); return;
                case ConsoleKey.D6: _renderer.ShowDialog("You fled the battle."); return;
            }

            if (_enemy.HP > 0)
                UseMove(_enemy, _player, _enemy.Moves[0]); // simple AI
        }

        _renderer.ShowDialog(_player.HP > 0 ? "You won the battle!" : "You lost...");
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
        Console.WriteLine($"Enemy: {_enemy.Name} Lv{_enemy.Level}   HP: {HpBar(_enemy)} {_enemy.HP}/{_enemy.MaxHP}");
        Console.WriteLine();
        Console.WriteLine("         [Enemy Pokémon sprite]");
        Console.WriteLine("                 VS");
        Console.WriteLine("         [Your Pokémon sprite]");
        Console.WriteLine();
        Console.WriteLine($"Your: {_player.Name} Lv{_player.Level}   HP: {HpBar(_player)} {_player.HP}/{_player.MaxHP}");
        Console.WriteLine();
        Console.WriteLine("1. " + _player.Moves[0].Name + "   2. " + _player.Moves[1].Name);
        Console.WriteLine("3. " + _player.Moves[2].Name + "   4. " + _player.Moves[3].Name);
        Console.WriteLine("5. Switch Pokémon   6. Flee");
    }

    private string HpBar(Pokemon p)
    {
        int barLength = 20;
        int filled = (int)((double)p.HP / p.MaxHP * barLength);
        return new string('█', filled).PadRight(barLength);
    }
}