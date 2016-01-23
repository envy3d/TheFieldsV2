using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public static class CardinalDirections
    {
        public static Vector2 GetVector2(Direction dir)
        {
            switch (dir)
            {
                case Direction.N:
                    return new Vector2(0, 1);
                case Direction.S:
                    return new Vector2(0, -1);
                case Direction.E:
                    return new Vector2(1, 0);
                case Direction.W:
                    return new Vector2(-1, 0);
                default:
                    return new Vector2();
            }
        }
    }

    public enum Direction
    {
        N,
        E,
        S,
        W
    }
}
