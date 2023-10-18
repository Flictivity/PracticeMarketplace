﻿using MaterialDesignThemes.Wpf;
using PracticeMarketplace.ADO;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        public RegistrationPage()
        {
            InitializeComponent();
        }

        private void RegisterBtnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tbName.Text) || string.IsNullOrEmpty(tbSurname.Text) ||
                    string.IsNullOrEmpty(tbLogin.Text) ||  string.IsNullOrEmpty(pbPassword.Password))
                {
                    snackbar.MessageQueue?.Enqueue("Необходимо заполнить все поля");
                    return;
                }

                var isExistUser = App.Connection.User.Any(x => x.Login == tbLogin.Text);

                if (isExistUser)
                {
                    snackbar.MessageQueue?.Enqueue("Пользователь с таким логином уже существует");
                    return;
                }

                var newUser = new User()
                {
                    FirstName = tbName.Text,
                    LastName = tbSurname.Text,
                    Login = tbLogin.Text,
                    Password = pbPassword.Password,
                    Role_Id = 2
                };

                App.Connection.User.Add(newUser);
                App.Connection.SaveChanges();

                snackbar.MessageQueue?.Enqueue("Успешная регистрация!");
                tbName.Text = "";
                tbSurname.Text = "";
                tbLogin.Text = "";
                pbPassword.Password = "";
                return;
            }
            catch
            {
                snackbar.MessageQueue?.Enqueue("Произошла ошибка!");
                return;
            }
        }
    }
}
