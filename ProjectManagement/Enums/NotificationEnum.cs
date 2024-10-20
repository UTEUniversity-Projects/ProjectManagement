using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Enums
{
    public enum ENotificationType
    {
        [Display(Name = "Project")]
        PROJECT,
        [Display(Name = "Task")]
        TASK,
        [Display(Name = "Evaluation")]
        EVALUATION,
        [Display(Name = "Comment")]
        COMMENT,
        [Display(Name = "Meeting")]
        MEETING,
        [Display(Name = "Give up")]
        GIVEUP,
    }
}
