using UnityEngine;

namespace flow {

    /// <summary>
    /// Scriptable object that stores information of a PackCategory
    /// </summary>
    [CreateAssetMenu(fileName = "PackCategory", menuName = "Flow/PackCategory", order = 3)]
    public class PackCategory : ScriptableObject
    {
        public bool allLevelsUnlocked;      //If all levels are unlocked
        public string categoryName;         //PackCategory name
        public LevelPack[] levelPacks;      //Array of LevelPacks
        public Color categoryColor;         //The color of the category
    }
}
