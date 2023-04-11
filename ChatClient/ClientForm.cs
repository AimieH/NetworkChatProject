using System.Net.Sockets;
using System.Net;
using System.Text;

namespace ChatClient
{
    public partial class ClientForm : Form
    {
        UdpClient client = new UdpClient();
        private Button sendButton;
        private TextBox chatBox;
        private TextBox sendBox;
        IPEndPoint targetEndpoint = new IPEndPoint(IPAddress.Parse("10.51.2.72"), 666);

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

        private void InitializeComponent()
        {
            sendButton = new Button();
            chatBox = new TextBox();
            sendBox = new TextBox();
            SuspendLayout();
            // 
            // sendButton
            // 
            sendButton.Location = new Point(705, 467);
            sendButton.Name = "sendButton";
            sendButton.Size = new Size(54, 23);
            sendButton.TabIndex = 0;
            sendButton.Text = "Send";
            sendButton.UseVisualStyleBackColor = true;
            sendButton.Click += SendButton_Click;
            // 
            // chatBox
            // 
            chatBox.Location = new Point(12, 12);
            chatBox.Multiline = true;
            chatBox.Name = "chatBox";
            chatBox.ReadOnly = true;
            chatBox.Size = new Size(747, 449);
            chatBox.TabIndex = 1;
            // 
            // sendBox
            // 
            sendBox.Location = new Point(12, 468);
            sendBox.Name = "sendBox";
            sendBox.Size = new Size(687, 23);
            sendBox.TabIndex = 2;
            // 
            // ClientForm
            // 
            BackColor = Color.Azure;
            ClientSize = new Size(771, 502);
            Controls.Add(sendBox);
            Controls.Add(chatBox);
            Controls.Add(sendButton);
            Name = "ClientForm";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}