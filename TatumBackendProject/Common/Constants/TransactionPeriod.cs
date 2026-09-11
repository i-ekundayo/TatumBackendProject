using System.ComponentModel.DataAnnotations;

namespace TatumBackendProject.Common.Constants
{
    public enum TransactionPeriod
    {
        [Display(Name = "Yesterday")]
        Yesterday,

        [Display(Name = "Last 7 Days")]
        Last7Days,

        [Display(Name = "Last 30 Days")]
        Last30Days,

        [Display(Name = "Last 90 Days")]
        Last90Days
    }
}
