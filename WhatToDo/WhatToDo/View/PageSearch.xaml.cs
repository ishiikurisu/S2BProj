using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using WhatToDo.Model.Entity;
using Windows.UI.Popups;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private List<Categoria> LCategorias; 
        public PageSearch()
        {
            PageSearchController psc = new PageSearchController();

            this.InitializeComponent();

            MyMap.Height = Window.Current.Bounds.Height;
            MyMap.Width = Window.Current.Bounds.Width - int.Parse(ColumnMenu.Width.ToString());
            Atividades = psc.DataBaseGetAtividadesCaller();
            ShowEventos();
            MenuOpened = true;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            User = e.Parameter as Usuario;
            LCategorias = new PageCreateController().DataBaseGetCategoriasCaller();
            CBCategory.ItemsSource = LCategorias;
            CBCategory.SelectedItem = 0;
        }

        // Get current user location
        // This method will update the location string variable 
        // Format: "lat lon"
        private async Task GetLocation()
        {
            Geolocator locator = new Geolocator();
            Geoposition pos = await locator.GetGeopositionAsync();

            var coord = pos.Coordinate.Point;

            location = coord.Position.Latitude.ToString() + " " + coord.Position.Longitude.ToString();
        }

        private async void ShowEventos()
        {
			await GetLocation();
			foreach (var atividade in Atividades)
            {
                if (!Geo.checkInsideRadius(location, atividade.LocalGPS, 200))
                {
                    continue;
                }

                var geoloc = atividade.LocalGPS.Split(' ');
                var latitude = double.Parse(geoloc[0]);
                var longitude = double.Parse(geoloc[1]);
                var l = new Location(latitude, longitude);
                var pushpin = new Pushpin();
                pushpin.SetValue(MapLayer.PositionProperty, l);
                pushpin.Tag = atividade.Nome;
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
                    var msg = new MessageDialog(atividade.Nome + "\n" + atividade.Descricao + "\n" + atividade.Data);
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

        private void TextName_LostFocus(object sender, RoutedEventArgs e)
        {
            //TODO
            //Implement search paranmeters
            if (TextName.Text.Trim().Equals(""))
            {
                return;
            }
            string nomecheck = TextName.Text;
            foreach (var atividade in Atividades)
            {
                if (!atividade.Nome.ToUpper().Contains(nomecheck.ToUpper()))
                {
                    var ps = from p in MyMap.Children
                             where ((string)((Pushpin)p).Tag) == atividade.Nome
                             select p;
                    var psa = ps.ToArray();
                    for (int i = 0; i < psa.Count(); i++)
                    {
                        MyMap.Children.Remove(psa[i]);
                    }
                }
            }
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
            foreach (var atividade in Atividades)
            {
                if ((FromData.Date.DateTime.CompareTo(atividade.Data) < 1) && (ToData.Date.DateTime.CompareTo(atividade.Data) > -1))
                {
                    var ps = from p in MyMap.Children
                             where ((string)((Pushpin)p).Tag) == atividade.Nome
                             select p;
                    var psa = ps.ToArray();
                    for (int i = 0; i < psa.Count(); i++)
                    {
                        MyMap.Children.Remove(psa[i]);
                    }
                }
            }
        }

        private void SliderRaio_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            //TODO
            //Implement search paranmeters
        }

        private void CBCategory_LostFocus(object sender, RoutedEventArgs e)
        {
            //TODO
            //Implement search paranmeters
            Categoria categoriacheck = CBCategory.SelectedItem as Categoria;
            foreach (var atividade in Atividades)
            {
                if (atividade.IdCategoria != categoriacheck.IdCategoria)
                {
                    var ps = from p in MyMap.Children
                             where ((string)((Pushpin)p).Tag) == atividade.Nome
                             select p;
                    var psa = ps.ToArray();
                    for (int i = 0; i < psa.Count(); i++)
                    {
                        MyMap.Children.Remove(psa[i]);
                    }
                }
            }
        }
    }
}
