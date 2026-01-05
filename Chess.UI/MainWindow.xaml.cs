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

        private bool flipBoard = false;
        private bool IsFlipped => flipBoard && Game.CurrentPlayer == Player.Black;

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
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    // 0, 0 -> A1 square on Chess Board
                    // 0, 0 -> A8 square on WPF Grid
                    // so for White we should turn rows upside down
                    // and if we flip Board we should change the order of columns
                    int uiRow, uiCol;

                    if (IsFlipped)
                    {
                        uiRow = r;
                        uiCol = 7 - c;
                    }
                    else
                    {
                        uiRow = 7 - r;
                        uiCol = c;
                    }

                    Piece piece = board[r, c];
                    ImageSource source = Images.GetImage(piece);

                    pieceImages[uiRow, uiCol].Source = source;
                    highlights[uiRow, uiCol].Fill = Brushes.Transparent;
                }
            }
        }

        private void BoardGrid_MouseDown(object sender, MouseEventArgs e)
        {
            Point point = e.GetPosition(BoardGrid);

            double squareSize = BoardGrid.ActualWidth / 8;

            // Convert to Grid Coordinates (0-7)
            int uiRow = (int)(point.Y / squareSize);
            int uiCol = (int)(point.X / squareSize);

            // Convert UI Coordinates to Chess Coordinates
            int r, c;

            if (IsFlipped)
            {
                r = uiRow;
                c = 7 - uiCol;
            }
            else
            {
                r = 7 - uiRow;
                c = uiCol;
            }

            Position clickedPos;
            if (Game.CurrentPlayer == Player.White)
            {
                clickedPos = new Position(r, c);
            }
            else
            {
                clickedPos = new Position(r, c);
            }

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
                int uiRow, uiCol;

                if (IsFlipped)
                {
                    uiRow = move.ToPos.Row;
                    uiCol = 7 - move.ToPos.Column;
                }
                else
                {
                    uiRow = 7 - move.ToPos.Row;
                    uiCol = move.ToPos.Column;
                }

                highlights[uiRow, uiCol].Fill = brush;
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