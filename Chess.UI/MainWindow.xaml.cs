using Chess.Core;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chess.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            var lightColor = Brushes.Wheat;
            var darkColor = Brushes.SaddleBrown;

            // Loop through every row (r) and column (c)
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Border bgSquare = new Border(); 
                                                                                
                    if ((r + c) % 2 == 0)
                    {
                        bgSquare.Background = lightColor;
                    }
                    else
                    {
                        bgSquare.Background = darkColor;
                    }
                    BoardGrid.Children.Add(bgSquare);
                }
            }
        }
    }
}