using StartTileColorChanger.Views;
using System.Windows;

namespace StartTileColorChanger {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            DataContext = new Tiles();
        }
    }
}
