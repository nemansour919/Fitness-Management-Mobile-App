
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Wger.Api.Models
{
    public class UserProfile
    {
        [Key]
        public int Id { get; set; }

        // Django's OneToOneField to User
        public string UserId { get; set; } // Assuming IdentityUser.Id is string
        public IdentityUser User { get; set; }

        // ForeignKey to Gym (assuming Gym model will be created)
        public int? GymId { get; set; }
        public Gym Gym { get; set; }

        public bool EmailVerified { get; set; } = false;
        public bool IsTemporary { get; set; } = false;

        // User preferences
        public bool ShowComments { get; set; } = true;
        public bool ShowEnglishIngredients { get; set; } = true;
        public bool WorkoutReminderActive { get; set; } = false;

        [Range(1, 30)]
        public int WorkoutReminder { get; set; } = 14;

        [Range(1, 30)]
        public int WorkoutDuration { get; set; } = 12;

        public DateTime? LastWorkoutNotification { get; set; }

        // ForeignKey to Language (assuming Language model will be created)
        public int NotificationLanguageId { get; set; } = 2; // Default to 2, as in Django
        public Language NotificationLanguage { get; set; }

        [Column(TypeName = "decimal(4, 2)")]
        public decimal? WeightRounding { get; set; }

        [Column(TypeName = "decimal(4, 2)")]
        public decimal? RepetitionsRounding { get; set; }

        // User statistics
        [Range(10, 100)]
        public int? Age { get; set; }

        public DateTime? Birthdate { get; set; }

        [Range(140, 230)]
        public int? Height { get; set; }

        [StringLength(1)]
        public string Gender { get; set; } = "1"; // Default to '1' (Male) as in Django

        [Range(4, 10)]
        public int? SleepHours { get; set; } = 7;

        [Range(1, 15)]
        public int? WorkHours { get; set; } = 8;

        [StringLength(1)]
        public string WorkIntensity { get; set; } = "1"; // Default to '1' (Low) as in Django

        [Range(1, 30)]
        public int? SportHours { get; set; } = 3;

        [StringLength(1)]
        public string SportIntensity { get; set; } = "2"; // Default to '2' (Medium) as in Django

        [Range(1, 15)]
        public int? FreetimeHours { get; set; } = 8;

        [StringLength(1)]
        public string FreetimeIntensity { get; set; } = "1"; // Default to '1' (Low) as in Django

        [Range(1500, 5000)]
        public int? Calories { get; set; } = 2500;

        [StringLength(2)]
        public string WeightUnit { get; set; } = "kg"; // Default to 'kg' as in Django

        public bool RoAccess { get; set; } = false;

        [Range(0, 30)]
        public int NumDaysWeightReminder { get; set; } = 0;

        // API
         public string? AddedById { get; set; }
        public virtual IdentityUser? AddedBy { get; set; }

        public bool CanAddUser { get; set; } = false;

        // Methods and properties like is_trustworthy, weight, address, clean, calculate_bmi, calculate_basal_metabolic_rate, calculate_activities
        // will need to be re-implemented as C# methods or properties, potentially using services for database interactions.
    }

    // Placeholder for Gym model
    public class Gym
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        // Add other properties as needed from Django's Gym model
    }
}

