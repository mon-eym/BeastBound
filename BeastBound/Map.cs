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

        public void Set(int x, int y, Tile tile) => _tiles[x, y] = tile;

        public Tile Get(int x, int y) => _tiles[x, y];

        public bool InBounds(int x, int y) => x >= 0 && y >= 0 && x < Width && y < Height;
    }

    internal static class DemoMaps
    {
        // Simple Pallet Town-like layout
        public static Map BuildPalletTown()
        {
            int w = 48, h = 24;
            var map = new Map(w, h, spawnX: 24, spawnY: 14);

            // Fill with grass
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                    map.Set(x, y, Tile.Make(TileType.GrassShort));

            // Ocean at bottom
            for (int y = 18; y < h; y++)
                for (int x = 0; x < w; x++)
                    map.Set(x, y, Tile.Make(TileType.Water));

            // Path through town
            for (int x = 0; x < w; x++)
                map.Set(x, 14, Tile.Make(TileType.Path));
            for (int y = 10; y <= 18; y++)
                map.Set(24, y, Tile.Make(TileType.Path));

            // Tall grass patches
            Rect(map, 6, 3, 10, 6, TileType.GrassTall);
            Rect(map, 32, 4, 10, 6, TileType.GrassTall);

            // Fences
            HorizontalFence(map, 2, 9, 44);
            HorizontalFence(map, 2, 19, 44);
            VerticalFence(map, 2, 9, 10);
            VerticalFence(map, 46, 9, 10);

            // Houses (2x)
            House(map, 8, 11);
            House(map, 34, 11);

            // Signs
            map.Set(23, 14, Tile.Make(TileType.Sign));
            map.Set(25, 14, Tile.Make(TileType.Sign));

            return map;
        }

        private static void Rect(Map map, int x, int y, int w, int h, TileType type)
        {
            for (int ix = x; ix < x + w; ix++)
                for (int iy = y; iy < y + h; iy++)
                    if (map.InBounds(ix, iy))
                        map.Set(ix, iy, Tile.Make(type));
        }

        private static void HorizontalFence(Map map, int x, int y, int length)
        {
            for (int i = 0; i < length; i++)
                map.Set(x + i, y, Tile.Make(TileType.Fence));
        }

        private static void VerticalFence(Map map, int x, int y, int length)
        {
            for (int i = 0; i < length; i++)
                map.Set(x, y + i, Tile.Make(TileType.Fence));
        }

        private static void House(Map map, int x, int y)
        {
            // Outer walls
            Rect(map, x, y, 10, 6, TileType.HouseWall);

            // Roof line
            for (int i = 0; i < 10; i++)
                map.Set(x + i, y, Tile.Make(TileType.HouseRoof));

            // Door
            map.Set(x + 4, y + 5, Tile.Make(TileType.Door));

            // Windows
            map.Set(x + 2, y + 2, Tile.Make(TileType.Window));
            map.Set(x + 7, y + 2, Tile.Make(TileType.Window));
        }
    }
}