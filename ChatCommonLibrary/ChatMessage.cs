using System;
using System.Collections.Generic;

namespace ChatCommonLibrary
{
    public enum MessageType
    {
        Message,
        Connect,
        Disconnect,
        Heartbeat
    }

    public class ChatMessage
    {
        public string message;
        public string username;
        public string color;
        public MessageType type;
    }
}
