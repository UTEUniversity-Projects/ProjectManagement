using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Enums
{
    public enum EUserGender
    {
        [Display(Name = "Male")]
        MALE,
        [Display(Name = "Female")]
        FEMALE
    }

    public enum EUserRole
    {
        [Display(Name = "Student")]
        STUDENT,
        [Display(Name = "Lecture")]
        LECTURE
    }
}
