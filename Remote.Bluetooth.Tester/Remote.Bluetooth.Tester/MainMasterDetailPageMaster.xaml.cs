using Remote.Bluetooth.Tester.GattClient;
using Remote.Bluetooth.Tester.GattServer;
using Remote.Bluetooth.Tester.RfcommClient;
using Remote.Bluetooth.Tester.RfcommServer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Remote.Bluetooth.Tester
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMasterDetailPageMaster : ContentPage
    {
        public ListView ListView;

        public MainMasterDetailPageMaster()
        {
            InitializeComponent();

            BindingContext = new MainMasterDetailPageMasterViewModel();
            ListView = MenuItemsListView;
        }

        class MainMasterDetailPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MainMasterDetailPageMenuItem> MenuItems { get; set; }
            
            public MainMasterDetailPageMasterViewModel()
            {
                MenuItems = new ObservableCollection<MainMasterDetailPageMenuItem>(new[]
                {
                    new MainMasterDetailPageMenuItem { Id = 0, Title = "Server", TargetType = typeof(ServiceListPage) },
                    new MainMasterDetailPageMenuItem { Id = 1, Title = "Client", TargetType = typeof(GattLEDeviceListPage)},
                    new MainMasterDetailPageMenuItem{Id = 2, Title="Rfcomm Client", TargetType=typeof(RfcommDeviceListPage)},
                    new MainMasterDetailPageMenuItem{Id = 2, Title="Rfcomm Server", TargetType=typeof(RfcommServerPage)}
                });
            }
            
            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}