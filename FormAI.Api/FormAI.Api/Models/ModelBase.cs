using System.ComponentModel.DataAnnotations;

namespace FormAI.Api.Models;

public abstract class ModelBase
{
    [Key]
    public long Id { get; set; }
}
