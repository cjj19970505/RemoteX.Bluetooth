using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using RemoteX.Bluetooth.LE.Gatt;
using RemoteX.Bluetooth;

namespace RemoteX.Droid.Bluetooth.LE.Gatt
{
    public partial class GattServer : IGattServer
    {
        public partial class GattServerService : IGattServerService
        {
            public partial class GattServerCharacteristic : IGattServerCharacteristic
            {
                public class GattServerDescriptor : IGattServerDescriptor
                {
                    public Guid Uuid
                    {
                        get
                        {
                            return DroidDescriptor.Uuid.ToGuid();
                        }
                    }

                    [Obsolete("Not Finished Yet")]
                    public GattPermissions Permissions
                    {
                        get
                        {
                            return new GattPermissions();
                        }
                    }

                    public Android.Bluetooth.BluetoothGattDescriptor DroidDescriptor { get; private set; }
                    public event EventHandler<DescriptorReadRequest> OnRead;
                    public event EventHandler<WriteRequest> OnWrite;

                    /// <summary>
                    /// Only avalivable when added to a Characteristic
                    /// </summary>
                    public IGattServerCharacteristic Characteristic { get; private set; }

                    public GattServerDescriptor(Guid uuid, GattPermissions permissions)
                    {
                        DroidDescriptor = new Android.Bluetooth.BluetoothGattDescriptor(uuid.ToJavaUuid(), permissions.ToDroidGattDescriptorPermission());
                    }

                    

                    public virtual void AddToCharacteristic(GattServerCharacteristic characteristic)
                    {
                        Characteristic = characteristic;
                        characteristic._Descritptor.Add(this);
                        characteristic.DroidCharacteristic.AddDescriptor(DroidDescriptor);
                    }

                    public virtual void OnWriteRequest(BluetoothDevice droidDevice, int requestId, bool preparedWrite, bool responseNeeded, int offset, byte[] value)
                    {
                        var bluetoothManager = (Characteristic.Service.Server as GattServer).BluetoothManager;
                        var device = BluetoothManager.BluetoothDeviceWrapper.GetBluetoothDeviceFromDroidDevice(bluetoothManager, droidDevice);
                        WriteRequest writeRequest = new WriteRequest
                        {
                            Device = device,
                            Offset = offset,
                            ResponseNeeded = responseNeeded,
                            RequestId = requestId,
                            Value = value
                        };
                        OnWrite?.Invoke(this, writeRequest);
                    }

                    public virtual void OnReadRequest(BluetoothDevice droidDevice, int requestId, int offset)
                    {
                        var bluetoothManager = (Characteristic.Service.Server as GattServer).BluetoothManager;
                        var device = BluetoothManager.BluetoothDeviceWrapper.GetBluetoothDeviceFromDroidDevice(bluetoothManager, droidDevice);
                        DescriptorReadRequest readRequest = new DescriptorReadRequest
                        {
                            Device = device,
                            RequestId = requestId,
                            TargetDescriptor = this,
                            Offset = offset,

                        };
                        OnRead?.Invoke(this, readRequest);
                    }

                    
                }
            }
            
        }
    }
}