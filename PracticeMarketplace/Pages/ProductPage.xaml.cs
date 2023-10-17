using PracticeMarketplace.ADO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
                cbCountries.SelectedItem = product.Country;
                cbCategories.SelectedItem = product.ProductCategory;
            }
            DataContext = product;
        }
    }
}
