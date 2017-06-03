using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebZhurnal.Models
{
    public class StudentRate
    {
        public int Id { get; set; }
        public string StudentId { get; set; }
        public ApplicationUser Student { get; set; }
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
        public string TeacherId { get; set; }
        public ApplicationUser Teacher { get; set; }
        public int Value { get; set; }
        public DateTime Date { get; set; }
    }

    public class StudentRateModel
    {
        public string CurrentUserId { get; set; }
        public List<ApplicationUser> Students { get; set; }
        public List<Subject> Subjects { get; set; }
        public List<StudentRate> Rates { get; set; }
        public List<Group> Groups { get; internal set; }
        public string CurrentUserName { get; internal set; }
    }
}
