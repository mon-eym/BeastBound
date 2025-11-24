using System;

namespace PokemonTownMap
{
    class Program
    {
        static char[,] map = new char[,]
        {
            // 0    1    2    3    4    5    6    7    8    9    10   11   12   13   14
            { '🌳','🌳','🌳','🌳','🌳','🌳','🌳','🌳','🌳','🌳','🌳','🌳','🌳','🌳','🌳' },
            { '🌳','░','░','░','░','░','░','░','░','░','░','░','░','░','🌳' },
            { '🌳','🏥','🏥','🏥','░','░','👩','🏠','🏠','🏠','░','░','░','░','🌳' },
            { '🌳','🏥','🏥','🏥','░','░','░','░','░','░','░','░','░','░','🌳' },
            { '🌳','░','░','░','░','░','░','░','░','░','░','░','░','░','🌳' },
            { '🌳','♣','♣','♣','♣','♣','♣','♣','♣','♣','♣','♣','♣','♣','🌳' },
            { '🌳','🌳','🌳','🌳','🌳','🌳','🌳','🌳','🌳','🌳','🌳','🌳','🌳','🌳','🌳' }
        };

        static int playerX = 4;
        static int playerY = 4;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            while (true)
            {
                Console.Clear();
                DrawMap();
                Console.WriteLine("Move with W/A/S/D | Q to quit");
                ConsoleKey key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.Q) break;

                MovePlayer(key);
            }
        }

        static void DrawMap()
        {
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    if (x == playerX && y == playerY)
                        Console.Write('👾'); // Player
                    else
                        Console.Write(map[y, x]);
                }
                Console.WriteLine();
            }
        }

        static void MovePlayer(ConsoleKey key)
        {
            int newX = playerX;
            int newY = playerY;

            switch (key)
            {
                case ConsoleKey.W: newY--; break;
                case ConsoleKey.S: newY++; break;
                case ConsoleKey.A: newX--; break;
                case ConsoleKey.D: newX++; break;
            }

            if (newX >= 0 && newX < map.GetLength(1) && newY >= 0 && newY < map.GetLength(0))
            {
                char terrain = map[newY, newX];
                if (terrain != '🌳') // Can't walk into trees
                {
                    playerX = newX;
                    playerY = newY;
                }
            }
        }
    }
}