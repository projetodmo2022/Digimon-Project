using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yggdrasil.Database;
using Yggdrasil.Entities;
using Yggdrasil.Helpers;
using Digital_World;

namespace Yggdrasil.Packets.Game
{
    public class Hatch : Packet
    {
        public Hatch(Digimon Mon, int slot)
        {
            packet.Type(1038);
            packet.WriteInt(slot);
            packet.WriteUInt(Mon.UID);
            packet.WriteInt(Mon.Species);
            packet.WriteString(Mon.Name);
            packet.WriteShort((short)Mon.Size);
            packet.WriteInt(Mon.EXP);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteShort((short)Mon.Level);
            DigimonData data = DigimonListDB.GetDigimon(Mon.Species);
            packet.WriteInt(data.HP);
            packet.WriteInt(data.DS);
            packet.WriteInt(data.DE);
            packet.WriteInt(data.AT);
            packet.WriteInt(data.HP);
            packet.WriteInt(data.DS);
            packet.WriteInt(0); //28
            packet.WriteInt(0);
            packet.WriteInt(data.EV);
            packet.WriteInt(data.CR);
            packet.WriteInt(data.MS);
            packet.WriteInt(data.AS);
            packet.WriteInt(data.AR);
            packet.WriteInt(data.HT);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(data.BL);
            packet.WriteByte((byte)Mon.Scale);
            packet.WriteInt(Mon.Species);
            packet.WriteByte((byte)Mon.Forms.Count);
            for (int i = 0; i < Mon.Forms.Count; i++)
            {
                EvolvedForm form = Mon.Forms[i];
                packet.WriteBytes(form.ToArray());
            }
            packet.WriteByte(1);
            packet.WriteBytes(new byte[29]);
        }
    }

    public class Archive : Packet
    {
        public Archive(int ArchiveSlots, Dictionary<int, Digimon> lDigis)
        {
            packet.Type(3204);
            packet.WriteInt(0);
            packet.WriteInt(ArchiveSlots);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            foreach (KeyValuePair<int, Digimon> kvp in lDigis)
            {
                Digimon Mon = kvp.Value;
                SysCons.LogInfo($"{Mon.Name} : {kvp.Key}");
                packet.WriteInt(kvp.Key);
                Digimon(Mon);
            }
            packet.WriteInt(1888);
        }

        private void Digimon(Digimon Mon)
        {
            packet.WriteUInt(Mon.UID);
            packet.WriteInt(Mon.Species);
            packet.WriteString(Mon.Name);
            packet.WriteShort((short)Mon.Size);
            packet.WriteInt(Mon.EXP);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteShort((short)Mon.Level);
            DigimonData data = DigimonListDB.GetDigimon(Mon.Species);
            packet.WriteInt(data.HP);
            packet.WriteInt(data.DS);
            packet.WriteInt(data.DE);
            packet.WriteInt(data.AT);
            packet.WriteInt(data.HP);
            packet.WriteInt(data.DS);
            packet.WriteInt(Mon.Stats.Intimacy);
            packet.WriteInt(data.BL);
            packet.WriteInt(data.EV);
            packet.WriteInt(data.CR);
            packet.WriteInt(data.CR);
            packet.WriteInt(data.AS);
            packet.WriteInt(data.AR);
            packet.WriteInt(data.HT);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteByte((byte)Mon.Scale);
            packet.WriteInt(Mon.Species);
            packet.WriteByte((byte)Mon.Forms.Count);
            for (int i = 0; i < Mon.Forms.Count; i++)
            {
                EvolvedForm form = Mon.Forms[i];
                form.skill_max_level = 10;
                packet.WriteBytes(form.ToArray());
            }
            packet.WriteInt(1);
            packet.WriteBytes(new byte[52]);
            packet.WriteUInt(Mon.UID);
            packet.WriteByte(0);
        }
    }
    public class StoreDigimon : Packet
    {
        public StoreDigimon(int Slot, int ArchiveSlot, int Bits)
        {
            packet.Type(3201);
            packet.WriteInt(Slot);
            packet.WriteInt(ArchiveSlot);
            packet.WriteInt(Bits);
        }
    }


    public class Switch : Packet
    {
        public Switch(uint DigimonHandle, byte slot, Digimon Mon1, Digimon Mon2)
        {
            packet.Type(1041);
            packet.WriteUInt(DigimonHandle);
            packet.WriteInt(Mon1.Species);
            packet.WriteByte(slot);
            packet.WriteInt(Mon2.Species);
            packet.WriteByte((byte)Mon2.Level);
            packet.WriteString(Mon2.Name);
            packet.WriteShort(Mon2.Size);
            packet.WriteInt(0);
            packet.WriteInt(1);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteShort(0);
            DigimonData data = DigimonListDB.GetDigimon(Mon2.Species);
            packet.WriteInt(data.HP);
            packet.WriteInt(data.DS);
            packet.WriteInt(data.DE);
            packet.WriteInt(data.AT);
            packet.WriteInt(data.HP);
            packet.WriteInt(data.DS);
            packet.WriteInt(Mon2.Stats.Intimacy);
            packet.WriteInt(data.BL);
            packet.WriteInt(data.EV);
            packet.WriteInt(data.CR);
            packet.WriteInt(data.AS);
            packet.WriteInt(data.AR);
            packet.WriteInt(data.HT);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(6834);
            packet.WriteByte(0);
        }
    }
}
