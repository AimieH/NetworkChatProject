using System.Net.Sockets;
using System.Net;
using System.Text;

namespace ChatClient;

public class Client
{
    private UdpClient client = new();
    private IPEndPoint? targetEndpoint;

    private ClientForm form;
    private bool receiving = true;

    public Client(ClientForm form)
    {
        this.form = form;
    }
    
    public void ConnectToServer(string ipString)
    {
        if (IPAddress.TryParse(ipString, out var ipAddress))
        {
            targetEndpoint = new IPEndPoint(ipAddress, 666);
            form.DisplayNotification("Connected to server :)))", NotificationType.Success);
            StartReceiving();
        }
        else
        {
            form.DisplayNotification("Failed to connect to server :( - 404 :(", NotificationType.Error);
        }
    }

    public void SendToServer(string message)
    {
        client.SendAsync(Encoding.UTF8.GetBytes(message), targetEndpoint);
    }

    private async void StartReceiving()
    {
        form.DisplayNotification("Receiving started...", NotificationType.Hint);
        
        while (receiving)
        {
            var receiveResult = await client.ReceiveAsync();
            form.DisplayNotification(Encoding.UTF8.GetString(receiveResult.Buffer), NotificationType.Hint);
        }
    }
}