using System.Net.Sockets;
using System.Net;
using System.Text;

namespace ChatClient
{
    public partial class ClientForm : Form
    {
        UdpClient client = new UdpClient();

        IPEndPoint targetEndpoint = new IPEndPoint(IPAddress.Parse("192.168.1.17"), 666);

        public ClientForm()
        {
            InitializeComponent();
            sendBox.Focus();
        }

        private void DisplayMessage(string message)
        {
            chatBox.AppendText(message + Environment.NewLine);
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            string message = $"{"username"} : {sendBox.Text}";

            client.SendAsync(Encoding.UTF8.GetBytes(message), targetEndpoint);

            DisplayMessage(message);

            sendBox.Clear();

            sendBox.Focus();
        }
    }
}