using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yggdrasil.Entities;

namespace Yggdrasil.Packets.Game
{
    public enum ChatType : short
    {
        Unknown = 261,
        Normal = 263,
        Party,
        Guild,
        Whisper,
        Shout,
        Megaphone,
        Notice
    }
    /// <summary>
    /// Chat Packet sent to speaker
    /// </summary>
    public class Megaphone : Packet
    {
        public Megaphone(string sender, string message, short level)
        {
            packet.Type(1006);
            packet.WriteShort(268);
            packet.WriteString(sender);
            packet.WriteString(message);
            packet.WriteInt(6602);
            packet.WriteShort(level);
        }
    }

    public class Whisper : Packet
    {
        public Whisper(string sender, string other, string message, byte type)
        {
            packet.Type(1006);
            packet.WriteShort(264);
            packet.WriteByte(type);
            packet.WriteString(other);
            packet.WriteString(sender);
            packet.WriteString(message);
            packet.WriteByte(0);
        }
    }



    public class WelcomeMessage : Packet
    {
        public WelcomeMessage(string message)
        {
            packet.Type(1047);
            packet.WriteByte(1);
            packet.WriteString(message);

        }
    }

    public class Message : Packet
    {
        public Message(string message)
        {
            packet.Type(1047);
            packet.WriteByte(1);
            packet.WriteString(message);

        }
    }
    public class BaseChat : Packet
    {
        public BaseChat()
        {
            packet.Type(1006);
        }

        /// <summary>
        /// Send to speaker
        /// </summary>
        /// <param name="chatType"></param>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        /// 
        public BaseChat(ChatType chatType, string sender, string message)
        {
            packet.Type(1006);
            packet.WriteShort((short)chatType);
            packet.WriteString(sender);
            packet.WriteString(message);
            packet.WriteByte(0);
        }

        /// <summary>
        /// Send a Shout
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        public BaseChat Megaphone(string sender, string message, int itemId, short tamerlevel)
        {
            packet.WriteShort((short)ChatType.Megaphone);
            packet.WriteString(sender);
            packet.WriteString(message);
            packet.WriteInt(itemId);
            packet.WriteShort(tamerlevel);
            return this;
        }

        public BaseChat Notice(string message)
        {
            packet.WriteShort((short)ChatType.Notice);
            packet.WriteString(message);
            packet.WriteByte(0);
            return this;
        }

        public BaseChat(ChatType chatType, string message)
        {
            packet.Type(1006);
            packet.WriteShort((short)chatType);
            packet.WriteString(message);
            packet.WriteByte(0);
        }

        /// <summary>
        /// Found with ChatType Unknown
        /// </summary>
        /// <param name="chatType"></param>
        /// <param name="message"></param>
        public BaseChat(ChatType chatType, Character Speaker, string message)
        {
            packet.Type(1006);
            packet.WriteShort((short)chatType);
            packet.WriteByte(0);
            packet.WriteUInt(Speaker.UID);
            packet.WriteInt(Speaker.Location.PosX);
            packet.WriteInt(Speaker.Location.PosY);
            packet.WriteShort(267); //Another Chattype
            packet.WriteString(Speaker.Name);
            packet.WriteString(message);
            packet.WriteByte(0);
        }

        /// <summary>
        /// Send to speaker
        /// </summary>
        /// <param name="chatType"></param>
        /// <param name="message"></param>
        public BaseChat(ChatType chatType, ushort handle, string message)
        {
            packet.Type(1006);
            packet.WriteShort((short)chatType);
            packet.WriteInt(handle);
            packet.WriteString(message);
            packet.WriteByte(0);
        }
    }
}
