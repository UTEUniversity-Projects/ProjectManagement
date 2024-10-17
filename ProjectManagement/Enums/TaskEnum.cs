using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Enums
{
    public enum ETaskStatus
    {
        [Display(Name = "Waiting")]
        WAITING,
        [Display(Name = "Processing")]
        PROCESSING,
        [Display(Name = "Completed")]
        COMPLETED,
        [Display(Name = "Canceled")]
        CANCELED
    }
    public enum ETaskPriority
    {
        [Display(Name = "High")]
        HIGH,
        [Display(Name = "Medium")]
        MEDIUM,
        [Display(Name = "Low")]
        LOW
    }
}
