using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteX.Bluetooth
{
    public class BluetoothUtils
    {
        public const string UUID_LONG_STYLE_PREFIX = "0000";
        public const string UUID_LONG_STYLE_POSTFIX = "-0000-1000-8000-00805F9B34FB";

        public static Guid ShortValueUuid(int uuidShortValue)
        {
            return Guid.Parse(UUID_LONG_STYLE_PREFIX + string.Format("{0:x4}", uuidShortValue & 0xffff) + UUID_LONG_STYLE_POSTFIX);
        }

        public static ulong AddressStringToInt64(string sAddress)
        {
            return Convert.ToUInt64(sAddress.Replace(":", ""), 16);
        }

        public static string AddressInt64ToString(ulong uAddress)
        {
            byte[] decbyte = BitConverter.GetBytes(uAddress);
            string ans = "";
            for (int i = 0; i < decbyte.Length - 2; i++)
            {
                //ans += Convert.ToString(decbyte[decbyte.Length - 3 - i], 16);
                ans += string.Format("{0:X2}", decbyte[decbyte.Length - 3 - i]);
            }
            for (int j = 2; j <= 14; j += 2)
            {
                ans = ans.Insert(j, ":");
                j++;
            }
            ans = ans.ToUpper();
            return ans;
        }

    }
}
