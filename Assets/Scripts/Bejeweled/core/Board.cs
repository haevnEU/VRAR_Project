namespace Bejeweled
{
    public class Board
    {
        private int size;
        private int maxJewels;
        private int[] field;

        public int Size { get { return size; } }
        public int MaxJewels { get { return maxJewels; } }
        public int INVALID_CELL { get { return Size * Size; } }

        public Board(int size, int maxJewels)
        {
            CustomRandomNumberGenerator rng = CustomRandomNumberGenerator.get();
            this.size = size;
            field = new int[size * size];
            this.maxJewels = maxJewels;

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    int f = rng.getNext(1, MaxJewels);

                    if (x > 1)
                    {
                        int cellLeft1 = getCell(x - 1, y);
                        int cellLeft2 = getCell(x - 2, y);
                        if (cellLeft1 == cellLeft2 && cellLeft1 == f)
                        {
                            int notCell = f;
                            do
                            {
                                f = rng.getNext(1, MaxJewels);
                            }
                            while (f == notCell);
                        }
                    }

                    if (y > 1)
                    {
                        int cellAbove1 = getCell(x, y - 1);
                        int cellAbove2 = getCell(x, y - 2);
                        if (cellAbove1 == cellAbove2 && cellAbove1 == f)
                        {
                            int notCell = f;
                            do
                            {
                                f = rng.getNext(1, MaxJewels);
                            }
                            while (f == notCell);
                        }
                    }

                    setCell(x, y, f);
                }
            }
        }

        public bool isCellInRange(int x, int y)
        {
            return (x >= 0 && x < size && y >= 0 && y < size);
        }

        public int getCell(int x, int y)
        {
            return isCellInRange(x, y) ? field[x + y * size] : INVALID_CELL;
        }

        public void setCell(int x, int y, int s)
        {
            if (isCellInRange(x, y) && s <= MaxJewels && s >= -MaxJewels)
            {
                field[x + y * size] = s;
            }
        }

        public void swap(int x1, int y1, int x2, int y2)
        {
            int state1 = getCell(x1, y1);
            int state2 = getCell(x2, y2);
            setCell(x1, y1, state2);
            setCell(x2, y2, state1);
        }
    }
}
