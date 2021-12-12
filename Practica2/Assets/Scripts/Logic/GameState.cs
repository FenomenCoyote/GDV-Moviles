using System;

namespace flow.Logic
{

    [Serializable]
    public struct Level
    {
        public bool completed;
        public bool locked;
        public int record;
    }

    [Serializable]
    public struct LvlPack
    {
        public int completedLevels;
        public Level[] levels;
    }

    [Serializable]
    public struct Category
    {
        public LvlPack[] packs;
    }

    [Serializable]
    public class GameState
    {
        public uint nHints = 5;
        public Category[] categories;

        public void init(PackCategory[] gmCategories)
        {
            int nCategories = gmCategories.Length;
            categories = new Category[nCategories];

            for (int c = 0; c < nCategories; ++c)
            {
                LevelPack[] packs = gmCategories[c].levelPacks;
                int nPacks = packs.Length;
                categories[c].packs = new LvlPack[nPacks];

                for (int p = 0; p < nPacks; ++p)
                {
                    categories[c].packs[p].completedLevels = 0;

                    string[] levelsInfo = packs[p].levels.ToString().Split('\n');
                    categories[c].packs[p].levels = new Level[levelsInfo.Length];

                    Level level = categories[c].packs[p].levels[0];
                    level.record = 0;
                    level.completed = false;
                    level.locked = false;

                    for (int l = 1; l < levelsInfo.Length; ++l)
                    {
                        level = categories[c].packs[p].levels[l];
                        level = new Level();

                        level.record = 0;
                        level.completed = false;
                        level.locked = true;
                    }
                }
            }
        }
    }
}
