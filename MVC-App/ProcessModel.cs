using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MVC_App
{
    public class ProcessModel
    {
        public class ProcessSchema
        {
            [JsonProperty("schema")]
            public dynamic schema { get; set; }
            public string schemaId { get; set; }
            [JsonProperty("processInstanceId")]
            public string schemaInstanceId { get; set; }
            public string schemaTitle { get; set; }
            [JsonProperty("stepStatuses")]
            public List<StepStatus> stepStatuses { get; set; }

            [OnDeserialized]
            internal void OnDeserializedMethod(StreamingContext context)
            {
                if (schema != null)
                {
                    schemaId = schema.id;
                    schemaTitle = schema.title;
                }
            }
        }
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
     