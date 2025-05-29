using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YaushServer.Url
{
    [Table("urls")]
    [Index(nameof(Hash), IsUnique = true, Name = "Index_Hash")]
    public class ShortenedUrl
    {
        public int ID { get; set; }
        [Required]
        public required string Url { get; set; }
        [Required]
        [MaxLength(30)]
        public string? Hash { get; set; }
        public DateTime Created { get; set; }
    }
}
