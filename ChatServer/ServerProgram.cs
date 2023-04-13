using System.Net.Sockets;
using System.Text;

Console.WriteLine("Starting server");

using var server = new UdpClient(666);

for (var i = 0;; i++)
{
    var receiveResult = await server.ReceiveAsync();

    Console.WriteLine($"{Encoding.UTF8.GetString(receiveResult.Buffer)}");

    await server.SendAsync(BitConverter.GetBytes(i), receiveResult.RemoteEndPoint);
}
