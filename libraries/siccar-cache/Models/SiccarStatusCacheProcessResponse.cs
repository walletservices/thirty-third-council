using Siccar.Shallow.Models;
using System;
using System.Collections.Generic;

namespace Siccar.CacheManager.Models
{
    public class SiccarStatusCacheProcessResponse
    {
        public string Schema { get; set; }
        public ProcessStatus ProcessStatus { get; set; }
        public List<KeyValuePair<string, string>> Attributes { get; set; }

        public SiccarStatusCacheProcessResponse(string schema, ProcessStatus aProcessStatus, List<KeyValuePair<string, string>> attributes)
        {
            Schema = schema;
            ProcessStatus = aProcessStatus;
            Attributes = attributes;
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
                SiccarStatusCacheProcessResponse p = (SiccarStatusCacheProcessResponse)obj;
                return p.Schema == this.Schema;
            }
        }

        public override int GetHashCode()
        {
            return Schema.GetHashCode();
        }
    }
}
