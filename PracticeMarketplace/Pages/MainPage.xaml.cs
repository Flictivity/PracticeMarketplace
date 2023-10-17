using Microsoft.Win32;
using PracticeMarketplace.ADO;
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

        private void RemoveFromBasketBtnClick(object sender, RoutedEventArgs e)
        {
            var productId = (int)((Button)sender).Tag;
            var basketProduct = App.CurrentUser.Basket.FirstOrDefault(x => x.Product_Id == productId);
            if (basketProduct != null && basketProduct.Count > 0)
            {
                basketProduct.Count--;
            }
            if(basketProduct.Count == 0)
            {
                App.Connection.Basket.Remove(basketProduct);
            }
            App.Connection.SaveChanges();
            lvProducts.ItemsSource = App.Connection.Product.ToList();
        }

        private void AddToBasketBtnClick(object sender, RoutedEventArgs e)
        {
            var productId = (int)((Button)sender).Tag;
            var basketProduct = App.CurrentUser.Basket.FirstOrDefault(x => x.Product_Id == productId);
            if (basketProduct != null)
            {
                basketProduct.Count++;
            }
            else
            {
                App.CurrentUser.Basket.Add(new Basket()
                {
                    Product_Id = productId,
                    Count = 1
                });
            }
            App.Connection.SaveChanges();
            lvProducts.ItemsSource = App.Connection.Product.ToList();
        }
    }
}
