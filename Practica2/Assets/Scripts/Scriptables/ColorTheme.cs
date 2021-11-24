using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flow
{
    [CreateAssetMenu(fileName = "ColorTheme", menuName = "Flow/ColorTheme", order = 2)]
    public class ColorTheme : ScriptableObject
    {
        public Color[] colors;
    }
}
