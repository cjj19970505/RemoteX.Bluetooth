using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RemoteX.Bluetooth.Rfcomm
{
    public class RfcommFixedLengthConnectionHandler
    {
        public event EventHandler<byte[]> OnReceived;
        public event EventHandler OnDisconnected;
        public IRfcommConnection Connection { get; }
        public int SendLength { get; }
        public int ReceiveLength { get; }

        Task ReceiveTask;
        public RfcommFixedLengthConnectionHandler(IRfcommConnection connection, int sendLength, int receiveLength)
        {
            Connection = connection;
            SendLength = sendLength;
            ReceiveLength = receiveLength;
            ReceiveTask = RunReceiveTask();
        }

        public RfcommFixedLengthConnectionHandler(IRfcommConnection connection, int sendAndReceiveLength):this(connection, sendAndReceiveLength, sendAndReceiveLength)
        {

        }

        public Task RunReceiveTask()
        {
            return Task.Run(() =>
            {
                while(true)
                {
                    byte[] readBuffer = new byte[ReceiveLength];
                    var actualReadSize = Connection.InputStream.Read(readBuffer, 0, readBuffer.Length);
                    if(actualReadSize < ReceiveLength)
                    {
                        throw new Exception("Receive size error");
                    }
                    if(actualReadSize == 0)
                    {
                        Connection.Dispose();
                        OnDisconnected?.Invoke(this, null);
                        return;
                    }
                    OnReceived?.Invoke(this, readBuffer);
                }
            });
        }

        public async Task SendAsync(byte[] sendBytes)
        {
            await Connection.InputStream.WriteAsync(sendBytes, 0, SendLength);
            await Connection.InputStream.FlushAsync();
        }

    }
}
