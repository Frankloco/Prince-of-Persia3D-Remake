namespace Pop3d {
    using System;
    //Movement direction of an entity
    [Flags]
    public enum MoveDirection {
        None = 0,
        Forwards = 1,
        Backwards = 2,
        Left = 4,
        Right = 8
    }

    //Interaction types for interactables
    public enum InteractionType {
        Push,
        Lever
    }
}