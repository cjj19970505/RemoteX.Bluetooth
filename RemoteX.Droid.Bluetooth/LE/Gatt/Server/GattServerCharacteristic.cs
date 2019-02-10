using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using RemoteX.Bluetooth.LE.Gatt;
using RemoteX.Bluetooth.Droid;
using RemoteX.Bluetooth;
using RemoteX.Bluetooth.LE.Gatt.Server;

namespace RemoteX.Bluetooth.Droid.LE.Gatt.Server
{
    public partial class GattServer : IGattServer
    {

        public partial class GattServerService : IGattServerService
        {
            public partial class GattServerCharacteristic : IGattServerCharacteristic
            {
                public Android.Bluetooth.BluetoothGattCharacteristic DroidCharacteristic { get; private set; }

                private List<GattServerDescriptor> _Descritptor;

                public event EventHandler<ICharacteristicReadRequest> OnRead;
                public event EventHandler<ICharacteristicWriteRequest> OnWrite;

                public IGattServerDescriptor[] Descriptors
                {
                    get
                    {
                        return _Descritptor.ToArray();
                    }
                }

                public GattPermissions Permissions
                {
                    get
                    {
                        return DroidCharacteristic.Permissions.ToGattPermissions();
                    }
                }

                public GattCharacteristicProperties CharacteristicProperties => throw new NotImplementedException();

                public int CharacteristicValueHandle
                {
                    get
                    {
                        return DroidCharacteristic.InstanceId;
                    }
                }

                public Guid Uuid
                {
                    get
                    {
                        return Guid.Parse(DroidCharacteristic.Uuid.ToString());
                    }
                }

                public byte[] Value
                {
                    get
                    {
                        return DroidCharacteristic.GetValue();
                    }
                    set
                    {
                        DroidCharacteristic.SetValue(value);
                    }
                }

                /// <summary>
                /// Only avalible when added to a service
                /// </summary>
                public IGattServerService Service { get; private set; }

                public GattServerCharacteristic(Guid uuid, GattCharacteristicProperties properties, GattPermissions permission)
                {
                    DroidCharacteristic = new Android.Bluetooth.BluetoothGattCharacteristic(uuid.ToJavaUuid(), properties.ToDroidGattProperty(), permission.ToDroidGattPermission());
                    _Descritptor = new List<GattServerDescriptor>();
                }

                public void AddDescriptor(GattServerDescriptor descriptor)
                {
                    descriptor.AddToCharacteristic(this);
                }

                public void AddToService(GattServerService service)
                {
                    Service = service;
                    service.GattCharacteristics.Add(this);
                    service.DroidService.AddCharacteristic(DroidCharacteristic);
                }
                public void NotifyValueChanged(IBluetoothDevice bluetoothDevice, bool confirm)
                {
                    //DroidGattServer.NotifyCharacteristicChanged(_ConnectedDevice, _GattServices.GetFromUuid(BatteryService.BATTERY_SERVICE_UUID).GattCharacteristics.GetFromUuid(BatteryService.BatteryLevelCharacteristic.BATTERY_LEVEL_UUID).DroidCharacteristic, false);
                    (Service.Server as GattServer).DroidGattServer.NotifyCharacteristicChanged((bluetoothDevice as BluetoothManager.BluetoothDeviceWrapper).DroidDevice, DroidCharacteristic, confirm);
                }

                public virtual void OnCharacteristicRead(Android.Bluetooth.BluetoothDevice device, int requestId, int offset)
                {
                    CharacteristicReadRequest readRequest = new CharacteristicReadRequest
                    {
                        SourceDevice = BluetoothManager.BluetoothDeviceWrapper.GetBluetoothDeviceFromDroidDevice((Service.Server as GattServer).BluetoothManager, device),
                        TargetCharacteristic = this,
                        Offset = offset,
                        RequestId = requestId,
                    };
                    OnRead?.Invoke(this, readRequest);
                    //Service.Server.SendResponse(BluetoothManager.BluetoothDeviceWrapper.GetBluetoothDeviceFromDroidDevice((Service.Server as GattServer).BluetoothManager, device), requestId, null);
                }

                public virtual void OnCharacteristicWrite(Android.Bluetooth.BluetoothDevice droidDevice, int requestId, Android.Bluetooth.BluetoothGattCharacteristic characteristic, bool preparedWrite, bool responseNeeded, int offset, byte[] value)
                {
                    var device = BluetoothManager.BluetoothDeviceWrapper.GetBluetoothDeviceFromDroidDevice((Service.Server as GattServer).BluetoothManager, droidDevice);
                    var writeRequest = new CharacteristicWriteRequest
                    {
                        SourceDevice = device,
                        TargetCharacteristic = this,
                        Offset = offset,
                        ResponseNeeded = responseNeeded,
                        RequestId = requestId,
                        Value = value
                    };
                    OnWrite?.Invoke(this, writeRequest);
                }


            }
        }
    }
    
}