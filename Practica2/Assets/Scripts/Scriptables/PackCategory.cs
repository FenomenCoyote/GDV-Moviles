using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flow {

    [CreateAssetMenu(fileName = "PackCategory", menuName = "Flow/PackCategory", order = 3)]
    public class PackCategory : ScriptableObject
    {
        public bool allLevelsUnlocked;
        public string categoryName;
        public LevelPack[] levelPacks;
        public Color categoryColor;
    }
}
