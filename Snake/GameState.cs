using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    public class GameState
    {
        //Properties

        //Row propertie
        public int Rows { get; }

        // Column propertie
        public int Columns { get; }

        //Grid value propertie
        public GridValue[,] Grid { get; }

        //Snake Direction propertie
        public Direction Dir { get; private set; }

        //Score propertie
        public int Score { get; private set; }


        //GameOver propertie
        public bool GamerOver { get; private set; }


        // SnakePosition propertie
        // I am using a linked list because it allows me to delete the front and end of the snake 
        private readonly LinkedList<Position> snakePosition = new LinkedList<Position>();

        //Random propertie to let the game where the food will spawn
        private readonly Random random = new Random();

        public GameState(int rows, int cols)
        {

            Rows = rows;
            Columns = cols;
            Grid = new GridValue[rows, cols];
            Dir = Direction.Right;

            //Methods
            AddSnake();
            AddFood();
        }

        private void AddSnake()
        {
            //variables
            //middle row
            int r = Rows / 2;

            //Loop over the columns
            for (int x = 1; x <= 3; x++)
            {
                Grid[r, x] = GridValue.Snake;
                snakePosition.AddFirst(new Position(r, x));
            }
        }

        private IEnumerable<Position>EmptyPosition()
        {
            for(int r = 0; r < Rows; r++)
            {
                for(int c = 0; c < Columns; c++)
                {
                    if (Grid[r,c] == GridValue.Empty)
                    {
                        yield return new Position(r, c);
                    }
                }
            }
        }

        private void AddFood()
        {
            List<Position> empty = new List<Position>(EmptyPosition());
            

            //If the player wins, return nothing so it won't crash
            if(empty.Count == 0)
            {
                return;
            }

            Position pos = empty[random.Next(empty.Count)];
            Grid[pos.Row, pos.Column] = GridValue.Food; 
        }

        //Return Position of the Snake's Head
        public Position HeadPosition()
        {
            return snakePosition.First.Value;

        }

        //Return Position of the Snake's Tail
        public Position TailPosition()
        {
            return snakePosition.Last.Value;
        }

        //Return All Snake's Positions
        public IEnumerable<Position> SnakePositions()
        {
            return snakePosition;
        }

        //Methods to Modify

        //Adding Head
        private void AddHead(Position pos)
        {
            snakePosition.AddFirst(pos);
            Grid[pos.Row, pos.Column] = GridValue.Snake;
        }

        //Adding Tail

        private void RemoveTail()
        {
            Position tail = snakePosition.Last.Value;
            Grid[tail.Row, tail.Column] = GridValue.Empty;
            snakePosition.RemoveLast();
        }

       
        public void ChangeDirection(Direction direction)
        {
            Dir = direction; 
        }

        //Death Methods
        private bool OutsideGrid(Position pos)
        {
            return pos.Row < 0 || pos.Row >= Rows || pos.Column < 0 || pos.Column >= Columns;   
        }

        private GridValue WillHit(Position newHeadPos)
        {
            if(OutsideGrid(newHeadPos))
            {
                return GridValue.Outside;
            }

            
            //If you think the game should end when the player touches the tail then you can you remove this Condition
            if(newHeadPos == TailPosition())
            {
                return GridValue.Empty;
            }

            return Grid[newHeadPos.Row, newHeadPos.Column];
        }


        public void Move()
        {
            Position newHeadPos = HeadPosition().Translate(Dir);
            GridValue hit = WillHit(newHeadPos);

            if(hit == GridValue.Outside || hit == GridValue.Snake)
            {
                GamerOver = true;
            }

            else if(hit == GridValue.Empty) {
                RemoveTail();
                AddHead(newHeadPos);
            }

            else if(hit == GridValue.Food)
            {
                AddHead(newHeadPos);
                Score++;
                AddFood();
            }
        }
    }

}
