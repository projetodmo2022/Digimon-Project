using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Yggdrasil.Helpers
{
    public class DigimonStats
    {
        private short m_hp = 100;
        private short m_ds = 100;


        public short MaxHP = 100;
        public short MaxDS = 100;

        public short HP
        {
            get
            {
                return m_hp;
            }
            set
            {
                if (value >= MaxHP) value = MaxHP;
                if (value <= 0) value = 0;
                m_hp = value;
            }
        }

        public short DS
        {
            get
            {
                return m_ds;
            }
            set
            {
                if (value >= MaxDS) value = MaxDS;
                if (value <= 0) value = 0;
                m_ds = value;
            }
        }
        /// <summary>
        /// Attack Stat
        /// </summary>
        public short AT = 1;
        /// <summary>
        /// Defense stat
        /// </summary>
        public short DE = 1;
        /// <summary>
        /// Hit Rate
        /// </summary>
        public short HT = 0;
        /// <summary>
        /// Evade
        /// </summary>
        public short EV = 10;
        /// <summary>
        /// Critical rate
        /// </summary>
        public short CR = 0;

        public short AR = 50;
        public short BL = 0;
        /// <summary>
        /// Attack Speed
        /// </summary>
        public short AS = 1000;
        /// <summary>
        /// Movement Speed?
        /// </summary>
        public short MS = 550;
        /// <summary>
        /// Unknown Stat
        /// </summary>
        public short uStat = 80;

        public short Intimacy = 0;

        public DigimonStats() { }

        public void Max()
        {
            MaxHP = MaxDS = DS = HP = short.MaxValue;
            AT = DE = HT = EV = CR = 1000;
            MS = 1200;
            AS = 5000;
            Intimacy = 100;
        }

        public byte[] ToArray()
        {
            byte[] buffer = null;
            using (MemoryStream m = new MemoryStream())
            {
                m.Write(BitConverter.GetBytes(MaxHP), 0, 4);
                m.Write(BitConverter.GetBytes(MaxDS), 0, 4);
                m.Write(BitConverter.GetBytes(DE), 0, 4);
                m.Write(BitConverter.GetBytes(AT), 0, 4);
                m.Write(BitConverter.GetBytes(HP), 0, 4);
                m.Write(BitConverter.GetBytes(DS), 0, 4);
                m.Write(BitConverter.GetBytes(Intimacy), 0, 4);
                m.Write(BitConverter.GetBytes(BL), 0, 4);
                m.Write(BitConverter.GetBytes(EV), 0, 4);
                m.Write(BitConverter.GetBytes(CR), 0, 4);
                m.Write(BitConverter.GetBytes(MS), 0, 4);
                m.Write(BitConverter.GetBytes(AS), 0, 4);
                m.Write(BitConverter.GetBytes(AR), 0, 4);
                m.Write(BitConverter.GetBytes(HT), 0, 4);


                buffer = m.ToArray();
            }

            return buffer;
        }

        public void Recover()
        {
            this.HP += (short)Math.Ceiling(this.MaxHP * 0.02);
            this.DS += (short)Math.Ceiling(this.MaxDS * 0.02);
        }
    }
}
