﻿using System;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WhatToDo.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PageCreate : Page
    {
        public PageCreate()
        {
            this.InitializeComponent();
            this.MyMap.PointerPressedOverride += MyMap_PointerPressedOverride;
        }

        void MyMap_PointerPressedOverride(object sender, PointerRoutedEventArgs e)
        {
            Bing.Maps.Location l = new Bing.Maps.Location();
            this.MyMap.TryPixelToLocation(e.GetCurrentPoint(this.MyMap).Position, out l);
            Bing.Maps.Pushpin pushpin = new Bing.Maps.Pushpin();
            pushpin.SetValue(Bing.Maps.MapLayer.PositionProperty, l);
            this.MyMap.Children.Add(pushpin);
        }

    }
}
