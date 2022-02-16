/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Digital_World.Entities;

namespace Yggdrasil.Packets.Game;
{
                public class FriendList : Packet
            {
                public FriendList()
                {
                    //Character tamer = client.Tamer;
                    packet.Type(2404);
                    //packet.WriteShort((short)tamer.friends.Count);
                    //packet.WriteBytes(tamer.friends.ToArray());
                    //packet.WriteShort(0);
                    /*short size = (short)tamer.friends.Count;
                    packet.WriteShort(size);

                    int counter = 0;

                    do
                    {
                        LoadFriends(tamer.friends[counter]);
                        Console.WriteLine(counter);
                        counter++;

                    } while (counter < tamer.friends.Count);
                }
            }

    private void LoadFriends(Friend x)
    {


    }
    public FriendList(KeyValuePair<string, int> FriendList)
    {
        packet.Type(2404);
        packet.WriteInt(0);
    }
}
    public class AddFriend : Packet
    {
            public AddFriend(string FriendName)
            {
                packet.Type(2401);
                packet.WriteByte(0);
                packet.WriteString(FriendName);
            }
    }
        public class DelFriend : Packet
        {
            public DelFriend(string FriendName)
            {
                packet.Type(2402);
                packet.WriteByte(0);
                packet.WriteString(FriendName);
            }
        }

public class BlockFriend : Packet
{
    public BlockFriend(string FriendName)
    {
        packet.Type(2403);
        packet.WriteByte(0);
        packet.WriteString(FriendName);
    }
}
public class Memo : Packet
{
    public Memo(string FriendName)
    {
        packet.Type(2403);
        packet.WriteByte(0);
        packet.WriteString(FriendName);
    }
}


public class ConfirmFriendList : Packet
{
    public ConfirmFriendList()
    {
        packet.Type(1713);
        packet.WriteShort(5);
        packet.WriteShort(3);
        packet.WriteInt(4);
        packet.WriteShort(1);
        packet.WriteShort(2);
        packet.WriteByte(0xFF);
    }
}

}
}*/