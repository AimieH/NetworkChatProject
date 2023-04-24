using System.Net.Sockets;
using System.Net;
using System.Text;
using ChatCommonLibrary;
using System.Text.Json;
using Message = ChatCommonLibrary.Message;

namespace ChatClient;

public class Client
{
    private readonly UdpClient client = new(777);
    private IPEndPoint? targetEndpoint;

    private readonly ClientForm form;
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
            form.DisplayNotification("Checking given server...", NotificationType.Hint);
            StartReceiving();
            SendToServer(MessageType.Connect);
        }
        else
        {
            form.DisplayNotification("Given server ip is not an available one :(", NotificationType.Error);
            form.Connect(false);
        }
    }

    public async void SendToServer(MessageType type, string message = "", string username = "", Color color = new())
    {
        var chatMessage = new Message(type, message, username, ColorTranslator.ToHtml(color));

        var json = JsonSerializer.Serialize(chatMessage);

        await client.SendAsync(Encoding.UTF8.GetBytes(json), targetEndpoint);
    }

    private async void StartReceiving()
    {
        form.DisplayNotification("Receiving started...", NotificationType.Hint);
        
        while (receiving)
        {
            var receiveResult = await client.ReceiveAsync();
            var receivedJson = Encoding.UTF8.GetString(receiveResult.Buffer);
            
            Message? receivedMessage = null;
            try
            {
                receivedMessage = JsonSerializer.Deserialize<Message>(receivedJson);
            }
            catch (JsonException ex)
            {
                form.DisplayNotification(ex.ToString(), NotificationType.Hint);
            }

            if (receivedMessage is null) return;

            switch (receivedMessage.Type)
            {
                case MessageType.ChatMessage:
                    // Display received message
                    var color = ColorTranslator.FromHtml(receivedMessage.Color);
                    form.DisplayMessage(receivedMessage.Text, receivedMessage.Username, color);
                    break;
                case MessageType.Connect:
                    form.DisplayNotification("Connected to server :)", NotificationType.Success);
                    form.Connect(true);
                    break;
                case MessageType.Disconnect:
                    form.DisplayNotification("Server disconnected :(", NotificationType.Error);
                    form.Connect(false);
                    break;
                case MessageType.Heartbeat:
                    break;
                default:
                    form.DisplayNotification("Message type not handled :(", NotificationType.Error);
                    break;
            }
        }
    }
}