using System;
using System.Collections.Generic;

namespace MMABooksEFClasses.MarisModels
{
    public partial class Invoice
    {
        public Invoice()
        {
            InvoiceLineItem = new HashSet<InvoiceLineItem>();
        }

        public int InvoiceID { get; set; }
        public int CustomerID { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal ProductTotal { get; set; }
        public decimal SalesTax { get; set; }
        public decimal Shipping { get; set; }
        public decimal InvoiceTotal { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<InvoiceLineItem> InvoiceLineItem { get; set; }
    }
}