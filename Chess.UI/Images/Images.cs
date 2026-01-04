using Chess.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Chess.UI
{
    public static class Images
    {
        private static readonly Dictionary<PieceType, ImageSource> whiteSources = new();
        private static readonly Dictionary<PieceType, ImageSource> blackSources = new();

        static Images()
        {
            foreach (PieceType type in Enum.GetValues(typeof(PieceType)))
            {
                whiteSources[type] = LoadImage(Player.White, type);
                blackSources[type] = LoadImage(Player.Black, type);
            }
        }

        private static ImageSource LoadImage(Player color, PieceType type)
        {
            return new BitmapImage(new Uri($"pack://application:,,,/Assets/{color}{type}.png"));
        }

        public static ImageSource? GetImage(Piece piece)
        {
            if (piece == null) return null;
            return (piece.Color == Player.White) ? whiteSources[piece.Type] : blackSources[piece.Type];
        }
    }
}
