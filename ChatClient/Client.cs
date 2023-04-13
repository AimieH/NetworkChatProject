using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient
{
    public class Client
    {
        private UdpClient client = new();
        private IPEndPoint targetEndpoint = new(IPAddress.Parse("10.51.2.72"), 666);

        public void SendToServer(string message)
        {
            client.SendAsync(Encoding.UTF8.GetBytes(message), targetEndpoint);
        }
    }
}
