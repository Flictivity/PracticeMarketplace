using PracticeMarketplace.ADO;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PracticeMarketplace.Pages
{
    /// <summary>
    /// Interaction logic for ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {
        private User user;
        public delegate void OnExitDelegate();
        public event OnExitDelegate OnExit;
        public ProfilePage()
        {
            InitializeComponent();
            if (App.CurrentUser != null)
            {
                user = App.CurrentUser;
                DataContext = user;
            }
        }

        private void SaveBtnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                user.Password = pbPassword.Password;
                if (string.IsNullOrWhiteSpace(user.Login) || string.IsNullOrWhiteSpace(user.FirstName)
                    || string.IsNullOrWhiteSpace(user.LastName) || string.IsNullOrWhiteSpace(user.Password))
                {
                    snackbar.MessageQueue.Enqueue("Необходимо заполнить все поля");
                    return;
                }

                var existUser = App.Connection.User.FirstOrDefault(x => x.Login == user.Login);

                if (existUser != null && existUser.Id != App.CurrentUser.Id)
                {
                    snackbar.MessageQueue.Enqueue("Пользователь с таким логином уже существует");
                    return;
                }

                App.CurrentUser = user;

                App.Connection.User.AddOrUpdate(App.CurrentUser);
                App.Connection.SaveChanges();

                snackbar.MessageQueue.Enqueue("Данные успешно обновлены");
                return;
            }
            catch
            {
                snackbar.MessageQueue.Enqueue("При обновлении данных произошла ошибка");
                return;
            }
        }

        private void ExitBtnClick(object sender, RoutedEventArgs e)
        {
            App.CurrentUser = null;
            OnExit?.Invoke();
            NavigationService.Navigate(new MainPage());
        }
    }
}
