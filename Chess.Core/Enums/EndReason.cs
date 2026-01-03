using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Core.Enums
{
    public enum EndReason
    {
        Checkmate,
        Stalemate,
        FiftyMoveRule,
        InsufficientMaterial,
        ThreefoldRepetition
    }
}
