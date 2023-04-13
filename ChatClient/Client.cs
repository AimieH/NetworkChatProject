using System.Net.Sockets;
using System.Net;
using System.Text;
using ChatCommonLibrary;
using System.Text.Json;
using Message = ChatCommonLibrary.Message;

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

    public async void SendToServer(string message, string username, Color color)
    {
        var chatMessage = new Message(MessageType.ChatMessage, username, ColorTranslator.ToHtml(color), message);

        form.DisplayNotification(chatMessage.Text, NotificationType.Hint);
        var json = JsonSerializer.Serialize(chatMessage);
        form.DisplayNotification(json, NotificationType.Hint);

        await client.SendAsync(Encoding.UTF8.GetBytes(json), targetEndpoint);
    }

    private async void StartReceiving()
    {
        form.DisplayNotification("Receiving started...", NotificationType.Hint);
        
        while (receiving)
        {
            var receiveResult = await client.ReceiveAsync();

            var receivedJson = Encoding.UTF8.GetString(receiveResult.Buffer);

            try
            {
                var receivedMessage = JsonSerializer.Deserialize<Message>(receivedJson);
                
                // Display received message
                var color = ColorTranslator.FromHtml(receivedMessage?.Color ?? string.Empty);
                form.DisplayMessage(receivedMessage?.Text ?? string.Empty, receivedMessage?.Username ?? string.Empty, color);
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}