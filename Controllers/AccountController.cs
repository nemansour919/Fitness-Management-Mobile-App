
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Wger.Api.DTOs;
using System.Threading.Tasks;
using System.Linq;
using Wger.Api.Data;
using Wger.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System;

namespace Wger.Api.Controllers
{
    [ApiController]
    [Route("api/v2/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(model.Username);
            }

            if (user == null)
            {
                return Unauthorized(new { message = "Username or password unknown" });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var authClaims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id)
                };

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }

            return Unauthorized(new { message = "Username or password unknown" });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status409Conflict, new { message = "User with this email already exists!" });
            }

            IdentityUser user = new() { Email = model.Email, SecurityStamp = Guid.NewGuid().ToString(), UserName = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "User creation failed! Please check user details and try again.", errors = result.Errors.Select(e => e.Description) });
            }

            // Create a UserProfile for the new user
            var userProfile = new UserProfile
            {
                UserId = user.Id,
                EmailVerified = false, // Default to false, will be verified later
                IsTemporary = false,
                ShowComments = true,
                ShowEnglishIngredients = true,
                WorkoutReminderActive = false,
                WorkoutReminder = 0,
                WorkoutDuration = 0,
                NotificationLanguageId = (await _context.Languages.FirstOrDefaultAsync())?.Id ?? 0, // Use first available language or 0 if none
                GymId = (await _context.Gyms.FirstOrDefaultAsync())?.Id, // Use first available gym or null if none
                WeightUnit = "kg", // Default weight unit
                Gender = "U", // Unknown
                WorkIntensity = "U", // Unknown
                SportIntensity = "U", // Unknown
                FreetimeIntensity = "U", // Unknown
                RoAccess = false,
                NumDaysWeightReminder = 0,
                AddedById = user.Id, // User adds themselves
                CanAddUser = false
            };

            _context.UserProfiles.Add(userProfile);
            await _context.SaveChangesAsync();

            // TODO: Implement email verification sending logic

            return StatusCode(StatusCodes.Status201Created, new { message = "User registered successfully!" });
        }
    }
}

