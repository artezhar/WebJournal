using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebZhurnal.Models
{
    public class Subject
    {
        public int Id { get; set; }

        [Display(Name="Название")]
        public string Name { get; set; }

        public List<Material> Materials { get; set; }

        public List<SubjectGroup> SubjectGroups { get; set; }
    }
}
