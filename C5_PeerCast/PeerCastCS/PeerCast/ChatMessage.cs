using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PeerCast
{
    public class ChatMessage
    {
        public ChatCommand CommandType { get; set; }
        public string Message { get; set; }

        public static ChatMessage Create(ChatCommand command, string message)
        {
            return new ChatMessage() { CommandType = command, Message = message }; 
        }


        public static ChatMessage P2PLibParse(string text)
        {
            //ex:"StreamVideo said: C:\Users\Public\Bear.wmv"            

            //split the string based on spaces
            var msgParts = text.Split(' ');

            //combine the first parts of the text
            string firstPart = msgParts[0] + " " + msgParts[1] + " ";

            //now remove from the original text
            string message = text.Remove(0, firstPart.Length);
            
            //create a text and return it
            ChatMessage msg = new ChatMessage();

            //create a ChatCommand enum
            msg.CommandType = (ChatCommand)Enum.Parse(typeof(ChatCommand), msgParts[0]);            
            msg.Message = message;                        
            return msg; 
        }
    }


}
