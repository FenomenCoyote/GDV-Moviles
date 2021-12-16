namespace flow.Logic
{
    public enum Dir
    {
        None, Up, Down, Left, Right
    }

    struct Direction
    {
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