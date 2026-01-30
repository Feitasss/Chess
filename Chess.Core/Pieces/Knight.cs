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

        private static readonly (int r, int c)[] offsets = new (int r, int c)[]
        {
            (2, 1), (2, -1), (-2, 1), (-2, -1),
            (1, 2), (1, -2), (-1, 2), (-1, -2)
        };

        public override IEnumerable<Move> GetValidMoves(Position from, Board board, Game? state)
        {
            return MoveToPositions(from, board, offsets);
        }
    }
}
