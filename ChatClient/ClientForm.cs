namespace ChatClient;

public class ClientForm : Form
{
    private Client client;

    private Button sendButton;
    private RichTextBox chatBox;
    private ColorDialog colorDialog;
    private Button colorButton;
    private TextBox sendBox;

    private string myUsername = "username";
    private TextBox usernameBox;
    private Label usernameLabel;
    private Label ipLabel;
    private TextBox ipTextBox;
    private Button connectButton;
    private Color myColor = Color.Black;
    
    public ClientForm()
    {
        client = new Client(this);
        InitializeComponent();
    }

    private void DisplayMessage(string message)
    {
        chatBox.AppendText(message + Environment.NewLine);
    }

    public void DisplayNotification(string message, NotificationType type)
    {
        chatBox.SelectionColor = type switch
        {
            NotificationType.Error => Color.Red,
            NotificationType.Hint => Color.Yellow,
            NotificationType.Success => Color.LawnGreen,
            _ => Color.Black
        };
        chatBox.SelectionFont = new Font(chatBox.Font, FontStyle.Bold);
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

    private void SendButton_Click(object? sender, EventArgs e)
    {
        var message = $"{myUsername} : {sendBox.Text}";

        client.SendToServer(message);

        DisplayMessage(sendBox.Text, myUsername, myColor);

        sendBox.Clear();
        sendBox.Focus();
    }

    private void UsernameBox_TextChanged(object? sender, EventArgs e)
    {
        myUsername = usernameBox.Text;
    }

    private void ColorButton_Click(object? sender, EventArgs e)
    {
        colorDialog.ShowDialog();
        colorButton.BackColor = colorDialog.Color;
        myColor = colorDialog.Color;
    }

    private void ConnectButton_Click(object? sender, EventArgs e)
    {
        client.ConnectToServer(ipTextBox.Text);
    }

    private void InitializeComponent()
    {
        sendButton = new Button();
        sendBox = new TextBox();
        chatBox = new RichTextBox();
        colorDialog = new ColorDialog();
        colorButton = new Button();
        usernameBox = new TextBox();
        usernameLabel = new Label();
        ipLabel = new Label();
        ipTextBox = new TextBox();
        connectButton = new Button();
        SuspendLayout();
        // 
        // sendButton
        // 
        sendButton.Location = new Point(832, 533);
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
        sendBox.Location = new Point(12, 533);
        sendBox.Name = "sendBox";
        sendBox.PlaceholderText = "Type your message";
        sendBox.Size = new Size(814, 26);
        sendBox.TabIndex = 2;
        // 
        // chatBox
        // 
        chatBox.Font = new Font("Bahnschrift", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
        chatBox.Location = new Point(12, 100);
        chatBox.Name = "chatBox";
        chatBox.ReadOnly = true;
        chatBox.Size = new Size(874, 427);
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
        colorButton.Location = new Point(443, 56);
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
        usernameBox.Location = new Point(196, 56);
        usernameBox.Name = "usernameBox";
        usernameBox.Size = new Size(241, 26);
        usernameBox.TabIndex = 5;
        usernameBox.TextChanged += UsernameBox_TextChanged;
        // 
        // usernameLabel
        // 
        usernameLabel.AutoSize = true;
        usernameLabel.Font = new Font("Bahnschrift", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
        usernameLabel.Location = new Point(22, 59);
        usernameLabel.Name = "usernameLabel";
        usernameLabel.Size = new Size(168, 18);
        usernameLabel.TabIndex = 6;
        usernameLabel.Text = "Choose your username :";
        // 
        // ipLabel
        // 
        ipLabel.AutoSize = true;
        ipLabel.Font = new Font("Bahnschrift", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
        ipLabel.Location = new Point(113, 27);
        ipLabel.Name = "ipLabel";
        ipLabel.Size = new Size(77, 18);
        ipLabel.TabIndex = 8;
        ipLabel.Text = "Server IP :";
        // 
        // ipTextBox
        // 
        ipTextBox.Font = new Font("Bahnschrift", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
        ipTextBox.Location = new Point(196, 24);
        ipTextBox.Name = "ipTextBox";
        ipTextBox.Size = new Size(241, 26);
        ipTextBox.TabIndex = 7;
        ipTextBox.Text = "10.51.2.72";
        // 
        // connectButton
        // 
        connectButton.Font = new Font("Bahnschrift", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
        connectButton.Location = new Point(443, 23);
        connectButton.Name = "connectButton";
        connectButton.Size = new Size(158, 26);
        connectButton.TabIndex = 9;
        connectButton.Text = "Connect";
        connectButton.UseVisualStyleBackColor = true;
        connectButton.Click += ConnectButton_Click;
        // 
        // ClientForm
        // 
        BackColor = Color.Azure;
        ClientSize = new Size(898, 571);
        Controls.Add(connectButton);
        Controls.Add(ipLabel);
        Controls.Add(ipTextBox);
        Controls.Add(usernameLabel);
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