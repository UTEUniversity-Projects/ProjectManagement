using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Enums
{
    public enum EGiveUpStatus
    {
        [Display(Name = "Pending")]
        PENDING,
        [Display(Name = "Accepted")]
        ACCEPTED,
        [Display(Name = "Rejected")]
        REJECTED
    }
}
