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
    public class CashShopDB
    {
        public static Dictionary<int, CASHINFO> CashShop = new Dictionary<int, CASHINFO>();

        public static void Load(string fileName)
        {
            if (File.Exists(fileName) == false) return;
            using (Stream s = File.OpenRead(fileName))
            {
                using (BitReader read = new BitReader(s))
                {
                    read.Seek(334);
                    int count1 = read.ReadInt();
                    //Console.WriteLine(count1+"Count");
                    for (int i = 0; i < count1; i++)
                    {
                        CASHINFO m = new CASHINFO();
                        //Console.WriteLine(i + "Valor de i");
                        m.cashshop_id = read.ReadInt();
                        //Console.WriteLine(m.cashshop_id + "ID");
                        int ccount1 = read.ReadInt();
                        //Console.WriteLine(ccount1 + "count");
                        for (int c = 0; c < ccount1; c++)
                        {

                            int NameSize = read.ReadInt();
                            //Console.WriteLine(NameSize + "=Nome");
                            read.Skip(NameSize * 2);
                            int DesSize = read.ReadInt();
                            read.Skip(DesSize * 2);
                            m.Enabled = read.ReadByte();
                            //Console.WriteLine(m.Enabled + " = Enable");
                            m.unique_id = read.ReadInt();
                            //Console.WriteLine(m.unique_id + "=Nome");
                            //m.Desc = read.ReadZString(Encoding.Unicode, 64);
                            //m.unique_id = read.ReadInt();
                            string Date1 = read.ReadZString(Encoding.ASCII, 64);
                            string Date2 = read.ReadZString(Encoding.ASCII, 64);

                            int prices = read.ReadInt();

                            m.price = new int[prices];

                            for (int k = 0; k < prices; k++)
                            {
                                m.price[k] = read.ReadInt();
                            }

                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadShort();
                            int ItemCount = read.ReadInt(); m.ItemCount = ItemCount; m.ItemCount = ItemCount;

                            m.Item = new int[][] { new int[ItemCount], new int[ItemCount] };


                            for (int g = 0; g < ItemCount; g++)
                            {
                                m.Item[0][g] = read.ReadInt();
                                m.Item[1][g] = read.ReadInt();
                            }
                            CashShop.Add(m.unique_id, m);
                        }

                        //Console.WriteLine($"{m.Name} | {m.unique_id} | {m.id}");
                        //Console.WriteLine($"{countxd} | {m.cashshop_id} | {m.Name} | {m.Desc} | {m.unique_id}");

                        //CashShop.Add(m.unique_id, m);
                    }

                    int EXPu1 = read.ReadInt();
                    int EXPu2 = read.ReadInt();
                    for (int i = 0; i < EXPu2 * 2; i++)
                    {
                        read.ReadByte();
                    }
                    int EXPcount = read.ReadInt();
                    for (int i = 0; i < EXPcount; i++)
                    {
                        CASHINFO m = new CASHINFO();
                        //Console.WriteLine(i);
                        m.cashshop_id = read.ReadInt();
                        int ccount1 = read.ReadInt();

                        for (int c = 0; c < ccount1; c++)
                        {

                            int NameSize = read.ReadInt();
                            read.Skip(NameSize * 2);
                            int DesSize = read.ReadInt();
                            read.Skip(DesSize * 2);
                            m.Enabled = read.ReadByte();
                            m.unique_id = read.ReadInt();
                            //m.Desc = read.ReadZString(Encoding.Unicode, 64);
                            //m.unique_id = read.ReadInt();
                            string Date1 = read.ReadZString(Encoding.ASCII, 64);
                            string Date2 = read.ReadZString(Encoding.ASCII, 64);

                            int prices = read.ReadInt();

                            m.price = new int[prices];

                            for (int k = 0; k < prices; k++)
                            {
                                m.price[k] = read.ReadInt();
                            }

                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadShort();
                            int ItemCount = read.ReadInt(); m.ItemCount = ItemCount;

                            m.Item = new int[][] { new int[ItemCount], new int[ItemCount] };


                            for (int g = 0; g < ItemCount; g++)
                            {
                                m.Item[0][g] = read.ReadInt();
                                m.Item[1][g] = read.ReadInt();
                            }

                            //Console.WriteLine($"{NameSize} : {m.Name} : {prices} : {m.unique_id} : {Date1} : {Date2} : {prices} : {ItemCount}");
                            CashShop.Add(m.unique_id, m);
                        }

                        // Console.WriteLine($"{m.Name} | {m.unique_id} | {m.id} | {m.amount}");
                        //Console.WriteLine($"{countxd} | {m.cashshop_id} | {m.Name} | {m.Desc} | {m.unique_id}");

                        //CashShop.Add(m.unique_id, m);
                    }
                    int Movementu1 = read.ReadInt();
                    int Movementu2 = read.ReadInt();
                    for (int i = 0; i < Movementu2 * 2; i++)
                    {
                        read.ReadByte();
                    }
                    int MovementCount = read.ReadInt();
                    for (int i = 0; i < MovementCount; i++)
                    {
                        CASHINFO m = new CASHINFO();
                        //Console.WriteLine(i);
                        m.cashshop_id = read.ReadInt();
                        int ccount1 = read.ReadInt();

                        for (int c = 0; c < ccount1; c++)
                        {

                            int NameSize = read.ReadInt();
                            read.Skip(NameSize * 2);
                            int DesSize = read.ReadInt();
                            read.Skip(DesSize * 2);
                            m.Enabled = read.ReadByte();
                            m.unique_id = read.ReadInt();
                            //m.Desc = read.ReadZString(Encoding.Unicode, 64);
                            //m.unique_id = read.ReadInt();
                            string Date1 = read.ReadZString(Encoding.ASCII, 64);
                            string Date2 = read.ReadZString(Encoding.ASCII, 64);

                            int prices = read.ReadInt();

                            m.price = new int[prices];

                            for (int k = 0; k < prices; k++)
                            {
                                m.price[k] = read.ReadInt();
                            }

                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadShort();
                            int ItemCount = read.ReadInt(); m.ItemCount = ItemCount;

                            m.Item = new int[][] { new int[ItemCount], new int[ItemCount] };


                            for (int g = 0; g < ItemCount; g++)
                            {
                                m.Item[0][g] = read.ReadInt();
                                m.Item[1][g] = read.ReadInt();
                            }

                            //Console.WriteLine($"{NameSize} : {m.Name} : {prices} : {m.unique_id} : {Date1} : {Date2} : {prices} : {ItemCount}");
                            CashShop.Add(m.unique_id, m);
                        }

                        // Console.WriteLine($"{m.Name} | {m.unique_id} | {m.id} | {m.amount}");
                        //Console.WriteLine($"{countxd} | {m.cashshop_id} | {m.Name} | {m.Desc} | {m.unique_id}");

                        //CashShop.Add(m.unique_id, m);
                    }
                    int ChatShopu1 = read.ReadInt();
                    int ChatShopu2 = read.ReadInt();
                    for (int i = 0; i < ChatShopu2 * 2; i++)
                    {
                        read.ReadByte();
                    }
                    int CashShopCount = read.ReadInt();
                    for (int i = 0; i < CashShopCount; i++)
                    {
                        CASHINFO m = new CASHINFO();
                        //Console.WriteLine(i);
                        m.cashshop_id = read.ReadInt();
                        int ccount1 = read.ReadInt();

                        for (int c = 0; c < ccount1; c++)
                        {

                            int NameSize = read.ReadInt();
                            read.Skip(NameSize * 2);
                            int DesSize = read.ReadInt();
                            read.Skip(DesSize * 2);
                            m.Enabled = read.ReadByte();
                            m.unique_id = read.ReadInt();
                            //m.Desc = read.ReadZString(Encoding.Unicode, 64);
                            //m.unique_id = read.ReadInt();
                            string Date1 = read.ReadZString(Encoding.ASCII, 64);
                            string Date2 = read.ReadZString(Encoding.ASCII, 64);

                            int prices = read.ReadInt();

                            m.price = new int[prices];

                            for (int k = 0; k < prices; k++)
                            {
                                m.price[k] = read.ReadInt();
                            }

                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadShort();
                            int ItemCount = read.ReadInt(); m.ItemCount = ItemCount;

                            m.Item = new int[][] { new int[ItemCount], new int[ItemCount] };


                            for (int g = 0; g < ItemCount; g++)
                            {
                                m.Item[0][g] = read.ReadInt();
                                m.Item[1][g] = read.ReadInt();
                            }

                            //Console.WriteLine($"{NameSize} : {m.Name} : {prices} : {m.unique_id} : {Date1} : {Date2} : {prices} : {ItemCount}");
                            CashShop.Add(m.unique_id, m);
                        }

                        // Console.WriteLine($"{m.Name} | {m.unique_id} | {m.id} | {m.amount}");
                        //Console.WriteLine($"{countxd} | {m.cashshop_id} | {m.Name} | {m.Desc} | {m.unique_id}");

                        //CashShop.Add(m.unique_id, m);
                    }
                    int ETCu1 = read.ReadInt();
                    int ETCu2 = read.ReadInt();
                    for (int i = 0; i < ETCu2 * 2; i++)
                    {
                        read.ReadByte();
                    }
                    int ETCCount = read.ReadInt();
                    for (int i = 0; i < ETCCount; i++)
                    {
                        CASHINFO m = new CASHINFO();
                        //Console.WriteLine(i);
                        m.cashshop_id = read.ReadInt();
                        int ccount1 = read.ReadInt();

                        for (int c = 0; c < ccount1; c++)
                        {

                            int NameSize = read.ReadInt();
                            read.Skip(NameSize * 2);
                            int DesSize = read.ReadInt();
                            read.Skip(DesSize * 2);
                            m.Enabled = read.ReadByte();
                            m.unique_id = read.ReadInt();

                            //m.Desc = read.ReadZString(Encoding.Unicode, 64);
                            //m.unique_id = read.ReadInt();
                            string Date1 = read.ReadZString(Encoding.ASCII, 64);
                            string Date2 = read.ReadZString(Encoding.ASCII, 64);

                            int prices = read.ReadInt();

                            m.price = new int[prices];

                            for (int k = 0; k < prices; k++)
                            {
                                m.price[k] = read.ReadInt();
                            }

                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadShort();
                            int ItemCount = read.ReadInt(); m.ItemCount = ItemCount;

                            m.Item = new int[][] { new int[ItemCount], new int[ItemCount] };


                            for (int g = 0; g < ItemCount; g++)
                            {
                                m.Item[0][g] = read.ReadInt();
                                m.Item[1][g] = read.ReadInt();
                            }

                            //Console.WriteLine($"{NameSize} : {m.Name} : {prices} : {m.unique_id} : {Date1} : {Date2} : {prices} : {ItemCount}");
                            CashShop.Add(m.unique_id, m);
                        }

                        // Console.WriteLine($"{m.Name} | {m.unique_id} | {m.id} | {m.amount}");
                        //Console.WriteLine($"{countxd} | {m.cashshop_id} | {m.Name} | {m.Desc} | {m.unique_id}");

                        //CashShop.Add(m.unique_id, m);
                    }
                    read.Skip(44);
                    int DigiEggu1 = read.ReadInt();
                    int DigiEggu2 = read.ReadInt();
                    for (int i = 0; i < DigiEggu2 * 2; i++)
                    {
                        read.ReadByte();
                    }
                    int DigiEggC = read.ReadInt();
                    for (int i = 0; i < DigiEggC; i++)
                    {
                        CASHINFO m = new CASHINFO();
                        //Console.WriteLine(i);
                        m.cashshop_id = read.ReadInt();
                        int ccount1 = read.ReadInt();

                        for (int c = 0; c < ccount1; c++)
                        {

                            int NameSize = read.ReadInt();
                            read.Skip(NameSize * 2);
                            int DesSize = read.ReadInt();
                            read.Skip(DesSize * 2);
                            m.Enabled = read.ReadByte();
                            m.unique_id = read.ReadInt();
                            //m.Desc = read.ReadZString(Encoding.Unicode, 64);
                            //m.unique_id = read.ReadInt();
                            string Date1 = read.ReadZString(Encoding.ASCII, 64);
                            string Date2 = read.ReadZString(Encoding.ASCII, 64);

                            int prices = read.ReadInt();

                            m.price = new int[prices];

                            for (int k = 0; k < prices; k++)
                            {
                                m.price[k] = read.ReadInt();
                            }

                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadShort();
                            int ItemCount = read.ReadInt(); m.ItemCount = ItemCount;

                            m.Item = new int[][] { new int[ItemCount], new int[ItemCount] };


                            for (int g = 0; g < ItemCount; g++)
                            {
                                m.Item[0][g] = read.ReadInt();
                                m.Item[1][g] = read.ReadInt();
                            }

                            //Console.WriteLine($"{NameSize} : {m.Name} : {prices} : {m.unique_id} : {Date1} : {Date2} : {prices} : {ItemCount}");
                            CashShop.Add(m.unique_id, m);
                        }

                        // Console.WriteLine($"{m.Name} | {m.unique_id} | {m.id} | {m.amount}");
                        //Console.WriteLine($"{countxd} | {m.cashshop_id} | {m.Name} | {m.Desc} | {m.unique_id}");

                        //CashShop.Add(m.unique_id, m);
                    }
                    int Evolutionu1 = read.ReadInt();
                    int Evolutionu2 = read.ReadInt();
                    for (int i = 0; i < Evolutionu2 * 2; i++)
                    {
                        read.ReadByte();
                    }
                    int EvolutionC = read.ReadInt();
                    for (int i = 0; i < EvolutionC; i++)
                    {
                        CASHINFO m = new CASHINFO();
                        //Console.WriteLine(i);
                        m.cashshop_id = read.ReadInt();
                        int ccount1 = read.ReadInt();

                        for (int c = 0; c < ccount1; c++)
                        {

                            int NameSize = read.ReadInt();
                            read.Skip(NameSize * 2);
                            int DesSize = read.ReadInt();
                            read.Skip(DesSize * 2);
                            m.Enabled = read.ReadByte();
                            m.unique_id = read.ReadInt();
                            //m.Desc = read.ReadZString(Encoding.Unicode, 64);
                            //m.unique_id = read.ReadInt();
                            string Date1 = read.ReadZString(Encoding.ASCII, 64);
                            string Date2 = read.ReadZString(Encoding.ASCII, 64);

                            int prices = read.ReadInt();

                            m.price = new int[prices];

                            for (int k = 0; k < prices; k++)
                            {
                                m.price[k] = read.ReadInt();
                            }

                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadShort();
                            int ItemCount = read.ReadInt(); m.ItemCount = ItemCount;

                            m.Item = new int[][] { new int[ItemCount], new int[ItemCount] };


                            for (int g = 0; g < ItemCount; g++)
                            {
                                m.Item[0][g] = read.ReadInt();
                                m.Item[1][g] = read.ReadInt();
                            }

                            //Console.WriteLine($"{NameSize} : {m.Name} : {prices} : {m.unique_id} : {Date1} : {Date2} : {prices} : {ItemCount}");
                            CashShop.Add(m.unique_id, m);
                        }

                        // Console.WriteLine($"{m.Name} | {m.unique_id} | {m.id} | {m.amount}");
                        //Console.WriteLine($"{countxd} | {m.cashshop_id} | {m.Name} | {m.Desc} | {m.unique_id}");

                        //CashShop.Add(m.unique_id, m);
                    }
                    int HatchTransu1 = read.ReadInt();
                    int HatchTransu2 = read.ReadInt();
                    for (int i = 0; i < HatchTransu2 * 2; i++)
                    {
                        read.ReadByte();
                    }
                    int HatchTransC = read.ReadInt();
                    for (int i = 0; i < HatchTransC; i++)
                    {
                        CASHINFO m = new CASHINFO();
                        //Console.WriteLine(i);
                        m.cashshop_id = read.ReadInt();
                        int ccount1 = read.ReadInt();

                        for (int c = 0; c < ccount1; c++)
                        {

                            int NameSize = read.ReadInt();
                            read.Skip(NameSize * 2);
                            int DesSize = read.ReadInt();
                            read.Skip(DesSize * 2);
                            m.Enabled = read.ReadByte();
                            m.unique_id = read.ReadInt();
                            //m.Desc = read.ReadZString(Encoding.Unicode, 64);
                            //m.unique_id = read.ReadInt();
                            string Date1 = read.ReadZString(Encoding.ASCII, 64);
                            string Date2 = read.ReadZString(Encoding.ASCII, 64);

                            int prices = read.ReadInt();

                            m.price = new int[prices];

                            for (int k = 0; k < prices; k++)
                            {
                                m.price[k] = read.ReadInt();
                            }

                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadShort();
                            int ItemCount = read.ReadInt(); m.ItemCount = ItemCount;

                            m.Item = new int[][] { new int[ItemCount], new int[ItemCount] };


                            for (int g = 0; g < ItemCount; g++)
                            {
                                m.Item[0][g] = read.ReadInt();
                                m.Item[1][g] = read.ReadInt();
                            }

                            //Console.WriteLine($"{NameSize} : {m.Name} : {prices} : {m.unique_id} : {Date1} : {Date2} : {prices} : {ItemCount}");
                            CashShop.Add(m.unique_id, m);
                        }

                        // Console.WriteLine($"{m.Name} | {m.unique_id} | {m.id} | {m.amount}");
                        //Console.WriteLine($"{countxd} | {m.cashshop_id} | {m.Name} | {m.Desc} | {m.unique_id}");

                        //CashShop.Add(m.unique_id, m);
                    }
                    int Reinforceu1 = read.ReadInt();
                    int Reinforceu2 = read.ReadInt();
                    for (int i = 0; i < Reinforceu2 * 2; i++)
                    {
                        read.ReadByte();
                    }
                    int ReinforceC = read.ReadInt();
                    for (int i = 0; i < ReinforceC; i++)
                    {
                        CASHINFO m = new CASHINFO();
                        //Console.WriteLine(i);
                        m.cashshop_id = read.ReadInt();
                        int ccount1 = read.ReadInt();

                        for (int c = 0; c < ccount1; c++)
                        {

                            int NameSize = read.ReadInt();
                            read.Skip(NameSize * 2);
                            int DesSize = read.ReadInt();
                            read.Skip(DesSize * 2);
                            m.Enabled = read.ReadByte();
                            m.unique_id = read.ReadInt();
                            //m.Desc = read.ReadZString(Encoding.Unicode, 64);
                            //m.unique_id = read.ReadInt();
                            string Date1 = read.ReadZString(Encoding.ASCII, 64);
                            string Date2 = read.ReadZString(Encoding.ASCII, 64);

                            int prices = read.ReadInt();

                            m.price = new int[prices];

                            for (int k = 0; k < prices; k++)
                            {
                                m.price[k] = read.ReadInt();
                            }

                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadShort();
                            int ItemCount = read.ReadInt(); m.ItemCount = ItemCount;

                            m.Item = new int[][] { new int[ItemCount], new int[ItemCount] };


                            for (int g = 0; g < ItemCount; g++)
                            {
                                m.Item[0][g] = read.ReadInt();
                                m.Item[1][g] = read.ReadInt();
                            }

                            //Console.WriteLine($"{NameSize} : {m.Name} : {prices} : {m.unique_id} : {Date1} : {Date2} : {prices} : {ItemCount}");
                            CashShop.Add(m.unique_id, m);
                        }

                        // Console.WriteLine($"{m.Name} | {m.unique_id} | {m.id} | {m.amount}");
                        //Console.WriteLine($"{countxd} | {m.cashshop_id} | {m.Name} | {m.Desc} | {m.unique_id}");

                        //CashShop.Add(m.unique_id, m);
                    }
                    int RidingU1 = read.ReadInt();
                    int RidingU2 = read.ReadInt();
                    for (int i = 0; i < RidingU2 * 2; i++)
                    {
                        read.ReadByte();
                    }
                    int RidingC = read.ReadInt();
                    for (int i = 0; i < RidingC; i++)
                    {
                        CASHINFO m = new CASHINFO();
                        //Console.WriteLine(i);
                        m.cashshop_id = read.ReadInt();
                        int ccount1 = read.ReadInt();

                        for (int c = 0; c < ccount1; c++)
                        {

                            int NameSize = read.ReadInt();
                            read.Skip(NameSize * 2);
                            int DesSize = read.ReadInt();
                            read.Skip(DesSize * 2);
                            m.Enabled = read.ReadByte();
                            m.unique_id = read.ReadInt();
                            //m.Desc = read.ReadZString(Encoding.Unicode, 64);
                            //m.unique_id = read.ReadInt();
                            string Date1 = read.ReadZString(Encoding.ASCII, 64);
                            string Date2 = read.ReadZString(Encoding.ASCII, 64);

                            int prices = read.ReadInt();

                            m.price = new int[prices];

                            for (int k = 0; k < prices; k++)
                            {
                                m.price[k] = read.ReadInt();
                            }

                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadShort();
                            int ItemCount = read.ReadInt(); m.ItemCount = ItemCount;

                            m.Item = new int[][] { new int[ItemCount], new int[ItemCount] };


                            for (int g = 0; g < ItemCount; g++)
                            {
                                m.Item[0][g] = read.ReadInt();
                                m.Item[1][g] = read.ReadInt();
                            }

                            //Console.WriteLine($"{NameSize} : {m.Name} : {prices} : {m.unique_id} : {Date1} : {Date2} : {prices} : {ItemCount}");
                            CashShop.Add(m.unique_id, m);
                        }

                        // Console.WriteLine($"{m.Name} | {m.unique_id} | {m.id} | {m.amount}");
                        //Console.WriteLine($"{countxd} | {m.cashshop_id} | {m.Name} | {m.Desc} | {m.unique_id}");

                        //CashShop.Add(m.unique_id, m);
                    }
                    int ETC2u1 = read.ReadInt();
                    int ETC2u2 = read.ReadInt();
                    for (int i = 0; i < ETC2u2 * 2; i++)
                    {
                        read.ReadByte();
                    }
                    int ETC2C = read.ReadInt();
                    for (int i = 0; i < ETC2C; i++)
                    {
                        CASHINFO m = new CASHINFO();
                        //Console.WriteLine(i);
                        m.cashshop_id = read.ReadInt();
                        int ccount1 = read.ReadInt();

                        for (int c = 0; c < ccount1; c++)
                        {

                            int NameSize = read.ReadInt();
                            read.Skip(NameSize * 2);
                            int DesSize = read.ReadInt();
                            read.Skip(DesSize * 2);
                            m.Enabled = read.ReadByte();
                            m.unique_id = read.ReadInt();
                            //m.Desc = read.ReadZString(Encoding.Unicode, 64);
                            //m.unique_id = read.ReadInt();
                            string Date1 = read.ReadZString(Encoding.ASCII, 64);
                            string Date2 = read.ReadZString(Encoding.ASCII, 64);

                            int prices = read.ReadInt();

                            m.price = new int[prices];

                            for (int k = 0; k < prices; k++)
                            {
                                m.price[k] = read.ReadInt();
                            }

                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadShort();
                            int ItemCount = read.ReadInt(); m.ItemCount = ItemCount;

                            m.Item = new int[][] { new int[ItemCount], new int[ItemCount] };


                            for (int g = 0; g < ItemCount; g++)
                            {
                                m.Item[0][g] = read.ReadInt();
                                m.Item[1][g] = read.ReadInt();
                            }

                            //Console.WriteLine($"{NameSize} : {m.Name} : {prices} : {m.unique_id} : {Date1} : {Date2} : {prices} : {ItemCount}");
                            CashShop.Add(m.unique_id, m);
                        }

                        // Console.WriteLine($"{m.Name} | {m.unique_id} | {m.id} | {m.amount}");
                        //Console.WriteLine($"{countxd} | {m.cashshop_id} | {m.Name} | {m.Desc} | {m.unique_id}");

                        //CashShop.Add(m.unique_id, m);
                    }
                    read.Skip(42);
                    int Reinforce2u1 = read.ReadInt();
                    int Reinforce2u2 = read.ReadInt();
                    for (int i = 0; i < Reinforce2u2 * 2; i++)
                    {
                        read.ReadByte();
                    }
                    int Reinforce2C = read.ReadInt();
                    for (int i = 0; i < Reinforce2C; i++)
                    {
                        CASHINFO m = new CASHINFO();
                        //Console.WriteLine(i);
                        m.cashshop_id = read.ReadInt();
                        int ccount1 = read.ReadInt();

                        for (int c = 0; c < ccount1; c++)
                        {

                            int NameSize = read.ReadInt();
                            read.Skip(NameSize * 2);
                            int DesSize = read.ReadInt();
                            read.Skip(DesSize * 2);
                            m.Enabled = read.ReadByte();
                            m.unique_id = read.ReadInt();
                            //m.Desc = read.ReadZString(Encoding.Unicode, 64);
                            //m.unique_id = read.ReadInt();
                            string Date1 = read.ReadZString(Encoding.ASCII, 64);
                            string Date2 = read.ReadZString(Encoding.ASCII, 64);

                            int prices = read.ReadInt();

                            m.price = new int[prices];

                            for (int k = 0; k < prices; k++)
                            {
                                m.price[k] = read.ReadInt();
                            }

                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadShort();
                            int ItemCount = read.ReadInt(); m.ItemCount = ItemCount;

                            m.Item = new int[][] { new int[ItemCount], new int[ItemCount] };


                            for (int g = 0; g < ItemCount; g++)
                            {
                                m.Item[0][g] = read.ReadInt();
                                m.Item[1][g] = read.ReadInt();
                            }

                            //Console.WriteLine($"{NameSize} : {m.Name} : {prices} : {m.unique_id} : {Date1} : {Date2} : {prices} : {ItemCount}");
                            CashShop.Add(m.unique_id, m);
                        }

                        // Console.WriteLine($"{m.Name} | {m.unique_id} | {m.id} | {m.amount}");
                        //Console.WriteLine($"{countxd} | {m.cashshop_id} | {m.Name} | {m.Desc} | {m.unique_id}");

                        //CashShop.Add(m.unique_id, m);
                    }
                    int Headu1 = read.ReadInt();
                    int Headu2 = read.ReadInt();
                    for (int i = 0; i < Headu2 * 2; i++)
                    {
                        read.ReadByte();
                    }
                    int HeadC = read.ReadInt();
                    for (int i = 0; i < HeadC; i++)
                    {
                        CASHINFO m = new CASHINFO();
                        //Console.WriteLine(i);
                        m.cashshop_id = read.ReadInt();
                        int ccount1 = read.ReadInt();

                        for (int c = 0; c < ccount1; c++)
                        {

                            int NameSize = read.ReadInt();
                            read.Skip(NameSize * 2);
                            int DesSize = read.ReadInt();
                            read.Skip(DesSize * 2);
                            m.Enabled = read.ReadByte();
                            m.unique_id = read.ReadInt();
                            //m.Desc = read.ReadZString(Encoding.Unicode, 64);
                            //m.unique_id = read.ReadInt();
                            string Date1 = read.ReadZString(Encoding.ASCII, 64);
                            string Date2 = read.ReadZString(Encoding.ASCII, 64);

                            int prices = read.ReadInt();

                            m.price = new int[prices];

                            for (int k = 0; k < prices; k++)
                            {
                                m.price[k] = read.ReadInt();
                            }

                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadShort();
                            int ItemCount = read.ReadInt(); m.ItemCount = ItemCount;

                            m.Item = new int[][] { new int[ItemCount], new int[ItemCount] };


                            for (int g = 0; g < ItemCount; g++)
                            {
                                m.Item[0][g] = read.ReadInt();
                                m.Item[1][g] = read.ReadInt();
                            }

                            //Console.WriteLine($"{NameSize} : {m.Name} : {prices} : {m.unique_id} : {Date1} : {Date2} : {prices} : {ItemCount}");
                            CashShop.Add(m.unique_id, m);
                        }

                        // Console.WriteLine($"{m.Name} | {m.unique_id} | {m.id} | {m.amount}");
                        //Console.WriteLine($"{countxd} | {m.cashshop_id} | {m.Name} | {m.Desc} | {m.unique_id}");

                        //CashShop.Add(m.unique_id, m);
                    }
                    int Topu1 = read.ReadInt();
                    int Topu2 = read.ReadInt();
                    for (int i = 0; i < Topu2 * 2; i++)
                    {
                        read.ReadByte();
                    }
                    int TopC = read.ReadInt();
                    for (int i = 0; i < TopC; i++)
                    {
                        CASHINFO m = new CASHINFO();
                        //Console.WriteLine(i);
                        m.cashshop_id = read.ReadInt();
                        int ccount1 = read.ReadInt();

                        for (int c = 0; c < ccount1; c++)
                        {

                            int NameSize = read.ReadInt();
                            read.Skip(NameSize * 2);
                            int DesSize = read.ReadInt();
                            read.Skip(DesSize * 2);
                            m.Enabled = read.ReadByte();
                            m.unique_id = read.ReadInt();
                            //m.Desc = read.ReadZString(Encoding.Unicode, 64);
                            //m.unique_id = read.ReadInt();
                            string Date1 = read.ReadZString(Encoding.ASCII, 64);
                            string Date2 = read.ReadZString(Encoding.ASCII, 64);

                            int prices = read.ReadInt();

                            m.price = new int[prices];

                            for (int k = 0; k < prices; k++)
                            {
                                m.price[k] = read.ReadInt();
                            }

                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadShort();
                            int ItemCount = read.ReadInt(); m.ItemCount = ItemCount;

                            m.Item = new int[][] { new int[ItemCount], new int[ItemCount] };


                            for (int g = 0; g < ItemCount; g++)
                            {
                                m.Item[0][g] = read.ReadInt();
                                m.Item[1][g] = read.ReadInt();
                            }

                            //Console.WriteLine($"{NameSize} : {m.Name} : {prices} : {m.unique_id} : {Date1} : {Date2} : {prices} : {ItemCount}");
                            CashShop.Add(m.unique_id, m);
                        }

                        // Console.WriteLine($"{m.Name} | {m.unique_id} | {m.id} | {m.amount}");
                        //Console.WriteLine($"{countxd} | {m.cashshop_id} | {m.Name} | {m.Desc} | {m.unique_id}");

                        //CashShop.Add(m.unique_id, m);
                    }
                    int Buttomu1 = read.ReadInt();
                    int Buttomu2 = read.ReadInt();
                    for (int i = 0; i < Buttomu2 * 2; i++)
                    {
                        read.ReadByte();
                    }
                    int ButtomC = read.ReadInt();
                    for (int i = 0; i < ButtomC; i++)
                    {
                        CASHINFO m = new CASHINFO();
                        //Console.WriteLine(i);
                        m.cashshop_id = read.ReadInt();
                        int ccount1 = read.ReadInt();

                        for (int c = 0; c < ccount1; c++)
                        {

                            int NameSize = read.ReadInt();
                            read.Skip(NameSize * 2);
                            int DesSize = read.ReadInt();
                            read.Skip(DesSize * 2);
                            m.Enabled = read.ReadByte();
                            m.unique_id = read.ReadInt();
                            //m.Desc = read.ReadZString(Encoding.Unicode, 64);
                            //m.unique_id = read.ReadInt();
                            string Date1 = read.ReadZString(Encoding.ASCII, 64);
                            string Date2 = read.ReadZString(Encoding.ASCII, 64);

                            int prices = read.ReadInt();

                            m.price = new int[prices];

                            for (int k = 0; k < prices; k++)
                            {
                                m.price[k] = read.ReadInt();
                            }

                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadShort();
                            int ItemCount = read.ReadInt(); m.ItemCount = ItemCount;

                            m.Item = new int[][] { new int[ItemCount], new int[ItemCount] };


                            for (int g = 0; g < ItemCount; g++)
                            {
                                m.Item[0][g] = read.ReadInt();
                                m.Item[1][g] = read.ReadInt();
                            }

                            //Console.WriteLine($"{NameSize} : {m.Name} : {prices} : {m.unique_id} : {Date1} : {Date2} : {prices} : {ItemCount}");
                            CashShop.Add(m.unique_id, m);
                        }

                        // Console.WriteLine($"{m.Name} | {m.unique_id} | {m.id} | {m.amount}");
                        //Console.WriteLine($"{countxd} | {m.cashshop_id} | {m.Name} | {m.Desc} | {m.unique_id}");

                        //CashShop.Add(m.unique_id, m);
                    }
                    int Glovesu1 = read.ReadInt();
                    int Glovesu2 = read.ReadInt();
                    for (int i = 0; i < Glovesu2 * 2; i++)
                    {
                        read.ReadByte();
                    }
                    int GlovesC = read.ReadInt();
                    for (int i = 0; i < GlovesC; i++)
                    {
                        CASHINFO m = new CASHINFO();
                        //Console.WriteLine(i);
                        m.cashshop_id = read.ReadInt();
                        int ccount1 = read.ReadInt();

                        for (int c = 0; c < ccount1; c++)
                        {

                            int NameSize = read.ReadInt();
                            read.Skip(NameSize * 2);
                            int DesSize = read.ReadInt();
                            read.Skip(DesSize * 2);
                            m.Enabled = read.ReadByte();
                            m.unique_id = read.ReadInt();
                            //m.Desc = read.ReadZString(Encoding.Unicode, 64);
                            //m.unique_id = read.ReadInt();
                            string Date1 = read.ReadZString(Encoding.ASCII, 64);
                            string Date2 = read.ReadZString(Encoding.ASCII, 64);

                            int prices = read.ReadInt();

                            m.price = new int[prices];

                            for (int k = 0; k < prices; k++)
                            {
                                m.price[k] = read.ReadInt();
                            }

                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadShort();
                            int ItemCount = read.ReadInt(); m.ItemCount = ItemCount;

                            m.Item = new int[][] { new int[ItemCount], new int[ItemCount] };


                            for (int g = 0; g < ItemCount; g++)
                            {
                                m.Item[0][g] = read.ReadInt();
                                m.Item[1][g] = read.ReadInt();
                            }

                            //Console.WriteLine($"{NameSize} : {m.Name} : {prices} : {m.unique_id} : {Date1} : {Date2} : {prices} : {ItemCount}");
                            CashShop.Add(m.unique_id, m);
                        }

                        // Console.WriteLine($"{m.Name} | {m.unique_id} | {m.id} | {m.amount}");
                        //Console.WriteLine($"{countxd} | {m.cashshop_id} | {m.Name} | {m.Desc} | {m.unique_id}");

                        //CashShop.Add(m.unique_id, m);
                    }
                    int Shoesu1 = read.ReadInt();
                    int Shoesu2 = read.ReadInt();
                    for (int i = 0; i < Shoesu2 * 2; i++)
                    {
                        read.ReadByte();
                    }
                    int ShoesC = read.ReadInt();
                    for (int i = 0; i < ShoesC; i++)
                    {
                        CASHINFO m = new CASHINFO();
                        //Console.WriteLine(i);
                        m.cashshop_id = read.ReadInt();
                        int ccount1 = read.ReadInt();

                        for (int c = 0; c < ccount1; c++)
                        {

                            int NameSize = read.ReadInt();
                            read.Skip(NameSize * 2);
                            int DesSize = read.ReadInt();
                            read.Skip(DesSize * 2);
                            m.Enabled = read.ReadByte();
                            m.unique_id = read.ReadInt();
                            //m.Desc = read.ReadZString(Encoding.Unicode, 64);
                            //m.unique_id = read.ReadInt();
                            string Date1 = read.ReadZString(Encoding.ASCII, 64);
                            string Date2 = read.ReadZString(Encoding.ASCII, 64);

                            int prices = read.ReadInt();

                            m.price = new int[prices];

                            for (int k = 0; k < prices; k++)
                            {
                                m.price[k] = read.ReadInt();
                            }

                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadShort();
                            int ItemCount = read.ReadInt(); m.ItemCount = ItemCount;

                            m.Item = new int[][] { new int[ItemCount], new int[ItemCount] };


                            for (int g = 0; g < ItemCount; g++)
                            {
                                m.Item[0][g] = read.ReadInt();
                                m.Item[1][g] = read.ReadInt();
                            }

                            //Console.WriteLine($"{NameSize} : {m.Name} : {prices} : {m.unique_id} : {Date1} : {Date2} : {prices} : {ItemCount}");
                            CashShop.Add(m.unique_id, m);
                        }

                        // Console.WriteLine($"{m.Name} | {m.unique_id} | {m.id} | {m.amount}");
                        //Console.WriteLine($"{countxd} | {m.cashshop_id} | {m.Name} | {m.Desc} | {m.unique_id}");

                        //CashShop.Add(m.unique_id, m);
                    }
                    int Fashionu1 = read.ReadInt();
                    int Fashionu2 = read.ReadInt();
                    for (int i = 0; i < Fashionu2 * 2; i++)
                    {
                        read.ReadByte();
                    }
                    int FashionC = read.ReadInt();
                    for (int i = 0; i < FashionC; i++)
                    {
                        CASHINFO m = new CASHINFO();
                        //Console.WriteLine(i);
                        m.cashshop_id = read.ReadInt();
                        int ccount1 = read.ReadInt();

                        for (int c = 0; c < ccount1; c++)
                        {

                            int NameSize = read.ReadInt();
                            read.Skip(NameSize * 2);
                            int DesSize = read.ReadInt();
                            read.Skip(DesSize * 2);
                            m.Enabled = read.ReadByte();
                            m.unique_id = read.ReadInt();
                            //m.Desc = read.ReadZString(Encoding.Unicode, 64);
                            //m.unique_id = read.ReadInt();
                            string Date1 = read.ReadZString(Encoding.ASCII, 64);
                            string Date2 = read.ReadZString(Encoding.ASCII, 64);

                            int prices = read.ReadInt();

                            m.price = new int[prices];

                            for (int k = 0; k < prices; k++)
                            {
                                m.price[k] = read.ReadInt();
                            }

                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadShort();
                            int ItemCount = read.ReadInt(); m.ItemCount = ItemCount;

                            m.Item = new int[][] { new int[ItemCount], new int[ItemCount] };


                            for (int g = 0; g < ItemCount; g++)
                            {
                                m.Item[0][g] = read.ReadInt();
                                m.Item[1][g] = read.ReadInt();
                            }

                            //Console.WriteLine($"{NameSize} : {m.Name} : {prices} : {m.unique_id} : {Date1} : {Date2} : {prices} : {ItemCount}");
                            CashShop.Add(m.unique_id, m);
                        }

                        // Console.WriteLine($"{m.Name} | {m.unique_id} | {m.id} | {m.amount}");
                        //Console.WriteLine($"{countxd} | {m.cashshop_id} | {m.Name} | {m.Desc} | {m.unique_id}");

                        //CashShop.Add(m.unique_id, m);
                    }
                    int Costumeu1 = read.ReadInt();
                    int Costumeu2 = read.ReadInt();
                    for (int i = 0; i < Fashionu2 * 2; i++)
                    {
                        read.ReadByte();
                    }
                    int CostumeC = read.ReadInt();
                    for (int i = 0; i < CostumeC; i++)
                    {
                        CASHINFO m = new CASHINFO();
                        //Console.WriteLine(i);
                        m.cashshop_id = read.ReadInt();
                        int ccount1 = read.ReadInt();

                        for (int c = 0; c < ccount1; c++)
                        {

                            int NameSize = read.ReadInt();
                            read.Skip(NameSize * 2);
                            int DesSize = read.ReadInt();
                            read.Skip(DesSize * 2);
                            m.Enabled = read.ReadByte();
                            m.unique_id = read.ReadInt();
                            //m.Desc = read.ReadZString(Encoding.Unicode, 64);
                            //m.unique_id = read.ReadInt();
                            string Date1 = read.ReadZString(Encoding.ASCII, 64);
                            string Date2 = read.ReadZString(Encoding.ASCII, 64);

                            int prices = read.ReadInt();

                            m.price = new int[prices];

                            for (int k = 0; k < prices; k++)
                            {
                                m.price[k] = read.ReadInt();
                            }

                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadShort();
                            int ItemCount = read.ReadInt(); m.ItemCount = ItemCount;

                            m.Item = new int[][] { new int[ItemCount], new int[ItemCount] };


                            for (int g = 0; g < ItemCount; g++)
                            {
                                m.Item[0][g] = read.ReadInt();
                                m.Item[1][g] = read.ReadInt();
                            }

                            //Console.WriteLine($"{NameSize} : {m.Name} : {prices} : {m.unique_id} : {Date1} : {Date2} : {prices} : {ItemCount}");
                            CashShop.Add(m.unique_id, m);
                        }

                        // Console.WriteLine($"{m.Name} | {m.unique_id} | {m.id} | {m.amount}");
                        //Console.WriteLine($"{countxd} | {m.cashshop_id} | {m.Name} | {m.Desc} | {m.unique_id}");

                        //CashShop.Add(m.unique_id, m);
                    }
                    read.Skip(28);
                    int Packagesu1 = read.ReadInt();
                    int Packagesu2 = read.ReadInt();
                    for (int i = 0; i < Packagesu2 * 2; i++)
                    {
                        read.ReadByte();
                    }
                    int PackagesC = read.ReadInt();
                    for (int i = 0; i < PackagesC; i++)
                    {
                        CASHINFO m = new CASHINFO();
                        //Console.WriteLine(i);
                        m.cashshop_id = read.ReadInt();
                        int ccount1 = read.ReadInt();

                        for (int c = 0; c < ccount1; c++)
                        {

                            int NameSize = read.ReadInt();
                            read.Skip(NameSize * 2);
                            int DesSize = read.ReadInt();
                            read.Skip(DesSize * 2);
                            m.Enabled = read.ReadByte(); ;
                            m.unique_id = read.ReadInt();
                            //m.Desc = read.ReadZString(Encoding.Unicode, 64);
                            //m.unique_id = read.ReadInt();
                            string Date1 = read.ReadZString(Encoding.ASCII, 64);
                            string Date2 = read.ReadZString(Encoding.ASCII, 64);

                            int prices = read.ReadInt();

                            m.price = new int[prices];

                            for (int k = 0; k < prices; k++)
                            {
                                m.price[k] = read.ReadInt();
                            }

                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadInt();
                            read.ReadShort();
                            int ItemCount = read.ReadInt(); m.ItemCount = ItemCount;

                            m.Item = new int[][] { new int[ItemCount], new int[ItemCount] };


                            for (int g = 0; g < ItemCount; g++)
                            {
                                m.Item[0][g] = read.ReadInt();
                                m.Item[1][g] = read.ReadInt();
                            }

                            //Console.WriteLine($"{NameSize} : {m.Name} : {prices} : {m.unique_id} : {Date1} : {Date2} : {prices} : {ItemCount}");
                            CashShop.Add(m.unique_id, m);
                        }

                        //CashShop.Add(m.unique_id, m);

                    }
                }


                SysCons.LogDB("CashShopDB", "Loaded {0} entries.", CashShop.Count);
            }
        }

        public static CASHINFO getID(int unique_id)
        {
            if (CashShop.ContainsKey(unique_id))
                return CashShop[unique_id];
            else
                return null;


        }

    }
    public class CASHINFO
    {
        public int cashshop_id;
        public string Name;
        public string Desc;
        public int Enabled;
        public string Date1;
        public string Date2;
        public byte u1;
        public int unique_id, id_2, id_3 = 0;
        public int[] price;
        public int[] id;
        public int[][] Item;
        public int prices;
        public int ItemCount;

        public CASHINFO() { }

    }



}