using System;
using System.Collections.Generic;

namespace ChatCommonLibrary
{
    [Serializable]
    public enum MessageType
    {
        ChatMessage,
        PlayersUpdate,
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
        public string Date { get; set; }
        public bool BoolSlot { get; set; }
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
            Date = "";
            BoolSlot = false;
            StringSlot = "";
            History = history;
        }
        
        public Message(MessageType type, string username, string color, string stringSlot = "", bool boolSlot = false)
        {
            Type = type;
            Text = "";
            Username = username;
            Color = color;
            Date = "";
            BoolSlot = boolSlot;
            StringSlot = stringSlot;
            History = new List<Message>();
        }
        
        public Message(MessageType type, string message, string username, string color, string date, bool boolSlot = false)
        {
            Type = type;
            Text = message;
            Username = username;
            Color = color;
            Date = date;
            BoolSlot = boolSlot;
            StringSlot = "";
            History = new List<Message>();
        }
    }
}