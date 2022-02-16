using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yggdrasil.Database;
using Yggdrasil.Entities;
using Yggdrasil.Helpers;

namespace Yggdrasil.Packets.Game
{
    public class UpdateStats:Packet
    {
        public UpdateStats(Character Tamer)
        {
            packet.WriteInt(Tamer.MaxHP);
            packet.WriteInt(Tamer.MaxDS);
            packet.WriteInt(Tamer.HP);
            packet.WriteInt(Tamer.DS);
            packet.WriteShort((short)Tamer.AT);
            packet.WriteShort((short)Tamer.DE);
            packet.WriteShort((short)Tamer.MS);
            packet.WriteInt(Tamer.Partner.Stats.HP);
            packet.WriteInt(Tamer.Partner.Stats.DS);
            packet.WriteInt(Tamer.Partner.Stats.HP);
            packet.WriteInt(Tamer.Partner.Stats.DS);
            packet.WriteShort(Tamer.Partner.Stats.Intimacy);
            packet.WriteShort(Tamer.Partner.Stats.AT);
            packet.WriteShort(Tamer.Partner.Stats.DE);
            packet.WriteShort(Tamer.Partner.Stats.CR);
            packet.WriteShort(Tamer.Partner.Stats.AS);
            packet.WriteShort(Tamer.Partner.Stats.EV);
            packet.WriteShort(Tamer.Partner.Stats.HT);
            packet.WriteShort(Tamer.Partner.Stats.AR);
            packet.WriteShort(Tamer.Partner.Stats.BL);
            packet.WriteShort(1); // -----
            packet.WriteShort(0); //AT
            packet.WriteShort(0); //--------
            packet.WriteShort(0); //CT
            packet.WriteShort(0); //AS
            packet.WriteShort(0); //EV
            packet.WriteShort(0);
            packet.WriteShort(0); //HP
            packet.WriteShort(0); //CLONE EXAMPLE 15/15
            packet.WriteShort(0); //CLONE EXAMPLE 15/15 ---
            packet.WriteShort(0); //CLONE EXAMPLE 15/15
            packet.WriteShort(0);
            packet.WriteShort(0); //CLONE EXAMPLE 15/15
            packet.WriteShort(0);
            packet.WriteShort(0); //CLONE EXAMPLE 15/15
        }

        public UpdateStats(Character Tamer, Digimon Partner)
        {
            packet.Type(1043);
            packet.WriteInt(Tamer.MaxHP);
            packet.WriteInt(Tamer.MaxDS);
            packet.WriteInt(Tamer.HP);
            packet.WriteInt(Tamer.DS);
            packet.WriteShort((short)Tamer.AT);
            packet.WriteShort((short)Tamer.DE);
            packet.WriteShort((short)Tamer.MS);
            packet.WriteInt(Partner.Stats.MaxHP);
            packet.WriteInt(Partner.Stats.MaxDS);
            packet.WriteInt(Partner.Stats.HP);
            packet.WriteInt(Partner.Stats.DS);
            packet.WriteShort(Partner.Stats.Intimacy);
            packet.WriteShort(Partner.Stats.AT);
            packet.WriteShort(Partner.Stats.DE);
            packet.WriteShort(Partner.Stats.CR);
            packet.WriteShort(Partner.Stats.AS);
            packet.WriteShort(Partner.Stats.EV);
            packet.WriteShort(Partner.Stats.HT);
            packet.WriteShort(0);
            packet.WriteShort(0);
            packet.WriteShort(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
        }

        public UpdateStats(Character Tamer, DigimonData data)
        {
            packet.Type(1043);
            packet.WriteInt(Tamer.MaxHP);
            packet.WriteInt(Tamer.MaxDS);
            packet.WriteInt(Tamer.HP);
            packet.WriteInt(Tamer.DS);
            packet.WriteShort((short)Tamer.AT);
            packet.WriteShort((short)Tamer.DE);
            packet.WriteShort((short)Tamer.MS);
            packet.WriteInt(data.HP);
            packet.WriteInt(data.DS);
            packet.WriteInt(data.HP);
            packet.WriteInt(data.DS);
            packet.WriteShort(Tamer.Partner.Stats.Intimacy);
            packet.WriteShort(data.AT);
            packet.WriteShort(data.DE);
            packet.WriteShort(data.CR);
            packet.WriteShort(data.AS);
            packet.WriteShort(data.EV);
            packet.WriteShort(data.HT);
            packet.WriteShort(data.AR);
            packet.WriteShort(data.BL);
            packet.WriteShort(1); // -----
            packet.WriteShort(0); //AT
            packet.WriteShort(0); //--------
            packet.WriteShort(0); //CT
            packet.WriteShort(0); //AS
            packet.WriteShort(0); //EV
            packet.WriteShort(0);
            packet.WriteShort(0); //HP
            packet.WriteShort(0); //CLONE EXAMPLE 15/15
            packet.WriteShort(0); //CLONE EXAMPLE 15/15 ---
            packet.WriteShort(0); //CLONE EXAMPLE 15/15
            packet.WriteShort(0);
            packet.WriteShort(0); //CLONE EXAMPLE 15/15
            packet.WriteShort(0);
            packet.WriteShort(0); //CLONE EXAMPLE 15/15
        }


        public UpdateStats(Character Tamer, Digimon Partner, DigimonStats Stats)
        {
            packet.Type(1043);
            packet.WriteInt(Tamer.MaxHP);
            packet.WriteInt(Tamer.MaxDS);
            packet.WriteInt(Tamer.HP);
            packet.WriteInt(Tamer.DS);
            packet.WriteShort((short)Tamer.AT);
            packet.WriteShort((short)Tamer.DE);
            packet.WriteShort((short)Tamer.MS);
            packet.WriteInt(Stats.MaxHP);
            packet.WriteInt(Stats.MaxDS);
            packet.WriteInt(Stats.HP);
            packet.WriteInt(Stats.DS);
            packet.WriteShort(Partner.Stats.Intimacy);
            packet.WriteShort(Stats.AT);
            packet.WriteShort(Stats.DE);
            packet.WriteShort(Stats.CR);
            packet.WriteShort(Stats.AS);
            packet.WriteShort(Stats.EV);
            packet.WriteShort(Stats.HT);
            packet.WriteShort(0);
            packet.WriteShort(1);
            packet.WriteShort(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
        }
    }
}
