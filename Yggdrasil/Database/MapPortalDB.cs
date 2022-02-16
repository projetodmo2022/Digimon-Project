using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yggdrasil.Helpers;
using System.IO;
using Yggdrasil;
using Digital_World;

namespace Yggdrasil.Database
{
    public class MapPortalDB
    {
        public static List<PortalCluster> PortalList = new List<PortalCluster>();

        public static void Load(string fileName)
        {
            if (PortalList.Count > 0) return;
            using (Stream s = File.OpenRead(fileName))
            {
                using (BitReader read = new BitReader(s))
                {

                    int count = read.ReadInt();

                    //Console.WriteLine(count);

                    for (int i = 0; i < count; i++)
                    {
                        PortalCluster Cluster = new PortalCluster();
                        Cluster.Count = read.ReadInt(); //4

                        for (int h = 0; h < Cluster.Count; h++)
                        {
                            Portal portal = new Portal();
                            portal.PortalId = read.ReadInt(); //4

                            //Console.WriteLine(portal.PortalId);

                            read.ReadInt();

                            for (int j = 0; j < portal.uInts1.Length; j++){
                                portal.uInts1[j] = read.ReadInt(); //16
                                //Console.WriteLine(portal.uInts1[j]);
                            }
                            portal.MapId = read.ReadInt(); //4

                            for (int j = 0; j < portal.uInts2.Length; j++)
                            {
                                portal.uInts2[j] = read.ReadInt(); //32
                                //Console.WriteLine(portal.uInts2[j]);

                            }

                            Cluster.Add(portal);

                            //Console.WriteLine("-----------------------------------");
                        }



                        PortalList.Add(Cluster);
                    }
                }
            }
            SysCons.LogDB("MapPortal.bin", "Loaded {0} portals", PortalList.Count);
        }

        public static Portal GetPortal(int portalId)
        {
            PortalCluster Cluster =  PortalList.Find(delegate(PortalCluster lCluster)
            {
                if (lCluster.PortalList.ContainsKey(portalId))
                    return true;
                return false;
            });
            return Cluster[portalId];
        }
    }

    public class PortalCluster
    {
        public int Count;
        public Dictionary<int, Portal> PortalList = new Dictionary<int, Portal>();

        public void Add(Portal portal)
        {
            PortalList.Add(portal.PortalId, portal);
        }

        public Portal this[int portalId]
        {
            get
            {
                if (PortalList.ContainsKey(portalId))
                    return PortalList[portalId];
                else
                    return null;
            }
        }
    }

    public class Portal
    {
        public int PortalId;
        public int MapId;
        public int[] uInts1;
        public int[] uInts2;

        public Portal()
        {
            uInts1 = new int[4];
            uInts2 = new int[8];
        }
    }
}
