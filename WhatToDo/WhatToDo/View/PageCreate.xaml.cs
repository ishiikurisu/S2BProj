using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using WhatToDo.Controller;

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

        public PageCreate()
        {
            this.InitializeComponent();
            this.PointerReleased += MyMap_PointerReleasedOverride;
            MenuOpened = true;
            MapMoved = false;
            MyMap.Height = Window.Current.Bounds.Height;
            MyMap.Width = Window.Current.Bounds.Width - int.Parse(ColumnMenu.Width.ToString());
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            User = e.Parameter as Usuario;
            List<Categoria> LCategoria = new PageCreateController().DataBaseCaller();
            foreach (var categoria in LCategoria)
            {
                CBCategoria.Items.Add(categoria.Nome);
            }
            // TODO:
            //
            // Adicionar a lista de nomes das categorias que são pegas do banco usando GetCategorias()
            // CBCategoria.ItemsSource = LISTA
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

        private void MyMap_PointerReleasedOverride(object sender, PointerRoutedEventArgs e)
        {
            if (!MapMoved)
            {
                AddPin(e);
            }
            MapMoved = false;
        }

        private void MyMap_ViewChanged(object sender, Bing.Maps.ViewChangedEventArgs e)
        {
            MapMoved = true;
        }

        private void AddPin(PointerRoutedEventArgs e)
        {
            Bing.Maps.Location l = new Bing.Maps.Location();
            MyMap.TryPixelToLocation(e.GetCurrentPoint(this.MyMap).Position, out l);
            Bing.Maps.Pushpin pushpin = new Bing.Maps.Pushpin();
            pushpin.SetValue(Bing.Maps.MapLayer.PositionProperty, l);
            TextLocal.Text = "" + l.Latitude.ToString() + " " + l.Longitude.ToString();
            MyMap.Children.Clear();
            MyMap.Children.Add(pushpin);
        }

        private void ButtonCreate_Click(object sender, RoutedEventArgs e)
        {
            PageCreateController PCC = new PageCreateController();
            // TODO
            // Pegar o id da categoria que está selecionada em CBCategoria
            List<Categoria> LCategoria = new PageCreateController().DataBaseCaller();
            int categoriaId = -1;
            foreach (var categoria in LCategoria)
            {
                if (categoria.Nome.Equals(CBCategoria.SelectedItem))
                {
                    categoriaId = categoria.IdCategoria;
                    break;
                }
            }
            PCC.DataBaseCaller(new Atividade(TextNome.Text, categoriaId, TextLocal.Text, "fut dos brothers", new DateTime()));
        }
    }
}
