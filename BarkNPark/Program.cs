using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarkNPark
{
    public class Program
    {

        public static void Main (string [] args)
        {
            
            System system = new System();
            Console.WriteLine("Welcome to Bark N Park Console Tester");
            Console.WriteLine("Please enter a commad: a = checkin, b- extend time, c-dispense item, d- checkout, s- summary");
            string s = "";
            string name = "";
            while(s != "exit")
            {
                s = Console.ReadLine();

                switch (s.ToLower()[0]) {

                    case 'a':
                        Console.WriteLine("Enter Appointment Name: ");
                         name = Console.ReadLine();
                        Console.WriteLine(system.CheckIn(name, 60));
                        if( system.getAppointmentByName(name) != null)
                        {
                            Console.WriteLine(system.getAppointmentByName(name).ToString());
                        }
                        
                        break;
                    case 'b':
                        Console.WriteLine("Enter Appointment Name: ");
                         name = Console.ReadLine();
                        Console.WriteLine(system.requestextension(name, 30));
                        if (system.getAppointmentByName(name) != null)
                        {
                            Console.WriteLine(system.getAppointmentByName("helen").ToString());
                        }
                        
                        break;
                    case 'c':
                        Console.WriteLine("Enter Appointment Name: ");
                         name = Console.ReadLine();
                        ItemType[] items = { ItemType.DOG_TREAT, ItemType.TOY };

                        Console.WriteLine(system.addItem(name, items));

                        break;
                    case 'd':
                        Console.WriteLine("Enter Appointment Name: ");
                         name = Console.ReadLine();
                        Console.WriteLine(system.Checkout(name));
                        break;
                    case 's':
                        Console.WriteLine(system.ToString());

                        break;

                }

               
            }

        }
    }

}
