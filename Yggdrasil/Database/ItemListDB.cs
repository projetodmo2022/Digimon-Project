using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yggdrasil.Helpers;
using System.IO;
using Digital_World;


namespace Yggdrasil.Database
{
    public class ItemListDB
    {
        public static Dictionary<int, ItemData> Items = new Dictionary<int, ItemData>();
        public static List<Shop> ShopItem = new List<Shop>();
        public static List<ItemMaking> ItemMaking = new List<ItemMaking>();
        public static Dictionary<int, Container> Containers = new Dictionary<int, Container>();
        public static Dictionary<int, XAI> XAI = new Dictionary<int, XAI>();

        public static void Load(string fileName)
        {

            if (File.Exists(fileName) == false) return;
            using (BitReader read = new BitReader(File.OpenRead(fileName)))
            {


                int count = read.ReadInt();
                //int count = 2;
                for (int i = 0; i < count; i++)
                {
                    read.Seek(4 + i * 1596);
                    ItemData iData = new ItemData();
                    iData.itemId = read.ReadInt(); // ->
                    iData.Name = read.ReadZString(Encoding.Unicode); // -> 
                    read.Seek(4 + 132 + i * 1596);
                    iData.uInt1 = read.ReadInt(); // - >
                    iData.Desc = read.ReadZString(Encoding.Unicode); // ->
                    read.Seek(4 + 1160 + i * 1596);     /// ->>   Valor para leitura no arquivo bin em decimal
                    iData.Icon = read.ReadZString(Encoding.ASCII);
                    read.Seek(4 + 1224 + i * 1596); // - >
                    iData.ItemType = read.ReadShort();
                    iData.Kind = read.ReadZString(Encoding.Unicode);
                    //-> Tentando ver se isso funciona

                    read.Seek(4 + 1342 + i * 1596);

                    for (int j = 0; j < iData.uShorts1.Length; j++)
                    {
                        iData.uShorts1[j] = read.ReadShort();
                    }
                    read.Seek(4 + 1364 + i * 1596);
                    iData.Type = read.ReadInt();
                    read.Seek(4 + 1374 + i * 1596);
                    iData.Stack = read.ReadShort();
                    for (int j = 0; j < iData.uShorts2.Length; j++)
                    {
                        iData.uShorts2[j] = read.ReadShort();
                    }
                    read.ReadInt();
                    read.Seek(4 + 1404 + i * 1596);
                    iData.Buy = read.ReadInt();
                    iData.Sell = read.ReadInt();
                    Items.Add(iData.itemId, iData);

                    read.Seek(4 + 1580 + i * 1596);
                    iData.RemainingType = read.ReadByte();

                    /*using (System.IO.StreamWriter file =
                    new System.IO.StreamWriter(@"logs\GDMOITEMDB.txt", true))
                    {
                        file.WriteLine("ID= {0} | Name= {1} |  uInt1= {2} |   Desc= {3} |   Icon= {4} |   ItemType= {5} |   Kind= {6} |  Type= {7} |    Stack= {8} |  Buy= {9} |   Sell= {10} |   RemainingType= {11}", iData.itemId, iData.Name, iData.uInt1, iData.Desc, iData.Icon, iData.ItemType, iData.Kind, iData.Type, iData.Stack, iData.Buy, iData.Sell, iData.RemainingType);
                    }*/

                    /*using (System.IO.StreamWriter file =
                    new System.IO.StreamWriter(@"logs\GDMOITEMDB.txt", true))
                    {
                        file.WriteLine("ID= {0} | Name= {1}", iData.itemId, iData.Name);
                    }*/
                }


            }
            if (ItemMaking.Count > 0) return;
            using (BitReader read = new BitReader(File.OpenRead(fileName)))
            {
                /*read.Seek(28676484); ////endereço dentro do binario decimal

                int count3 = read.ReadInt();
                for (int i = 0; i < count3; i++)
                {
                    int NPCiD = read.ReadInt();
                    int List = read.ReadInt();
                    for (int g = 0; g < List; g++)
                    {
                        int ID = read.ReadInt();
                        int ID2 = read.ReadInt();
                        int ID3 = read.ReadInt();
                        for (int c = 0; c < ID3; c++)
                        {
                            read.ReadShort();
                        }
                        int Categories = read.ReadInt();
                        for (int a = 0; a < Categories; a++)
                        {
                            int CID = read.ReadInt();
                            int u1 = read.ReadInt();
                            int u2 = read.ReadInt();
                            for (int d = 0; d < u2; d++)
                            {
                                read.ReadShort();
                            }
                            int Items = read.ReadInt();
                            for (int r = 0; r < Items; r++)
                            {
                                ItemMaking item = new ItemMaking();
                                item.NpcID = NPCiD;
                                item.ID = read.ReadInt();
                                item.ItemID = read.ReadInt();
                                item.Enabled = read.ReadInt();
                                item.SuccessRate = read.ReadInt();
                                read.ReadInt();
                                read.ReadInt();
                                item.costprice = read.ReadInt();
                                item.neededitems = read.ReadInt();
                                item.neededitem = new int[][] { new int[item.neededitems], new int[item.neededitems] };
                                for (int y = 0; y < item.neededitems; y++)
                                {
                                    item.neededitem[0][y] = read.ReadInt();
                                    item.neededitem[1][y] = read.ReadInt();
                                    //Console.WriteLine($"Item: {item.neededitem[0][y]}");
                                    //Console.WriteLine($"Amount: {item.neededitem[1][y]}");
                                }
                                ItemMaking.Add(item);
                            }
                        }

                    }
                }*/
            }
            if (XAI.Count > 0) return;
            using (BitReader read = new BitReader(File.OpenRead(fileName)))
            {
                //SHOP ITEM

                /*read.Seek(28274058); ////endereço dentro do binario
                int cShop = read.ReadInt();
                for (int s = 0; s < cShop; s++)
                {
                    Shop shop = new Shop();
                    shop.ID = read.ReadInt();
                    shop.NPC = read.ReadInt();
                    read.ReadInt();
                    read.ReadInt();
                    shop.ItemID = read.ReadInt();
                    shop.Items = new int[4];
                    for (int q = 0; q < 4; q++)
                    {
                        shop.Items[q] = read.ReadInt();
                    }
                    shop.Amount = read.ReadShort();
                    read.ReadShort();
                    read.ReadInt();
                    shop.ItemCount = read.ReadInt();
                    ShopItem.Add(shop);
                }*/

                read.Seek(23999030);

                if (XAI.Count > 0) return;
                int XAICount = read.ReadInt();
                for (int i = 0; i < XAICount; i++)
                {
                    XAI xai = new XAI();
                    xai.ItemID = read.ReadInt();
                    xai.XGauge = read.ReadInt();
                    xai.Unknown = read.ReadByte();
                    XAI.Add(xai.ItemID, xai);
                }

                /*
                read.Seek(28770456); ////endereço dentro do binario

                int cContainers = read.ReadInt();

                for (int p = 0; p < cContainers; p++)
                {
                    Container container = new Container();
                    container.ItemID = read.ReadInt();
                    container.u1 = read.ReadInt();
                    container.Type = read.ReadShort();
                    container.ItemCount = read.ReadInt();
                    container.Items_ItemID = new int[container.ItemCount];
                    container.Items_Amount = new short[container.ItemCount];
                    for (int c = 0; c < container.ItemCount; c++)
                    {
                        container.Items_ItemID[c] = read.ReadInt();
                        container.Items_Amount[c] = read.ReadShort();
                    }
                    Containers.Add(container.ItemID, container);

                    // Log Item_Rank

                    /*
                    using (System.IO.StreamWriter file =
                        new System.IO.StreamWriter(@"logs\Containers.txt", true))
                    {
                        file.WriteLine("container.ItemID= {0} |  container.u1= {1} |  container.Type= {2} |  container.ItemCount= {3} |  container.Items_ItemID= {4} |  container.Items_Amount= {5}", container.ItemID,  container.u1, container.Type, container.ItemCount,container.Items_ItemID,container.Items_Amount);
                    }
                }*/
            }
            //SysCons.LogDB("ItemList.bin", $"Loaded {Items.Count} items. {ShopItem.Count} ShopItem. {ItemMaking.Count} ItemMaking. {Containers.Count} Containers. {XAI.Count} XAI ");
            SysCons.LogDB("ItemList.bin", $"Loaded {Items.Count} items. {ShopItem.Count} / {ItemMaking.Count} / {Containers.Count} / {XAI.Count}");
            //SysCons.LogDB("ItemList.bin", $"Loaded {ItemMaking.Count} ItemMaking. ");
            //SysCons.LogDB("ItemList.bin", $"Loaded {ShopItem.Count} ShopItem.");
            //SysCons.LogDB("ItemList.bin", $"Loaded {Containers.Count} items Containers.");
            //SysCons.LogDB("ItemList.bin", $"Loaded {XAI.Count} XAI items.");
            //SysCons.LogDB("ItemList.bin", $"Loaded {Items.Count} items.");
        }
        public static ItemData GetItem(int fullId)
        {
            ItemData iData = null;
            foreach (KeyValuePair<int, ItemData> kvp in Items)
            {
                if (kvp.Value.itemId == fullId)
                {
                    iData = kvp.Value;
                    break;
                }
            }
            return iData;
        }

        public static ItemMaking GetID(int ID)
        {
            ItemMaking item = null;
            foreach (ItemMaking _item in ItemMaking)
                if (_item.ID != 0 && _item.ID == ID)
                {
                    item = _item;
                    break;
                }
            return item;
        }

        public static XAI GetXAI(int fullId)
        {
            XAI iData = null;
            foreach (KeyValuePair<int, XAI> kvp in XAI)
            {
                if (kvp.Value.ItemID == fullId)
                {
                    iData = kvp.Value;
                    break;
                }
            }
            return iData;
        }

        public static Container GetContainer(int fullId)
        {
            Container iData = null;
            foreach (KeyValuePair<int, Container> kvp in Containers)
            {
                if (kvp.Value.ItemID == fullId)
                {
                    iData = kvp.Value;
                    break;
                }
            }
            return iData;
        }
    }

    public class ItemData
    {

        public int itemId;
        public ushort Mod;
        public string Name;
        public int uInt1;
        public string Desc;
        public string Icon;
        public short ItemType;
        public int Type;
        public string Kind;
        public short Stack;
        public short[] uShorts1; //8
        public short[] uShorts2; //12
        public int Buy, Sell;
        public byte RemainingType;

        public ItemData()
        {
            //TOTAL 8 + 12 + STACK = 23 - > VER155
            uShorts1 = new short[8];
            uShorts2 = new short[7];
        }


    }

    public class Container
    {
        public int ItemID;
        public int ItemCount;
        public short Type;
        public int u1;
        public int[] Items_ItemID;
        public short[] Items_Amount;

    }

    public class Shop
    {
        public int ID;
        public int ItemID;
        public int NPC;
        public int[] Items;
        public int ItemCount;
        public int Amount;
    }

    public class ItemMaking
    {
        public int Count;
        public int NpcID;
        public string Name;
        public int NpcList;
        public int ID;
        public int ItemID;
        public int Enabled;
        public int SuccessRate;
        public int costprice;
        public int neededitems;
        public int[][] neededitem;

    }
    public class XAI
    {
        public int ItemID;
        public int XGauge;
        public byte Unknown;
    }
}
