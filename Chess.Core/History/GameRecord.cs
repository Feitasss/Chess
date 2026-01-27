using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Core
{
    public class GameRecord
    {
        public DateTime Date { get; set; } = DateTime.Now;
        public Player Winner { get; set; }
        public EndReason Reason { get; set; }
        public List<MoveRecord> Moves { get; set; } = new List<MoveRecord>();
    }
}
