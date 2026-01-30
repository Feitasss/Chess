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
            Color = color;
            forwardDir = (color == Player.White) ? 1 : -1;
        }

        public override IEnumerable<Move> GetValidMoves(Position from, Board board, Game state)
        {
            Position oneStep = new Position(from.Row + forwardDir, from.Column);

            int promotionRow = (Color == Player.White) ? 7 : 0;

            if (Board.IsInside(oneStep) && board.IsEmpty(oneStep))
            {
                if (oneStep.Row == promotionRow)
                {
                    yield return new PromotionMove(from, oneStep, PieceType.Queen);
                }
                else
                {
                    yield return new Move(from, oneStep);
                    // first 2 squares move
                    if (!HasMoved)
                    {
                        Position twoSteps = new Position(from.Row + (2 * forwardDir), from.Column);
                        if (Board.IsInside(twoSteps) && board.IsEmpty(twoSteps))
                        {
                            yield return new Move(from, twoSteps);
                        }
                    }
                }

            }
            
            // captures
            int[] captureOffsets = { -1, 1 };

            foreach (int offset in captureOffsets)
            {
                Position diagTarget = new Position(from.Row + forwardDir, from.Column + offset);

                if (Board.IsInside(diagTarget))
                {
                    Piece piece = board[diagTarget];

                    if (piece != null && piece.Color != this.Color)
                    {
                        if (diagTarget.Row == promotionRow)
                        {
                            yield return new PromotionMove(from, diagTarget, PieceType.Queen);
                        }
                        else
                        {
                            yield return new Move(from, diagTarget);
                        }
                    }
                }
            }

            // En Passant
            if (state != null && state.MoveHistory.Count > 0)
            {
                Move lastMove = state.MoveHistory.Last();
                Position lastFrom = lastMove.FromPos;
                Position lastTo = lastMove.ToPos;
                Piece lastPiece = lastMove.MovedPiece;

                // if last move was made by Enemy Pawn by 2 squares
                // near our initial pawn
                if (lastPiece.Type == PieceType.Pawn &&
                    lastPiece.Color != this.Color &&
                    Math.Abs(lastFrom.Row - lastTo.Row) == 2 &&
                    from.Row == lastTo.Row &&
                    Math.Abs(from.Column - lastTo.Column) == 1)
                {
                    Position captureDest = new Position(from.Row + forwardDir, lastTo.Column);
                    yield return new EnPassantMove(from, captureDest, lastTo);
                }
            }
        }
    }
}
