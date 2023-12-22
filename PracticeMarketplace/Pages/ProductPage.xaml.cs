using Microsoft.Win32;
using PracticeMarketplace.ADO;
using QRCoder;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

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
            if (dbProduct != null)
            {
                product = dbProduct;
                isEdit = true;
                GenerateCode();
            }
            DataContext = product;
        }

        private void GenerateCode()
        {
            try
            {
                var link = $"https://www.ozon.ru/search/?deny_category_prediction=true&from_global=true&text={string.Join("", product.Name.Split())}";
                var generator = new QRCodeGenerator();
                var codeData = generator.CreateQrCode(link, QRCodeGenerator.ECCLevel.L);
                QRCode code = new QRCode(codeData);

                Bitmap bitmap = code.GetGraphic(100);
                using (MemoryStream memory = new MemoryStream())
                {
                    bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                    memory.Position = 0;
                    BitmapImage bitmapimage = new BitmapImage();
                    bitmapimage.BeginInit();
                    bitmapimage.StreamSource = memory;
                    bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapimage.EndInit();
                    imgQr.Source = bitmapimage;
                }
            }
            catch
            {

            }
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
            BindingOperations.GetBindingExpressionBase(imgPhoto, System.Windows.Controls.Image.SourceProperty).UpdateTarget();
            App.Connection.SaveChangesAsync();
        }

        private void SaveBtnClick(object sender, RoutedEventArgs e)
        {
            snackbar.Background = System.Windows.Media.Brushes.Red;
            if (product.Cost <= 0)
            {
                snackbar.MessageQueue.Enqueue("Стоимость товара должны быть больше 0");
                return;
            }
            if (string.IsNullOrWhiteSpace(product.Name))
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
            if (product.Country == null)
            {
                snackbar.MessageQueue.Enqueue("Необходимо выбрать страну товара");
                return;
            }
            if (product.ProductCategory == null)
            {
                snackbar.MessageQueue.Enqueue("Необходимо выбрать категорию товара");
                return;
            }
            if (product.Image == null)
            {
                snackbar.MessageQueue.Enqueue("Необходимо выбрать изображение товара");
                return;
            }

            if (!decimal.TryParse(tbCost.Text.Replace('.', ','), out decimal dec))
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
                product.IsDeleted = false;
                App.Connection.Product.AddOrUpdate(product);
                App.Connection.SaveChanges();
                if (isEdit)
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

        private void DeleteBtnClick(object sender, RoutedEventArgs e)
        {
            if (product.IsDeleted.Value)
            {
                product.IsDeleted = false;
            }
            else
            {
                product.IsDeleted = true;
            }
            App.Connection.Product.AddOrUpdate(product);
            App.Connection.SaveChanges();
            btnDelete.GetBindingExpression(Button.ContentProperty).UpdateTarget();
            MessageBox.Show(product.IsDeleted.Value ? "Товар снят с продажи" : "Товар возвращен в продажу", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void RemoveFromBasketBtnClick(object sender, RoutedEventArgs e)
        {
            var productId = (int)((Button)sender).Tag;
            var basketProduct = App.CurrentUser.Basket.FirstOrDefault(x => x.Product_Id == productId);
            if (basketProduct != null && basketProduct.Count > 0)
            {
                basketProduct.Count--;
            }
            if (basketProduct?.Count == 0)
            {
                App.Connection.Basket.Remove(basketProduct);
            }
            App.Connection.SaveChanges();
            tbCount.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
        }

        private void AddToBasketBtnClick(object sender, RoutedEventArgs e)
        {
            if (App.CurrentUser is null || App.CurrentUser.Role_Id != 2)
            {
                snackbar.MessageQueue.Enqueue("Для добавления товара в корзину необходимо авторизоваться");
                return;
            }
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
            tbCount.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
        }
    }
}
