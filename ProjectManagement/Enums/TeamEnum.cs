using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Enums
{
    public enum ETeamStatus
    {
        [Display(Name = "Registered")]
        REGISTERED,
        [Display(Name = "Accepted")]
        ACCEPTED,
        [Display(Name = "Rejected")]
        REJECTED
    }
}
