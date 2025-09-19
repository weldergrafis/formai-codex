using System.ComponentModel.DataAnnotations;

namespace FormAI.Api.Models
{
    public class Gallery : ModelBase
    {
        [Required]
        public required string Name { get; set; }

        public IList<Photo> Photo { get; set; }
    }

}
