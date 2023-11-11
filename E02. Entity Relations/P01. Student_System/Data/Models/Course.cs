using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models.Constraints;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_StudentSystem.Data.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }

        [Unicode]
        [StringLength(DataConstraints.MaxCourseNameCharacters)]
        [Required]
        public string Name { get; set; } = null!;

        [Unicode]
        public string Description { get; set; } = null!;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal Price { get; set; }

        public ICollection<StudentCourse> StudentsCourses { get; set; } = null!;
        public ICollection<Resource> Resources { get; set; } = null!;
        public ICollection<Student> Students { get; set; } = null!;
        public ICollection<Homework> Homeworks { get; set; } = null!;
    }
}
