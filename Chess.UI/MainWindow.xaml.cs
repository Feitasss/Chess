using Chess.Core;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chess.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Image[,] pieceImages = new Image[8, 8];
        private readonly Rectangle[,] highlights = new Rectangle[8, 8];

        private Game Game;
        private Position? selectedPos = null;

        public MainWindow()
        {
            InitializeComponent();
            InitializeBoard();

            Game = new Game(Board.Initial(), Player.White);
            DrawBoard(Game.Board);
        }

        private void InitializeBoard()
        {
            var lightColor = Brushes.Wheat;
            var darkColor = Brushes.SaddleBrown;

            // Loop through every row (r) and column (c)
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Border bgSquare = new Border(); 
                                                                                
                    if ((r + c) % 2 == 0)
                    {
                        bgSquare.Background = lightColor;
                    }
                    else
                    {
                        bgSquare.Background = darkColor;
                    }
                    BoardGrid.Children.Add(bgSquare);

                    // --- The Highlights ---
                    Rectangle highlight = new Rectangle();
                    highlights[r, c] = highlight;
                    HighlightGrid.Children.Add(highlight);

                    // --- The Pieces ---
                    Image pieceImage = new Image();
                    pieceImages[r, c] = pieceImage;
                    PieceGrid.Children.Add(pieceImage);
                }
            }
        }

        private void DrawBoard(Board board)
        {
            Console.WriteLine(Game.CurrentPlayer);

            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Piece piece = board[r, c];

                    ImageSource source = Images.GetImage(piece);

                    // 0, 0 -> A1 square on Chess Board
                    // 0, 0 -> A8 square on WPF Grid
                    pieceImages[7 - r, c].Source = source;
                }
            }
        }

        private void BoardGrid_MouseDown(object sender, MouseEventArgs e)
        {
            Point point = e.GetPosition(BoardGrid);

            double squareSize = BoardGrid.ActualWidth / 8;

            // Convert to Grid Coordinates (0-7)
            int row = (int)(point.Y / squareSize);
            int col = (int)(point.X / squareSize);

            // Convert UI Row to Chess Row
            Position clickedPos = new Position(7 - row, col);

            if (selectedPos == null)
            {
                OnSquareSelected(clickedPos);
            }
            else
            {
                OnSquareMove(clickedPos);
            }
        }

        private void OnSquareSelected(Position pos)
        {
            if (Game.Board.IsEmpty(pos) || Game.Board[pos].Color != Game.CurrentPlayer)
                return;

            selectedPos = pos;

            var moves = Game.GetLegalMovesFor(pos);
            ShowHighlights(moves);
        }

        private void OnSquareMove(Position targetPos)
        {
            var moves = Game.GetLegalMovesFor(selectedPos.Value);

            // Is the clicked square in that list?
            Move validMove = moves.FirstOrDefault(m => m.ToPos == targetPos);

            if (validMove != null)
            {
                Game.MakeMove(validMove);

                DrawBoard(Game.Board);

                selectedPos = null;
                HideHighlights();
            }
            else
            {
                selectedPos = null;
                HideHighlights();
                if (!Game.Board.IsEmpty(targetPos) && Game.Board[targetPos].Color == Game.CurrentPlayer)
                {
                    OnSquareSelected(targetPos);
                }
            }
        }

        private void ShowHighlights(IEnumerable<Move> moves)
        {
            Color highlightColor = Color.FromArgb(150, 125, 255, 125);
            SolidColorBrush brush = new SolidColorBrush(highlightColor);

            foreach (Move move in moves)
            {
                // logic coordinates (0 = Bottom) -> ui coordinates (0 = Top)
                // must flip the row
                int r = 7 - move.ToPos.Row;
                int c = move.ToPos.Column;

                highlights[r, c].Fill = brush;
            }
        }

        private void HideHighlights()
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    highlights[r, c].Fill = Brushes.Transparent;
                }
            }
        }
    }
}