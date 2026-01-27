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

        public Piece MovedPiece { get; protected set; }
        public Piece CapturedPiece { get; protected set; }

        private bool wasFirstMove;

        public Move(Position from, Position to)
        {
            FromPos = from;
            ToPos = to;
        }

        public virtual void Execute(Board board)
        {
            MovedPiece = board[FromPos];
            CapturedPiece = board[ToPos];
            wasFirstMove = !MovedPiece.HasMoved;

            board[ToPos] = MovedPiece;
            board[FromPos] = null;

            MovedPiece.HasMoved = true;
        }

        public virtual void Undo(Board board)
        {
            board[FromPos] = MovedPiece;
            board[ToPos] = CapturedPiece;

            if (wasFirstMove)
            {
                MovedPiece.HasMoved = false;
            }
        }
    }
}
