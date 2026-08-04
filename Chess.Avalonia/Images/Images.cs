using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Chess.Core;
using System;
using System.Collections.Generic;

namespace Chess.Avalonia
{
    public static class Images
    {
        private static readonly Dictionary<PieceType, Bitmap> whiteSources = new();
        private static readonly Dictionary<PieceType, Bitmap> blackSources = new();

        static Images()
        {
            foreach (PieceType type in Enum.GetValues(typeof(PieceType)))
            {
                whiteSources[type] = LoadImage(Player.White, type);
                blackSources[type] = LoadImage(Player.Black, type);
            }
        }

        private static Bitmap LoadImage(Player color, PieceType type)
        {
            return new Bitmap(AssetLoader.Open(new Uri($"avares://Chess.Avalonia/Assets/{color}{type}.png")));
        }

        public static Bitmap? GetImage(Piece piece)
        {
            if (piece == null) return null;
            return (piece.Color == Player.White) ? whiteSources[piece.Type] : blackSources[piece.Type];
        }
    }
}
