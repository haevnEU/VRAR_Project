namespace Bejeweled
{
    public class Bejeweled
    {
        private Board board;
        private IRender renderer;
        private Logic logic = new Logic();
        private int points = 0;

        public Board Board { get { return board; } }
        public int Points { get { return points; } set { points = value; } }

        public Bejeweled(int size, int maxJewels, IRender renderer)
        {
            this.board = new Board(size, maxJewels);
            this.renderer = renderer;
            this.renderer.render(this);
        }

        public void swap(int x1, int y1, int x2, int y2)
        {
            if (logic.isMoveAllowed(board, x1, y1, x2, y2))
            {
                board.swap(x1, y1, x2, y2);
            }
            do
            {
                int cleared = logic.clearConsecutives(board);
                logic.moveDown(board);
                logic.refill(board);
                renderer.render(this);
                points += cleared;
            } while (logic.Active);
        }

        public bool isGameOver()
        {
            return logic.isGameOver(board);
        }
    }
}
