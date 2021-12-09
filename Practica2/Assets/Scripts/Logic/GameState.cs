using System;
using UnityEngine;

namespace flow.Logic
{
    [Serializable]
    public class GameState
    {
        public int nHints = 5;
        public byte[] hashCode;

    }
}