using System;

namespace PokelikeConsole
{
    internal sealed class Map
    {
        private readonly Tile[,] _tiles;

        public int Width { get; }
        public int Height { get; }
        public int SpawnX { get; }
        public int SpawnY { get; }

        public Map(int width, int height, int spawnX, int spawnY)
        {
            Width = width;
            Height = height;
            SpawnX = spawnX;
            SpawnY = spawnY;
            _tiles = new Tile[width, height];
        }

        public void Set(int x, int y, Tile tile)
        {
            _tiles[x, y] = tile;
        }

        public Tile Get(int x, int y)
        {
            return _tiles[x, y];
        }

        public bool InBounds(int x, int y)
        {
            return x >= 0 && y >= 0 && x < Width && y < Height;
        }
    }
}