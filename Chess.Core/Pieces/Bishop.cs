using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Core
{
    public class Bishop : Piece
    {
        public override PieceType Type => PieceType.Bishop;
        public override Player Color { get; }

        private static readonly (int r, int c)[] dirs = new (int r, int c)[] 
        {
            (1, 1), (-1, 1), (-1, -1), (1, -1) 
        };

        public Bishop(Player color)
        {
            Color = color;
        }

        public override Piece Copy()
        {
            Bishop copy = new Bishop(Color);
            copy.HasMoved = this.HasMoved;
            return copy;
        }

        public override IEnumerable<Move> GetValidMoves(Position from, Board board, Game? state)
        {
            return MoveInDirections(from, board, dirs);
        }
    }
}
