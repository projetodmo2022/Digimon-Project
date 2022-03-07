using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yggdrasil.Helpers;
using System.IO;
using Digital_World;


namespace Yggdrasil.Database
{
    public class RideDB
    {
        public static Dictionary<int, Ride> Ride = new Dictionary<int, Ride>();

        public static void Load(string fileName)
        {
            
            using (Stream s = File.OpenRead(fileName))
            {
                using (BitReader read = new BitReader(s))
                {
                    int dcount = read.ReadInt();
                    for (int i = 0; i < dcount; i++)
                    {                      
                        Ride ride = new Ride();
                        ride.s_dwDigimonID = read.ReadInt();
                        ride.s_dwChangeRide = read.ReadInt();
                        ride.s_fMoveSpeed = read.ReadFloat();
                        ride.s_szComment = read.ReadZString(Encoding.Unicode, 512 * 2);
                        ride.s_nRideType = read.ReadInt();
                        ride.s_fAniRate_Run = read.ReadFloat();
                        ride.s_nItemType_S = read.ReadInt();
                        ride.s_nNeedCount = read.ReadInt();
                        ride.s_nItemType_S_2 = read.ReadInt();
                        ride.s_nNeedCount_2 = read.ReadInt();


                        if (!Ride.ContainsKey(ride.s_dwDigimonID))
                        {
                           Ride.Add(ride.s_dwDigimonID, ride);
                        }

                    }
                }
            }
            SysCons.LogDB("Ride.bin", "Loaded {0} Ride.", Ride.Count);
        }
    }

    public class Ride
    {
     public   int dcount;
     public int s_dwDigimonID;
     public int  s_dwChangeRide;
     public float s_fMoveSpeed;
     public string  s_szComment;
     public  int s_nRideType;
     public float s_fAniRate_Run;
     public  int s_nItemType_S;
     public int s_nNeedCount;
     public int s_nItemType_S_2;
      public int s_nNeedCount_2;





    }





}


