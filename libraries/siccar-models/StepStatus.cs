using Newtonsoft.Json;
using System;

namespace Siccar.Shallow.Models
{
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
            var stepTransactionHashCode = stepTransactionId == null ? "".GetHashCode() : stepTransactionId.GetHashCode();
            var stepTitleHashCode = stepTitle == null ? "".GetHashCode() : stepTitle.GetHashCode();
            return stepTransactionHashCode + stepTitleHashCode;

        }
    }
}
