using Microsoft.Win32;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PracticeMarketplace.Pages
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            lvProducts.ItemsSource = App.Connection.Product.ToList();
        }

        private void AddImageBtnClick(object sender, RoutedEventArgs e)
        {
            var window = new OpenFileDialog();
            window.Filter = "Image files (*.png;*.jpg)|*.png;*.jpg";

            if (window.ShowDialog() != true)
            {
                MessageBox.Show($"не выбрано изоражение!",
        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            var byteArray = File.ReadAllBytes(window.FileName);
            var product = App.Connection.Product.FirstOrDefault(x => x.Id == 1);
            product.Image = byteArray;
            App.Connection.SaveChangesAsync();
        }
    }
}
