using System;
using System.Collections.Generic;

namespace Snake
{
    public class Position
    {
        public int Row { get; }
        public int Column { get; }
 
       //Constructor that stores values
       public Position(int row, int column)
        {
            Row = row;
            Column = column;
        }

        //Translate Method
        public Position Translate(Direction dir)
        {
            //This method allows us to override The Equal method and the GetHashCode methods
            return new Position(Row + dir.RowOffset, Column + dir.ColumnOffset);
        }

        public override bool Equals(object obj)
        {
            return obj is Position position &&
                Row == position.Row
                && Column == position.Column;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Column);
        }

        public static bool operator ==(Position left, Position right) { 
        return EqualityComparer<Position>.Default.Equals(left, right);
        }
        public static bool operator !=(Position left, Position right)
        {
            return !(left == right);
        }

    }
}
