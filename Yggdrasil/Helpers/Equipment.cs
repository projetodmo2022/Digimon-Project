using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yggdrasil.Entities;
using System.Collections;
using Yggdrasil.Entities;

namespace Yggdrasil.Helpers
{
    [Serializable]
    public class Equipment :IEnumerable<Item>
    {
        public enum Slot : int
        {
            Head = 0,
            Fashion,
            Top,
            Bottom,
            Shoes,
            Gloves,
            Costume,
            Rings,
            Necklaces,
            Earring

        }

        protected List<Item> m_equip;
        
        /// <summary>
        /// Initialize new Equipment with empty slots.
        /// </summary>
        public Equipment()
        {
            m_equip = new List<Item>();
            for (int i = 0; i < 15; i++)
                m_equip.Add(new Item());
        }

        public Item this[int index]
        {
            get
            {
                return m_equip[index];
            }
            set
            {
                m_equip[index] = value;
            }
        }

        public IEnumerator<Item> GetEnumerator()
        {
            return m_equip.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (m_equip as IEnumerable).GetEnumerator();
        }
    }
}
