namespace flow.Logic
{
    /// <summary>
    /// Enum which represents four directions and none
    /// </summary>
    public enum Dir
    {
        None, Up, Down, Left, Right
    }

    /// <summary>
    /// Struct which contains methods for directions
    /// </summary>
    struct Direction
    {
        /// <summary>
        /// Get the opposite direction from the given direction
        /// </summary>
        /// <param name="d">Direction to get the opposite</param>
        /// <returns>Opposite direction from given one</returns>
        static public Dir Opposite(Dir d)
        {
            switch (d)
            {
                case Dir.Up:
                    return Dir.Down;
                case Dir.Down:
                    return Dir.Up;
                case Dir.Left:
                    return Dir.Right;
                case Dir.Right:
                    return Dir.Left;
                default:
                    return Dir.None;
            }
        }

        /// <summary>
        /// By a given vector, returns it's direction enum
        /// Vector has to be normalized
        /// </summary>
        /// <param name="dir">Vector to translate to enum</param>
        /// <returns>Dir enum of given vector</returns>
        static public Dir GetDirectionFromVector(UnityEngine.Vector2 dir)
        {
            if(dir == UnityEngine.Vector2.up)
                return Dir.Up;

            if (dir == UnityEngine.Vector2.down)
                return Dir.Down;

            if (dir == UnityEngine.Vector2.left)
                return Dir.Left;

            if (dir == UnityEngine.Vector2.right)
                return Dir.Right;

            return Dir.None;
        }
    }
}