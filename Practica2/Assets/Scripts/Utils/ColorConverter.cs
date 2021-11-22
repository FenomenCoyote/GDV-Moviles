using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flow.Utils
{
    public class ColorConverter
    {
        static public Color intToColor(uint HexVal)
        {
            byte R = (byte)((HexVal >> 24) & 0xFF);
            byte G = (byte)((HexVal >> 16) & 0xFF);
            byte B = (byte)((HexVal >> 08) & 0xFF);
            byte A = (byte)(HexVal & 0xFF);
            return new Color32(R, G, B, A);
        }
    }

}
