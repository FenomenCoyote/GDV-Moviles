using System;

namespace flow.Logic
{

    [Serializable]
    public class Level
    {
        public bool completed;
        public bool locked;
        public int record;
    }

    [Serializable]
    public class LvlPack
    {
        public string name;
        public int completedLevels;
        public int perfectLevels;
        public Level[] levels;
    }

    [Serializable]
    public class Category
    {
        public string name;
        public int completedPacks;
        public int perfectPacks;
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
                categories[c] = new Category();
                Category category = categories[c];

                category.completedPacks = 0;
                category.perfectPacks = 0;

                LevelPack[] packs = gmCategories[c].levelPacks;
                int nPacks = packs.Length;

                category.packs = new LvlPack[nPacks];
                category.name = gmCategories[c].categoryName;

                for (int p = 0; p < nPacks; ++p)
                {
                    categories[c].packs[p] = new LvlPack();
                    LvlPack pack = categories[c].packs[p];

                    pack.completedLevels = 0;
                    pack.perfectLevels = 0;

                    pack.name = gmCategories[c].levelPacks[p].packName;

                    string[] levelsInfo = packs[p].levels.ToString().Split('\n');
                    pack.levels = new Level[levelsInfo.Length - 1];

                    //El primer nivel es el único desbloqueado
                    pack.levels[0] = new Level();
                    Level level = pack.levels[0];
                    level.record = 0;
                    level.completed = false;
                    level.locked = false;
                    pack.levels[0] = level;

                    for (int l = 1; l < levelsInfo.Length - 1; ++l)
                    {
                        pack.levels[l] = new Level();
                        level = pack.levels[l];

                        level.record = 0;
                        level.completed = false;
                        level.locked = false;
                    }
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
