using System;
using System.Collections.Generic;

namespace BarkNParkApplication.Models
{
    public partial class Transactions
    {
        public int TransId { get; set; }
        public string TransType { get; set; }
        public string TransTax { get; set; }
        public decimal TransSubtotal { get; set; }
        public decimal TransTotal { get; set; }
        public string TransReceipt { get; set; }
    }
}
