using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BarkNPark;
using BarkNParkApplication.Models;

namespace BarkNParkApplication
{
    public class AApointment : Appointment
    {
        ASystem system;
        protected Dictionary<ItemType, double> priceList = new Dictionary<ItemType, double>() {
            { ItemType.DOG_TREAT, 1.99 },
            { ItemType.TOY,4.99 },
            { ItemType.WATER, 2.99},
            {ItemType.HOUR, 10.00 }
        };
        public AApointment(ASystem mySystem, string name, DateTime checkinTime, DateTime checkoutTime) : base(mySystem,name,checkinTime,checkoutTime){
            system = mySystem;
        }

        
        public override int ProcessTransaction(ItemType[] items, TransactionType type)
        {
            Sale transaction = null;

            switch (type)
            {
                case TransactionType.SALE:
                    transaction = new Sale(items);

                    break;
                case TransactionType.REFUND:
                    transaction = new Refund(items);

                    break;

            }


           
            Transactions dbtrans = system.Context.Transactions.LastOrDefault();
            int newId = dbtrans == null ? 200 : dbtrans.TransId + 20;

            this.system.Context.Add(
                new Transactions {

                    TransId = newId,
                    TransType = ((int)type).ToString(),
                    TransTax = transaction.getTaxCode().ToString(),
                    TransSubtotal = (decimal)transaction.getSubTotal(),
                    TransTotal = (decimal)transaction.getFinalTotal(),
                    TransReceipt = PrintSales(newId.ToString(), items, transaction.getTaxCode().ToString(), transaction.getFinalTotal().ToString())

                }

            );
            this.system.Context.SaveChanges();
            return transaction.ProcessPayment("email");
        }

        public override int Checkout()
        {
            int confCode = (int)ErrorCode.IN_FAIL;
            double minutesUnused = Math.Floor((DateTime.Now - this.ScheduledCheckOutTime).TotalHours);
            if (minutesUnused <= 10)
            {
                confCode = ProcessTransaction(createHoursArray(minutesUnused * -1), TransactionType.REFUND);
            }
            else // 10 minute grace period before additional charges
            {
                confCode = ProcessTransaction(createHoursArray(minutesUnused), TransactionType.SALE);

            }
            
            
            return confCode;

        }

        public override int ExtendTime(double duration)
        {
            DateTime temp = scheduledAppointmentCheckout;
            int confCode = (int)ErrorCode.EXT_FAIL;
            if (durationValid(this.CheckInTime, duration))
            {
                double hours = requestTimeFrame(this.CheckInTime, duration);
                confCode = ProcessTransaction(createHoursArray(hours), TransactionType.SALE);
                
            }

            return confCode;


        }

        public override int DispenseItem(ItemType[] items)
        {
                 
            return ProcessTransaction(items, TransactionType.SALE);
        }

        public string PrintSales(string saleNumber, ItemType[] saleItems, string tax, string total)
        {         
       
            string receipt = "";
            receipt += String.Format("Your Receipt For Sale {0} ", saleNumber + "\n");
            foreach (ItemType item in saleItems)
            {
                double price = 0;
                switch (item)
                {
                    case ItemType.DOG_TREAT:
                        price = priceList[ItemType.DOG_TREAT];
                        receipt += String.Format("Dog Treat: $ {0} ", price) + "\n";
                        break;
                    case ItemType.TOY:
                        price = priceList[ItemType.TOY];
                        receipt += String.Format("Dog Toy: $ {0} ", price) + "\n";
                        break;
                    case ItemType.WATER:
                        price = priceList[ItemType.WATER];
                        receipt += String.Format("Water : $ {0} ", price) + "\n";
                        break;
                    case ItemType.HOUR:
                        price = priceList[ItemType.HOUR];
                        receipt += String.Format("Hour(s): $ {0} ", price) + "\n";
                        break;

                    default:

                        break;


                }

            }

            receipt += String.Format("Tax : {0} ", tax + "\n");
            receipt += String.Format("Total Cost : {0} ", total + "\n");

            return receipt;
        }
    }
   
}
