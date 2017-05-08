using System.ComponentModel.DataAnnotations;

namespace WebZhurnal.Models
{
    public enum UserType
    {
        [Display(Name="Учитель")]
        Teacher,
        [Display(Name = "Ученик")]
        Student
    }
}