using System;
using System.Collections.Generic;

namespace MMABooksEFClasses.MarisModels
{
    public partial class Customer
    {
        public Customer()
        {
            Invoice = new HashSet<Invoice>();
        }

        public int CustomerID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string ZipCode { get; set; }

        public override string ToString()
        {
            return CustomerID + ", " + Name + ", " + Address + ", " + City + ", " + StateCode + ", " + ZipCode;
        }

        public virtual State State { get; set; }
        public virtual ICollection<Invoice> Invoice { get; set; }
    }
}