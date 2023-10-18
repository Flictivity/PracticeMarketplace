﻿using System.Linq;
using System.Windows;

namespace PracticeMarketplace.ADO
{
    public partial class Product
    {
        public Visibility RemoveBtnVisibility => App.CurrentUser != null && App.CurrentUser.Basket.FirstOrDefault(x => x.Product_Id == this.Id) != null
            ? Visibility.Visible
            : Visibility.Collapsed;

        public Visibility ManagerButtonsVisibility => App.CurrentUser != null && App.CurrentUser.Role_Id == 1 ? Visibility.Collapsed : Visibility.Visible;
        public Visibility ManagerProductButtonsVisibility => App.CurrentUser != null && App.CurrentUser.Role_Id == 1 ? Visibility.Visible : Visibility.Collapsed;
        public string ProductBtnContent => App.CurrentUser != null && App.CurrentUser.Role_Id == 1 ? "Изменить" : "Подробнее";

        public int Count
        {
            get
            {
                if (App.CurrentUser == null)
                {
                    return 0;
                }
                var productInBucket = App.CurrentUser.Basket.FirstOrDefault(x => x.Product_Id == this.Id);
                if (productInBucket != null)
                {
                    return (int)productInBucket.Count;
                }
                return 0;
            }
        }

        public string ShortName => this.Name.Length > 15 ? $"{this.Name.Substring(0, 15)}..." : this.Name;

        public bool IsProductReadonly => App.CurrentUser == null || (App.CurrentUser != null && App.CurrentUser.Role_Id == 2);
    }
}