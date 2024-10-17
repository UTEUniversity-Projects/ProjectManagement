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

    public enum ETeamRole
    {
        [Display(Name = "Leader")]
        LEADER,
        [Display(Name = "Member")]
        MEMBER
    }
}
