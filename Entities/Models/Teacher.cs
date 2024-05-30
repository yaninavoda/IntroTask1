using System.ComponentModel.DataAnnotations;

namespace IntroTask.Entities
{
    public class Teacher
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the First Name is 60 characters.")]
        public string Name { get; set; } = string.Empty;
        public ICollection<Course>? Courses { get; set; }
    }
}
