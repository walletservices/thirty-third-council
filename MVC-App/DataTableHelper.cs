using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_App
{
    public static class DataTableHelper
    {
        public static void Populate(DataTable table, IList<ProcessModel.StepStatus> data)
        {

            var firstNull = 0;
            foreach (var d in data)
            {
                if (d.completionTime == null)
                {
                    firstNull = d.stepIndex;
                    break;
                }
            }

            foreach (var d in data.Reverse())
            {
                var status = d.completionTime == null ? "To Do" : "Completed";
                if (d.completionTime == firstNull)
                {
                    status = "In Progress";    
                }
                object[] row = new object[3] { d.stepIndex, d.stepTitle, status };
                table.Rows.Add(row);
            }
        }

    }
}
