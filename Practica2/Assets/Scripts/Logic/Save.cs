using System;

namespace flow.Logic
{
    [Serializable]
    public class Save
    {
        public byte[] hashCode;
        public GameState gameState;
    }
}