using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Core
{
    public class Pawn : Piece
    {
        public override PieceType Type => PieceType.Pawn;
        public override Player Color { get; }

        private readonly int forwardDir;

        public Pawn(Player color) 
        { 
            Player Color = color;
            forwardDir = (color == Player.White) ? 1 : -1;
        }

        public override Piece Copy()
        {
            Pawn copy = new Pawn(Color);
            copy.HasMoved = this.HasMoved;
            return copy;
        }

        public override IEnumerable<Move> GetValidMoves(Position from, Board board)
        {
            Position oneStep = new Position(from.Row + forwardDir, from.Column);

            if (Board.IsInside(oneStep) && board.IsEmpty(oneStep))
            {
                yield return new Move(from, oneStep);

                if (!HasMoved)
                {
                    Position twoSteps = new Position(from.Row + (2 * forwardDir), from.Column);
                    if (Board.IsInside(twoSteps) && board.IsEmpty(twoSteps))
                    {
                        yield return new Move(from, twoSteps);
                    }
                }
            }
            
            int[] captureOffsets = { -1, 1 };

            foreach (int offset in captureOffsets)
            {
                Position diagTarget = new Position(from.Row + forwardDir, from.Column + offset);

                if (Board.IsInside(diagTarget))
                {
                    Piece piece = board[diagTarget];

                    if (piece != null && piece.Color != this.Color)
                    {
                        yield return new Move(from, diagTarget);
                    }
                }
            }
        }
    }
}
