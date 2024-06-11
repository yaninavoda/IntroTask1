using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntroTask.Entities
{
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the {0} is 60 characters.")]
        public string Title { get; set; } = string.Empty;

        [ForeignKey(nameof(Teacher))]
        public int? TeacherId { get; set; }
        public Teacher? Teacher { get; set; }
        public ICollection<Student> Students { get; set; } = new List<Student>();

        public override bool Equals(object? obj)
        {
            return obj is Course other && Id == other.Id && Title == other.Title;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Title);
        }
    }
}
