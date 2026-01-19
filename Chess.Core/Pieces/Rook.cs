using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Core
{
    public class Rook : Piece
    {
        public override PieceType Type => PieceType.Rook;
        public override Player Color { get; }

        private static readonly (int r, int c)[] dirs = new (int r, int c)[] 
        {
            (1, 0), (-1, 0), (0, 1), (0, -1) 
        };

        public Rook(Player color)
        {
            Color = color;
        }

        public override Piece Copy()
        {
            Rook copy = new Rook(Color);
            copy.HasMoved = this.HasMoved;
            return copy;
        }

        public override IEnumerable<Move> GetValidMoves(Position from, Board board, Game? state)
        {
            return MoveInDirections(from, board, dirs);
        }
    }
}
