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


            int confCode = transaction.ProcessPayment("email");


            Transactions dbtrans = system.Context.Transactions.LastOrDefault();
            int newId = dbtrans == null ? 200 : dbtrans.TransId + 20;

            this.system.Context.Add(
                new Transactions {

                    TransId = newId,
                    TransType = ((int)type).ToString(),
                    TransTax = transaction.getTaxCode().ToString(),
                    TransSubtotal = (decimal)transaction.getSubTotal(),
                    TransTotal = (decimal)transaction.getFinalTotal()


                }

            );
            this.system.Context.SaveChanges();
            return confCode;
        }
    }
}
