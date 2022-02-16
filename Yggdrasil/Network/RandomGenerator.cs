using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yggdrasil.Network
{
    public class RandomGenerator
    {
	    protected const int TABLE_SIZE = 250;
	    private uint m_nState;
	    private int m_nIndex1;
	    private int m_nIndex2;
	    private uint m_seed;
	    private uint[] m_table = new uint[TABLE_SIZE];

        public RandomGenerator(uint seed)
        {
            Init(seed);
        }

        public uint Generate()
        {
            uint retval = (m_table[m_nIndex1] ^= m_table[m_nIndex2]);
	        m_nIndex1++;
	        if (m_nIndex1 == TABLE_SIZE)
		        m_nIndex1 = 0;
	        m_nIndex2++;
	        if (m_nIndex2 == TABLE_SIZE)
		        m_nIndex2 = 0;
	        return retval;
        }
	    public void Reset()
        {
            m_nState = m_seed;
            GenerateSeeds();
        }

        private void Init(uint seed)
        {
            m_seed = seed;
            m_nState = seed;
            GenerateSeeds();
        }

        protected void GenerateSeeds()
        {
            int n;
	        uint msk, bit;
	        m_nIndex1 = 0;
	        m_nIndex2 = 103;
	        for (n = TABLE_SIZE - 1; n >= 0; n--)
		        m_table[n] = GenerateSimple();
	        for (n = 3, msk = 0xffffffff, bit = 0x80000000; bit != 0; n += 7)
	        {
		        m_table[n] = (m_table[n] & msk) | bit;
		        msk >>= 1;
		        bit >>= 1;
	        }
        }

        protected uint GenerateSimple()
        {
            uint n, bit, temp;
	        temp = m_nState;
	        for (n = 0; n < 32; n++)
	        {
		        bit = ((temp >> 0) ^ (temp >> 1) ^ (temp >> 2) ^ (temp >> 3) ^ (temp >> 5) ^ (temp >> 7)) & 1;
		        temp = (uint)(((temp >> 1) | (temp << 31)) & ~1) | bit;
	        }
	        m_nState = temp;
	        return m_nState;
        }
    }
}
