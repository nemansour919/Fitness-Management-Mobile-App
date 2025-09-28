
using System.ComponentModel.DataAnnotations;

namespace Wger.Api.Models
{
    public class Language
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(2)]
        public string ShortName { get; set; }

        [Required]
        [StringLength(30)]
        public string FullName { get; set; }

        [Required]
        [StringLength(30)]
        public string FullNameEn { get; set; }
    }
}

