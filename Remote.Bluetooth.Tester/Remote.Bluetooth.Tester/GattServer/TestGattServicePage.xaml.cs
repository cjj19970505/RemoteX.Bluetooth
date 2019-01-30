﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Remote.Bluetooth.Tester.GattServer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TestGattServicePage : ContentPage
    {
        public GattServiceModel GattServiceModel { get; }
        public TestGattServiceWrapper TestGattServiceWrapper { get; }
        public TestGattServicePage(GattServiceModel gattServiceModel, TestGattServiceWrapper testGattServiceWrapper)
        {
            GattServiceModel = gattServiceModel;
            TestGattServiceWrapper = testGattServiceWrapper;
            BindingContext = GattServiceModel;
            InitializeComponent();
            RequestListView.ItemsSource = testGattServiceWrapper.GattRequestViewModels;
        }
    }
}