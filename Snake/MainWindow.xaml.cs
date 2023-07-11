using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace Snake
{

    /// Interaction logic for MainWindow.xaml
    public partial class MainWindow : Window
    {
        private readonly Dictionary<GridValue, ImageSource> gridValToImage = new()
        {
            {GridValue.Empty, Images.Empty },
            {GridValue.Snake, Images.Body },
            {GridValue.Food, Images.Food }
        };

        //Dictionary to track head rotations
        private readonly Dictionary<Direction, int> dirToRotation = new()
        {
            { Direction.Up, 0},
            { Direction.Right, 90},
            { Direction.Down, 180},
            { Direction.Left, 270},
        };


        //You can easily modify the rows and cols as long as they are equal values (ex: rows = 200, cols = 200)
        private readonly int rows = 40, cols = 40;
        private readonly Image[,] gridImages;
        private GameState gameState;
        private bool gameRunning;
        public MainWindow()
        {
            InitializeComponent();
  
            gridImages = SetupGrid();
            gameState = new GameState(rows, cols);
        }


        private async Task RunGame()
        {
            Draw();
            await ShowCountDown();
          
            Overlay.Visibility = Visibility.Hidden;
            await GameLoop();

            await ShowGameover();

            //Fresh game state
            gameState = new GameState(rows,cols);
        }

        private async void Window_PreviwKeyDown(object sender, KeyEventArgs e)
        {
            if(Overlay.Visibility == Visibility.Visible)
            {
                e.Handled = true;
            }


            //If the game is not running
            if(!gameRunning)
            {
                //Set the game to true to run the game
                gameRunning = true;
                //Waiting calll to run the game method
                await RunGame();
                //Then switch it off
                gameRunning = false;   
            }
        }

        //Game Controls
         private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(gameState.GamerOver)
            {
                return;
            }

            switch(e.Key)
            {
                case Key.W:
                case Key.Up:
                    gameState.ChangeDirection(Direction.Up); break;

                case Key.S:
                case Key.Down:
                gameState.ChangeDirection(Direction.Down); break;

                case Key.D:
                case Key.Right:
                gameState.ChangeDirection(Direction.Right); break;

                case Key.A:
                case Key.Left:
                gameState.ChangeDirection(Direction.Left); break;


            }

        }


        private async Task GameLoop()
        {
            while(!gameState.GamerOver)
            {
                await Task.Delay(50);
                gameState.Move();
                Draw();
            }
        }



        private Image[,] SetupGrid()
        {
            Image[,] images = new Image[rows, cols];
            GameGrid.Rows = rows;
            GameGrid.Columns = cols;
            

            for (int r = 0; r< rows; r++)
            {
                for(int c = 0; c< cols; c++)
                {
                    Image image = new Image
                    {
                        Source = Images.Empty,
                        RenderTransformOrigin = new Point(0.5, 0.5)

                    };

                    images[r, c] = image;   
                    GameGrid.Children.Add(image);
                } 
            }

            return images;

        }


        //Draw Method
        private void Draw()
        {
            DrawGrid();
            DrawSnakeHead();
            ScoreText.Text = $"SCORE {gameState.Score}";
        }

        //Method to get row state 
        private void DrawGrid()
        {
            for(int r = 0; r< rows; r++)
            {
                for(int c = 0; c < cols; c++ )
                {
                    GridValue gridVal = gameState.Grid[r, c];
                    gridImages[r, c].Source = gridValToImage[gridVal];
                    gridImages[r, c].RenderTransform = Transform.Identity;
                }
            }
        }


        private void DrawSnakeHead()
        {
            Position headPos = gameState.HeadPosition();
            Image image = gridImages[headPos.Row, headPos.Column];
            image.Source = Images.Head;

            int rotation = dirToRotation[gameState.Dir];
            image.RenderTransform = new RotateTransform(rotation);
        }

        //Count down 
        private async Task ShowCountDown()
        {
            for(int i = 3; i>=1; i--) { 
            
            OverlayText.Text = i.ToString();
            await Task.Delay(500);
            }

        }


        private async Task DrawDeadSnake()
        {
            List<Position> position = new List<Position>(gameState.SnakePositions());
            for(int i = 0; i< position.Count; i++)
            {
                Position pos = position[i]; 
                ImageSource source = (i == 0) ? Images.DeadHead : Images.DeadBody;

                gridImages[pos.Row, pos.Column].Source = source;
                await Task.Delay(50);
            }
        }
        private async Task ShowGameover()
        {
            await DrawDeadSnake();
            await Task.Delay(1000);
            Overlay.Visibility = Visibility.Visible;
            OverlayText.Text = "PRESS ANY KEY TO PLAY";
        }
    }
}
