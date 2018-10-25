using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarkNPark
{
    public class Refund : Sale
    {
        ItemType[] refundItems;
        private int REFUND_NUM = 1;
        public Refund(ItemType [] items) : base (items)
        {
            refundItems = items;
        }

        private int RefundNumber { get { return REFUND_NUM++; } }

        public override string ToString()
        {
            receipt = "";
            receipt += String.Format("Your Receipt For Refund {0}", this.RefundNumber + "\n");
            foreach (ItemType item in refundItems)
            {
                double price = 0;
                switch (item)
                {
                    case ItemType.DOG_TREAT:
                        price = priceList[ItemType.DOG_TREAT];
                        receipt += String.Format("Dog Treat: {0}", price) + "\n";
                        break;
                    case ItemType.TOY:
                        price = priceList[ItemType.TOY];
                        receipt += String.Format("Dog Toy: {0}", price) + "\n";
                        break;
                    case ItemType.WATER:
                        price = priceList[ItemType.WATER];
                        receipt += String.Format("Water : {0}", price) + "\n";
                        break;
                    case ItemType.HOUR:
                        price = priceList[ItemType.HOUR];
                        receipt += String.Format("Hours in station: {0}", price) + "\n";
                        break;
                    default:

                        break;


                }

            }

            receipt += String.Format("Tax Refunded : {0}", calculateTaxAmount()) + "\n";
            receipt += String.Format("Total Refund : {0}", calculateFinalTotal()) + "\n";

            return receipt;
        }
    }

}

