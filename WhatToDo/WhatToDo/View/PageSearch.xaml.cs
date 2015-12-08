using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WhatToDo.Model.Entity;

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
        public PageSearch()
        {
            this.InitializeComponent();
            MenuOpened = true;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            User = e.Parameter as Usuario;
        }

        private void ButtonCollapse_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (MenuOpened)
            {
                ButtonReturn.Visibility = Visibility.Collapsed;
                ButtonSearch.Visibility = Visibility.Collapsed;
                TextQuery.Visibility = Visibility.Collapsed;
                TextResults.Visibility = Visibility.Collapsed;
                ButtonCollapse.Content = ">";
                ColumnMenu.Width = new GridLength(40);
            }
            else
            {
                ButtonReturn.Visibility = Visibility.Visible;
                ButtonSearch.Visibility = Visibility.Visible;
                TextQuery.Visibility = Visibility.Visible;
                TextResults.Visibility = Visibility.Visible;
                ButtonCollapse.Content = "<<<";
                ColumnMenu.Width = new GridLength(200);
            }

            MenuOpened = !MenuOpened;
        }

        private void ButtonReturn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage), User);
        }
    }
}
