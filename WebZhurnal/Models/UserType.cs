using System.ComponentModel.DataAnnotations;

namespace WebZhurnal.Models
{
    public enum UserType
    {
        [Display(Name="Учитель")]
        Teacher,
        [Display(Name = "Ученик")]
        Student,
             [Display(Name = "Администратор")]
        Admin,
             [Display(Name = "Класс")]
        Group
    }

    public static class Extensions
    {
        public static string Translate (this string s)
        {
            switch (s)
            {
                case "Teacher": return "Учитель";
                case "Student": return "Ученик";
                case "Group": return "Класс";
                case "Admin": return "Администратор";
            }
            return s;
        }
    }
}