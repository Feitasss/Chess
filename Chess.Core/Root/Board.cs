using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Core
{
    public class Board
    {
        private readonly Piece[,] pieces = new Piece[8, 8];

        public Piece this[int row, int col]
        {
            get { return pieces[row, col]; }
            set { pieces[row, col] = value; }
        }

        public Piece this[Position pos]
        {
            get { return pieces[pos.Row, pos.Column]; }
            set { pieces[pos.Row, pos.Column] = value; }
        }

        public static Board Initial()
        {
            Board board = new Board();
            board.AddStartPieces();
            return board;
        }

        private void AddStartPieces()
        {
            this[0, 0] = new Rook(Player.White);
            this[0, 1] = new Knight(Player.White);
            this[0, 2] = new Bishop(Player.White);
            this[0, 3] = new Queen(Player.White);
            this[0, 4] = new King(Player.White);
            this[0, 5] = new Bishop(Player.White);
            this[0, 6] = new Knight(Player.White);
            this[0, 7] = new Rook(Player.White);

            for (int c = 0; c < 8; c++)
            {
                this[1, c] = new Pawn(Player.White);
            }

            for (int c = 0; c < 8; c++)
            {
                this[6, c] = new Pawn(Player.Black);
            }

            this[7, 0] = new Rook(Player.Black);
            this[7, 1] = new Knight(Player.Black);
            this[7, 2] = new Bishop(Player.Black);
            this[7, 3] = new Queen(Player.Black);
            this[7, 4] = new King(Player.Black);
            this[7, 5] = new Bishop(Player.Black);
            this[7, 6] = new Knight(Player.Black);
            this[7, 7] = new Rook(Player.Black);
        }

        public static bool IsInside(Position pos)
        {
            return pos.Row >= 0 && pos.Row < 8 && pos.Column >= 0 && pos.Column < 8;
        }

        public bool IsEmpty(Position pos)
        {
            return this[pos] == null;
        }

        public bool IsInCheck(Player player)
        {
            Position kingPos = GetKingPosition(player);

            return IsAttacked(kingPos, player == Player.White ? Player.Black : Player.White);
        }

        private Position GetKingPosition(Player player)
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Position pos = new Position(r, c);
                    Piece piece = this[pos];

                    if (piece != null && piece.Type == PieceType.King && piece.Color == player)
                    {
                        return pos;
                    }
                }
            }
            return new Position(-1, -1);
        }

        public bool IsAttacked(Position target, Player attacker)
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Position pos = new Position(r, c);
                    Piece piece = this[pos];

                    if (piece == null || piece.Color != attacker) continue;

                    if (piece.Type == PieceType.King)
                    {
                        int deltaRow = Math.Abs(pos.Row - target.Row);
                        int deltaCol = Math.Abs(pos.Column - target.Column);

                        if (deltaRow <= 1 && deltaCol <= 1)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (piece.GetValidMoves(pos, this).Any(m => m.ToPos == target))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
