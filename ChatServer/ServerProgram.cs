using System.Net;
using System.Net.Sockets;
using System.Text;

Console.WriteLine("Starting server");

using var server = new UdpClient(666);

var clients = new List<IPEndPoint>();

while (true)
{
    var receiveResult = await server.ReceiveAsync();
    var msgBuffer = receiveResult.Buffer;
    var sender = receiveResult.RemoteEndPoint;
    
    Console.WriteLine($"{Encoding.UTF8.GetString(msgBuffer)}");
    
    if (!clients.Contains(sender)) clients.Add(sender);
    foreach (var client in clients.Where(client => !Equals(client, sender)))
    {
        await server.SendAsync(msgBuffer, msgBuffer.Length, client);
    }
}
