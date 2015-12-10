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

        public MainPage()
        {
            MainPageController MPC = new MainPageController();

            this.InitializeComponent();
            MyMap.Height = Window.Current.Bounds.Height;
            MyMap.Width = Window.Current.Bounds.Width - int.Parse(ColumnMenu.Width.ToString());
            Atividades = MPC.DataBaseCaller();
            ShowEventos();
            MenuOpened = true;
        }

        private void ShowEventos()
        {
            string [] geoloc;
            double latitude;
            double longitude;

            Bing.Maps.Location l;
            Bing.Maps.Pushpin pushpin;

            foreach (var atividade in Atividades)
            {
                geoloc = atividade.LocalGPS.Split(' ');
                latitude = double.Parse(geoloc[0]);
                longitude = double.Parse(geoloc[1]);
                l = new Bing.Maps.Location(latitude, longitude);
                //MyMap.TryPixelToLocation(e.GetCurrentPoint(this.MyMap).Position, out l);
                pushpin = new Bing.Maps.Pushpin();
                pushpin.SetValue(Bing.Maps.MapLayer.PositionProperty, l);
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
                /* it never shows the user's name. why? */
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
                ImageExpand.Text = ">";
                ColumnMenu.Width = new GridLength(60);
            }
            else
            {
                LabelUser.Visibility = Visibility.Visible;
                ButtonCreate.Visibility = Visibility.Visible;
                ButtonSearch.Visibility = Visibility.Visible;
                ImageExpand.Text = "<<<";
                ColumnMenu.Width = new GridLength(200);
            }

            MyMap.Width = Window.Current.Bounds.Width - int.Parse(ColumnMenu.Width.ToString());
            MenuOpened = !MenuOpened;
        }

        async private void Pushpin_PointerPressedOverride(object sender, PointerRoutedEventArgs e)
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
            Atividades = MPC.DataBaseCaller();
            ShowEventos();
        }
    }
}
