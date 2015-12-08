using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using WhatToDo.Model.Entity;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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
        public PageCreate()
        {
            this.InitializeComponent();
            MenuOpened = true;
            MyMap.Height = Window.Current.Bounds.Height;
            MyMap.Width = Window.Current.Bounds.Width - int.Parse(ColumnMenu.Width.ToString());
        }

        async void MyMap_PointerPressedOverride(object sender, PointerRoutedEventArgs e)
        {
            Bing.Maps.Location l = new Bing.Maps.Location();
            this.MyMap.TryPixelToLocation(e.GetCurrentPoint(this.MyMap).Position, out l);
            Bing.Maps.Pushpin pushpin = new Bing.Maps.Pushpin();
            pushpin.SetValue(Bing.Maps.MapLayer.PositionProperty, l);
            var msg = new MessageDialog("" + l.Latitude.ToString() + " " + l.Longitude.ToString());
            await msg.ShowAsync();
            this.MyMap.Children.Add(pushpin);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            User = e.Parameter as Usuario;
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
                LabelLocal.Visibility = Visibility.Collapsed;
                TextLocal.Visibility = Visibility.Collapsed;
                ButtonCollapse.Content = ">";
                ColumnMenu.Width = new GridLength(60);
            }
            else
            {
                ButtonReturn.Visibility = Visibility.Visible;
                ButtonCreate.Visibility = Visibility.Visible;
                LabelNome.Visibility = Visibility.Visible;
                TextNome.Visibility = Visibility.Visible;
                LabelLocal.Visibility = Visibility.Visible;
                TextLocal.Visibility = Visibility.Visible;
                ButtonCollapse.Content = "<<<";
                ColumnMenu.Width = new GridLength(200);
            }

            MyMap.Width = Window.Current.Bounds.Width - int.Parse(ColumnMenu.Width.ToString());
            MenuOpened = !MenuOpened;
        }
    }
}
