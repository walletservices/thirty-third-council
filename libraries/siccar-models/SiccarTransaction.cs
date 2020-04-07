using System;
using System.Collections.Generic;

namespace Siccar.Shallow.Models
{
    public class SiccarTransaction
    {
        public string TransactionId { get; set; }

        public HashSet<KeyValuePair<string, string>> Attributes { get; set; }

        public SiccarTransaction()
        {
            Attributes = new HashSet<KeyValuePair<string, string>>();
        }

        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                SiccarTransaction p = (SiccarTransaction)obj;
                return p.TransactionId == this.TransactionId;
            }
        }

        public override int GetHashCode()
        {
            var txHashCode = TransactionId == null ? "".GetHashCode() : TransactionId.GetHashCode();
            return txHashCode;
        }
    }
}
