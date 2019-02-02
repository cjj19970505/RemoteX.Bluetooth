using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteX.Bluetooth.LE.Gatt.Server
{


    public interface IGattServiceBuilder
    {
        GattServiceType ServiceType { get; set; }
        Guid Uuid { get; set; }
        
        IGattServiceBuilder SetServiceType(GattServiceType gattServiceType);
        IGattServiceBuilder SetUuid(Guid uuid);

        /// <summary>
        /// 这个的实现就直接调用下面的参数为IEnumerable版本就好了
        /// </summary>
        /// <param name="characteristics"></param>
        /// <returns></returns>
        IGattServiceBuilder AddCharacteristics(params IGattServerCharacteristic[] characteristics);
        IGattServiceBuilder AddCharacteristics(IEnumerable<IGattServerCharacteristic> characteristics);
        IGattServerService Build();
    }

    public interface IGattCharacteristicBuilder
    {
        GattCharacteristicProperties Properties { get; set; }
        Guid Uuid { get; set; }
        GattPermissions Permissions { get; }
        IGattCharacteristicBuilder SetPermissions(GattPermissions permissions);
        IGattCharacteristicBuilder SetUuid(Guid uuid);
        IGattCharacteristicBuilder SetProperties(GattCharacteristicProperties properties);
        IGattCharacteristicBuilder AddDescriptors(params IGattServerDescriptor[] gattDescriptors);
        IGattCharacteristicBuilder AddDescriptors(IEnumerable<IGattServerDescriptor> gattDescriptors);
        IGattServerCharacteristic Build();
    }

    public interface IGattDescriptorBuilder
    {
        Guid Uuid { get; set; }
        GattPermissions Permissions { get; set; }
        IGattDescriptorBuilder SetUuid(Guid uuid);
        IGattDescriptorBuilder SetPermissions(GattPermissions permissions);
        IGattServerDescriptor Build();
    }
}
