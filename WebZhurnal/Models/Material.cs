using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebZhurnal.Models
{
    public class Material
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string FileName { get; set; }
        public string Link { get; set; }
        public int? SubjectId { get; set; }
        public Subject Subject { get; set; }
        public List<MaterialGroup> MaterialGroups { get; set; }
    }

    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ApplicationUser> Users { get; set; }
        public List<MaterialGroup> MaterialGroups { get; set; }
        public List<TeacherGroup> TeacherGroups { get; set; }
    }

    public class MaterialGroup
    {
        public int MaterialId { get; set; }
        public int GroupId { get; set; }

        public Material Material { get; set; }
        public Group Group { get; set; }
    }

    public class TeacherGroup
    {
        public string TeacherId { get; set; }
        public int GroupId { get; set; }

        public ApplicationUser Teacher { get; set; }
        public Group Group { get; set; }
    }

    
}
