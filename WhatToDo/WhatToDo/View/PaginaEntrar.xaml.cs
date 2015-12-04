using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WhatToDo.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PaginaEntrar : Page
    {
        public PaginaEntrar()
        {
            this.InitializeComponent();
        }

        private async void ButtonEntrar_Click(object sender, RoutedEventArgs e)
        {
            string email = TextEmail.Text;
            string senha = PasswordSenha.Password;

            var msg = new MessageDialog(string.Format("the email {0} has {1} as a password", email, senha));
            await msg.ShowAsync();
        }
    }
}
