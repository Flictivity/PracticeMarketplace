using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using PracticeMarketplace.ADO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PracticeMarketplace.Pages
{
    /// <summary>
    /// Interaction logic for ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        private Product product = new Product();
        private bool isEdit;
        public ProductPage(int productId)
        {
            InitializeComponent();
            cbCategories.ItemsSource = App.Connection.ProductCategory.ToList();
            cbCountries.ItemsSource = App.Connection.Country.ToList();

            var dbProduct = App.Connection.Product.FirstOrDefault(x => x.Id == productId);
            if(dbProduct != null) 
            {
                product = dbProduct;
                isEdit = true;
            }
            DataContext = product;
        }

        private void AddImageBtnClick(object sender, RoutedEventArgs e)
        {
            var window = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpg)|*.png;*.jpg"
            };

            if (window.ShowDialog() != true)
            {
                MessageBox.Show($"не выбрано изоражение!",
        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var byteArray = File.ReadAllBytes(window.FileName);
            product.Image = byteArray;
            BindingOperations.GetBindingExpressionBase(imgPhoto, Image.SourceProperty).UpdateTarget();
            App.Connection.SaveChangesAsync();
        }

        private void SaveBtnClick(object sender, RoutedEventArgs e)
        {
            snackbar.Background = Brushes.Red;
            if (product.Cost <= 0)
            {
                snackbar.MessageQueue.Enqueue("Стоимость товара должны быть больше 0");
                return;
            }
            if(string.IsNullOrWhiteSpace(product.Name))
            {
                snackbar.MessageQueue.Enqueue("Заполните наименование товара");
                return;
            }
            if (string.IsNullOrWhiteSpace(product.ArticleNumber))
            {
                snackbar.MessageQueue.Enqueue("Заполните артикул товара");
                return;
            }
            if (string.IsNullOrWhiteSpace(product.Description))
            {
                snackbar.MessageQueue.Enqueue("Заполните описание товара");
                return;
            }
            if(product.Country == null)
            {
                snackbar.MessageQueue.Enqueue("Необходимо выбрать страну товара");
                return;
            }
            if (product.ProductCategory == null)
            {
                snackbar.MessageQueue.Enqueue("Необходимо выбрать категорию товара");
                return;
            }
            if(product.Image == null) {
                snackbar.MessageQueue.Enqueue("Необходимо выбрать изображение товара");
                return;
            }

            if(!decimal.TryParse(tbCost.Text.Replace('.',','), out decimal dec))
            {
                snackbar.MessageQueue.Enqueue("Неверная стоимость товара!");
                return;
            }

            if (!int.TryParse(tbArticle.Text, out int article))
            {
                snackbar.MessageQueue.Enqueue("Введите корректный артикул!");
                return;
            }

            try
            {
                App.Connection.Product.AddOrUpdate(product);
                App.Connection.SaveChanges();
                if(isEdit)
                {
                    MessageBox.Show($"Товар успешно изменен", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show($"Товар успешно добавлен", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                NavigationService.Navigate(new MainPage());
            }
            catch
            {
                MessageBox.Show("При сохранении товара произошла ошибка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
