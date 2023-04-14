using System;

namespace ChatCommonLibrary
{
    [Serializable]
    public enum MessageType
    {
        ChatMessage,
        Connect,
        Disconnect,
        Heartbeat
    }

    [Serializable]
    public class Message
    {
        public MessageType Type { get; set; }
        public string Text { get; set; }
        public string Username { get; set; }
        public string Color { get; set; }

        public Message()
        {
            // Required for deserialization
        }
        
        public Message(MessageType type, string message, string username, string color)
        {
            Type = type;
            Text = message;
            Username = username;
            Color = color;
        }
    }
}