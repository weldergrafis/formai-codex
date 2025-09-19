using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FormAI.Api.Models
{
    [Index(nameof(Face1Id), nameof(Face2Id), IsUnique = true)]
    public class FaceComparison : ModelBase
    {
        [Required]
        [InverseProperty(nameof(Face.FaceComparisonsAsFace1))]
        public Face Face1 { get; set; }

        [Required]
        [ForeignKey(nameof(Face))]
        public long Face1Id { get; set; }


        [Required]
        [InverseProperty(nameof(Face.FaceComparisonsAsFace2))]
        public Face Face2 { get; set; }

        [Required]
        [ForeignKey(nameof(Face))]
        public long Face2Id { get; set; }

        [Required]
        public double Score { get; set; }

        public bool? IsSamePerson { get; set; }
    }
}
