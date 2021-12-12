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
        public string name;
        public int completedLevels;
        public Level[] levels;
    }

    [Serializable]
    public struct Category
    {
        public string name;
        public LvlPack[] packs;

        public LvlPack getPackByName(string name)
        {
            int i = 0;
            while (packs[i].name != name) i++;
            return packs[i];
        }
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
                categories[c].name = gmCategories[c].categoryName;

                for (int p = 0; p < nPacks; ++p)
                {
                    LvlPack pack = categories[c].packs[p];

                    pack.completedLevels = 0;
                    pack.name = gmCategories[c].levelPacks[p].packName;

                    string[] levelsInfo = packs[p].levels.ToString().Split('\n');
                    pack.levels = new Level[levelsInfo.Length];

                    //El primer nivel es el único desbloqueado
                    Level level = pack.levels[0];
                    level.record = 0;
                    level.completed = false;
                    level.locked = false;
                    pack.levels[0] = level;

                    for (int l = 1; l < levelsInfo.Length - 1; ++l)
                    {
                        level = pack.levels[l];
                        level = new Level();

                        level.record = 0;
                        level.completed = false;
                        level.locked = true;
                        pack.levels[l] = level;
                    }

                    categories[c].packs[p] = pack;
                }
            }
        }

        public Category getCategoryByName(string name)
        {
            int i = 0;
            while (categories[i].name != name) i++;
            return categories[i];
        }
    }
}
