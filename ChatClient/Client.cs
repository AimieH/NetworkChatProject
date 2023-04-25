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
    private bool connected;

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
            SendMessage(MessageType.Connect);
        }
        else
        {
            form.DisplayNotification("Given server ip isn't valid :(", NotificationType.Error);
            form.Connect(false);
        }
    }

    private async void SendToServer(Message message)
    {
        if (targetEndpoint is null) return;
        
        var json = JsonSerializer.Serialize(message);
        await client.SendAsync(Encoding.UTF8.GetBytes(json), targetEndpoint);
        
    }
    public void SendMessage(MessageType type, string message = "", string username = "", Color color = new())
    {
        SendToServer(new Message(type, message, username, ColorTranslator.ToHtml(color)));
    }
    
    public void ChangeColor(string username, Color color, Color lastColor)
    {
        SendToServer(new Message(MessageType.ChangeColor, "", username, ColorTranslator.ToHtml(color), false, ColorTranslator.ToHtml(lastColor)));
    }
    
    public void ChangeUsername(string username, string lastUsername, Color color)
    {
        SendToServer(new Message(MessageType.ChangeUsername, "", username, ColorTranslator.ToHtml(color), false, lastUsername));
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

            var color = ColorTranslator.FromHtml(receivedMessage.Color);
            switch (receivedMessage.Type)
            {
                case MessageType.ChatMessage:
                    if (connected)
                        form.DisplayMessage(receivedMessage.Text, receivedMessage.Username, color, receivedMessage.IsLastSender);
                    break;
                case MessageType.ChangeColor:
                    if (connected)
                    {
                        var lastColor = ColorTranslator.FromHtml(receivedMessage.StringSlot);
                        form.DisplayChange(receivedMessage.Username, receivedMessage.Username, color, lastColor);
                    }
                    break;
                case MessageType.ChangeUsername:
                    if (connected)
                        form.DisplayChange(receivedMessage.Username, receivedMessage.StringSlot, color, color);
                    break;
                case MessageType.Connect:
                    form.DisplayNotification("Connected to server :)", NotificationType.Success);
                    form.Connect(true);
                    connected = true;
                    break;
                case MessageType.Disconnect:
                    form.DisplayNotification("Server disconnected :(", NotificationType.Hint);
                    form.Connect(false);
                    connected = false;
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