using Yggdrasil.Helpers;

namespace Yggdrasil.Entities
{
    public class MonsterEntity
    {
        public uint Model = 0;
        public string Name = "";
        public int Level = 1;
        public int Species = 31001;
        public short Size = 10000;

        public int MaxHP = 1000;

        public int HP = 1000;

        public int Collision = 0;


        public bool isAlive => HP > 0;


        public Position Location = new Position();

        public MonsterEntity()
        {

        }
        public uint ProperModel()
        {
            uint pModel = 0;
            pModel += (uint)((Species * 128) + 16);
            return (pModel << 8);
        }

        public override string ToString()
        {
            return string.Format("{0}\nLv {1} {2}", Name, Level, Species);
        }
        public int Handle;
    }
}
