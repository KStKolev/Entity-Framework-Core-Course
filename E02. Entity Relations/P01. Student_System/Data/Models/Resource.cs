using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using P01_StudentSystem.Data.Models.Constraints;
using P01_StudentSystem.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_StudentSystem.Data.Models
{
    public class Resource
    {
        [Key]
        public int ResourceId { get; set; }

        [Required]
        [StringLength(DataConstraints.MaxResourceNameCharacters)]
        [Unicode]
        public string Name { get; set; } = null!;

        public string Url { get; set; } = null!;

        public ResourceType ResourceType {get; set; }

        public int CourseId { get; set; }
        [ForeignKey(nameof(CourseId))]

        public Course Course { get; set; } = null!;
    }
}
