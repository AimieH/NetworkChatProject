using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using ChatCommonLibrary;

Console.WriteLine("Starting server");

using var server = new UdpClient(666);

var clients = new List<IPEndPoint>();
var messages = new List<Message>();
IPEndPoint? lastSender = null;
string? lastColor = null;
string? lastUsername = null;

while (true)
{
    try
    {
        var receiveResult = await server.ReceiveAsync();
        var msgBuffer = receiveResult.Buffer;
        var sender = receiveResult.RemoteEndPoint;
        var receivedJson = Encoding.UTF8.GetString(msgBuffer);
        
        Message? receivedMessage = null;
        try
        {
            receivedMessage = JsonSerializer.Deserialize<Message>(receivedJson);
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"{ex}");
        }

        if (receivedMessage is null) return;

        Message messageToSend;
        switch (receivedMessage.Type)
        {
            case MessageType.ChatMessage:
                Console.WriteLine($"Message by {sender} : {receivedJson}");
                
                var isLastSender = Equals(lastSender, sender) && Equals(lastColor, receivedMessage.Color) && Equals(lastUsername, receivedMessage.Username);
                messageToSend = new Message(MessageType.ChatMessage, receivedMessage.Text, receivedMessage.Username, receivedMessage.Color, receivedMessage.Date, isLastSender);
        
                foreach (var client in clients)
                {
                    await server.SendAsync(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(messageToSend)), client);
                }
                messages.Add(messageToSend);
                lastSender = sender;
                lastColor = receivedMessage.Color;
                lastUsername = receivedMessage.Username;
                
                break;
            case MessageType.ChangeColor:
                Console.WriteLine($"The sender {sender} changed color");
                
                foreach (var client in clients)
                {
                    await server.SendAsync(msgBuffer, msgBuffer.Length, client);
                }
                messages.Add(receivedMessage);
                break;
            case MessageType.ChangeUsername:
                Console.WriteLine($"The sender {sender} changed username");
                
                foreach (var client in clients)
                {
                    await server.SendAsync(msgBuffer, msgBuffer.Length, client);
                }
                messages.Add(receivedMessage);
                break;
            case MessageType.Connect:
                Console.WriteLine($"The sender {sender} is trying to connect");
                if (!clients.Contains(sender))
                {
                    clients.Add(sender);
                }
                
                await server.SendAsync(msgBuffer, msgBuffer.Length, sender);

                messageToSend = new Message(MessageType.PlayersUpdate, receivedMessage.Username, receivedMessage.Color, "", true);
                foreach (var client in clients)
                {
                    await server.SendAsync(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(messageToSend)), client);
                }

                var bytesToSend = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new Message(messages)));
                await server.SendAsync(bytesToSend, bytesToSend.Length, sender);
                break;
            case MessageType.Disconnect:
                break;
            case MessageType.Heartbeat:
                break;
            default:
                Console.WriteLine("ERROR : Message type not handled :(");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"{ex}");
    }
}
