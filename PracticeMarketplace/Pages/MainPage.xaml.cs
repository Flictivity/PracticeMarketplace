using Microsoft.Win32;
using PracticeMarketplace.ADO;
using PracticeMarketplace.Dto;
using System;
using System.Collections.Generic;
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
        public List<Product> SourceData { get; set; }
        private static CostFilterDto costFilterData = new CostFilterDto();

        private Predicate<Product> _productTypeFilterQuery = x => true;
        private Predicate<Product> _countryFilterQuery = x => true;
        private Predicate<Product> _costFilterQuery = x => true;

        private Func<Product, object> _sortQuery = x => x.Id;

        private List<string> _sortTitles = new List<string>() { "По умолчанию", "По возрастанию цены", "По убыванию цены" };
        public MainPage()
        {
            InitializeComponent();
            FillFilterData();
            UpdateData();
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
            if (basketProduct?.Count == 0)
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

        private void FillFilterData()
        {
            var categories = App.Connection.ProductCategory.ToList();
            categories.Add(new ProductCategory { Name = "Все" });

            cbCategories.ItemsSource = categories;
            cbCategories.SelectedItem = categories.Last();

            var countries = App.Connection.Country.ToList();
            countries.Add(new Country { Name = "Все" });

            cbCountries.ItemsSource = countries;
            cbCountries.SelectedItem = countries.Last();

            cbSortByCost.ItemsSource = _sortTitles;
            cbSortByCost.SelectedItem = _sortTitles.FirstOrDefault(x => x == "По умолчанию");

            costFilterData.CostMin = 0;
            costFilterData.CostMax = (int)App.Connection.Product.Max(x => x.Cost);

            this.DataContext = costFilterData;
        }

        private void UpdateData()
        {
            if (lvProducts == null)
            {
                return;
            }

            SourceData = App.Connection.Product.ToList()
                .Where(x => _productTypeFilterQuery(x) && _costFilterQuery(x) && _countryFilterQuery(x))
                .OrderBy(x => _sortQuery(x))
                .ToList();
            Search();
        }

        private void Search()
        {
            if (lvProducts == null)
            {
                return;
            }

            lvProducts.ItemsSource = SourceData.Where(x => x.Description.ToLower().Contains(tbSearch.Text.ToLower())
                                                              || x.Name.ToLower().Contains(tbSearch.Text.ToLower())).ToList();
        }

        private void SortByCost(object sender, SelectionChangedEventArgs e)
        {
            if (lvProducts == null || cbSortByCost.SelectedItem == null)
            {
                return;
            }
;
            var selectedSort = cbSortByCost.SelectedItem.ToString();

            switch (selectedSort)
            {
                case "По возрастанию цены":
                    _sortQuery = x => x.Cost;
                    break;

                case "По убыванию цены":
                    _sortQuery = x => -x.Cost;
                    break;

                default:
                    _sortQuery = x => x.Id;
                    break;
            }

            UpdateData();
        }

        private void CategoriesFilter()
        {
            if (lvProducts == null || cbCategories.SelectedItem == null)
            {
                return;
            }

            var category = cbCategories.SelectedItem as ProductCategory;

            if (category?.Name == "Все")
            {
                _productTypeFilterQuery = x => true;
            }
            else
            {
                _productTypeFilterQuery = x => x.ProductCategory.Id == category?.Id;
            }
            UpdateData();
        }

        private void CountriesFilter()
        {
            if (lvProducts == null || cbCountries.SelectedItem == null)
            {
                return;
            }

            var country = cbCountries.SelectedItem as Country;

            if (country?.Name == "Все")
            {
                _countryFilterQuery = x => true;
            }
            else
            {
                _countryFilterQuery = x => x.Country.Id == country?.Id;
            }
            UpdateData();
        }

        private void CostFilter()
        {
            if (lvProducts == null)
            {
                return;
            }

            _costFilterQuery = x => x.Cost >= costFilterData.CostMin && x.Cost <= costFilterData.CostMax;
            UpdateData();
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search();
        }

        private void CostMinTextChanged(object sender, TextChangedEventArgs e)
        {
            CostFilter();
        }

        private void CostMaxTextChanged(object sender, TextChangedEventArgs e)
        {
            CostFilter();
        }

        private void CbCategories_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CategoriesFilter();
        }

        private void CbCountries_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CountriesFilter();
        }

        private void ApplyCostFilter(object sender, RoutedEventArgs e)
        {
            CostFilter();

            UpdateData();
        }
    }
}
