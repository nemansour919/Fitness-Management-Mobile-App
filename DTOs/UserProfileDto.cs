
using System;
using System.ComponentModel.DataAnnotations;

namespace Wger.Api.DTOs
{
    public class UserProfileDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public bool EmailVerified { get; set; }
        public bool IsTemporary { get; set; }
        public bool ShowComments { get; set; }
        public bool ShowEnglishIngredients { get; set; }
        public bool WorkoutReminderActive { get; set; }

        [Range(1, 30)]
        public int WorkoutReminder { get; set; }

        [Range(1, 30)]
        public int WorkoutDuration { get; set; }

        public DateTime? LastWorkoutNotification { get; set; }
        public int NotificationLanguageId { get; set; }
        public decimal? WeightRounding { get; set; }
        public decimal? RepetitionsRounding { get; set; }

        [Range(10, 100)]
        public int? Age { get; set; }

        public DateTime? Birthdate { get; set; }

        [Range(140, 230)]
        public int? Height { get; set; }

        [StringLength(1)]
        public string Gender { get; set; }

        [Range(4, 10)]
        public int? SleepHours { get; set; }

        [Range(1, 15)]
        public int? WorkHours { get; set; }

        [StringLength(1)]
        public string WorkIntensity { get; set; }

        [Range(1, 30)]
        public int? SportHours { get; set; }

        [StringLength(1)]
        public string SportIntensity { get; set; }

        [Range(1, 15)]
        public int? FreetimeHours { get; set; }

        [StringLength(1)]
        public string FreetimeIntensity { get; set; }

        [Range(1500, 5000)]
        public int? Calories { get; set; }

        [StringLength(2)]
        public string WeightUnit { get; set; }

        public bool RoAccess { get; set; }

        [Range(0, 30)]
        public int NumDaysWeightReminder { get; set; }

        public string AddedById { get; set; }
        public bool CanAddUser { get; set; }
    }
}

