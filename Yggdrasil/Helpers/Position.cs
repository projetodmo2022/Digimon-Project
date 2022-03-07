using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yggdrasil.Database;

namespace Yggdrasil.Helpers
{
    public class Position
    {
        public int Map = 1;
        public int PosX = 0;
        public int PosY = 0;

        public Position()
        {
            Map = 3; //NEW DATS! -> 1 -> DATS_001    // Talvez sem o mapa onde inciar o tamer no game tem descubrir
            PosX = 50;
            PosY = 50;
        }

        public Position(int map, int x, int y)
        {
            Map = map;
            PosX = x;
            PosY = y;
        }

        public Position(Portal Portal)
        {
            this.Map = Portal.s_dwPortalID;
            this.PosX = Portal.s_nDestTargetX;
            this.PosY = Portal.s_nDestTargetY;
        }

        public override string ToString()
        {
            
            return string.Format("{0} {3} [{1}, {2}]", MapData.DisplayName, PosX, PosY, Map);
        }

        public MapData MapData
        {
            get
            {
                return MapListDB.GetMap(Map);
            }
        }

        public string MapName
        {
            get
            {
                return MapData.DisplayName;
            }
        }

        public Position Clone()
        {
            return new Position(Map, PosX, PosY);
        }
    }
}
