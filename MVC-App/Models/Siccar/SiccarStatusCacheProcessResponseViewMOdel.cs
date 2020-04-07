using Siccar.CacheManager.Models;
using System;
using System.Collections.Generic;
using Siccar.Shallow.Models;

namespace MVC_App.Models
{
    public class SiccarStatusCacheProcessResponseViewModel
    {
        public string Schema { get; set; }
        public ProcessStatus ProcessStatus { get; set; }
        public List<KeyValuePair<string, string>> Attributes { get; set; }

        public SiccarStatusCacheProcessResponseViewModel(SiccarStatusCacheProcessResponse parent)
        {
            this.Schema = parent.Schema;
            this.ProcessStatus = parent.ProcessStatus;
            this.Attributes = parent.Attributes;
        }

    }
}
