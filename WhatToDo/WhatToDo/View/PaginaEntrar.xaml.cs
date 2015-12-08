﻿using System;
using WhatToDo.Controller;
using WhatToDo.Model.Entity;
using WhatToDo.Service.Connection;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WhatToDo.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PaginaEntrar : Page
    {
        public MessageDialog msg { get; set; }
        public PaginaEntrar()
        {
            this.InitializeComponent();
        }

        private async void ButtonEntrar_Click(object sender, RoutedEventArgs e)
        {
            PaginaEntrarController PEC = new PaginaEntrarController();

            string email = TextEmail.Text;
            string senha = PasswordSenha.Password;
            if (email == "admin" && senha == "admin")
            {
                Frame.Navigate(typeof(MainPage), new Usuario("admin"));
            }
            else if(DatabaseConnection.ValidateRegister(new Usuario(email, senha)) == 0)
            {
                Frame.Navigate(typeof(MainPage), new Usuario(email));
            }
            else
            {
                msg = new MessageDialog("Error! Invalid email or password. Try again.");
                await msg.ShowAsync();
            }
        }

        private void ButtonCancelar_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(IntroPage));
        }

        private async void PasswordSenha_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                string email = TextEmail.Text;
                string senha = PasswordSenha.Password;
                if (email == "admin" && senha == "admin")
                {
                    Frame.Navigate(typeof(MainPage), new Usuario("admin"));
                }
                else if (DatabaseConnection.ValidateRegister(new Usuario(email, senha)) == 0)
                {
                    Frame.Navigate(typeof(MainPage), new Usuario(email));
                }
                else
                {
                    msg = new MessageDialog("Error! Invalid email or password. Try again.");
                    await msg.ShowAsync();
                }
            }
        }
    }
}
