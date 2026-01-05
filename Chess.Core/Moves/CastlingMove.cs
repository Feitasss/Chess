using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Core
{
    public class CastlingMove : Move
    {
        private readonly Position rookFrom;
        private readonly Position rookToPos;
        public CastlingMove(Position kingFrom, Position kingTo, Position rookFrom)
            : base(kingFrom, kingTo)
        {
            this.rookFrom = rookFrom;

            // If King goes to G column (6) -> Rook goes to F column (5)
            // If King goes to C column (2) -> Rook goes to D column (3)
            int rookDestCol = (kingTo.Column == 6) ? 5 : 3;
            rookToPos = new Position(kingTo.Row, rookDestCol);
        }

        public override void Execute(Board board)
        {
            // Moving the King
            base.Execute(board);

            // Moving the Rook
            Piece rook = board[rookFrom];

            board[rookFrom] = null;
            board[rookToPos] = rook;
            rook.HasMoved = true;
        }

        public override void Undo(Board board)
        {
            base.Undo(board);

            Piece rook = board[rookToPos];
            board[rookToPos] = null;
            board[rookFrom] = rook;
            rook.HasMoved = false;
        }
    }
}
