using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Core
{
    public class Knight : Piece
    {
        public override PieceType Type => PieceType.Knight;
        public override Player Color { get; }

        public Knight(Player color)
        {
            Color = color;
        }

        public override Piece Copy()
        {
            Knight copy = new Knight(Color);
            copy.HasMoved = this.HasMoved;
            return copy;
        }

        private static readonly (int r, int c)[] offsets = new (int r, int c)[]
        {
            (2, 1), (2, -1), (-2, 1), (-2, -1),
            (1, 2), (1, -2), (-1, 2), (-1, -2)
        };

        public override IEnumerable<Move> GetValidMoves(Position from, Board board, Game? state)
        {
            var potentialTargets = offsets.Select(offset =>
                new Position(from.Row + offset.r, from.Column + offset.c));

            return MoveToPositions(from, board, potentialTargets);
        }
    }
}
