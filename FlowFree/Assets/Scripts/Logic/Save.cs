using System;

namespace flow.Logic
{
    /// <summary>
    /// Class which is saved to a text document to restore game progress
    /// </summary>
    [Serializable]
    public class Save
    {
        public string hashCode; //Hash to ensure security
        public GameState gameState; //game progress
    }
}