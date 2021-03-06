﻿using OpenClassic.Server.Util;
using System;
using System.Diagnostics;

namespace OpenClassic.Server.Domain
{
    public struct Point : IEquatable<Point>, IComparable<Point>
    {
        public static readonly Point OUT_OF_BOUNDS_LOCATION = new Point(0, 0);

        public readonly short X;
        public readonly short Y;

        public Point(short x, short y)
        {
            Debug.Assert(x >= 0);
            Debug.Assert(y >= 0);

            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Point)) return false;

            var that = (Point)obj;
            return X == that.X && Y == that.Y;
        }

        public override int GetHashCode() => HashCodeHelper.GetHashCode(X, Y);

        public override string ToString() => $"({X},{Y})";

        public bool Equals(Point other) => X == other.X && Y == other.Y;
        public static bool Equals(Point a, Point b) => a.X == b.X && a.Y == b.Y;

        public int DistanceFromOriginSquared()
        {
            // We calculate distance from origin, instead of distance, so that
            // we can avoid working with floating point numbers. Integer
            // calculations are computationally less expensive.
            var distFromOriginSquared = (X * X) + (Y * Y);

            return distFromOriginSquared;
        }

        public static int DistanceSquared(Point a, Point b)
        {
            var xDiff = (a.X - b.X);
            var yDiff = (a.Y - b.Y);

            // We use distance squared to avoid computing floating point numbers.
            var distSquared = (xDiff * xDiff) + (yDiff * yDiff);

            return distSquared;
        }

        public static bool WithinRange(Point a, Point b, int range)
        {
            var twoTimesRangeSquared = 2 * (range * range);
            var distBetweenPoints = DistanceSquared(a, b);

            return distBetweenPoints <= twoTimesRangeSquared;
        }

        public int CompareTo(Point other)
        {
            var thisDistFromOrigin = DistanceFromOriginSquared();
            var otherDistFromOrigin = other.DistanceFromOriginSquared();

            return thisDistFromOrigin - otherDistFromOrigin;
        }
    }
}
