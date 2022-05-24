using UnityEngine;

namespace Bejeweled
{
    internal class Logic
    {
        private bool active = false;
        private int clearingRuns = 0;
        public bool Active { get { return active; } }
        public bool isMoveAllowed(Board board, int x1, int y1, int x2, int y2)
        {
            if (!board.isCellInRange(x1, y1) || !board.isCellInRange(x2, y2))
            {
                return false;
            }

            bool horizontalAxis = isHorizontalAxisAllowed(board, x1, x2, y1, y2) || isHorizontalAxisAllowed(board, x2, x1, y2, y1);
            bool vericalAxis = isVerticalAxisAllowed(board, x1, x2, y1, y2) || isVerticalAxisAllowed(board, x2, x1, y2, y1);

            return horizontalAxis || vericalAxis;
        }

        public int clearConsecutives(Board board)
        {
            clearingRuns++;
            active = true;
            int cleared = 0;

            // Search for index
            for (int index = 1; index <= board.MaxJewels; index++)
            {

                // Search horizontal
                // Using a search cart where X = index and x prev/next item => | _ | _ | x | X | x | _ | _ |
                // => Start at 1 and finish at size - 1
                for (int x = 1; x < (board.Size - 1); x++)
                {
                    for (int y = 0; y < board.Size; y++)
                    {
                        // Get all required cells
                        int prevCell = board.getCell(x - 1, y);
                        int currentCell = board.getCell(x, y);
                        int nextCell = board.getCell(x + 1, y);

                        // IF a cell of the board is negativ normalize the value
                        prevCell = (prevCell < 0 ? -prevCell : prevCell);
                        currentCell = (currentCell < 0 ? -currentCell : currentCell);
                        nextCell = (nextCell < 0 ? -nextCell : nextCell);

                        // When at least all cells are identical set their values to negatives
                        if (currentCell == prevCell && currentCell == nextCell)
                        {
                            board.setCell(x - 1, y, -prevCell);
                            board.setCell(x, y, -currentCell);
                            board.setCell(x + 1, y, -nextCell);
                        }
                    }
                }

                // Search vertical                                              y
                // Using a search cart where Y = index and y prev/next item =>  Y      
                // => Start at 1 and finish at size - 1                         y
                for (int x = 0; x < board.Size; x++)
                {
                    for (int y = 1; y < (board.Size - 1); y++)
                    {
                        // Get all required cells
                        int prevCell = board.getCell(x, y - 1);
                        int currentCell = board.getCell(x, y);
                        int nextCell = board.getCell(x, y + 1);

                        // IF a cell of the board is negativ normalize the value
                        prevCell = (prevCell < 0 ? -prevCell : prevCell);
                        currentCell = (currentCell < 0 ? -currentCell : currentCell);
                        nextCell = (nextCell < 0 ? -nextCell : nextCell);

                        // When at least all cells are identical set their values to negatives
                        if (currentCell == prevCell && currentCell == nextCell)
                        {
                            board.setCell(x, y - 1, -prevCell);
                            board.setCell(x, y, -currentCell);
                            board.setCell(x, y + 1, -nextCell);
                        }
                    }
                }
            }

            for (int x = 0; x < board.Size; x++)
            {
                for (int y = 0; y < board.Size; y++)
                {
                    if (board.getCell(x, y) < 0)
                    {
                        cleared++;
                        board.setCell(x, y, 0);
                    }
                }
            }

            if (cleared == 0 || clearingRuns == 10)
            {
                active = false;
                clearingRuns = 0;
            }

            return cleared;
        }

        public void moveDown(Board board)
        {
            for (int tmp = 0; tmp < 100; tmp++)
                for (int x = 0; x < board.Size; x++)
                {
                    for (int y = (board.Size - 1); y > 0; y--)
                    {
                        if (board.getCell(x, y) == 0)
                        {
                            board.swap(x, y, x, y - 1);
                        }
                    }
                }
        }

        public void refill(Board board)
        {
            CustomRandomNumberGenerator rng = CustomRandomNumberGenerator.get();
            for (int x = 0; x < board.Size; x++)
            {
                for (int y = 0; y < board.Size; y++)
                {
                    if (board.getCell(x, y) <= 0)
                    {
                        board.setCell(x, y, rng.getNext(1, board.MaxJewels));
                    }
                }
            }
        }

        private bool isHorizontalAxisAllowed(Board board, int x1, int x2, int y1, int y2)
        {
            int len = 0;
            int lenType = -1;

            for(int i = 0; i < board.Size; i++)
            {
                int cell = board.getCell(x2, i);

                if(x1 == x2)
                {
                    if (i == y1) cell = board.getCell(x1, y2);
                    if (i == y2) cell = board.getCell(x2, y1);
                }
                else
                {
                    if (i == y2)
                    {
                        cell = board.getCell(x1, y1);
                    }
                }

                if (cell == lenType)
                {
                    len++;
                }
                else
                {
                    len = 1;
                    lenType = cell;
                }

                if (len >= 3)
                {
                    return true;
                }
            }

            return false;
        }

        private bool isVerticalAxisAllowed(Board board, int x1, int x2, int y1, int y2)
        {
            int len = 0;
            int lenType = -1;

            for (int i = 0; i < board.Size; i++)
            {
                int cell = board.getCell(i, y2);

                if (y1 == y2)
                {
                    if (i == x1) cell = board.getCell(x2, y2);
                    if (i == x2) cell = board.getCell(x1, y1);
                }
                else
                {
                    if (i == x2) cell = board.getCell(x1, y1);
                }

                if (cell == lenType)
                {
                    len++;
                }
                else
                {
                    len = 1;
                    lenType = cell;
                }

                if (len >= 3)
                {
                    return true;
                }
            }

            return false;
        }

        public bool isGameOver(Board board)
        {
            bool oneMoveExists = false;

            for(int x= 0; x < board.Size - 1; x++)
            {
                for(int y= 0; y < board.Size - 1; y++)
                {
                    bool vertical = isMoveAllowed(board, x, y, x, y + 1);
                    bool horizontal = isMoveAllowed(board, x, y, x + 1, y);

                    if(vertical || horizontal)
                    {
                        oneMoveExists = true; break;
                    }
                }

                if (oneMoveExists) break;
            }

            return !oneMoveExists;
        }
    }
}
