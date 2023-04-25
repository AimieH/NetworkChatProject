using System;
using System.Collections.Generic;

namespace ChatCommonLibrary
{
    [Serializable]
    public enum MessageType
    {
        ChatMessage,
        ChangeColor,
        ChangeUsername,
        Connect,
        Disconnect,
        Heartbeat,
        HistorySend
    }

    [Serializable]
    public class Message
    {
        public MessageType Type { get; set; }
        public string Text { get; set; }
        public string Username { get; set; }
        public string Color { get; set; }
        public bool IsLastSender { get; set; }
        public string StringSlot { get; set; }
        public List<Message> History { get; set; }

        public Message()
        {
            // Required for deserialization
        }
        
        public Message(List<Message> history)
        {
            Type = MessageType.HistorySend;
            Text = "";
            Username = "";
            Color = "";
            IsLastSender = false;
            StringSlot = "";
            History = history;
        }
        
        public Message(MessageType type, string message, string username, string color, bool isLastSender = false, string stringSlot = "")
        {
            Type = type;
            Text = message;
            Username = username;
            Color = color;
            IsLastSender = isLastSender;
            StringSlot = stringSlot;
            History = new List<Message>();
        }
    }
}