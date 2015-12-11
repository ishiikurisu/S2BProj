using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Shapes;
using Bing.Maps;
using WhatToDo.Model.Entity;
using WhatToDo.View;
using WhatToDo.Controller;
using Windows.Devices.Input;
using Windows.UI.Popups;
using WhatToDo.Service.Auxiliar;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;

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

        public MainPage()
        {
            GetLocation();
			this.InitializeComponent();

			MainPageController mpc = new MainPageController();
            MyMap.Height = Window.Current.Bounds.Height;
            MyMap.Width = Window.Current.Bounds.Width - int.Parse(ColumnMenu.Width.ToString());

            Atividades = mpc.DataBaseGetAtividadeCaller();
            ShowEventos();

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

        private async void ShowEventos()
        {
            foreach (var atividade in Atividades)
            {
                await GetLocation();
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
                //pushpin.SetValue(Bing.Maps.MapLayer.StyleProperty, "icon:https://40.media.tumblr.com/9a64d09c9af0f4e50133b63651170417/tumblr_nz7rlzmiLE1ttpfgfo1_75sq.png");
                pushpin.PointerPressed += Pushpin_PointerPressedOverride;
                MyMap.Children.Add(pushpin);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            User = e.Parameter as Usuario;
            try
            {
                LabelUser.Text = User.Nome;
            }
            catch (Exception any)
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

        private void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            MainPageController MPC = new MainPageController();
            MyMap.Children.Clear();
            Atividades = MPC.DataBaseGetAtividadeCaller();
            GetLocation();
            ShowEventos();
            
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
                }
            }

        }
    }
}
