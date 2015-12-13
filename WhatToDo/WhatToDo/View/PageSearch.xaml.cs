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
using Windows.UI.Xaml.Media;
using Windows.UI;

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
			Atividades = psc.DataBaseGetAtividadesCaller();

			this.InitializeComponent();

            MyMap.Height = Window.Current.Bounds.Height;
            MyMap.Width = Window.Current.Bounds.Width - int.Parse(ColumnMenu.Width.ToString());
            
			CreateEventosPushpins();
            MenuOpened = true;
        }

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			User = e.Parameter as Usuario;
			LCategorias = new PageCreateController().DataBaseGetCategoriasCaller();

			Categoria emptyCategoria = new Categoria();
			emptyCategoria.IdCategoria = -1;
			emptyCategoria.Nome = "";
			LCategorias.Insert(0, emptyCategoria);

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

        private async void CreateEventosPushpins()
        {
			await GetLocation();

			var pushpin = new Pushpin();
			var geoloc = location.Split(' ');
			var latitude = double.Parse(geoloc[0]);
			var longitude = double.Parse(geoloc[1]);
			var l = new Location(latitude, longitude);

			pushpin.SetValue(MapLayer.PositionProperty, l);
			pushpin.Tag = "user";
			pushpin.Background = new SolidColorBrush(Colors.Red);
			MyMap.Children.Add(pushpin);

			foreach (var atividade in Atividades)
            {
                if (!Geo.checkInsideRadius(location, atividade.LocalGPS, 200))
                {
                    continue;
                }

                geoloc = atividade.LocalGPS.Split(' ');
                latitude = double.Parse(geoloc[0]);
                longitude = double.Parse(geoloc[1]);
                l = new Location(latitude, longitude);
                pushpin = new Pushpin();
                pushpin.SetValue(MapLayer.PositionProperty, l);
                pushpin.Tag = atividade.IdAtividade;
                pushpin.PointerPressed += Pushpin_PointerPressedOverride;
                MyMap.Children.Add(pushpin);
            }


		}

		public void ShowHidePushpins(object sender, RoutedEventArgs e)
		{
			if (MyMap == null)
				return;

			var atividadesCopy = new List<Atividade>(Atividades);

			foreach (var pushpin in MyMap.Children.OfType<Pushpin>().Skip(1))
			{
				var atividade = atividadesCopy.Find(obj => obj.IdAtividade == (int)pushpin.Tag);
				pushpin.Visibility = Visibility.Visible;

				//-------------------
				// Filter by category
				//-------------------
				if(CBCategory.SelectedIndex > 0 &&
					atividade.IdCategoria != ((Categoria)CBCategory.SelectedItem).IdCategoria)
				{
					pushpin.Visibility = Visibility.Collapsed;
					atividadesCopy.Remove(atividade);
				}

				//-------------------
				// Filter by activity name
				//-------------------
				if (!String.IsNullOrWhiteSpace(TextName.Text) &&
					! atividade.Nome.ToUpper().Contains(TextName.Text.ToUpper()))
                {
					pushpin.Visibility = Visibility.Collapsed;
					atividadesCopy.Remove(atividade);
				}


				//-------------------
				// Filter by start date
				//-------------------
				if (checkbFromDate.IsChecked == true &&
                    (atividade.Data < FromData.Date.DateTime))					
				{
					pushpin.Visibility = Visibility.Collapsed;
					atividadesCopy.Remove(atividade);
				}

				//-------------------
				// Filter by end date
				//-------------------
				if (checkbToDate.IsChecked == true &&
					(atividade.Data > FromData.Date.DateTime))
				{
					pushpin.Visibility = Visibility.Collapsed;
					atividadesCopy.Remove(atividade);
				}

				//-------------------
				// Filter by radius
				//-------------------
				if(! Geo.checkInsideRadius(location, atividade.LocalGPS, SliderRaio.Value))
                {
					pushpin.Visibility = Visibility.Collapsed;
					atividadesCopy.Remove(atividade);
				}


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
                    var msg = new MessageDialog("Nome: " + atividade.Nome + "\n(" + atividade.IdCategoria + ")\n" + atividade.Descricao + "\n" + atividade.Data);
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

		private void DateChanged_Event(object sender, DatePickerValueChangedEventArgs e)
		{
			ShowHidePushpins(null, null);
		}

	}
}
