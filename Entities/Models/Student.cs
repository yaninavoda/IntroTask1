﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntroTask.Entities
{
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} is a required field.")]
        [DisplayName("First Name")]
        [MaxLength(60, ErrorMessage = "Maximum length for the {0} is 60 characters.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "{0} is a required field.")]
        [DisplayName("Last Name")]
        [MaxLength(60, ErrorMessage = "Maximum length for the {0} is 60 characters.")]
        public string LastName { get; set; } = string.Empty;
        public ICollection<Course> Courses { get; set; } = new List<Course>();

        public override bool Equals(object? obj)
        {
            return obj is Student other
                && Id == other.Id
                && FirstName == other.FirstName
                && LastName == other.LastName;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, FirstName, LastName);
        }
    }
}
