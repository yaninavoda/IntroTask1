using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntroTask.Entities
{
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "First Name is a required field.")]
        [DisplayName("First Name")]
        [MaxLength(60, ErrorMessage = "Maximum length for the First Name is 60 characters.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last Name is a required field.")]
        [DisplayName("Last Name")]
        [MaxLength(60, ErrorMessage = "Maximum length for the Last Name is 60 characters.")]
        public string LastName { get; set; } = string.Empty;
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
