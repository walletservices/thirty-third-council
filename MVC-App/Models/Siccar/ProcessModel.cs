using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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

            public override bool Equals(Object obj)
            {
                //Check for null and compare run-time types.
                if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                {
                    return false;
                }
                else
                {
                    ProcessSchema p = (ProcessSchema)obj;
                    return p.schemaId == this.schemaId
                        && p.schemaInstanceId == this.schemaInstanceId
                        && p.schemaTitle == this.schemaTitle
                        && !p.stepStatuses.Except(this.stepStatuses).Union(this.stepStatuses.Except(p.stepStatuses)).Any();
                }
            }

            public override int GetHashCode()
            {
                return schemaId.GetHashCode()
                    + schemaInstanceId.GetHashCode()
                    + schemaTitle.GetHashCode()
                + stepStatuses.GetHashCode();
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

            [JsonProperty("stepTransactionId")]
            public string stepTransactionId { get; set; }

            public override bool Equals(Object obj)
            {
                //Check for null and compare run-time types.
                if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                {
                    return false;
                }
                else
                {
                    StepStatus p = (StepStatus)obj;
                    return p.stepIndex == this.stepIndex
                        && p.stepTitle == this.stepTitle
                        && p.completionTime == this.completionTime
                        && p.stepTransactionId == this.stepTransactionId;
                }
            }

            public override int GetHashCode()
            {
                if (stepTransactionId == null)
                {
                    return stepTitle.GetHashCode();
                }
                else
                {
                    return stepTitle.GetHashCode() + stepTransactionId.GetHashCode();
                }
                
                    
            }
        }

    }
}
