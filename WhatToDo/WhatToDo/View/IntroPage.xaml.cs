using System;
using WhatToDo.View;
using WhatToDo.Service.Constants;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WhatToDo.Service.Connection;
using WhatToDo.Model.Entity;
using WhatToDo.Service.Auxiliar;
using Windows.Devices.Geolocation;
using WhatToDo.Controller;

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
			AskLocationPermission();
            this.InitializeComponent();

        }

        private async void AskLocationPermission()
		{
			var uri = new Uri("ms-settings:privacy-location");
			var accessStatus = await Geolocator.RequestAccessAsync();

			return;
		}

		private void ButtonRegistrar_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PaginaRegistrar));
        }

        private void ButtonEntrar_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PaginaEntrar));

            /*//************************* NÃO ESQUECER DE TIRAR! *************************
            PaginaEntrarController PEC = new PaginaEntrarController();
            Frame.Navigate(typeof(MainPage), PEC.DataBaseGetUsuarioCaller("admin"));
            //************************* NÃO ESQUECER DE TIRAR! *************************
            */
        }
    }
}
