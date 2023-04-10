namespace ChatClient
{
    partial class ClientForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            chatBox = new TextBox();
            sendBox = new TextBox();
            sendButton = new Button();
            SuspendLayout();
            // 
            // chatBox
            // 
            chatBox.Location = new Point(12, 12);
            chatBox.Multiline = true;
            chatBox.Name = "chatBox";
            chatBox.ReadOnly = true;
            chatBox.Size = new Size(776, 397);
            chatBox.TabIndex = 0;
            // 
            // sendBox
            // 
            sendBox.Location = new Point(12, 415);
            sendBox.Name = "sendBox";
            sendBox.Size = new Size(715, 23);
            sendBox.TabIndex = 1;
            // 
            // sendButton
            // 
            sendButton.Location = new Point(733, 415);
            sendButton.Name = "sendButton";
            sendButton.Size = new Size(55, 23);
            sendButton.TabIndex = 2;
            sendButton.Text = "Send";
            sendButton.UseVisualStyleBackColor = true;
            sendButton.Click += SendButton_Click;
            // 
            // ClientForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(sendButton);
            Controls.Add(sendBox);
            Controls.Add(chatBox);
            Name = "ClientForm";
            Text = "Client";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox chatBox;
        private TextBox sendBox;
        private Button sendButton;
    }
}