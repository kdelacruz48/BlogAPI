using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace BlogAPI.Models
{
    public class PostModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Title { get; set; }  
        public string Post { get; set; }
        public string? ImageUrl { get; set; }
        public string? Tag { get; set; }
        public DateTimeOffset Created_date { get; set; }
        public DateTimeOffset Updated_date { get; set; }
    }
}
