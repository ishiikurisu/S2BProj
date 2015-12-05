using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using WhatToDo.Service.Constants;
using WhatToDo.View;
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
            DataBaseConstants.ConnectionTest();
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
