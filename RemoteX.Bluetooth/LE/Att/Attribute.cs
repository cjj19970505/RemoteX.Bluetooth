using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteX.Bluetooth.LE.Att
{
    public enum AttAccessPermission { Readable, Writable, ReadableAndWritable}
    public enum AttEncryptionPermission { EncryptionRequired, NoEncryptionRequired }
    public enum AttAuthenticationPermission { AuthenticationRequired, NoAuthenticationRequired }
    public enum AttAuthorizationPermission { AuthorizationRequired, NoAuthorizationRequired }
    public struct AttPermission
    {

    }

    
}
