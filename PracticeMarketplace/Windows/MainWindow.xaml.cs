using PracticeMarketplace.Pages;
using System.Windows;

namespace PracticeMarketplace
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Frame.Navigate(new MainPage());
        }

        private void MainPageNavigate(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(new MainPage());
        }
    }
}
