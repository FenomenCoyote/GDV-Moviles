using UnityEngine;

namespace flow
{
    /// <summary>
    /// Scriptable object that stores an array of colors (ColorTheme)
    /// </summary>
    [CreateAssetMenu(fileName = "ColorTheme", menuName = "Flow/ColorTheme", order = 2)]
    public class ColorTheme : ScriptableObject
    {
        public Color[] colors;
    }
}
