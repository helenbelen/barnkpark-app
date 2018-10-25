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

        public Users SystemUser(int id) {  return this.systemContext.Users.FirstOrDefault(u => u.UserId == id); }

        public int AddUser(string firstname, string lastname)
        {
            Users user = this.systemContext.Users.LastOrDefault();
            int newId = user == null ? 1 : user.UserId + 1;

            this.systemContext.Add(
                new Users {
                    UserId = newId,
                    UserFirstname = firstname,
                    UserLastname = lastname,

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
                Appointment newappt = new Appointment(this, name, checkinTime, checkinTime.AddMinutes(duration));
                int confCode = newappt.Checkin(availStation, duration);
                if (confCode != (int)ErrorCode.SUCCESS)
                {
                    return confCode;
                }

                Users user = systemContext.Users.FirstOrDefault(u => u.UserFirstname == newappt.Name);
                Appointments appointment = systemContext.Appointments.LastOrDefault();
                int userId = user == null ? 1 : user.UserId + 1;
                int newId = appointment == null ? 100 : appointment.ApptId + 10;

                systemContext.Add(
                    new Appointments
                    {
                        ApptId = newId,
                        ApptUser = userId,
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


    }
}
