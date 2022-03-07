using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yggdrasil.Helpers;
using System.IO;
using Yggdrasil.Entities;
using Digital_World;


namespace Yggdrasil.Database
{
    public class EffectListDB
    {
        public static Dictionary<int, Effect> Effect = new Dictionary<int, Effect>();

        public static void Load(string fileName)
        {
            
            using (Stream s = File.OpenRead(fileName))
            {
                using (BitReader read = new BitReader(s))
                {
                    int count = read.ReadInt();
                    for (int i = 0; i < count; i++)
                    {
                        Effect effect = new Effect();
                        effect.dwItemCode = read.ReadInt();
                        int text = read.ReadInt();
                        effect.effectFile = read.ReadInt();
                        //effect.effectFile = effect.effectFile.Substring(0, effect.effectFile.IndexOf(".nif")+4);
                     
                        if (!Effect.ContainsKey(effect.dwItemCode))
                        {
                            Effect.Add(effect.dwItemCode, effect);
                        }

                    }
                }
            }
            SysCons.LogDB("EffectList.bin", "Loaded {0} EffectList.", Effect.Count);
        }
    }
    public class Effect
    {
       public int  dwItemCode;
       public int text;
       public int  effectFile;




    }



}

