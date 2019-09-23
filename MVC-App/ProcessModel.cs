using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_App
{
    public class ProcessModel
    {
        [JsonObject]
        public class ProcessSchema
        {
            [JsonProperty("schemaId")]
            public string schemaId { get; set; }
            [JsonProperty("schemaInstanceId")]
            public string schemaInstanceId { get; set; }
            [JsonProperty("schemaTitle")]
            public string schemaTitle { get; set; }
            [JsonProperty("stepStatuses")]
            public List<StepStatus> stepStatuses { get; set; }
        }
        [JsonObject]
        public class StepStatus
        {
            [JsonProperty("stepIndex")]
            public int stepIndex { get; set; }
            [JsonProperty("stepTitle")]
            public string stepTitle { get; set; }
            [JsonProperty("completionTime")]
            public int? completionTime { get; set; }
        }

    }
}
     