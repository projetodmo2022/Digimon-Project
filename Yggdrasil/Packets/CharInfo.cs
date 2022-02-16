using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yggdrasil.Entities;
using Yggdrasil.Database;
using Yggdrasil.Helpers;
using System.IO;

namespace Yggdrasil.Packets.Game
{
    public class CharInfo : Packet
    {
        public CharInfo()
        {
            packet.Type(3133);
            packet.WriteByte(0);
            packet.WriteShort(0x010C);
        }
        public CharInfo(Character Tamer)
        {
            packet.Type(0x03EB);
            packet.WriteInt(1);
            packet.WriteInt(Tamer.Location.PosX);
            packet.WriteInt(Tamer.Location.PosY);
            packet.WriteUInt(Tamer.UID);
            packet.WriteInt((int)Tamer.Model);
            packet.WriteString(Tamer.Name);
            packet.WriteInt(Tamer.EXP);
            packet.WriteInt(0);
            packet.WriteShort((short)Tamer.Level);
            packet.WriteInt(Tamer.MaxHP); // Max HP
            packet.WriteInt(Tamer.MaxDS); // Max DS
            packet.WriteInt(Tamer.HP); // HP
            packet.WriteInt(Tamer.DS); // DS
            packet.WriteInt(Tamer.Fatigue); // Fatigue
            packet.WriteInt(Tamer.AT); // AT
            packet.WriteInt(Tamer.DE); // DE
            packet.WriteInt(Tamer.MS); // MS
            for (int i = 0; i < 13; i++)
            {
                Item item = Tamer.Equipment[i];
                packet.WriteBytes(item.ToArray());
            }
            packet.WriteBytes(Tamer.ChipSets.ToArray());
            packet.WriteBytes(Tamer.Equipment[13].ToArray());
            packet.WriteBytes(ToByteArray("0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000280610000000000000000000000000000000000000000080030000003C00310000000000000080FB0104180300000000FFFFBFF9FDE36F63FFFF633E010000000C0006C0000000000000000000000000000000000000000000008200A00030008288700000000000000000EC0000000000000000FEFFFF7F0AC0813040BF61B002EE06F80F3C30100000001CFE10000000002006000000000400000000FCFFFF1FF0BFFF6F0000F00F08FEFF3F05FCFF3F30020000BC05B8030080FFF7DF7733F031707D3F8054308B1A2230E6C000000201000040008000000000008980687AC00600000000000E001800000003200010000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000F8FF1F0080FFFFFF7F80FFFFFF1FF03F00000000000000000000000000000000407310FC9F0100FFA7F7E7FBDCA2C83FCFBA0E00ECF9CF9D671F00400400000000EF01F8D7EFEEFFEBFFFCEBDBBDFFFFBFFCBE010000FC0000000000F87F00E06EB77AEFFDFFFFFFBFFFFF7FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF0FFCFFFFFFF9FF0FFF03FFFFFFFFFFFFF0FFFFFFFF0B00000000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF0F780000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000F8FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF0F0000F0FFFBDEAFEAF7BFFBF7EBFF7FFF0F0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000"));
            packet.WriteInt(Tamer.Incubator);
            packet.WriteInt(Tamer.IncubatorLevel);
            packet.WriteInt(-1);
            packet.WriteInt(Tamer.IncubatorBackup);
            packet.WriteInt(-1);
            Digimon_Partner(Tamer, Tamer.Partner, 0);
            int Mons = 1;
            for (int i = 1; i < 5; i++)
            {
                if (Tamer.DigimonList[i] != null)
                {
                    Mons++;
                    Digimon_Mercs(Tamer.DigimonList[i], i);
                }

            }
            packet.WriteByte(0x63);
            packet.WriteInt(0);
            packet.WriteShort(1); //Channel
            packet.WriteShort(0);
            packet.WriteByte(0);
            packet.WriteShort(0x87);
            packet.WriteByte(0);
            packet.WriteInt(0x81);
            packet.WriteInt(0);
            packet.WriteShort(0);
            packet.WriteInt(32927);
            packet.WriteBytes(new byte[29]);
            packet.WriteInt(65409);
            packet.WriteInt(0);
            packet.WriteInt(0x81);
            packet.WriteBytes(new byte[148]);
            packet.WriteByte(0x63);
            packet.WriteInt(Tamer.CurrentTitle);
            packet.WriteBytes(new byte[158]);
            packet.WriteInt(4867);
            packet.WriteInt(0);
            packet.WriteByte(0);
            packet.WriteInt(20);
            packet.WriteInt(0);
            packet.WriteInt(21);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);

        }
        private static byte[] ToByteArray(String HexString)
        {
            int NumberChars = HexString.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(HexString.Substring(i, 2), 16);
            }
            return bytes;
        }
        private void Digimon_Partner(Character Tamer, Digimon Mon, int slot)
        {
            packet.WriteShort(0);
            packet.WriteByte((byte)Tamer.mercenaryLimit); //SLOTS OPEN FOR MERC!
            packet.WriteUInt(Tamer.DigimonUID);
            packet.WriteInt(Mon.CurrentForm);
            //packet.WriteByte(0);
            //packet.WriteByte(0x80);
            packet.WriteString(Mon.Name);
            packet.WriteByte((byte)Mon.Scale); // Scale
            packet.WriteShort(Mon.Size); //Partner Size
            packet.WriteInt64(Mon.EXP); //Partner EXP - > Mon.EXP
            packet.WriteInt64(0);
            packet.WriteShort((short)Mon.Level); //Partner Level
            packet.WriteInt(Mon.Stats.MaxHP); // Max HP
            packet.WriteInt(Mon.Stats.MaxDS); // Max MP
            packet.WriteInt(Mon.Stats.DE); //DE
            packet.WriteInt(Mon.Stats.AT); //AT
            packet.WriteInt(Mon.Stats.HP); // HP
            packet.WriteInt(Mon.Stats.DS); // MP
            packet.WriteInt(Mon.Stats.Intimacy); // Intimacy
            packet.WriteInt(0); //BL
            packet.WriteInt(Mon.Stats.EV);
            packet.WriteInt(Mon.Stats.CR);
            packet.WriteInt(Mon.Stats.MS);
            packet.WriteInt(Mon.Stats.AS);
            packet.WriteInt(80);
            packet.WriteInt(Mon.Stats.HT); //HT //EV //CR //MS //AS
            packet.WriteShort(0);
            packet.WriteShort(0);
            packet.WriteShort(0);
            packet.WriteShort(0);
            packet.WriteShort(0);
            packet.WriteShort(0);
            packet.WriteShort(0);
            packet.WriteShort(0);
            packet.WriteInt(Mon.Species);
            packet.WriteByte((byte)Mon.Forms.Count);
            for (int i = 0; i < Mon.Forms.Count; i++)
            {
                EvolvedForm form = Mon.Forms[i];


                //EVOLUTER
                //UNLOCKED EVOLUTIONS 
                if (Mon.levels_unlocked > i)
                {

                    form.skill_max_level = 0xA;

                }
                else
                //DEFAULT NOT UNLOCKED
                {

                    form.skill_max_level = 0x01;

                }
                form.skill_points = 0x0A; //SKILL POINTS - > 10
                form.skill1_level = 0x01; //1ST SKILL LEVEL
                form.skill2_level = 0x01; //2ND SKILL LEVEL 
                form.skill3_level = 0x01; //2ND SKILL LEVEL 
                form.skill4_level = 0x01; //2ND SKILL LEVEL 
                form.skill5_level = 0x01; //2ND SKILL LEVEL 

                packet.WriteBytes(form.ToArray());



            }
            //75 bytes

            packet.WriteInt(1);
            packet.WriteBytes(new byte[26]);
            packet.WriteShort(1);
            packet.WriteInt(105756);
            packet.WriteInt(1613731706);
            packet.WriteInt(8000421);
            packet.WriteBytes(new byte[26]);
            packet.WriteInt(17303);
            packet.WriteByte(0);
            //File.WriteAllBytes("C:\\testt.packet", BitConverter.GetBytes(Mon.Model));

        }


        private void Digimon_Mercs(Digimon Mon, int slot)
        {
            packet.WriteByte((byte)slot); //SLOT
            packet.WriteUInt(Mon.UID); // 17302 //17201
            packet.WriteInt(Mon.Species);
            packet.WriteString(Mon.Name);
            packet.WriteByte((byte)Mon.Scale); // Scale
            packet.WriteShort(Mon.Size); //Partner Size
            packet.WriteInt64(Mon.EXP); //Partner EXP - > Mon.EXP
            packet.WriteInt64(0);
            packet.WriteShort((short)Mon.Level); //Partner Level
            packet.WriteInt(Mon.Stats.MaxHP); // Max HP
            packet.WriteInt(Mon.Stats.MaxDS); // Max MP
            packet.WriteInt(Mon.Stats.DE); //DE
            packet.WriteInt(Mon.Stats.AT); //AT
            packet.WriteInt(Mon.Stats.HP); // HP
            packet.WriteInt(Mon.Stats.DS); // MP
            packet.WriteInt(Mon.Stats.Intimacy); // Intimacy
            packet.WriteInt(0); //BL
            packet.WriteInt(Mon.Stats.EV);
            packet.WriteInt(Mon.Stats.CR);
            packet.WriteInt(Mon.Stats.MS);
            packet.WriteInt(Mon.Stats.AS);
            packet.WriteInt(80);
            packet.WriteInt(Mon.Stats.HT); //HT //EV //CR //MS //AS
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(Mon.Species);
            packet.WriteByte((byte)Mon.Forms.Count);
            for (int i = 0; i < Mon.Forms.Count; i++)
            {
                EvolvedForm form = Mon.Forms[i];

                //EVOLUTER


                packet.WriteBytes(form.ToArray());



            }
            //75 bytes
            packet.WriteInt(1);
            packet.WriteBytes(new byte[52]);
            packet.WriteInt(16404);
            packet.WriteByte(0);

        }
    }
}



