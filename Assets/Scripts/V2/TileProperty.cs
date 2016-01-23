using System;

[Flags]
public enum TileProperty
{
    Traversable = 1,
    Landmine = 2,
    Hole = 4,
    Crater = 8,
    Start = 16,
    Finish = 32,
    Golden = 64
}

