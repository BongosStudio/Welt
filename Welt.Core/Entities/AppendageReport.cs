using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Welt.API;

namespace Welt.Core.Entities
{
    public class AppendageReport
    {
        /// <summary>
        ///     The affected appendage of the report.
        /// </summary>
        public Appendage AffectedAppendage { get; set; }
        /// <summary>
        ///     The status of the appendage.
        /// </summary>
        public AppendageStatus Status { get; set; }
        /// <summary>
        ///     The time remaining for the appendage to remain in this status. If set to -1, 
        ///     it will remain this way indefinitely. 
        /// </summary>
        public float TimeRemaining { get; set; }

        public AppendageReport(Appendage appendage) : this(appendage, AppendageStatus.Normal, -1)
        {

        }

        public AppendageReport(Appendage appendage, AppendageStatus status, float time)
        {
            AffectedAppendage = appendage;
            Status = status;
            TimeRemaining = time;
        }
    }
}
