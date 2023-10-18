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
            UpdateVisual();
        }

        private void MainPageNavigate(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(new MainPage());
        }

        private void BasketNavigate(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(new BasketPage());
        }

        private void OrdersNavigate(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(new OrdersPage());
        }

        private void ProfileNavigate(object sender, RoutedEventArgs e)
        {
            if (App.CurrentUser == null)
            {
                var authPage = new AuthorizationPage();
                authPage.OnAuthorization += UpdateVisual;
                Frame.Navigate(authPage);
            }
            else
            {
                Frame.Navigate(new ProfilePage());
            }
        }

        private void UpdateVisual()
        {
            if (App.CurrentUser == null)
            {
                btnProfile.Content = "Войти";
                btnOrders.Visibility = Visibility.Collapsed;
                btnBasket.Visibility = Visibility.Collapsed;
            }
            else
            {
                btnProfile.Content = "Профиль";
                btnOrders.Visibility = Visibility.Visible;
                btnBasket.Visibility = Visibility.Visible;
            }

            if (App.CurrentUser != null && App.CurrentUser.Role_Id == 1)
            {
                btnBasket.Visibility = Visibility.Collapsed;
            }
        }
    }
}
