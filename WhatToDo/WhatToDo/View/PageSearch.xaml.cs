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
using Windows.UI.Xaml.Controls.Maps;
using Windows.Foundation;

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
            
			CreateAtividadesIcons();
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

        private async void CreateAtividadesIcons()
        {
			await GetLocation();

			foreach (var atividade in Atividades)
            {
                if (!Geo.checkInsideRadius(location, atividade.LocalGPS, 200))
                {
                    continue;
                }

				var icon = new MapIcon();

				var geoloc = atividade.LocalGPS.Split(' ');
				var latitude = double.Parse(geoloc[0]);
				var longitude = double.Parse(geoloc[1]);

				icon.Location = new Geopoint(new BasicGeoposition()
				{ Latitude = latitude, Longitude = longitude });

				icon.NormalizedAnchorPoint = new Point(0.5, 1.0);
				icon.Title = atividade.Nome;
				MyMap.MapElements.Add(icon);
            }
		}

		public void ShowHideIcons(object sender, RoutedEventArgs e)
		{
			if (MyMap == null)
				return;

			var atividadesCopy = new List<Atividade>(Atividades);

			foreach(var icon in MyMap.MapElements.OfType<MapIcon>())
			{
				var atividade = atividadesCopy.Find(obj => obj.Nome == icon.Title);
				icon.Visible = true;

				//-------------------
				// Filter by category
				//-------------------
				if (CBCategory.SelectedIndex > 0 &&
					atividade.IdCategoria != ((Categoria)CBCategory.SelectedItem).IdCategoria)
				{
					icon.Visible = false;
					atividadesCopy.Remove(atividade);
				}

				//-------------------
				// Filter by activity name
				//-------------------
				if (!String.IsNullOrWhiteSpace(TextName.Text) &&
					!atividade.Nome.ToUpper().Contains(TextName.Text.ToUpper()))
				{
					icon.Visible = false;
					atividadesCopy.Remove(atividade);
				}


				//-------------------
				// Filter by start date
				//-------------------
				if (checkbFromDate.IsChecked == true &&
					(atividade.Data < FromData.Date.DateTime))
				{
					icon.Visible = false;
					atividadesCopy.Remove(atividade);
				}

				//-------------------
				// Filter by end date
				//-------------------
				if (checkbToDate.IsChecked == true &&
					(atividade.Data > FromData.Date.DateTime))
				{
					icon.Visible = false;
					atividadesCopy.Remove(atividade);
				}

				//-------------------
				// Filter by radius
				//-------------------
				if (!Geo.checkInsideRadius(location, atividade.LocalGPS, SliderRaio.Value))
				{
					icon.Visible = false;
					atividadesCopy.Remove(atividade);
				}

			}
		}

		private async void MyMap_MapElementClick(MapControl sender, MapElementClickEventArgs args)
		{
			var icon = args.MapElements.OfType<MapIcon>().FirstOrDefault();
			if (icon == null)
				return;

			var local = icon.Location;

			var atividade = Atividades.First(i => i.Nome == icon.Title);
			var msg = new MessageDialog(atividade.Nome + "\n" + atividade.Descricao);
			await msg.ShowAsync();

			return;
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
			ShowHideIcons(null, null);
		}

	}
}
