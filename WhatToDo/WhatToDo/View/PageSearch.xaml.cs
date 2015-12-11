using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WhatToDo.Model.Entity;
using Windows.UI.Popups;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Bing.Maps;
using WhatToDo.Controller;
using WhatToDo.Service.Auxiliar;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WhatToDo.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PageSearch : Page
    {
        private bool MenuOpened { get; set; }
        private Usuario User { get; set; }
        private List<Atividade> Atividades;
        private string location;
        public PageSearch()
        {
            GetLocation();

            PageSearchController psc = new PageSearchController();

            this.InitializeComponent();
            MyMap.Height = Window.Current.Bounds.Height;
            MyMap.Width = Window.Current.Bounds.Width - int.Parse(ColumnMenu.Width.ToString());
            Atividades = psc.DataBaseCaller();
            ShowEventos();
            MenuOpened = true;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            User = e.Parameter as Usuario;
        }

        private async Task GetLocation()
        {
            Geolocator locator = new Geolocator();
            Geoposition pos = await locator.GetGeopositionAsync();

            var coord = pos.Coordinate.Point;

            location = coord.Position.Latitude.ToString() + " " + coord.Position.Longitude.ToString();
        }

        private void ShowEventos()
        {
            foreach (var atividade in Atividades)
            {
                GetLocation();
                if (!Geo.checkInsideRadius(location, atividade.LocalGPS, 200))
                {
                    continue;
                }
                var geoloc = atividade.LocalGPS.Split(' ');
                var latitude = double.Parse(geoloc[0]);
                var longitude = double.Parse(geoloc[1]);
                var l = new Location(latitude, longitude);
                var pushpin = new Pushpin();
                pushpin.SetValue(Bing.Maps.MapLayer.PositionProperty, l);
                pushpin.PointerPressed += Pushpin_PointerPressedOverride;
                MyMap.Children.Add(pushpin);
            }
        }

        private async void Pushpin_PointerPressedOverride(object sender, PointerRoutedEventArgs e)
        {
            Pushpin pushpin = sender as Pushpin;
            Location local = pushpin.GetValue(MapLayer.PositionProperty) as Location;

            foreach (var atividade in Atividades)
            {
                string[] geoloc = atividade.LocalGPS.Split(' ');
                double latitude = double.Parse(geoloc[0]);
                double longitude = double.Parse(geoloc[1]);

                if (latitude == local.Latitude && longitude == local.Longitude)
                {
                    var msg = new MessageDialog(atividade.Nome + "\n" + atividade.Descricao);
                    await msg.ShowAsync();
                    break;
                }
            }
        }

        private void ButtonCollapse_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (MenuOpened)
            {
                ButtonReturn.Visibility = Visibility.Collapsed;
                ButtonCollapse.Content = ">";
                PanelOptions.Visibility = Visibility.Collapsed;
                ColumnMenu.Width = new GridLength(60);
            }
            else
            {
                ButtonReturn.Visibility = Visibility.Visible;
                PanelOptions.Visibility = Visibility.Visible;
                ButtonCollapse.Content = "<<<";
                ColumnMenu.Width = new GridLength(200);
            }

            MyMap.Width = Window.Current.Bounds.Width - int.Parse(ColumnMenu.Width.ToString());
            MenuOpened = !MenuOpened;
        }

        private void ButtonReturn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage), User);
        }

        private async void TextCategory_LostFocus(object sender, RoutedEventArgs e)
        {
            //TODO
            //Implement search paranmeters
        }

        private void TextName_LostFocus(object sender, RoutedEventArgs e)
        {
            //TODO
            //Implement search paranmeters
        }

        private void FromData_LostFocus(object sender, RoutedEventArgs e)
        {
            //TODO
            //Implement search paranmeters
        }

        private void ToData_LostFocus(object sender, RoutedEventArgs e)
        {
            //TODO
            //Implement search paranmeters
        }

        private void SliderRaio_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            //TODO
            //Implement search paranmeters
        }
    }
}
