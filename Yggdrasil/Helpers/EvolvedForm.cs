using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Yggdrasil.Helpers
{
    [Serializable]
    public class EvolvedForms
    {
        private EvolvedForm[] m_coll;

        public EvolvedForms()
        {
            m_coll = new EvolvedForm[4];
            for (int i = 0; i < m_coll.Length; i++)
                m_coll[i] = new EvolvedForm();
        }

        /// <summary>
        /// CHANGED!!!! int to uint
        /// </summary>
        /// <param name="count"></param>
        public EvolvedForms(int count)
        {
            m_coll = new EvolvedForm[count];
            for (int i = 0; i < m_coll.Length; i++)
                m_coll[i] = new EvolvedForm();
        }

        public byte[] Serialize()
        {
            byte[] buffer = new byte[0];
            using (MemoryStream m = new MemoryStream())
            {
                BinaryFormatter f = new BinaryFormatter();
                f.Serialize(m, this);

                buffer = m.ToArray();
            }
            return buffer;
        }

        public int Count
        {
            get
            {
                return m_coll.Length;
            }
        }

        public EvolvedForm this[int idx]
        {
            get
            {
                return m_coll[idx];
            }
            set
            {
                m_coll[idx] = value;
            }
        }
    }

    [Serializable]
    public class EvolvedForm
    {
        public short[] uShorts1;
        public byte skill_points, skill1_level, skill2_level, skill3_level, skill4_level, skill5_level, skill_max_level, uByte3, Skill1, Skill2, Skill3;
        public short uShort1;
        public int skill_EXP;

        public byte unlocked;

        //->NEW
        public byte[] bytes;
        //->NEW

        public EvolvedForm()
        {
            /*uShort1 = 0;
            uShorts1 = new short[24];
            b128 = 128;
            Skill1 = 1;
            Skill2 = 1;
            */

            bytes = new byte[13]; //13 BYTES ARE needed
            
        }

        public EvolvedForm(byte[] Unknowns, byte[] Skills)
        {

        }

        public byte[] ToArray()
        {
            byte[] buffer = new byte[0];
            using (MemoryStream m = new MemoryStream())
            {

                /*for (int i = 0; i < 3; i++)
                {
                    m.WriteByte(0x00);
                }*/

                m.WriteByte(0x0);
                m.WriteByte(0x0);
                m.WriteByte(0x0);
                m.WriteByte(0x4);
                m.WriteByte(unlocked);
                m.WriteByte(0x0);
                m.WriteByte(0x0);
                m.WriteByte(0x0);
                m.WriteByte(0x0);
                m.WriteByte(skill1_level); //- > CONTROL SKILL POINTS
                m.WriteByte(skill2_level); // - > CONTROL SKILL 2 LEVEL
                m.WriteByte(skill3_level); // - > CONTROL SKILL 2 LEVEL
                m.WriteByte(skill4_level); // - > CONTROL SKILL 2 LEVEL
                m.WriteByte(skill5_level); // - > CONTROL SKILL 2 LEVEL
                m.WriteByte(skill_max_level);
                m.WriteByte(skill_max_level);
                m.WriteByte(skill_max_level);
                m.WriteByte(skill_max_level);
                m.WriteByte(skill_max_level);
                buffer = m.ToArray();
            }
            return buffer;
        }
    }
}
