using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls.Maps;
using WhatToDo.Model.Entity;
using WhatToDo.View;
using WhatToDo.Controller;
using Windows.UI.Popups;
using WhatToDo.Service.Auxiliar;
using System.Threading.Tasks;
using Windows.Services.Maps;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;
using Windows.UI;
using WhatToDo.Service.Constant;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WhatToDo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public Usuario User{ get; set; }
        private bool MenuOpened { get; set; }
        private List<Atividade> Atividades;
        private string location;
        private Geopoint atividadeGeopoint;

        public MainPage()
        {
			this.InitializeComponent();

			MainPageController mpc = new MainPageController();
            MyMap.Height = Window.Current.Bounds.Height;
            MyMap.Width = Window.Current.Bounds.Width - int.Parse(ColumnMenu.Width.ToString());

            Atividades = mpc.DataBaseGetAtividadeCaller();
            ShowIcons();

            MenuOpened = true;
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

        private Uri SelectIconImage(int idCategoria)
        {
            switch (idCategoria)
            {
                case 1:
                    return Icons.Esportes;
                case 3:
                    return Icons.Festas;
                default:
                    return null;
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            User = e.Parameter as Usuario;
            try
            {
                LabelUser.Text = User.Nome;
            }
            catch (Exception)
            {
                LabelUser.Text = User.Email;
            }
        }

        private void ButtonCreate_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PageCreate), User);
        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PageSearch), User);
        }

        private void ImageExpand_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (MenuOpened)
            {
                LabelUser.Visibility = Visibility.Collapsed;
                ButtonCreate.Visibility = Visibility.Collapsed;
                ButtonSearch.Visibility = Visibility.Collapsed;
                ButtonRefresh.Visibility = Visibility.Collapsed;
                ImageExpand.Text = ">";
                ColumnMenu.Width = new GridLength(60);
            }
            else
            {
                LabelUser.Visibility = Visibility.Visible;
                ButtonCreate.Visibility = Visibility.Visible;
                ButtonSearch.Visibility = Visibility.Visible;
                ButtonRefresh.Visibility = Visibility.Visible;
                ImageExpand.Text = "<<<";
                ColumnMenu.Width = new GridLength(200);
            }

            MyMap.Width = Window.Current.Bounds.Width - int.Parse(ColumnMenu.Width.ToString());
            MenuOpened = !MenuOpened;
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
                "Mostrar rota",
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

        private async void ButtonRefresh_Click(object sender, RoutedEventArgs e)
		{
			MainPageController MPC = new MainPageController();
			MyMap.Children.Clear();
			Atividades = MPC.DataBaseGetAtividadeCaller();
			await GetLocation();
			ShowIcons();

		}

		private async void ImageUser_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            FileOpenPicker flop = new FileOpenPicker();
            flop.ViewMode = PickerViewMode.Thumbnail;
            flop.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            flop.FileTypeFilter.Add(".jpg");
            flop.FileTypeFilter.Add(".jpeg");
            flop.FileTypeFilter.Add(".png");
            StorageFile file = await flop.PickSingleFileAsync();

            if (file != null)
            {
                using (IRandomAccessStream fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    await bitmapImage.SetSourceAsync(fileStream);
                    ImageUser.Source = bitmapImage;
					User.Foto = bitmapImage;

					MainPageController mpc = new MainPageController();
					mpc.DataBaseInsertUsuarioFotoCaller(User);
				}
            }

        }
	}
}
