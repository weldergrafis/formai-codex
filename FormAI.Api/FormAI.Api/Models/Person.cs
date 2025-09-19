using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FormAI.Api.Models
{
    public class Person : ModelBase
    {
        public string Name { get; set; }
    }
}
