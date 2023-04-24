using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using ChatCommonLibrary;

Console.WriteLine("Starting server");

using var server = new UdpClient(666);

var clients = new List<IPEndPoint>();

while (true)
{
    try
    {
        var receiveResult = await server.ReceiveAsync();
        var msgBuffer = receiveResult.Buffer;
        var sender = receiveResult.RemoteEndPoint;
        var receivedJson = Encoding.UTF8.GetString(receiveResult.Buffer);
            
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

        switch (receivedMessage.Type)
        {
            case MessageType.ChatMessage:
                Console.WriteLine($"{Encoding.UTF8.GetString(msgBuffer)}");
        
                if (!clients.Contains(sender)) clients.Add(sender);
                foreach (var client in clients.Where(client => !Equals(client, sender)))
                {
                    await server.SendAsync(msgBuffer, msgBuffer.Length, client);
                }
                break;
            case MessageType.Connect:
                await server.SendAsync(msgBuffer, msgBuffer.Length, sender);
                break;
            case MessageType.Disconnect:
                break;
            case MessageType.Heartbeat:
                break;
            default:
                Console.WriteLine("Message type not handled :(");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"{ex}");
    }
}
