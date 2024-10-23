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
        REJECTED,
        [Display(Name = "0")]
        NONE
    }

    public enum ETeamRole
    {
        [Display(Name = "Leader")]
        LEADER,
        [Display(Name = "Member")]
        MEMBER,
        [Display(Name = "0")]
        NONE
    }
}
