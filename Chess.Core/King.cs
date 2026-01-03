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

        public override Piece Copy()
        {
            King copy = new King(Color);
            copy.HasMoved = this.HasMoved;
            return copy;
        }

        private static readonly (int r, int c)[] offsets = new (int r, int c)[]
        {
            (1, 0), (1, 1), (0, 1), (-1, 1),
            (-1, 0), (-1, -1), (0, -1), (1, -1)
        };

        public override IEnumerable<Move> GetValidMoves(Position from, Board board)
        {
            var potentialTargets = offsets.Select(offset =>
                new Position(from.Row + offset.r, from.Column + offset.c));

            foreach (var move in MoveToPositions(from, board, potentialTargets))
            {
                yield return move;
            }
        }
    }
}
