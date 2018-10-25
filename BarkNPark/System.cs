using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarkNPark
{
    public enum ErrorCode
    {
        EXT_FAIL = 0,
        SALE_FAIL = 1,
        DIS_FAIL = 2,
        IN_FAIL = 3,
        OUT_FAIL = 4,
        DUR_INV = 5,
        NO_STAT = 6,
        STAT_FAIL = 7,
        PAY_FAIL = 8,
        SUCCESS = 200,
    }
    public class System
    {
        protected List<IStation> stations;
        List<IAppointment> appointments;

        public System()
        {
            stations = new List<IStation>() { new Station(StationCode.STATION_0), new Station(StationCode.STATION_1), new Station(StationCode.STATIOn_2) };
             appointments = new List<IAppointment>() { };
        }
        
        public List<IStation> listStations() { return stations; }

        public virtual int CheckIn( string name, double duration)
        {

            IStation availStation = getFirstAvailableStation();
            if (availStation != null)
            {
                DateTime checkinTime = DateTime.Now;
                Appointment newappt = new Appointment(this, name, checkinTime,checkinTime.AddMinutes(duration));
                int confCode = newappt.Checkin(availStation,duration);
                if ( confCode != (int)ErrorCode.SUCCESS)
                {
                    return confCode;
                }

                appointments.Add(newappt);
                return confCode;
            }
            else
            {
                return (int)ErrorCode.NO_STAT;
            }

            
        }

        public int Checkout(string name)
        {
            IAppointment relappt = getAppointmentByName(name);

           
           return relappt.Checkout();
           

        }

        public int requestextension(string name, double timeToAdd)
        {
            IAppointment relappt = getAppointmentByName(name);
   
                       
           return relappt.ExtendTime(timeToAdd);
        
        }

        public int addItem(string name,ItemType [] items)
        {
            IAppointment relappt = getAppointmentByName(name);
            if (relappt != null)
            {
                
                return relappt.DispenseItem(items);
            }

            return (int)ErrorCode.DIS_FAIL;
            
        }

        public IStation getFirstAvailableStation()
        {
            foreach (IStation station in stations){
                if (station.isAvailable()) { return station; }
            }

            return null;
        }

        public IAppointment getAppointmentByName(string name)
        {
            foreach (IAppointment appt in appointments)
            {
                if (appt.Name == name) return appt;
            }

            return null;
        }



        public IStation GetStation(StationCode code)
        {
            foreach (Station s in stations)
            {
                if (s.Code == code)
                {
                    return s;
                }
            }

            return null;
        }
                  
               

        public override string ToString()
        {
            string s = "System Appointments: \n";
            foreach (IAppointment appt in this.appointments)
            {
                s += appt.ToString() + "\n";
                s += appt.PrintSales() + "\n";
                
                
            }
            s += "End Of System Appointments \n";
            return s;
        }
    

        

      
    }
}
