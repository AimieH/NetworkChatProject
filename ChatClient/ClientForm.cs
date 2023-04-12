using System.Net.Sockets;
using System.Net;
using System.Text;

namespace ChatClient
{
    public partial class ClientForm : Form
    {
        UdpClient client = new UdpClient();
        IPEndPoint targetEndpoint = new IPEndPoint(IPAddress.Parse("10.51.2.72"), 666);

        private Button sendButton;
        private RichTextBox chatBox;
        private ColorDialog colorDialog;
        private Button colorButton;
        private TextBox sendBox;

        private string myUsername = "username";
        private TextBox usernameBox;
        private Label label1;
        private Color myColor = Color.Black;

        public ClientForm()
        {
            InitializeComponent();
            sendBox.Focus();
        }

        private void DisplayMessage(string message)
        {
            chatBox.AppendText(message + Environment.NewLine);
        }

        private void DisplayMessage(string message, string username, Color color)
        {
            // Display time
            chatBox.SelectionColor = Color.Gray;
            chatBox.SelectionFont = new Font(chatBox.Font.FontFamily, 8f, FontStyle.Regular);
            chatBox.AppendText(DateTime.Now.ToString("HH:mm tt"));

            // Display username
            chatBox.SelectionColor = color;
            chatBox.SelectionFont = new Font(chatBox.Font, FontStyle.Bold);
            chatBox.AppendText($" {username} : ");

            // Display message
            chatBox.SelectionColor = chatBox.ForeColor;
            chatBox.SelectionFont = new Font(chatBox.Font, FontStyle.Regular);
            chatBox.AppendText(message + Environment.NewLine);
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            string message = $"{"username"} : {sendBox.Text}";

            client.SendAsync(Encoding.UTF8.GetBytes(message), targetEndpoint);

            //DisplayMessage(message);
            DisplayMessage(sendBox.Text, myUsername, myColor);

            sendBox.Clear();
            sendBox.Focus();
        }

        private void UsernameBox_TextChanged(object sender, EventArgs e)
        {
            myUsername = usernameBox.Text;
        }

        private void ColorButton_Click(object sender, EventArgs e)
        {
            colorDialog.ShowDialog();
            colorButton.BackColor = colorDialog.Color;
            myColor = colorDialog.Color;
        }

        private void InitializeComponent()
        {
            sendButton = new Button();
            sendBox = new TextBox();
            chatBox = new RichTextBox();
            colorDialog = new ColorDialog();
            colorButton = new Button();
            usernameBox = new TextBox();
            label1 = new Label();
            SuspendLayout();
            // 
            // sendButton
            // 
            sendButton.Location = new Point(705, 467);
            sendButton.Name = "sendButton";
            sendButton.Size = new Size(54, 27);
            sendButton.TabIndex = 0;
            sendButton.Text = "Send";
            sendButton.UseVisualStyleBackColor = true;
            sendButton.Click += SendButton_Click;
            // 
            // sendBox
            // 
            sendBox.Font = new Font("Bahnschrift", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            sendBox.Location = new Point(12, 468);
            sendBox.Name = "sendBox";
            sendBox.PlaceholderText = "Type your message";
            sendBox.Size = new Size(687, 26);
            sendBox.TabIndex = 2;
            // 
            // chatBox
            // 
            chatBox.Font = new Font("Bahnschrift", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            chatBox.Location = new Point(12, 44);
            chatBox.Name = "chatBox";
            chatBox.ReadOnly = true;
            chatBox.Size = new Size(747, 417);
            chatBox.TabIndex = 3;
            chatBox.Text = "";
            // 
            // colorDialog
            // 
            colorDialog.AnyColor = true;
            // 
            // colorButton
            // 
            colorButton.Font = new Font("Bahnschrift", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            colorButton.Location = new Point(478, 12);
            colorButton.Name = "colorButton";
            colorButton.Size = new Size(158, 26);
            colorButton.TabIndex = 4;
            colorButton.Text = "Choose your  color";
            colorButton.UseVisualStyleBackColor = true;
            colorButton.Click += ColorButton_Click;
            // 
            // usernameBox
            // 
            usernameBox.Font = new Font("Bahnschrift", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            usernameBox.Location = new Point(231, 12);
            usernameBox.Name = "usernameBox";
            usernameBox.Size = new Size(241, 26);
            usernameBox.TabIndex = 5;
            usernameBox.TextChanged += UsernameBox_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Bahnschrift", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(57, 15);
            label1.Name = "label1";
            label1.Size = new Size(168, 18);
            label1.TabIndex = 6;
            label1.Text = "Choose your username :";
            // 
            // ClientForm
            // 
            BackColor = Color.Azure;
            ClientSize = new Size(771, 502);
            Controls.Add(label1);
            Controls.Add(usernameBox);
            Controls.Add(colorButton);
            Controls.Add(chatBox);
            Controls.Add(sendBox);
            Controls.Add(sendButton);
            Name = "ClientForm";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}