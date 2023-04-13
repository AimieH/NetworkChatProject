using System.Net.Sockets;
using System.Net;
using System.Text;

namespace ChatClient;

public class Client
{
    public class Client
    {
        private UdpClient client = new();
        private IPEndPoint? targetEndpoint = null;

        private ClientForm form;

        public void SetForm(ClientForm _form)
        {
            form = _form;
        }
        
        public void ConnectToServer(string ipString)
        {
            if (IPAddress.TryParse(ipString, out var ipAddress))
            {
                targetEndpoint = new(ipAddress, 666);
                form.DisplayError("Connected to server :)))");
            }
            else
            {
                form.DisplayError("Failed to connect to server :( - 404 :(");
            }
        }

        public void SendToServer(string message)
        {
            client.SendAsync(Encoding.UTF8.GetBytes(message), targetEndpoint);
        }
    }
}