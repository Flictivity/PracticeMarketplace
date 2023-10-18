﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    /// Interaction logic for AuthorizationPage.xaml
    /// </summary>
    public partial class AuthorizationPage : Page
    {
        public delegate void AuthorizationDelegate();
        public event AuthorizationDelegate OnAuthorization;
        public AuthorizationPage()
        {
            InitializeComponent();
        }

        private void AuthorizationBtnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tbLogin.Text) || string.IsNullOrEmpty(pbPassword.Password))
                {
                    snackbar.MessageQueue?.Enqueue("Необходимо заполнить все поля");
                    return;
                }

                var user = App.Connection.User.FirstOrDefault(x => x.Login == tbLogin.Text);

                if (user == null)
                {
                    snackbar.MessageQueue?.Enqueue("Пользователь не найден!");
                    return;
                }
                if(user.Password != pbPassword.Password)
                {
                    snackbar.MessageQueue?.Enqueue("Неверный пароль");
                    return;
                }

                App.CurrentUser = user;
                OnAuthorization?.Invoke();

                NavigationService.Navigate(new MainPage());
            }
            catch
            {
                snackbar.MessageQueue?.Enqueue("При выполнении авторизации произошла ошибка");
                return;
            }

        }
    }
}
