using System;
using System.Collections.Generic;

namespace MMABooksEFClasses.MarisModels
{
    public partial class State
    {
        public State()
        {
            Customer = new HashSet<Customer>();
        }

        public string StateCode { get; set; }
        public string StateName { get; set; }

        public override string ToString()
        {
            return StateCode + ", " + StateName;
        }

        public virtual ICollection<Customer> Customer { get; set; }
    }
}
