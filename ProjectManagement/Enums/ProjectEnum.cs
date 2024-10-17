using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Enums
{
    public enum EProjectStatus
    {
        [Display(Name = "Waiting")]
        WAITING,
        [Display(Name = "Published")]
        PUBLISHED,
        [Display(Name = "Registered")]
        REGISTERED,
        [Display(Name = "Processing")]
        PROCESSING,
        [Display(Name = "Completed")]
        COMPLETED,
        [Display(Name = "GaveUp")]
        GAVEUP
    }
}
