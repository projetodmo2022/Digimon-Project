using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yggdrasil.Helpers;
using System.IO;
using Digital_World;


namespace Yggdrasil.Database
{
    public class MapPortalDB
    {
        public static List<PortalCluster> PortalList = new List<PortalCluster>();
        public static List<Portal> PortalWarpList = new List<Portal>();

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
                        PortalCluster Cluster = new PortalCluster();
                        Cluster.Count = read.ReadInt();
                        for (int h = 0; h < Cluster.Count; h++)
                        {
                            Portal portal = new Portal();
                            portal.s_dwPortalID = read.ReadInt();
                            portal.s_dwPortalType = read.ReadInt();
                            portal.s_dwSrcMapID = read.ReadInt();
                            portal.s_nSrcTargetX = read.ReadInt();
                            portal.s_nSrcTargetY = read.ReadInt();
                            portal.s_nSrcRadius = read.ReadInt();
                            portal.s_dwDestMapID = read.ReadInt();
                            portal.s_nDestTargetX = read.ReadInt();
                            portal.s_nDestTargetY = read.ReadInt();
                            portal.s_nDestRadius = read.ReadInt();
                            portal.s_ePortalType = read.ReadInt();
                            portal.s_dwUniqObjectID = read.ReadInt();
                            portal.s_nPortalKindIndex = read.ReadInt();
                            portal.s_nViewTargetX = read.ReadInt();
                            portal.s_nViewTargetY = read.ReadInt();
                            Cluster.Add(portal);
                            PortalWarpList.Add(portal);
                        }



                        PortalList.Add(Cluster);
                    }
                }
            }
            SysCons.LogDB("MapPortal.bin", "Loaded {0} portals", PortalList.Count);
        }

        public static Portal GetPortal(int portalId)
        {
            PortalCluster Cluster = PortalList.Find(delegate (PortalCluster lCluster)
           {
               if (lCluster.PortalList.ContainsKey(portalId))
                   return true;
               return false;
           });
            return Cluster[portalId];
        }

        public static Portal GetPortalDestinationByMapId(int mapId)
        {
            return PortalWarpList.FirstOrDefault(x => x.s_dwDestMapID == mapId);
        }
    }

    public class PortalCluster
    {
        public int Count;
        public Dictionary<int, Portal> PortalList = new Dictionary<int, Portal>();

        public void Add(Portal portal)
        {
            PortalList.Add(portal.s_dwPortalID, portal);
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
        public int s_dwPortalID;
        public int s_dwPortalType;
        public int s_dwSrcMapID;
        public int s_nSrcTargetX;
        public int s_nSrcTargetY;
        public int s_nSrcRadius;
        public int s_dwDestMapID;
        public int s_nDestTargetX;
        public int s_nDestTargetY;
        public int s_nDestRadius;
        public int s_ePortalType;
        public int s_dwUniqObjectID;
        public int s_nPortalKindIndex;
        public int s_nViewTargetX;
        public int s_nViewTargetY;
        public int pMapGroup;
    }
}
