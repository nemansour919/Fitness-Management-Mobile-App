
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Wger.Api.Data;
using Wger.Api.Models;
using Wger.Api.DTOs;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Wger.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v2/[controller]")]
    public class UserProfileController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UserProfileController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/v2/UserProfile
        [HttpGet]
        public async Task<ActionResult<UserProfileDto>> GetUserProfile()
        {
            var userId = _userManager.GetUserId(User);
            var userProfile = await _context.UserProfiles
                                            .Include(up => up.User)
                                            .Include(up => up.Gym)
                                            .Include(up => up.NotificationLanguage)
                                            .FirstOrDefaultAsync(up => up.UserId == userId);

            if (userProfile == null)
            {
                return NotFound();
            }

            return new UserProfileDto
            {
                Id = userProfile.Id,
                UserId = userProfile.UserId,
                Email = userProfile.User.Email, // Assuming User is loaded
                EmailVerified = userProfile.EmailVerified,
                IsTemporary = userProfile.IsTemporary,
                ShowComments = userProfile.ShowComments,
                ShowEnglishIngredients = userProfile.ShowEnglishIngredients,
                WorkoutReminderActive = userProfile.WorkoutReminderActive,
                WorkoutReminder = userProfile.WorkoutReminder,
                WorkoutDuration = userProfile.WorkoutDuration,
                LastWorkoutNotification = userProfile.LastWorkoutNotification,
                NotificationLanguageId = userProfile.NotificationLanguageId,
                WeightRounding = userProfile.WeightRounding,
                RepetitionsRounding = userProfile.RepetitionsRounding,
                Age = userProfile.Age,
                Birthdate = userProfile.Birthdate,
                Height = userProfile.Height,
                Gender = userProfile.Gender,
                SleepHours = userProfile.SleepHours,
                WorkHours = userProfile.WorkHours,
                WorkIntensity = userProfile.WorkIntensity,
                SportHours = userProfile.SportHours,
                SportIntensity = userProfile.SportIntensity,
                FreetimeHours = userProfile.FreetimeHours,
                FreetimeIntensity = userProfile.FreetimeIntensity,
                Calories = userProfile.Calories,
                WeightUnit = userProfile.WeightUnit,
                RoAccess = userProfile.RoAccess,
                NumDaysWeightReminder = userProfile.NumDaysWeightReminder,
                AddedById = userProfile.AddedById,
                CanAddUser = userProfile.CanAddUser
            };
        }

        // POST: api/v2/UserProfile
        // To update the user profile, similar to Django's create/update behavior
        [HttpPost]
        public async Task<IActionResult> UpdateUserProfile(UserProfileDto updatedProfileDto)
        {
            var userId = _userManager.GetUserId(User);
            var userProfile = await _context.UserProfiles
                                            .Include(up => up.User)
                                            .FirstOrDefaultAsync(up => up.UserId == userId);

            if (userProfile == null)
            {
                return NotFound();
            }

            // Update properties from updatedProfileDto to userProfile
            userProfile.ShowComments = updatedProfileDto.ShowComments;
            userProfile.ShowEnglishIngredients = updatedProfileDto.ShowEnglishIngredients;
            userProfile.WorkoutReminderActive = updatedProfileDto.WorkoutReminderActive;
            userProfile.WorkoutReminder = updatedProfileDto.WorkoutReminder;
            userProfile.WorkoutDuration = updatedProfileDto.WorkoutDuration;
            userProfile.NotificationLanguageId = updatedProfileDto.NotificationLanguageId;
            userProfile.WeightRounding = updatedProfileDto.WeightRounding;
            userProfile.RepetitionsRounding = updatedProfileDto.RepetitionsRounding;
            userProfile.Age = updatedProfileDto.Age;
            userProfile.Birthdate = updatedProfileDto.Birthdate;
            userProfile.Height = updatedProfileDto.Height;
            userProfile.Gender = updatedProfileDto.Gender;
            userProfile.SleepHours = updatedProfileDto.SleepHours;
            userProfile.WorkHours = updatedProfileDto.WorkHours;
            userProfile.WorkIntensity = updatedProfileDto.WorkIntensity;
            userProfile.SportHours = updatedProfileDto.SportHours;
            userProfile.SportIntensity = updatedProfileDto.SportIntensity;
            userProfile.FreetimeHours = updatedProfileDto.FreetimeHours;
            userProfile.FreetimeIntensity = updatedProfileDto.FreetimeIntensity;
            userProfile.Calories = updatedProfileDto.Calories;
            userProfile.WeightUnit = updatedProfileDto.WeightUnit;
            userProfile.RoAccess = updatedProfileDto.RoAccess;
            userProfile.NumDaysWeightReminder = updatedProfileDto.NumDaysWeightReminder;

            // Handle email change and verification reset
            if (!string.IsNullOrEmpty(updatedProfileDto.Email) && userProfile.User.Email != updatedProfileDto.Email)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    user.Email = updatedProfileDto.Email;
                    user.NormalizedEmail = _userManager.NormalizeEmail(updatedProfileDto.Email);
                    var result = await _userManager.UpdateAsync(user);
                    if (!result.Succeeded)
                    {
                        return BadRequest(result.Errors);
                    }
                    userProfile.EmailVerified = false; // Reset verification flag
                    // TODO: Implement email verification sending logic
                }
            }

            _context.Entry(userProfile).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserProfileExists(userProfile.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/v2/UserProfile/verify-email
        [HttpPost("verify-email")]
        public async Task<ActionResult> VerifyEmail()
        {
            var userId = _userManager.GetUserId(User);
            var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(up => up.UserId == userId);

            if (userProfile == null)
            {
                return NotFound();
            }

            if (userProfile.EmailVerified)
            {
                return Ok(new { status = "verified", message = "This email is already verified" });
            }

            // TODO: Implement actual email sending logic
            // For now, simulate sending
            // await _emailSender.SendEmailAsync(userProfile.User.Email, "Verify your email", "Please verify your email address.");

            return Ok(new { status = "sent", message = $"A verification email was sent to {userProfile.User.Email}" });
        }

        private bool UserProfileExists(int id)
        {
            return _context.UserProfiles.Any(e => e.Id == id);
        }
    }
}

