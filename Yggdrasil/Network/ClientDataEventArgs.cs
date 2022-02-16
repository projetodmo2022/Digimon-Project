using System.Collections.Generic;

namespace Yggdrasil.Network
{
    /// <summary>
    /// Description of ConnectionDataEventArgs.
    /// </summary>
    public sealed class ClientDataEventArgs : ClientEventArgs
    {
        private byte[] _Data = null;
        public byte[] Data
        {
            get { return _Data; }
            private set { _Data = value; }
        }

        public ClientDataEventArgs(IClient client, byte[] data)
            : base(client)
        {
            this.Data = data ?? new byte[0];
        }

        public override string ToString()
        {
            return Client.RemoteEndPoint != null
                ? string.Format("{0}: {1} bytes", Client.RemoteEndPoint, Data.Length)
                : string.Format("Not Connected: {0} bytes", Data.Length);
        }
    }
}