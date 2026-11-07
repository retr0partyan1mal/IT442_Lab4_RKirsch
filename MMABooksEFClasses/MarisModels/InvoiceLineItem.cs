using System;
using System.Collections.Generic;

namespace MMABooksEFClasses.MarisModels
{
    public partial class InvoiceLineItem
    {
        public int InvoiceID { get; set; }
        public string ProductCode { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal ItemTotal { get; set; }

        public virtual Invoice Invoice { get; set; }
        public virtual Product ProductCodeNavigation { get; set; }
    }
}