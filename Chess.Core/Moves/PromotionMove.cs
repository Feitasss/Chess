using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Chess.Core
{
    public class PromotionMove : Move
    {
        PieceType NewType { get; }

        public PromotionMove(Position from, Position to, PieceType newtype)
            : base(from, to)
        {
            NewType = newtype;
        }

        public override void Execute(Board board)
        {
            Piece pawn = board[FromPos];
            board[FromPos] = null;

            Piece newPiece = CreatePromotionPiece(pawn.Color);
            newPiece.HasMoved = true;

            Piece capturedPiece = board[ToPos];
            board[ToPos] = newPiece;

            MovedPiece = pawn;
            CapturedPiece = capturedPiece;
        }

        public override void Undo(Board board)
        {
            Piece pawn = new Pawn(MovedPiece.Color);
            pawn.HasMoved = true;

            board[FromPos] = pawn;
            board[ToPos] = CapturedPiece;
        }

        public Piece CreatePromotionPiece(Player color)
        {
            switch (NewType)
            {
                case PieceType.Rook: return new Rook(color);
                case PieceType.Bishop: return new Bishop(color);
                case PieceType.Knight: return new Knight(color);
                default: return new Queen(color);
            }
        }
    }
}
