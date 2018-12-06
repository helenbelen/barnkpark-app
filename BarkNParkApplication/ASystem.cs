using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BarkNPark;
using BarkNParkApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace BarkNParkApplication
{
    public class ASystem : BarkNPark.System

    {
       BarkParkContext systemContext;

        public ASystem(BarkParkContext context) : base()
        {
            systemContext = context;
            this.stations.Clear();
            foreach (Stations s in systemContext.Stations)
            {
                this.stations.Add(new BarkNPark.Station((StationCode)s.StationId));
            }

        }

        public BarkParkContext Context { get { return this.systemContext; } }

        public DbSet<Appointments> SystemAppointments {get{return this.systemContext.Appointments;} }

        public DbSet<Stations> SystemStations { get { return this.systemContext.Stations; } }

        public DbSet<Transactions> SystemTransactions { get { return this.systemContext.Transactions; } }

        public DbSet<Users> SystemUsers { get { return this.systemContext.Users; } }

        public Users SystemUser(string name) {  return this.systemContext.Users.FirstOrDefault(u => u.UserFirstname == name); }

        public Appointments UserAppointment (string name) {

            Users user = SystemUser(name);
            return this.systemContext.Appointments.LastOrDefault(a => a.ApptUser == user.UserId);
        }

        public int AddUser(Users newUser)
        {
            Users user = this.systemContext.Users.LastOrDefault();
            int newId = user == null ? 1 : user.UserId + 1;

            this.systemContext.Add(
                new Users {
                    UserId = newId,
                    UserFirstname = newUser.UserFirstname,
                    UserLastname = newUser.UserLastname,
                    UserPaypal = newUser.UserPaypal
                }
             );

            this.systemContext.SaveChanges();
            return newId;
        }
        public override int CheckIn(string name, double duration)
        {

            IStation availStation = getFirstAvailableStation();
            if (availStation != null)
            {
                DateTime checkinTime = DateTime.Now;
                AApointment newappt = new AApointment(this, name, checkinTime, checkinTime.AddMinutes(duration));
                int confCode = newappt.Checkin(availStation, duration);
                if (confCode != (int)ErrorCode.SUCCESS)
                {
                    return confCode;
                }

                Users user = SystemUser(newappt.Name);
                Appointments appointment = systemContext.Appointments.LastOrDefault();
                
                int newId = appointment == null ? 100 : appointment.ApptId + 10;

                systemContext.Add(
                    new Appointments
                    {
                        ApptId = newId,
                        ApptUser = user.UserId,
                        ApptCheckin = newappt.CheckInTime,
                        ApptCheckout = newappt.ScheduledCheckOutTime,
                        ApptStation = (int)newappt.AppointmentStationCode
                    }


                );
                systemContext.SaveChanges();
                return confCode;
            }
            else
            {
                return (int)ErrorCode.NO_STAT;
            }


        }

        public override int Checkout(string name)
        {
            Appointments appt = UserAppointment(name);
            AApointment newAppointment = new AApointment(this, name, appt.ApptCheckin, appt.ApptCheckout);
            appt.ApptCheckout = DateTime.Now;
            systemContext.Update(appt);
            systemContext.SaveChanges();
           return newAppointment.Checkout();

        }

        public override int requestextension(string name, double timeToAdd)
        {
            Appointments appt = UserAppointment(name);
            AApointment newAppointment = new AApointment(this, name, appt.ApptCheckin, appt.ApptCheckout);
            DateTime oldcheckout = appt.ApptCheckout;
            appt.ApptCheckout = oldcheckout.AddMinutes(timeToAdd);
            systemContext.Update(appt);
            systemContext.SaveChanges();
            return newAppointment.ExtendTime(timeToAdd);
        }

        public int addItem(string name, string[] items)
        {
            ItemType[] newItems = new ItemType[items.Length];
            for(int i = 0; i < items.Length; i++)
            {
                newItems[i] = (ItemType)Int32.Parse(items[i]);
            }
            Appointments appt = UserAppointment(name);
            AApointment newAppointment = new AApointment(this, name, appt.ApptCheckin, appt.ApptCheckout);

            return newAppointment.DispenseItem(newItems);

        }
    }
}
