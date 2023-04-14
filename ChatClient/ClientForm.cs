using ChatClient.Properties;

namespace ChatClient;

public class ClientForm : Form
{
    private static readonly string[] usernames = {"ChatonFéroce", "ChoupiPanda", "RatonLaveur", "TitiGivré", "BouleDePoule", "TigrouFou", "LapinDentelle", "KoalaFunky", "RenardAstucieux", "LoupBlagueur", "ZèbreFarceur", "GirafeTordue", "ChimpanzéCinglé", "HippopotameHilarant", "ÉcureuilÉtourdi", "PhoqueFoufou", "GrenouilleGivrée", "AlligatorAmusant", "OursRieur", "PingouinPince-Sans-Rire", "CrocodileComique", "PerroquetPlaisantin", "SingeSarcastique", "LamaLunatique", "VacheVentriloque", "ÉléphantEsprit", "LoutreLoufoque", "MarmotteMarrante", "HérissonHilarant", "CastorCocasse", "NarvalNarcissique", "OrnithorynqueOriental", "RhinocérosRieur", "SaumonSarcastique", "AigleAmusé", "BaleineBlagueuse", "CanardClownesque", "DindonDramatique", "EscargotÉtonné", "FourmiFarfelue", "GirouetteGrotesque", "HéronHilarant", "IbisIrrévérencieux", "JaguarJovial", "KangourouKamikaze", "LézardLoufoque", "MéduseMarrante", "NandouNarquois", "OrqueObservateur"};
    private Client client;
    private RichTextBox chatBox;
    private ColorDialog colorDialog;
    private Button colorButton;
    private TextBox sendBox;

    private string myUsername;
    private TextBox usernameBox;
    private Label usernameLabel;
    private Label ipLabel;
    private TextBox ipTextBox;
    private Button connectButton;
    private Button sendButton;
    private Color myColor = Color.Black;

    public ClientForm()
    {
        client = new Client(this);
        InitializeComponent();
        SetRandomColor();
        SetRandomUsername();
        sendBox.Enabled = false;
    }

    private void SetRandomColor()
    {
        int r = Random.Shared.Next(200) + 55;
        int g = Random.Shared.Next(200) + 55;
        int b = Random.Shared.Next(200) + 55;
        myColor = Color.FromArgb(255, r, g, b);
        colorButton.BackColor = myColor;
    }
    
    private void SetRandomUsername()
    {
        myUsername = usernames[Random.Shared.Next(usernames.Length)-1];
        usernameBox.Text = myUsername;
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

        chatBox.ScrollToCaret();
    }

    public void DisplayMessage(string message, string username, Color color)
    {
        // Display username
        chatBox.SelectionColor = color;
        chatBox.SelectionFont = new Font(chatBox.Font, FontStyle.Bold);
        chatBox.AppendText($"{username} ");
        
        // Display time
        chatBox.SelectionColor = Color.GhostWhite;
        chatBox.SelectionFont = new Font(chatBox.Font.FontFamily, 8f, FontStyle.Regular);
        chatBox.AppendText(DateTime.Now.ToString("HH:mm tt" + Environment.NewLine));
        
        // Display message
        chatBox.SelectionColor = chatBox.ForeColor;
        chatBox.SelectionFont = new Font(chatBox.Font, FontStyle.Regular);
        chatBox.AppendText(message + Environment.NewLine);
        
        // Display separation
        chatBox.SelectionColor = chatBox.ForeColor;
        chatBox.SelectionFont = new Font(chatBox.Font.FontFamily, 5f, FontStyle.Regular);
        chatBox.AppendText(Environment.NewLine);

        chatBox.ScrollToCaret();
    }

    private void SendMessage()
    {
        string message = sendBox.Text;

        if (sendBox.Text == string.Empty) return;

        client.SendToServer(message, myUsername, myColor);

        DisplayMessage(sendBox.Text, myUsername, myColor);

        sendBox.Clear();
        sendBox.Focus();
    }

    private void SendButton_Click(object? sender, EventArgs e)
    {
        SendMessage();
    }

    private void SendBox_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            SendMessage();
            e.Handled = true;
            e.SuppressKeyPress = true;
        }
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

    public void Connect(bool success)
    {
        sendBox.Enabled = success;
        if (success)
        {
            sendBox.Focus();
        }
    }

    private void InitializeComponent()
    {
        sendBox = new TextBox();
        chatBox = new RichTextBox();
        colorDialog = new ColorDialog();
        colorButton = new Button();
        usernameBox = new TextBox();
        usernameLabel = new Label();
        ipLabel = new Label();
        ipTextBox = new TextBox();
        connectButton = new Button();
        sendButton = new Button();
        SuspendLayout();
        // 
        // sendBox
        // 
        sendBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        sendBox.BackColor = Color.FromArgb(75, 75, 75);
        sendBox.BorderStyle = BorderStyle.FixedSingle;
        sendBox.Font = new Font("Bahnschrift", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
        sendBox.ForeColor = SystemColors.Control;
        sendBox.Location = new Point(12, 666);
        sendBox.Name = "sendBox";
        sendBox.PlaceholderText = Resources.type_message;
        sendBox.Size = new Size(762, 26);
        sendBox.TabIndex = 2;
        sendBox.KeyDown += SendBox_KeyDown;
        // 
        // chatBox
        // 
        chatBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        chatBox.BackColor = Color.FromArgb(44, 44, 44);
        chatBox.BorderStyle = BorderStyle.None;
        chatBox.Font = new Font("Bahnschrift", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
        chatBox.ForeColor = SystemColors.Control;
        chatBox.Location = new Point(12, 77);
        chatBox.Name = "chatBox";
        chatBox.ReadOnly = true;
        chatBox.ScrollBars = RichTextBoxScrollBars.Vertical;
        chatBox.Size = new Size(798, 582);
        chatBox.TabIndex = 3;
        chatBox.Text = "";
        // 
        // colorDialog
        // 
        colorDialog.AnyColor = true;
        // 
        // colorButton
        // 
        colorButton.BackColor = SystemColors.WindowFrame;
        colorButton.FlatStyle = FlatStyle.Popup;
        colorButton.Font = new Font("Bahnschrift", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
        colorButton.ForeColor = SystemColors.Control;
        colorButton.Location = new Point(442, 45);
        colorButton.Name = "colorButton";
        colorButton.Size = new Size(158, 26);
        colorButton.TabIndex = 4;
        colorButton.Text = Resources.choose_color;
        colorButton.UseVisualStyleBackColor = false;
        colorButton.Click += ColorButton_Click;
        // 
        // usernameBox
        // 
        usernameBox.BackColor = Color.FromArgb(75, 75, 75);
        usernameBox.BorderStyle = BorderStyle.FixedSingle;
        usernameBox.Font = new Font("Bahnschrift", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
        usernameBox.ForeColor = SystemColors.Control;
        usernameBox.Location = new Point(195, 45);
        usernameBox.Name = "usernameBox";
        usernameBox.Size = new Size(241, 26);
        usernameBox.TabIndex = 5;
        usernameBox.TextChanged += UsernameBox_TextChanged;
        // 
        // usernameLabel
        // 
        usernameLabel.AutoSize = true;
        usernameLabel.Font = new Font("Bahnschrift", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
        usernameLabel.ForeColor = SystemColors.Control;
        usernameLabel.Location = new Point(21, 48);
        usernameLabel.Name = "usernameLabel";
        usernameLabel.Size = new Size(168, 18);
        usernameLabel.TabIndex = 6;
        usernameLabel.Text = Resources.choose_username;
        // 
        // ipLabel
        // 
        ipLabel.AutoSize = true;
        ipLabel.Font = new Font("Bahnschrift", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
        ipLabel.ForeColor = SystemColors.Control;
        ipLabel.Location = new Point(112, 16);
        ipLabel.Name = "ipLabel";
        ipLabel.Size = new Size(77, 18);
        ipLabel.TabIndex = 8;
        ipLabel.Text = Resources.server_ip;
        // 
        // ipTextBox
        // 
        ipTextBox.BackColor = Color.FromArgb(75, 75, 75);
        ipTextBox.BorderStyle = BorderStyle.FixedSingle;
        ipTextBox.Font = new Font("Bahnschrift", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
        ipTextBox.ForeColor = SystemColors.Control;
        ipTextBox.Location = new Point(195, 13);
        ipTextBox.Name = "ipTextBox";
        ipTextBox.Size = new Size(241, 26);
        ipTextBox.TabIndex = 7;
        ipTextBox.Text = "10.51.2.72";
        // 
        // connectButton
        // 
        connectButton.BackColor = SystemColors.WindowFrame;
        connectButton.FlatStyle = FlatStyle.Popup;
        connectButton.Font = new Font("Bahnschrift", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
        connectButton.ForeColor = SystemColors.Control;
        connectButton.Location = new Point(442, 12);
        connectButton.Name = "connectButton";
        connectButton.Size = new Size(158, 26);
        connectButton.TabIndex = 9;
        connectButton.Text = Resources.connect;
        connectButton.UseVisualStyleBackColor = false;
        connectButton.Click += ConnectButton_Click;
        // 
        // sendButton
        // 
        sendButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        sendButton.BackColor = Color.Transparent;
        sendButton.BackgroundImage = Resources.sendIcon;
        sendButton.BackgroundImageLayout = ImageLayout.Stretch;
        sendButton.FlatAppearance.BorderSize = 0;
        sendButton.FlatAppearance.MouseDownBackColor = Color.Transparent;
        sendButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
        sendButton.FlatStyle = FlatStyle.Flat;
        sendButton.ForeColor = Color.Transparent;
        sendButton.Location = new Point(780, 666);
        sendButton.Name = "sendButton";
        sendButton.Size = new Size(30, 26);
        sendButton.TabIndex = 10;
        sendButton.UseVisualStyleBackColor = false;
        sendButton.Click += SendButton_Click;
        // 
        // ClientForm
        // 
        BackColor = Color.FromArgb(61, 61, 61);
        ClientSize = new Size(822, 703);
        Controls.Add(sendButton);
        Controls.Add(connectButton);
        Controls.Add(ipLabel);
        Controls.Add(ipTextBox);
        Controls.Add(usernameLabel);
        Controls.Add(usernameBox);
        Controls.Add(colorButton);
        Controls.Add(chatBox);
        Controls.Add(sendBox);
        MinimumSize = new Size(622, 306);
        Name = "ClientForm";
        ResumeLayout(false);
        PerformLayout();
    }
}