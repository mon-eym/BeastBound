using System;

namespace PokelikeConsole
{
    internal sealed class Map
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
}


        public void Set(int x, int y, Tile tile) => _tiles[x, y] = tile;

        public Tile Get(int x, int y) => _tiles[x, y];

        public bool InBounds(int x, int y) => x >= 0 && y >= 0 && x < Width && y < Height;
    }

    using System;

namespace PokelikeConsole
{
    internal static class DemoMaps
    {
        // ============================================================
        //  OVERWORLD (160x80, maze-style hub)
        // ============================================================
        public static Map BuildOverworld()
        {
            int w = 160, h = 80;
            var map = new Map(w, h, spawnX: 80, spawnY: 40);

            // Fill with clean floor
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                    map.Set(x, y, Tile.Make(TileType.Floor));

            // CENTRAL HUB CROSS
            for (int x = 0; x < w; x++)
                map.Set(x, 40, Tile.Make(TileType.Floor));

            for (int y = 0; y < h; y++)
                map.Set(80, y, Tile.Make(TileType.Floor));

            // ============================
            // TOP ROOM — TOWN
            // ============================
            Rect(map, 60, 10, 40, 20, TileType.Floor);
            Box(map, 60, 10, 40, 20, TileType.Wall);

            // Houses (simple clean rooms)
            Box(map, 66, 14, 10, 6, TileType.Wall);
            map.Set(71, 19, Tile.Make(TileType.DoorClosed));

            Box(map, 86, 14, 10, 6, TileType.Wall);
            map.Set(91, 19, Tile.Make(TileType.DoorClosed));

            // Sign at bottom
            map.Set(80, 31, Tile.Make(TileType.Sign));

            // Path to hub
            for (int y = 31; y <= 39; y++)
                map.Set(80, y, Tile.Make(TileType.Floor));

            // ============================
            // LEFT ROOM — FOREST MAZE
            // ============================
            Rect(map, 10, 30, 40, 20, TileType.GrassLight);
            Box(map, 10, 30, 40, 20, TileType.Wall);

            // Maze walls inside
            for (int x = 14; x < 46; x += 4)
                for (int y = 32; y < 48; y += 3)
                    map.Set(x, y, Tile.Make(TileType.Wall));

            // Path to hub
            for (int x = 40; x <= 79; x++)
                map.Set(x, 40, Tile.Make(TileType.Floor));

            map.Set(39, 40, Tile.Make(TileType.Sign));

            // ============================
            // BOTTOM ROOM — BEACH
            // ============================
            Rect(map, 50, 55, 60, 15, TileType.GrassMedium);
            Box(map, 50, 55, 60, 15, TileType.Wall);

            // Water area
            Rect(map, 50, 70, 60, 10, TileType.Water);

            // Pier
            for (int y = 55; y <= 69; y++)
                map.Set(80, y, Tile.Make(TileType.Floor));

            map.Set(80, 54, Tile.Make(TileType.Sign));

            // ============================
            // RIGHT ROOM — CAVE ENTRANCE
            // ============================
            Rect(map, 110, 30, 30, 20, TileType.GrassMedium);
            Box(map, 110, 30, 30, 20, TileType.Wall);

            // Cave entrance (small building)
            Box(map, 120, 35, 10, 6, TileType.Wall);
            map.Set(125, 38, Tile.Make(TileType.DoorClosed));
            map.Set(124, 37, Tile.Make(TileType.Sign));

            // Path to hub
            for (int x = 81; x <= 119; x++)
                map.Set(x, 40, Tile.Make(TileType.Floor));

            // ============================
            // BATTLE HOUSE (near center)
            // ============================
            Box(map, 72, 44, 16, 10, TileType.Wall);
            map.Set(80, 53, Tile.Make(TileType.DoorClosed));
            map.Set(80, 54, Tile.Make(TileType.Sign)); // "Battle House"

            return map;
        }

        // ============================================================
        //  BATTLE HOUSE INTERIOR
        // ============================================================
        public static Map BuildBattleHouse()
        {
            int w = 30, h = 18;
            var map = new Map(w, h, spawnX: 15, spawnY: 14);

            // Floor
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                    map.Set(x, y, Tile.Make(TileType.Floor));

            // Outer walls
            Box(map, 0, 0, w, h, TileType.Wall);

            // Exit door (bottom center)
            map.Set(w / 2, h - 1, Tile.Make(TileType.DoorOpen));

            // EASY trainer room
            Box(map, 2, 2, 8, 6, TileType.Wall);
            map.Set(6, 5, Tile.Make(TileType.NPC));
            map.Set(6, 9, Tile.Make(TileType.Sign));  // Easy sign

            // MEDIUM trainer room
            Box(map, 11, 2, 8, 6, TileType.Wall);
            map.Set(15, 5, Tile.Make(TileType.NPC));
            map.Set(15, 9, Tile.Make(TileType.Sign)); // Medium sign

            // HARD trainer room
            Box(map, 20, 2, 8, 6, TileType.Wall);
            map.Set(24, 5, Tile.Make(TileType.NPC));
            map.Set(24, 9, Tile.Make(TileType.Sign)); // Hard sign

            return map;
        }

        // ============================================================
        //  SIMPLE CAVE (using new tileset)
        // ============================================================
        public static Map BuildCave()
        {
            int w = 40, h = 20;
            var map = new Map(w, h, spawnX: 20, spawnY: 10);

            // Fill with solid walls (rock)
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                    map.Set(x, y, Tile.Make(TileType.Wall));

            // Carve a floor area inside
            Rect(map, 3, 3, 34, 14, TileType.Floor);

            // Exit back to overworld (bottom middle of the room)
            map.Set(20, 17, Tile.Make(TileType.DoorOpen));

            return map;
        }

        // ============================================================
        //  SIMPLE HOUSE INTERIOR (using new tileset)
        // ============================================================
        public static Map BuildHouseInterior()
        {
            int w = 20, h = 12;
            var map = new Map(w, h, spawnX: 10, spawnY: 6);

            // Floor
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                    map.Set(x, y, Tile.Make(TileType.Floor));

            // Walls
            Box(map, 0, 0, w, h, TileType.Wall);

            // Exit door at bottom center
            map.Set(w / 2, h - 1, Tile.Make(TileType.DoorOpen));

            return map;
        }

        // ============================================================
        //  HELPERS (Rect + Box)
        // ============================================================
        private static void Rect(Map map, int x, int y, int w, int h, TileType type)
        {
            for (int ix = x; ix < x + w; ix++)
                for (int iy = y; iy < y + h; iy++)
                    if (map.InBounds(ix, iy))
                        map.Set(ix, iy, Tile.Make(type));
        }

        private static void Box(Map map, int x, int y, int w, int h, TileType type)
        {
            // Top and bottom
            for (int ix = x; ix < x + w; ix++)
            {
                if (map.InBounds(ix, y))
                    map.Set(ix, y, Tile.Make(type));
                if (map.InBounds(ix, y + h - 1))
                    map.Set(ix, y + h - 1, Tile.Make(type));
            }

            // Left and right
            for (int iy = y; iy < y + h; iy++)
            {
                if (map.InBounds(x, iy))
                    map.Set(x, iy, Tile.Make(type));
                if (map.InBounds(x + w - 1, iy))
                    map.Set(x + w - 1, iy, Tile.Make(type));
            }
        }
    }
}
        }
    }
}