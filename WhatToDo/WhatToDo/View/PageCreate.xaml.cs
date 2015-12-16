using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using WhatToDo.Model.Entity;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using WhatToDo.Controller;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Input;
using WhatToDo.Service.Constant;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WhatToDo.View
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class PageCreate : Page
    {
        private Usuario User { get; set; }
        private bool MenuOpened { get; set; }
        private bool MapMoved { get; set; }
		private string localGps = "";
	    private string location;
		private MapIcon newIcon = new MapIcon();

        public PageCreate()
        {
            this.InitializeComponent();
            MenuOpened = true;
            MapMoved = false;
            MyMap.Height = Window.Current.Bounds.Height;
            MyMap.Width = Window.Current.Bounds.Width - int.Parse(ColumnMenu.ActualWidth.ToString());

            ShowUserLocation();

			ButtonCreate.IsEnabled = false;
		}

	    private async void ShowUserLocation()
	    {
            await GetLocation();

            var icon = new MapIcon();
            var geoloc = location.Split(' ');
            var latitude = double.Parse(geoloc[0]);
            var longitude = double.Parse(geoloc[1]);

            Geolocator locator = new Geolocator();
            Geoposition pos = await locator.GetGeopositionAsync();

            icon.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/PinIcons/current_location_pin.png"));
            icon.Location = new Geopoint(new BasicGeoposition()
            { Latitude = latitude, Longitude = longitude });
            icon.NormalizedAnchorPoint = new Point(0.5, 1.0);
            icon.Title = "Você está aqui.";
            MyMap.MapElements.Add(icon);

            await MyMap.TrySetViewAsync(pos.Coordinate.Point, 15);
        }

        private async Task GetLocation()
        {
            Geolocator locator = new Geolocator();
            Geoposition pos = await locator.GetGeopositionAsync();

            var coord = pos.Coordinate.Point;

            location = coord.Position.Latitude.ToString() + " " + coord.Position.Longitude.ToString();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            User = e.Parameter as Usuario;
            List<Categoria> LCategoria = new PageCreateController().DataBaseGetCategoriasCaller();
			CBCategoria.ItemsSource = LCategoria;
			CBCategoria.SelectedIndex = 0;
        }

		private void ButtonReturn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage), User);
        }

        private void ButtonCollapse_Click(object sender, RoutedEventArgs e)
        {
            if (MenuOpened)
            {
                ButtonReturn.Visibility = Visibility.Collapsed;
                ButtonCreate.Visibility = Visibility.Collapsed;
                LabelNome.Visibility = Visibility.Collapsed;
                TextNome.Visibility = Visibility.Collapsed;
                LabelCategoria.Visibility = Visibility.Collapsed;
                CBCategoria.Visibility = Visibility.Collapsed;
                LabelLocal.Visibility = Visibility.Collapsed;
                TextLocal.Visibility = Visibility.Collapsed;
                LabelData.Visibility = Visibility.Collapsed;
                PickerDate.Visibility = Visibility.Collapsed;
                PickerTime.Visibility = Visibility.Collapsed;
                LabelDescricao.Visibility = Visibility.Collapsed;
                TextDescricao.Visibility = Visibility.Collapsed;

                ButtonCollapse.Content = ">";
                ColumnMenu.Width = new GridLength(60);
            }
            else
            {
                ButtonReturn.Visibility = Visibility.Visible;
                ButtonCreate.Visibility = Visibility.Visible;
                LabelNome.Visibility = Visibility.Visible;
                TextNome.Visibility = Visibility.Visible;
                LabelCategoria.Visibility = Visibility.Visible;
                CBCategoria.Visibility = Visibility.Visible;
                LabelLocal.Visibility = Visibility.Visible;
                TextLocal.Visibility = Visibility.Visible;
                LabelData.Visibility = Visibility.Visible;
                PickerDate.Visibility = Visibility.Visible;
                PickerTime.Visibility = Visibility.Visible;
                LabelDescricao.Visibility = Visibility.Visible;
                TextDescricao.Visibility = Visibility.Visible;

                ButtonCollapse.Content = "<<<";
                ColumnMenu.Width = new GridLength(200);
            }

            MyMap.Width = Window.Current.Bounds.Width - int.Parse(ColumnMenu.Width.ToString());
            MenuOpened = !MenuOpened;
        }

        private void ButtonCreate_Click(object sender, RoutedEventArgs e)
        {
            PageCreateController PCC = new PageCreateController();

			var categoria = (Categoria)CBCategoria.SelectedItem;
			DateTime date = PickerDate.Date.Date.Add(PickerTime.Time);

			PCC.DataBaseInsertAtividadeCaller(new Atividade(TextNome.Text, categoria.IdCategoria, localGps, TextLocal.Text, TextDescricao.Text, date));
            Frame.Navigate(typeof(MainPage), User);
        }

		private void MyMap_MapTapped(MapControl sender, MapInputEventArgs args)
		{
			var location = args.Location;
            var categoria = (Categoria)CBCategoria.SelectedItem;

            newIcon.Location = location;
			newIcon.NormalizedAnchorPoint = new Point(0.5, 1.0);

			localGps = location.Position.Latitude.ToString() + " " + location.Position.Longitude.ToString();
            newIcon.Image = RandomAccessStreamReference.CreateFromUri(SelectIconImage(categoria.IdCategoria));
            MyMap.MapElements.Remove(newIcon);
			MyMap.MapElements.Add(newIcon);

			ButtonCreate.IsEnabled = true;
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

        private void MyMap_MapDoubleTapped(MapControl sender, MapInputEventArgs args)
		{
			return;
		}

        private async void FocousOnCurrentLocation(object sender, RoutedEventArgs e)
        {
            foreach (var icon in MyMap.MapElements.OfType<MapIcon>())
            {
                if (icon.Title == "Você está aqui.")
                {
                    MyMap.MapElements.Remove(icon);
                    break;
                }
            }
            await GetLocation();
            ShowUserLocation();
        }
    }
}
