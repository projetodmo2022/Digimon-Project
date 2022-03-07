using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yggdrasil.Helpers;
using System.IO;
using Digital_World;


namespace Yggdrasil.Database
{
    public class UITextDB
    {
        public static Dictionary<long, UIText> UIText = new Dictionary<long, UIText>();

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

                        UIText text = new UIText();
                        text.ID_Maybe = read.ReadLong(); 
                        text.TextSize = read.ReadInt();
                        text.Text = read.ReadZString(Encoding.ASCII, text.TextSize);    
                        


                        if (!UIText.ContainsKey(text.ID_Maybe))
                        {
                            UIText.Add(text.ID_Maybe, text);
                        }

                    }
                }
            }
            SysCons.LogDB("UIText.bin", "Loaded {0} UIText.", UIText.Count);
        }
    }
   public class UIText
    {

        public long  ID_Maybe;
        public  int TextSize;
        public string  Text;




    }




}

