using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarkNPark
{


    public enum ItemType
    {
        HOUR = 0,
        DOG_TREAT = 1,
        WATER = 2,
        TOY = 3,

    }

    interface ISale {

        int ProcessPayment(string payPalEmail);
        double getSubTotal();
        double getFinalTotal();
        double getTaxTotal();
        char getTaxCode();
    }

    public class Sale : ISale
    {
        ItemType[] saleItems;
        List<ItemType> paidItems = new List<ItemType>();
        protected Dictionary<ItemType, double> priceList = new Dictionary<ItemType, double>() {
            { ItemType.DOG_TREAT, 1.99 },
            { ItemType.TOY,4.99 },
            { ItemType.WATER, 2.99},
            {ItemType.HOUR, 10.00 }
        };

        protected string receipt;
        private int SALE_NUM = 1;
        protected double SALE_TOTAL = 0;
        protected double FINAL_TOTAL = 0;
        protected double TAX_TOTAL = 0;
        protected char TAX_CODE = 'D';

        public Sale(ItemType[] items)
        {
            saleItems = items;
        }

        private int SaleNumber { get { return SALE_NUM++; } }
        public double getSubTotal() => SALE_TOTAL;
        public double getFinalTotal() => FINAL_TOTAL;
        public double getTaxTotal() => TAX_TOTAL;
        public char getTaxCode() => TAX_CODE;


        private double addToTotal(double amount)
        {
            return SALE_TOTAL += amount;
        }
        protected double calculateTaxAmount(char taxRate = 'D')
        {
            if (taxRate == 'D')
                TAX_TOTAL = SALE_TOTAL * .10;
            else
                TAX_TOTAL = SALE_TOTAL * .15;
            return TAX_TOTAL;
        }

        
        protected double calculateFinalTotal(char taxRate = 'D')
        {
            FINAL_TOTAL = SALE_TOTAL + calculateTaxAmount(taxRate);
            
            return Double.Parse(String.Format("{0:00.##}",FINAL_TOTAL));
        }

        public int ProcessPayment(string payPalEmail)
        {
            double currentSaleTotal = SALE_TOTAL;
            foreach(ItemType item in saleItems)
            {
                if (!paidItems.Contains(item))
                {
                    addToTotal(priceList[item]);
                    paidItems.Add(item);
                }
            }
            if (currentSaleTotal < SALE_TOTAL)
            {
                calculateFinalTotal();
                return (int)ErrorCode.SUCCESS;
            }
            return (int)ErrorCode.PAY_FAIL;
        }

        public override string ToString()
        {
            receipt = "";
            receipt += String.Format("Your Receipt For Sale {0}", this.SaleNumber + "\n");
            foreach (ItemType item in saleItems)
            {
                double price = 0;
                switch (item) {
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
                    default:
                        
                        break;


                }

            }

            receipt += String.Format("Tax : {0}", calculateTaxAmount()) + "\n";
            receipt += String.Format("Total Cost : {0}", calculateFinalTotal()) + "\n";

            return receipt;
        }
    }
}
