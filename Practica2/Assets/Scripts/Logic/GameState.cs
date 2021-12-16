using System;

namespace flow.Logic
{

    /// <summary>
    /// Class which contains level info to be saved and loaded
    /// </summary>
    [Serializable]
    public class Level
    {
        public bool completed;
        public bool locked;
        public int record;
    }

    /// <summary>
    /// Class which contains levelPack info to be saved and loaded
    /// </summary>
    [Serializable]
    public class LvlPack
    {
        public string name;
        //Number of completed levels from the pack
        public int completedLevels;
        //Number of perfectly done levels from the pack
        public int perfectLevels;
        public Level[] levels;
    }

    /// <summary>
    /// Class which contains category info to be saved and loaded
    /// </summary>
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

    /// <summary>
    /// Class which contains all info to be saved and loaded
    /// </summary>
    [Serializable]
    public class GameState
    {
        public uint nHints = 5;
        public Category[] categories;

        /// <summary>
        /// This method is called when no save was done before
        /// Initializes everything to a starting state
        /// </summary>
        /// <param name="gmCategories">Categories from game manager (scriptable objects)</param>
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
                        if (gmCategories[c].allLevelsUnlocked)
                            level.locked = false;
                        else level.locked = true;
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
