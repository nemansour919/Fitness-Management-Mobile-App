
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wger.Api.Models
{
    public class GymConfig
    {
        [Key]
        public int Id { get; set; }

        // ForeignKey to Gym
        public int? DefaultGymId { get; set; }
        public Gym DefaultGym { get; set; }

        // The custom save logic from Django will need to be implemented in a service layer
        // or a repository method in ASP.NET Core, not directly in the model.
    }
}

