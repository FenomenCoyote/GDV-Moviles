using System;

namespace flow.Logic
{

    [Serializable]
    public struct Level
    {
        public bool completedOptimally;
        public bool completed;
        public bool locked;
        public int record;
    }

    [Serializable]
    public struct LvlPack
    {
        int completedLevels;
        Level[] levels;
    }

    [Serializable]
    public class GameState
    {
        public uint nHints = 5;
        public LvlPack[] lvlPacks;
    
    }
}
