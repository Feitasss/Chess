using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Core
{
    internal class EnPassantMove : Move
    {
        private readonly Position CapturePos;

        public EnPassantMove(Position from, Position to, Position capturePos)
            : base(from, to)
        {
            this.CapturePos = capturePos;
        }

        public override void Execute(Board board)
        {
            base.Execute(board);

            this.CapturedPiece = board[CapturePos];
            board[CapturePos] = null;
        }
    }
}
