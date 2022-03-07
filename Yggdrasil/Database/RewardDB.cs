using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yggdrasil.Helpers;
using System.IO;
using Digital_World;


namespace Yggdrasil.Database
{
    public class RewardDB
    {
        public static Dictionary<int, sINFOReward> sinfoRewardList = new Dictionary<int, sINFOReward>();
         public static void Load(string fileName)
        {
            if (File.Exists(fileName) == false) return;
            using (Stream s = File.OpenRead(fileName))
            {
                using (BitReader read = new BitReader(s))
                {
                    int count = read.ReadInt();
                    for (int i = 0; i < count; i++)
                    {
                        //read.Seek((int)read.InnerStream.BaseStream.Position);
                        sINFOReward infoR = new sINFOReward();
                        infoR.s_nID = read.ReadUShort();
                        //infoR.s_nID = read.ReadUShort();
                        infoR.s_szCommnet = read.ReadZString(Encoding.ASCII);
                        infoR.s_nStartDate = read.ReadInt();
                        infoR.s_nExpDate = read.ReadInt();
                        infoR.s_nItemCode1 = read.ReadUInt();
                        infoR.s_nItemCount1 = read.ReadUShort();
                        sinfoRewardList.Add(infoR.s_nID, infoR);

                        if (!sinfoRewardList.ContainsKey(infoR.s_nID))
                        {
                            sinfoRewardList.Add(infoR.s_nID, infoR);
                        }

                    }
                }
            }
            SysCons.LogDB("Reward.bin", "Loaded {0} Rewards.", sinfoRewardList.Count);
        }

    }

    public class sINFOReward
    {
        public ushort s_nID;
        public string s_szCommnet;
        //public char s_nStartDate;
        //public char s_nExpDate;
        public int s_nStartDate;
        public int s_nExpDate;
        public uint s_nItemCode1;
        public ushort s_nItemCount1;
    }
}
