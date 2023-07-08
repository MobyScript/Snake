using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Snake
{
    public class Direction
    {

        ////////////////////
        //Static Variables//
        ////////////////////

        // To move left
        public readonly static Direction Left = new Direction(0, -1);

        // To move right
        public readonly static Direction Right = new Direction(0, 1);

        // To move up
        public readonly static Direction Up = new Direction(-1, 0);

        // To move down
        public readonly static Direction Down = new Direction(1, 0);


        public int RowOffset { get; }
        public int ColumnOffset { get; }

        //Private Constructor that takes the RowOffset and ColumnOffSet as parameters
        private Direction(int rowOffset, int colOffset) {

            RowOffset = rowOffset;
            ColumnOffset = colOffset;
        }

        //Opposite Method
        public Direction Opposite()
        {
            return new Direction(-RowOffset, -ColumnOffset);
        }

        public override bool Equals(object obj)
        {
            return obj is Direction direction &&
                RowOffset == direction.RowOffset &&
                ColumnOffset == direction.ColumnOffset;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(RowOffset, ColumnOffset);
        }

        public static bool operator ==(Direction left, Direction right)
        {
            return EqualityComparer<Direction>.Default.Equals(left, right);
        }
        public static bool operator !=(Direction left, Direction right)
        {
            return !(left == right);
        }

    }
}
