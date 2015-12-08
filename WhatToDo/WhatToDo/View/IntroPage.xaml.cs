﻿using WhatToDo.View;
using WhatToDo.Service.Constants;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WhatToDo.Service.Connection;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WhatToDo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class IntroPage: Page
    {
        public IntroPage()
        {
            this.InitializeComponent();

			var usr = DatabaseConnection.GetUsuario("joe@bacon.pizza");
        }

        private void ButtonRegistrar_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PaginaRegistrar));
        }

        private void ButtonEntrar_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PaginaEntrar));
        }
    }
}
