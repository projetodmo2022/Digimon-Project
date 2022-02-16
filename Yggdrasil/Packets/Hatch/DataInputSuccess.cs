using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yggdrasil.Packets.Game
{
    public class DataInputSuccess : Packet
    {
        /// <summary>
        /// Data Input success
        /// </summary>
        /// <param name="Handle"></param>
        public DataInputSuccess(uint Handle)
        {
            packet.Type(1037); //1040
            packet.WriteUInt(Handle);
            packet.WriteByte(1);
        }

        /// <summary>
        /// Data Input success. Allow the egg to hatch.
        /// </summary>
        /// <param name="Handle">Tamer UID</param>
        /// <param name="Scale">Digimon Scale</param>
        public DataInputSuccess(uint Handle, byte Scale)
        {
            packet.Type(1037);
            packet.WriteUInt(Handle);
            packet.WriteByte(Scale);
        }
    }
}