namespace Chess.Core
{
    public enum Player
    {
        None,
        White,
        Black
    }

    public enum PieceType
    {
        Pawn,
        King,
        Queen,
        Rook,
        Bishop,
        Knight
    }

    public enum EndReason
    {
        Checkmate,
        Stalemate,
        FiftyMoveRule,
        InsufficientMaterial,
        ThreefoldRepetition
    }
}