using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Core
{
    public class Move
    {
        public Position FromPos { get; }
        public Position ToPos { get; }
        public Piece CapturedPiece { get; private set; }

        private bool wasFirstMove;

        public Move(Position from, Position to)
        {
            FromPos = from;
            ToPos = to;
        }

        public virtual void Execute(Board board)
        {
            Piece piece = board[FromPos];

            CapturedPiece = board[ToPos];
            wasFirstMove = !piece.HasMoved;

            board[ToPos] = piece;
            board[FromPos] = null;

            piece.HasMoved = true;
        }

        public virtual void Undo(Board board)
        {
            Piece piece = board[ToPos];

            board[FromPos] = piece;

            board[ToPos] = CapturedPiece;

            if (wasFirstMove)
            {
                piece.HasMoved = false;
            }
        }
    }
}
