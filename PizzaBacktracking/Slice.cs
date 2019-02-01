namespace PizzaSlices
{
    public class Slice
    {
        public bool IsInitialized { get; }
        public int StartRow { get; } = -1;
        public int StartCol { get; } = -1;
        public int EndRow { get; } = -1;
        public int EndCol { get; } = -1;


        public Slice(int startRow, int startCol, int endRow, int endCol)
        {
            StartRow = startRow;
            StartCol = startCol;
            EndRow = endRow;
            EndCol = endCol;

            this.IsInitialized = true;
        }

        public Slice()
        {
            this.IsInitialized = false;
        }
    }
}


