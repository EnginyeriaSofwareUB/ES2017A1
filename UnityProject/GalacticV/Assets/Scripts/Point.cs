using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Point
{
    public int X { get; set; }

    public int Y { get; set; }

    public Point(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is Point))
            return false;
        Point p2 = (Point)obj;
        return (this.X == p2.X && this.Y == p2.Y);
    }
}
