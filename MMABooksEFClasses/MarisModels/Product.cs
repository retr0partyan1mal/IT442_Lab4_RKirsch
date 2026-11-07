using System;
using System.Collections.Generic;

namespace MMABooksEFClasses.MarisModels
{
    public partial class Product
    {
        public Product()
        {
            InvoiceLineItem = new HashSet<InvoiceLineItem>();
        }

        public string ProductCode { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public int OnHandQuantity { get; set; }

        public virtual ICollection<InvoiceLineItem> InvoiceLineItem { get; set; }
    }
}
