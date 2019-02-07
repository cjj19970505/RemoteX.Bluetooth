using RemoteX.Bluetooth.LE.Att;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RemoteX.Bluetooth.LE.Gatt.Client
{
    public struct ReadCharacteristicValueResult
    {
        public byte[] Value;
    }

    

    public interface IGattClient
    {
        /// <summary>
        /// ConnectedToGattServer
        /// </summary>
        /// <returns></returns>
        Task ConnectToServerAsync();

        /// <summary>
        /// Core V5.0
        /// 4.4.1 Discover All Primary Services
        /// </summary>
        /// <returns></returns>
        Task<GattServiceResult> DiscoverAllPrimaryServiceAsync();
    }

    public interface IGattClientService : IGattService, IAttribute
    {
        /// <summary>
        /// Core v5.0
        /// 4.6.1 Discover All Characteristics of a Service
        /// </summary>
        /// <returns></returns>
        Task<GattCharacteristicsResult> DiscoverAllCharacteristicsAsync();
    }

    public interface IGattClientCharacteristic : IGattCharacteristic, IAttribute
    {
        IGattClientService Service { get; }

        /// <summary>
        /// Core v5.0
        /// 4.10.1 Notification
        /// </summary>
        event EventHandler<byte[]> OnNotified;

        /// <summary>
        /// Core v5.0
        /// 4.8.1 Read Characteristic Value
        /// </summary>
        /// <returns></returns>
        Task<ReadCharacteristicValueResult> ReadCharacteristicValueAsync();

        byte[] LatestValue { get; }
    }

    
}
