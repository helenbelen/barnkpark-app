using System;

namespace BarkNPark
{
    public enum StationCode
    {

        STATION_0 = 0,
        STATION_1 = 1,
        STATIOn_2 = 2
    }


    public interface IStation
    {
        bool isAvailable();
        int open_door();
        int check_out();
        int DispenseItem(ItemType[] items);
        int changeStatus(bool broken, bool inMain);
        StationCode stationCode { get; }

    }

    public class Station : IStation
    {
        StationCode station_code;
        Random r = new Random();
        public bool Broken { get; set; }
        public bool Maintenance { get; set; }
        public bool Occupied { get; set; }
        public StationCode Code { get { return station_code; } }

        public Station(StationCode code)
        {
            station_code = code;
        }
        public bool isAvailable()
        {
            return !(Broken | Maintenance | Occupied);
        }

        public StationCode stationCode { get { return this.station_code; } } 
        

        public int open_door()
        {
            Console.WriteLine("DOOR OPENED. CHECKEDIN TO " + station_code);
            Occupied = true;


            return (int)ErrorCode.SUCCESS;
        }

        public int check_out()
        {
            Console.WriteLine("CHECKEDOUT OF " + station_code + " BYE!");
            Occupied = false;
            return (int)ErrorCode.SUCCESS;
        }

        public int DispenseItem(ItemType[] items)
        {
            Console.WriteLine("Dispensing....");
            for(int i = 0; i < items.Length; i++)
            {
                Console.WriteLine(items[i]);

            }
            Console.WriteLine("Finised Dispensing.");
            return (int)ErrorCode.SUCCESS;
        }
        public int changeStatus(bool broken, bool inMain)
        {
            if (broken && !inMain)
            {
                this.Broken = broken;
                return (int)ErrorCode.SUCCESS;
            }
           else if (inMain && !broken)
            {
                this.Maintenance = true;
                return (int)ErrorCode.SUCCESS;
            }

            return (int)ErrorCode.STAT_FAIL;
        }
        public override string ToString()
        {
            return station_code.ToString() + " Check-In status: " + Occupied;
        }

    }
}