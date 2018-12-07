using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarkNPark
{

    public enum TransactionType {

        SALE = 0,
        REFUND = 1,

    }

    public interface IAppointment

    {
        int Checkin(IStation station, double duration);
        int ExtendTime(double duration);
        int Checkout();
        int DispenseItem(ItemType[] items);
        string Name { get; set; }
        StationCode AppointmentStationCode { get; }
        string PrintSales();
        
    }

    public class Appointment : IAppointment
    {
        System system;
        protected IStation appointmentStation;
        protected string appointmentName;
        protected DateTime appointmentCheckin, scheduledAppointmentCheckout, actualAppointmentCheckout;
        List<ISale> appointmentSales = new List<ISale>();

        public Appointment(System mySystem, string name, DateTime checkinTime, DateTime checkoutTime)
        {
           system = mySystem;
           appointmentName = name;
           appointmentCheckin = checkinTime;
           scheduledAppointmentCheckout = checkoutTime;
            actualAppointmentCheckout = scheduledAppointmentCheckout;
        }

        public string Name
        {
            get { return this.appointmentName; }
            set { appointmentName = value; }
        }

        public StationCode AppointmentStationCode { get { return appointmentStation.stationCode; }  }

        public DateTime CheckInTime { get { return appointmentCheckin; } set { appointmentCheckin = value; }  }

        public DateTime ScheduledCheckOutTime { get { return scheduledAppointmentCheckout; } set { scheduledAppointmentCheckout = value; } }

        public DateTime ActualCheckoutTime { get { return actualAppointmentCheckout; } set { actualAppointmentCheckout = value; } }

        public virtual int Checkin(IStation station, double duration)
        {
            if (durationValid(this.CheckInTime, duration))
            {

                double hours = requestTimeFrame(this.CheckInTime, duration);
                int confCode = ProcessTransaction(createHoursArray(hours),TransactionType.SALE);
                this.appointmentStation = station;

                appointmentStation.open_door();

                return confCode;
            }               

            return (int)ErrorCode.DUR_INV;
        }

        public virtual int Checkout()
        {
            int confCode = -1;
            double minutesUnused = Math.Floor((DateTime.Now - this.ScheduledCheckOutTime).TotalHours);
            if (minutesUnused < 0)
            {
               confCode = ProcessTransaction(createHoursArray(minutesUnused *-1), TransactionType.REFUND);
            }
            else if(minutesUnused > 10) // 10 minute grace period before additional charges
            {
                confCode = ProcessTransaction(createHoursArray(minutesUnused), TransactionType.SALE);

            }
            actualAppointmentCheckout = DateTime.Now;
            appointmentStation.check_out();
            return confCode;
        }

        public virtual int DispenseItem(ItemType[] items)
        {
            int confCode = ProcessTransaction(items, TransactionType.SALE);
            this.appointmentStation.DispenseItem(items);
            return (int)ErrorCode.SUCCESS;
        }

        public virtual int ExtendTime(double duration)
        {
            DateTime temp = scheduledAppointmentCheckout;
            int confCode = -1;
            if (durationValid(this.CheckInTime, duration))
            {
                double hours = requestTimeFrame(this.CheckInTime, duration);
                confCode = ProcessTransaction(createHoursArray(hours), TransactionType.SALE);
                ScheduledCheckOutTime = ScheduledCheckOutTime.AddMinutes(duration);
            }

            return confCode;


        }

        public virtual int ProcessTransaction(ItemType[] items, TransactionType type)
        {
            Sale transaction = null;
           
            switch (type) {
                case TransactionType.SALE:
                    transaction = new Sale(items);
                  
                    break;
                case TransactionType.REFUND:
                    transaction = new Refund(items);
                   
                    break;

            }
                        
            
            int confCode = transaction.ProcessPayment("email");

            Console.WriteLine();
            Console.Write(transaction.ToString());
            Console.WriteLine();
            ISale sale = transaction;
            appointmentSales.Add(sale);

            return confCode;
        }

      

        public ItemType[] createHoursArray(double numberofHours)
        {

            int roundedHours = numberofHours >= 0 ? (int)Math.Ceiling(numberofHours) : (int)Math.Ceiling(numberofHours *-1);
            ItemType[] appoint_hours = new ItemType[roundedHours];
            for (int i = 0; i < appoint_hours.Length; i++)
            {
                appoint_hours[i] = ItemType.HOUR;
            }
            return appoint_hours;
        }

        public double requestTimeFrame(DateTime checkinTime, double duration)
        {
            DateTime proposedCheckOut = this.scheduledAppointmentCheckout.AddMinutes(duration);
            TimeSpan minutesInStation = proposedCheckOut - checkinTime;
            return minutesInStation.TotalHours;
        }

        public bool durationValid(DateTime checkInTime, double duration)
        {
            
            if(requestTimeFrame(checkInTime,duration) >= 4)
            {
                return false;
            }
           
            return true;
        }

        public string PrintSales()
        {
            string s = "Sales for " + this.appointmentName + ": \n";

            foreach(ISale sale in appointmentSales)
            {
                s += sale.ToString() + "\n";
            }
            return s;
        }
        override public string ToString()
        {
            if (this.appointmentStation != null)
            {
                return "This appointment station is : " + this.appointmentStation.ToString() + " \n Check In Occured At : " + this.CheckInTime.ToString() + " \n Check Out Scheduled For : " + this.ScheduledCheckOutTime.ToString() + "\n";
            }
            else if(this.ActualCheckoutTime < DateTime.Now)
            {
                return "This appointment station is : " + this.appointmentStation.ToString() + " \n Check In Occured At : " + this.CheckInTime.ToString() + "\n Check out Occured at : " + this.ActualCheckoutTime.ToString() + "\n";
            }

            return "";
        }
    }
}
