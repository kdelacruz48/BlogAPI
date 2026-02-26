using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models.DTO
{
    public class PostUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
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
