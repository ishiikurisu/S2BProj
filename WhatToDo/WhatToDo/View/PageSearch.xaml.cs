using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using WhatToDo.Model.Entity;
using Windows.UI.Popups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using WhatToDo.Controller;
using WhatToDo.Service.Auxiliar;
using Windows.UI.Xaml.Controls.Maps;
using Windows.Foundation;
using Windows.Services.Maps;
using Windows.Storage.Streams;
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
	    private Geopoint atividadeGeopoint;


        public PageSearch()
        {
            PageSearchController psc = new PageSearchController();
			Atividades = psc.DataBaseGetAtividadesCaller();

			this.InitializeComponent();

            MyMap.Height = Window.Current.Bounds.Height;
            MyMap.Width = Window.Current.Bounds.Width - int.Parse(ColumnMenu.Width.ToString());
            
			ShowIcons();
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

        private async void ShowIcons()
        {
            await GetLocation();

            ShowUserLocation();

            ShowAtividadesLocation();
        }

        private void ShowAtividadesLocation()
        {
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
                icon.Image = RandomAccessStreamReference.CreateFromUri(SelectIconImage(atividade.IdCategoria));
                MyMap.MapElements.Add(icon);
            }
        }

        private async void ShowUserLocation()
        {
            var icon = new MapIcon();
            var geoloc = location.Split(' ');
            var latitude = double.Parse(geoloc[0]);
            var longitude = double.Parse(geoloc[1]);

            Geolocator locator = new Geolocator();
            Geoposition pos = await locator.GetGeopositionAsync();
            icon.Location = new Geopoint(new BasicGeoposition()
            { Latitude = latitude, Longitude = longitude });
            icon.NormalizedAnchorPoint = new Point(0.5, 1.0);
            icon.Title = "Você está aqui.";
            icon.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/PinIcons/current_location_pin.png"));
            MyMap.MapElements.Add(icon);
            await MyMap.TrySetViewAsync(pos.Coordinate.Point, 15);
        }

	    private Uri SelectIconImage(int idCategoria)
	    {
	        switch (idCategoria)
	        {
                case 1:
	                return new Uri("ms-appx:///Assets/PinIcons/Esportes_pin.png");
                case 3:
	                return new Uri("ms-appx:///Assets/PinIcons/Festas_pin.png");
                default:
	                return null;
	        }
	    }

        public void ShowHideIcons(object sender, RoutedEventArgs e)
		{
			if (MyMap == null)
				return;

			var atividadesCopy = new List<Atividade>(Atividades);

			foreach(var icon in MyMap.MapElements.OfType<MapIcon>().Skip(1))
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

		private void MyMap_MapElementClick(MapControl sender, MapElementClickEventArgs args)
		{
			var icon = args.MapElements.OfType<MapIcon>().FirstOrDefault();
			if (icon == null)
				return;

			atividadeGeopoint = icon.Location;

			var atividade = Atividades.First(i => i.Nome == icon.Title);
            ShowAtividadeOptions(atividade);
        }

	    private async void ShowAtividadeOptions(Atividade atividade)
	    {
            var msg = new MessageDialog(atividade.Nome + "\n" + atividade.Local + "\n" + atividade.Descricao + "\n" + atividade.Data);
            msg.Commands.Add(new UICommand(
                "Criar rota",
                new UICommandInvokedHandler(this.CommandInvokedHandlerCriarRota)));
            msg.Commands.Add(new UICommand(
                "Cancelar"));
            msg.DefaultCommandIndex = 0;
            msg.CancelCommandIndex = 1;
            await msg.ShowAsync();
        }

        private async void CommandInvokedHandlerCriarRota(IUICommand command)
        {
            GetLocation();

            var geoloc = location.Split(' ');
            // Start at Microsoft in Redmond, Washington.
            BasicGeoposition startLocation = new BasicGeoposition();
            startLocation.Latitude = double.Parse(geoloc[0]);
            startLocation.Longitude = double.Parse(geoloc[1]);
            Geopoint startPoint = new Geopoint(startLocation);

            // End at the city of Seattle, Washington.
            BasicGeoposition endLocation = new BasicGeoposition();
            endLocation.Latitude = atividadeGeopoint.Position.Latitude;
            endLocation.Longitude = atividadeGeopoint.Position.Longitude;
            Geopoint endPoint = new Geopoint(endLocation);

            // Get the route between the points.
            MapRouteFinderResult routeResult =
                await MapRouteFinder.GetDrivingRouteAsync(
                startPoint,
                endPoint,
                MapRouteOptimization.Time,
                MapRouteRestrictions.None);
            if (routeResult.Status == MapRouteFinderStatus.Success)
            {
                MyMap.Routes.Clear();
                // Use the route to initialize a MapRouteView.
                MapRouteView viewOfRoute = new MapRouteView(routeResult.Route);
                viewOfRoute.RouteColor = Colors.DodgerBlue;
                viewOfRoute.OutlineColor = Colors.DodgerBlue;

                // Add the new MapRouteView to the Routes collection
                // of the MapControl.
                MyMap.Routes.Add(viewOfRoute);

                // Fit the MapControl to the route.
                await MyMap.TrySetViewBoundsAsync(
                    routeResult.Route.BoundingBox,
                    null,
                    Windows.UI.Xaml.Controls.Maps.MapAnimationKind.None);
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
			ShowHideIcons(null, null);
		}
	}
}
