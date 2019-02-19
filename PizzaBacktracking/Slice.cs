using System;
using System.Runtime.InteropServices.ComTypes;

namespace PizzaSlices
{
    public class Slice : IComparable
    {
        public int Points { get; } = 0;
        public bool IsInitialized { get; }
        public int StartRow { get; } = -1;
        public int StartCol { get; } = -1;
        public int EndRow { get; } = -1;
        public int EndCol { get; } = -1;


        public Slice(int startRow, int startCol, int endRow, int endCol, int points)
        {
            StartRow = startRow;
            StartCol = startCol;
            EndRow = endRow;
            EndCol = endCol;
            Points = points;

            this.IsInitialized = true;
        }

        public Slice()
        {
            this.IsInitialized = false;
        }

        public int CompareTo(object obj)
        {
            var areEquals = -1;
            Slice otherSlice = obj as Slice;

            if (otherSlice != null)
            {
                var endRowEquals = otherSlice.EndRow == this.EndRow;

                var endColEquals = otherSlice.EndCol == this.EndCol;

                var startColEquals = otherSlice.StartCol == this.StartCol;

                var startRowEquals = otherSlice.StartRow == this.StartRow;

                if (endRowEquals && endColEquals && startColEquals && startRowEquals)
                {
                    areEquals = 0;
                }
            }
            return areEquals;
        }
    }
}


