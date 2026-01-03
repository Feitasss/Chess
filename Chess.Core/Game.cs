using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Core
{
    public class Game
    {
        public Board Board { get; }
        public Player CurrentPlayer { get; private set; }

        public Player Winner { get; private set; } = Player.None;
        public EndReason GameOverReason { get; private set; }
        public bool IsGameOver { get; private set; } = false;

        public Game(Board board, Player player)
        {
            Board = board;
            CurrentPlayer = player;
        }

        public void MakeMove(Move move)
        {
            move.Execute(Board);

            CurrentPlayer = CurrentPlayer == Player.White ? Player.Black : Player.White;
        }

        private bool IsMoveLegal(Move move)
        {
            bool legal = true;

            move.Execute(Board);

            legal = !Board.IsInCheck(CurrentPlayer);

            move.Undo(Board);

            return legal;
        }
    }
}
