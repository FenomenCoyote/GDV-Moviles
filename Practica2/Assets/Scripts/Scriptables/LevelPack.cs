using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flow
{
    [CreateAssetMenu(fileName = "LevelPack", menuName = "Flow/LevelPack", order = 1)]
    public class LevelPack : ScriptableObject
    {
        public TextAsset levels;
        public ColorTheme theme;
        public string packName;
    }
}