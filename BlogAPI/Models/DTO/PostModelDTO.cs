using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models.DTO
{
    public class PostModelDTO
    {
        
        public int Id { get; set; }
        public string UserName { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        [MaxLength(5000)]
        public string Post { get; set; }
        public string? ImageUrl { get; set; }
        public string Tag { get; set; }

    }
}
