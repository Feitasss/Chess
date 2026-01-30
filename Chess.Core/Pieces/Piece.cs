using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Core
{
    public abstract class Piece
    {
        public abstract PieceType Type { get; }
        public abstract Player Color { get; }
        public bool HasMoved { get; set; } = false;

        public abstract IEnumerable<Move> GetValidMoves(Position from, Board board, Game? state = null);

        // for rook, bishop and queen
        protected IEnumerable<Move> MoveInDirections(Position from, Board board, (int dr, int dc)[] directions)
        {
            foreach (var (dr, dc) in directions)
            {
                Position pos = new Position(from.Row + dr, from.Column + dc);

                while (Board.IsInside(pos))
                {
                    if (board.IsEmpty(pos))
                    {
                        yield return new Move(from, pos);

                        pos = new Position(pos.Row + dr, pos.Column + dc);
                    }
                    else
                    {
                        Piece piece = board[pos];
                        if (piece.Color != this.Color)
                        {
                            yield return new Move(from, pos);
                        }
                        break;
                    }
                }
            }
        }

        // for knight and king
        protected IEnumerable<Move> MoveToPositions(Position from, Board board, (int r, int c)[] offsets)
        {
            foreach (var (r, c) in offsets)
            {
                Position to = new Position(from.Row + r, from.Column + c);
                if (!Board.IsInside(to))
                    continue;

                Piece targetPiece = board[to];

                if (targetPiece == null || targetPiece.Color != this.Color)
                {
                    yield return new Move(from, to);
                }
            }
        }
    }
}
