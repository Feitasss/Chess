using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Core
{
    public class King : Piece
    {
        public override PieceType Type => PieceType.King;
        public override Player Color { get; }

        public King(Player color)
        {
            Color = color;
        }

        private static readonly (int r, int c)[] offsets = new (int r, int c)[]
        {
            (1, 0), (1, 1), (0, 1), (-1, 1),
            (-1, 0), (-1, -1), (0, -1), (1, -1)
        };

        public override IEnumerable<Move> GetValidMoves(Position from, Board board, Game? state)
        {
            foreach (var move in MoveToPositions(from, board, offsets))
            {
                yield return move;
            }

            if (this.HasMoved) yield break;

            // -- Checking for castling possibility
            Player opponent = (this.Color == Player.White) ? Player.Black : Player.White;
            int[] directions = { 1, -1 };

            foreach (int dir in directions)
            {
                for (int i = 1; i < 8; i++)
                {
                    int targetCol = from.Column + (i * dir);

                    if (targetCol < 0 || targetCol > 7) break;

                    Position currentPos = new Position(from.Row, targetCol);
                    Piece piece = board[currentPos];

                    if (piece == null) continue;
                    if (piece.Type == PieceType.Rook && piece.Color == this.Color && !piece.HasMoved)
                    {
                        int destCol = (dir == 1) ? 6 : 2;
                        Position kingDest = new Position(from.Row, destCol);

                        // Checking if the King path to castling possition is safe
                        // (could not be attaked by any enemy piece)
                        if (IsPathSafe(from, kingDest, board, opponent))
                        {
                            yield return new CastlingMove(from, kingDest, currentPos);
                            break;
                        }
                    }
                    break;
                }
            }
        }

        private bool IsPathSafe(Position start, Position end, Board board, Player enemy)
        {
            int minCol = Math.Min(start.Column, end.Column);
            int maxCol = Math.Max(start.Column, end.Column);

            for (int i = minCol; i <= maxCol; i++)
            {
                Position pos = new Position(start.Row, i);
                if (board.IsAttacked(pos, enemy))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
