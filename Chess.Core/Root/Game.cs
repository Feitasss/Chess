using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Core
{
    public class Game
    {
        public Board Board { get; }
        public Player CurrentPlayer { get; private set; }
        public List<Move> MoveHistory { get; } = new List<Move>();

        public Player Winner { get; private set; } = Player.None;
        public EndReason GameOverReason { get; private set; }
        public bool IsGameOver { get; private set; } = false;


        public Game(Board board, Player player)
        {
            Board = board;
            CurrentPlayer = player;
        }

        public IEnumerable<Move> GetLegalMovesFor(Position pos)
        {
            if (IsGameOver || Board.IsEmpty(pos) || Board[pos].Color != CurrentPlayer || Board[pos] == null)
            {
                return Enumerable.Empty<Move>();
            }

            Piece piece = Board[pos];
            IEnumerable<Move> candidates = piece.GetValidMoves(pos, Board, this);

            return candidates.Where(move => IsMoveLegal(move));
        }

        private bool IsMoveLegal(Move move)
        {
            bool legal = true;

            move.Execute(Board);

            legal = !Board.IsInCheck(CurrentPlayer);

            move.Undo(Board);

            return legal;
        }

        public void MakeMove(Move move)
        {
            MoveHistory.Add(move);

            move.Execute(Board);

            CurrentPlayer = (CurrentPlayer == Player.White) ? Player.Black : Player.White;

            CheckForGameOver();
        }

        private void CheckForGameOver()
        {
            if (!AllLegalMovesFor(CurrentPlayer).Any())
            {
                if (Board.IsInCheck(CurrentPlayer))
                {
                    IsGameOver = true;
                    Winner = (CurrentPlayer == Player.White) ? Player.Black : Player.White;
                    GameOverReason = EndReason.Checkmate;
                }
                else
                {
                    IsGameOver = true;
                    Winner = Player.None;
                    GameOverReason = EndReason.Stalemate;
                }
            }
        }

        private IEnumerable<Move> AllLegalMovesFor(Player player)
        {
            IEnumerable<Move> allMoves = Enumerable.Empty<Move>();

            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Position pos = new Position(r, c);
                    Piece piece = Board[pos];

                    if (piece != null && piece.Color == player)
                    {
                        allMoves = allMoves.Concat(GetLegalMovesFor(pos));
                    }
                }
            }
            return allMoves;
        }
    }
}
