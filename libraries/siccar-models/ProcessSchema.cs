
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Siccar.Shallow.Models
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


        public ProcessSchema()
        {
            stepStatuses = new List<StepStatus>();
        }

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
            var schemaIdHashCode = schemaId != null ? schemaId.GetHashCode() : "".GetHashCode();
            var schemaInstanceIdHashCode = schemaInstanceId != null ? schemaInstanceId.GetHashCode() : "".GetHashCode();
            var schemaTitleHashCode = schemaTitle != null ? schemaTitle.GetHashCode() : "".GetHashCode();
            var schemaHashCode = schema != null ? schema.GetHashCode() : "".GetHashCode();

            return schemaIdHashCode + schemaInstanceIdHashCode + schemaTitleHashCode + schemaHashCode;
        }
    }
}
