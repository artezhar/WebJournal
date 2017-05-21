using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebZhurnal.Models
{
    public class FileModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public DateTime DateTimeUploaded { get; set; }

    }
}
