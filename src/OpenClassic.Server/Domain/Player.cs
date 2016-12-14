﻿using System;

namespace OpenClassic.Server.Domain
{
    public class Player : IPlayer, IEquatable<Player>
    {
        public short Index { get; set; }

        public bool IsActive { get; set; }

        private Point _location = new Point(329, 552);
        public Point Location
        {
            get { return _location; }
            set { _location = value; }
        }

        public bool Equals(IPlayer other)
        {
            return other == this;
        }

        public bool Equals(Player other)
        {
            return other == this;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as IPlayer);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            var x = _location.X;
            var y = _location.Y;
            return $"Player Index={Index}, Location=({x},{y})";
        }
    }
}
