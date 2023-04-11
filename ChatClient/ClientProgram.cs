using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatClient
{
    internal static class ClientProgram
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new ClientForm());
        }
    }
}