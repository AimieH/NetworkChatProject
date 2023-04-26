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
    private bool receiving;
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
            SendMessage(MessageType.Connect, "", form.Username, form.Color);
        }
        else
        {
            form.DisplayNotification("Given server ip isn't valid :(", NotificationType.Error);
            form.Connect(false);
            connected = false;
        }
    }

    public void Disconnect(bool sendToServer)
    {
        if (sendToServer) SendMessage(MessageType.Disconnect, "", form.Username, form.Color);
        form.Connect(false);
        targetEndpoint = null;
        connected = false;
        receiving = false;
    }

    public bool IsSameIp(string ip)
    {
        return targetEndpoint is not null && Equals(targetEndpoint.Address.ToString(), ip);
    }
    
    private async void SendToServer(Message message)
    {
        if (targetEndpoint is null) return;
        
        var json = JsonSerializer.Serialize(message);
        try
        {
            await client.SendAsync(Encoding.UTF8.GetBytes(json), targetEndpoint);
        }
        catch (Exception e)
        {
            form.DisplayNotification(e.ToString(), NotificationType.Error);
        }
    }
        
    public void SendMessage(MessageType type, string message = "", string? username = "", Color color = new())
    {
        SendToServer(new Message(type, message, username, ColorTranslator.ToHtml(color), GetDateToString()));
    }
    
    public void ChangeColor(string? username, Color color, Color lastColor)
    {
        SendToServer(new Message(MessageType.ChangeColor, username, ColorTranslator.ToHtml(color), ColorTranslator.ToHtml(lastColor)));
    }
    
    public void ChangeUsername(string username, string? lastUsername, Color color)
    {
        SendToServer(new Message(MessageType.ChangeUsername,  username, ColorTranslator.ToHtml(color), lastUsername));
    }

    private string GetDateToString()
    { 
        return DateTime.Now.ToString("HH:mm tt");
    }
    private async void StartReceiving()
    {
        form.DisplayNotification("Receiving started...", NotificationType.Hint);
        receiving = true;
        
        while (receiving)
        {
            UdpReceiveResult? tryReceiveResult = null;
            try
            {
                tryReceiveResult = await client.ReceiveAsync();
            }
            catch (SocketException)
            {
                form.DisplayNotification("Connection failed", NotificationType.Error);
                form.Connect(false);
                Disconnect(false);
            }
            
            if (tryReceiveResult is null) return;
            var receiveResult = (UdpReceiveResult) tryReceiveResult;
            
            var receivedJson = Encoding.UTF8.GetString(receiveResult.Buffer);
            
            Message? receivedMessage = null;
            try
            {
                receivedMessage = JsonSerializer.Deserialize<Message>(receivedJson);
            }
            catch (JsonException)
            {
                form.DisplayNotification("History has been lost", NotificationType.Error);
                form.DisplayNotification(receivedJson, NotificationType.Error);
            }
            
            if (receivedMessage is null) return;

            var color = ColorTranslator.FromHtml(receivedMessage.Color);
            switch (receivedMessage.Type)
            {
                case MessageType.ChatMessage:
                    if (connected)
                        form.DisplayMessage(receivedMessage.Text, receivedMessage.Username, color, receivedMessage.Date, receivedMessage.BoolSlot);
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
                    if (receivedMessage.BoolSlot)
                    {
                        form.DisplayPlayersUpdate(receivedMessage.Username, color, true);
                    }
                    else
                    {
                        form.DisplayNotification("Connected to server :)", NotificationType.Success);
                        form.Connect(true);
                        connected = true;
                    }
                    break;
                case MessageType.Disconnect:
                    if (receivedMessage.BoolSlot)
                    {
                        form.DisplayPlayersUpdate(receivedMessage.Username, color, false);
                    }
                    else
                    {
                        form.DisplayNotification("Server disconnected :(", NotificationType.Hint);
                        form.Connect(false);
                        Disconnect(false);
                    }
                    break;
                case MessageType.Heartbeat:
                    break;
                case MessageType.HistorySend:
                    foreach (var message in receivedMessage.History)
                    {
                        var messageColor = ColorTranslator.FromHtml(message.Color);
                        switch (message.Type)
                        {
                            case MessageType.ChangeColor:
                                var lastColor = ColorTranslator.FromHtml(message.StringSlot);
                                form.DisplayChange(message.Username, message.Username, messageColor, lastColor);
                                break;
                            case MessageType.ChangeUsername:
                                form.DisplayChange(message.Username, message.StringSlot, messageColor, messageColor);
                                break;
                            case MessageType.Connect:
                                form.DisplayPlayersUpdate(message.Username, messageColor, true);
                                break;
                            case MessageType.Disconnect:
                                form.DisplayPlayersUpdate(message.Username, messageColor, false);
                                break;
                            case MessageType.ChatMessage:
                            case MessageType.Heartbeat:
                            case MessageType.HistorySend:
                            default:
                                form.DisplayMessage(message.Text, message.Username, messageColor, message.Date, message.BoolSlot);
                                break;
                        }
                    }
                    break;
                default:
                    form.DisplayNotification("Message type not handled :(", NotificationType.Error);
                    break;
            }
        }
    }
}