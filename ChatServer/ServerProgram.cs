using System.Net;
using System.Net.Sockets;
using System.Text;

Console.WriteLine("Starting server");

using var server = new UdpClient(666);

var clients = new List<IPEndPoint>();

while (true)
{
    var receiveResult = await server.ReceiveAsync();
    
    Console.WriteLine($"{Encoding.UTF8.GetString(receiveResult.Buffer)}");
    
    if (!clients.Contains(receiveResult.RemoteEndPoint)) clients.Add(receiveResult.RemoteEndPoint);
    foreach (var client in clients)
    {
        await server.SendAsync(receiveResult.Buffer, receiveResult.Buffer.Length, client);
    }
}
