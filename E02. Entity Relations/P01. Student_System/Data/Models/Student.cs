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
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        [Unicode]
        [MaxLength(DataConstraints.MaxStudentNameCharacters)]
        public string Name { get; set; } = null!;

        [MinLength(10)]
        [StringLength(10)]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        public DateTime RegisteredOn { get; set; }

        public DateTime? Birthday { get; set; }

        public ICollection<StudentCourse> StudentsCourses { get; set; } = null!;
        public ICollection<Course> Courses { get; set; } = null!;
        public ICollection<Homework> Homeworks { get; set; } = null!;
    }
}
