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

        private bool isReplayMode = false;
        private List<GameRecord> history;

        private List<Move> replayMoves;
        private int replayCurrentMoveIndex = 0;

        public MainWindow()
        {
            InitializeComponent();

            UpdateHistoryList();

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
            if (isReplayMode) return;  // Block clicks on replay

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

                if (Game.IsGameOver)
                {
                    ShowGameOver();
                }
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

        private void ShowGameOver()
        {
            WinnerText.Text = $"{Game.Winner} Wins!";

            if (Game.GameOverReason == EndReason.Stalemate ||
                Game.GameOverReason == EndReason.FiftyMoveRule ||
                Game.GameOverReason == EndReason.InsufficientMaterial)
            {
                WinnerText.Text = "Draw!";
            }

            ReasonText.Text = $"Reason: {Game.GameOverReason}";
            GameOverMenu.Visibility = Visibility.Visible;

            SaveGameButton.Visibility = Visibility.Visible;
        }

        private void SaveGameButton_Click(object sender, EventArgs e)
        {
            HistoryManager.SaveGame(Game);
            UpdateHistoryList();

            SaveGameButton.Visibility = Visibility.Collapsed;
        }
        
        private void DeleteGame_Click(object sender, RoutedEventArgs e)
        {
            Button deleteBtn = (Button)sender;

            if (deleteBtn.DataContext is GameRecord gameToDelete)
            {
                history.Remove(gameToDelete);

                HistoryManager.SaveHistory(history);

                HistoryList.ItemsSource = null;
                HistoryList.ItemsSource = history;
            }
            //e.Handled = true;
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            ReplayControls.Visibility = Visibility.Collapsed;
            GameOverMenu.Visibility = Visibility.Collapsed;

            HistoryList.SelectedItem = null;
            isReplayMode = false;

            Game = new Game(Board.Initial(), Player.White);
            selectedPos = null;
            HideHighlights();

            DrawBoard(Game.Board);
        }

        private void HistoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (HistoryList.SelectedItem is GameRecord selectedGame)
            {
                ReplayControls.Visibility = Visibility.Visible;
                GameOverMenu.Visibility = Visibility.Collapsed;

                StartReplay(selectedGame);
            }
        }

        private void StartReplay(GameRecord record)
        {
            isReplayMode = true;
            GameInfoText.Text = $"Replay: {record.Date.ToShortDateString()} ({record.Winner})";

            Game simState = new Game(Board.Initial(), Player.White);
            replayMoves = new List<Move>();

            foreach (MoveRecord rec in record.Moves)
            {
                Move legalMove = simState.GetLegalMovesFor(rec.From).FirstOrDefault(m => m.ToPos == rec.To);

                if (legalMove != null)
                {
                    replayMoves.Add(legalMove);
                    simState.MakeMove(legalMove);
                }
            }

            Game = new Game(Board.Initial(), Player.White);
            replayCurrentMoveIndex = 0;
            DrawBoard(Game.Board);
            HideHighlights();
        }

        private void ReplayPrevMove_Click(object sender, RoutedEventArgs e)
        {
            //if (!isReplayMode) return;

            if (replayCurrentMoveIndex > 0)
            {
                replayCurrentMoveIndex--;
                Move move = replayMoves[replayCurrentMoveIndex];

                move.Undo(Game.Board);

                DrawBoard(Game.Board);
            }
        }

        private void ReplayNextMove_Click(Object sender, RoutedEventArgs e)
        {
            //if (!isReplayMode) return;

            if (replayCurrentMoveIndex < replayMoves.Count)
            {
                Move move = replayMoves[replayCurrentMoveIndex];

                move.Execute(Game.Board);
                replayCurrentMoveIndex++;

                DrawBoard(Game.Board);
            }
        }

        private void UpdateHistoryList()
        {
            history = HistoryManager.LoadHistory();

            history.Reverse();

            HistoryList.ItemsSource = null;
            HistoryList.ItemsSource = history;
        }

    }
}