using UnityEngine;

namespace flow
{
    /// <summary>
    /// Scriptable object that stores information of a LevelPack
    /// </summary>
    [CreateAssetMenu(fileName = "LevelPack", menuName = "Flow/LevelPack", order = 1)]
    public class LevelPack : ScriptableObject
    {
        public TextAsset levels;
        public string packName;             //The LevelPack name
        public Color[] levelPanelColors;    //The color of each level panel
        public string[] levelPanelName;     //The name of each level panel
    }
}
