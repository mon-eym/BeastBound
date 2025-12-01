using System.Collections.Generic;

namespace PokelikeConsole
{
    internal sealed class World
    {
        private readonly Dictionary<string, Map> _maps = new Dictionary<string, Map>();
        // ...


        public string CurrentMapName { get; private set; }
        public Map CurrentMap => _maps[CurrentMapName];
        public Player Player { get; }

        public World()
        {
            _maps["Overworld"] = DemoMaps.BuildOverworld();
            _maps["BattleHouse"] = DemoMaps.BuildBattleHouse();
            _maps["Cave"] = DemoMaps.BuildCave();
            _maps["House1"] = DemoMaps.BuildHouseInterior();

            CurrentMapName = "Overworld";
            var spawn = CurrentMap;
            Player = new Player(spawn.SpawnX, spawn.SpawnY);
        }

        public void SwitchMap(string name, int spawnX, int spawnY)
        {
            if (_maps.ContainsKey(name))
            {
                CurrentMapName = name;
                Player.MoveTo(spawnX, spawnY);
            }
        }
    }
}