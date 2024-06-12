using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntroTask.Entities
{
    public class Teacher
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the {0} is 60 characters.")]
        public string Name { get; set; } = string.Empty;
        public ICollection<Course>? Courses { get; set; } =new List<Course>();

        public override bool Equals(object? obj)
        {
            return obj is Teacher other && Id == other.Id && Name == other.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name);
        }
    }
}
