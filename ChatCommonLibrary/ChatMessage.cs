using System;
using System.Collections.Generic;

namespace ChatCommonLibrary
{
    [Serializable]
    public enum MessageType
    {
        Message,
        Connect,
        Disconnect,
        Heartbeat
    }

    [Serializable]
    public class ChatMessage
    {
        public string Message { get; set; }
        public string Username { get; set; }
        public string Color { get; set; }
        public MessageType Type { get; set; }

        public ChatMessage(string message, string username, string color, MessageType type)
        {
            Message = message;
            Username = username;
            Color = color;
            Type = type;
        }
    }
}
