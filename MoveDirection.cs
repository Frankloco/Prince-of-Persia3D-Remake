namespace Pop3d
{
    using System;
    [Flags]
    public enum MoveDirection
    {
        None = 0,
        Forwards = 1,
        Backwards = 2,
        Left = 4,
        Right = 8
    }
}
