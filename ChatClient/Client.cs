using System.Net.Sockets;
using System.Net;
using System.Text;
using ChatCommonLibrary;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace ChatClient;

public class Client
{
    private UdpClient client = new(777);
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

    public void SendToServer(string message, string username, Color color)
    {
        ChatMessage chatMessage = new ChatMessage(message, username, ColorTranslator.ToHtml(color), MessageType.Message);

        string json = JsonSerializer.Serialize(chatMessage);

        client.SendAsync(Encoding.UTF8.GetBytes(json), targetEndpoint);
    }

    private async void StartReceiving()
    {
        form.DisplayNotification("Receiving started...", NotificationType.Hint);
        
        while (receiving)
        {
            var receiveResult = await client.ReceiveAsync();

            string receivedJson = Encoding.UTF8.GetString(receiveResult.Buffer);

            ChatMessage? receivedMessage = null;

            try
            {
                receivedMessage = JsonSerializer.Deserialize<ChatMessage>(receivedJson);
            }
            catch (Exception)
            {
                // ignored
            }
            
            if (receivedMessage != null)
            {
                // Display received message
                Color color = ColorTranslator.FromHtml(receivedMessage.color);
                form.DisplayMessage(receivedMessage.message, receivedMessage.username, color);
            }
        }
    }
}