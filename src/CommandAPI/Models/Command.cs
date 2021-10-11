using System.ComponentModel.DataAnnotations;
namespace CommandAPI.Models
{
    public class Command {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string HowTo { get; set; }
        public string Platform { get; set; }
        public string CommandLine { get; set; }
    
    }
}