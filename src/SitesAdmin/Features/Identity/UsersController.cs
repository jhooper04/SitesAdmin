using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SitesAdmin.Data;
using SitesAdmin.Features.Identity.Dto;
using SitesAdmin.Features.Identity.Data;
using SitesAdmin.Services;
using System.Security.Cryptography;

namespace SitesAdmin.Features.Identity
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationDbContext _context;
        private readonly ITokenService _tokenService;

        public UsersController(UserManager<ApplicationUser> userManager, IApplicationDbContext context,
            ITokenService tokenService, ILogger<UsersController> logger)
        {
            _userManager = userManager;
            _context = context;
            _tokenService = tokenService;
        }

        [HttpGet]
        [Route("session", Name = "[controller]Session")]
        public async Task<ActionResult<CurrentSession?>> Session()
        {
            if (HttpContext.User.Identity == null || HttpContext.User.Identity.Name == null)
            {
                return Ok(null);
            }

            return Ok(new CurrentSession { Username = HttpContext.User.Identity.Name });
        }

        [HttpPost]
        [Route("register", Name = "[controller]Register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userManager.CreateAsync(
                new ApplicationUser { UserName = request.Username, Email = request.Email, Role = Role.User },
                request.Password!
            );

            if (result.Succeeded)
            {
                request.Password = "";
                return CreatedAtAction(nameof(Register), new { email = request.Email, role = Role.User }, request);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return BadRequest(ModelState);
        }


        [HttpPost]
        [Route("login", Name = "[controller]Login")]
        public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var managedUser = await _userManager.FindByEmailAsync(request.Email!);

            if (managedUser == null)
            {
                return BadRequest("Bad credentials");
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, request.Password!);

            if (!isPasswordValid)
            {
                return BadRequest("Bad credentials");
            }

            var userInDb = _context.Users.FirstOrDefault(u => u.Email == request.Email);

            if (userInDb is null)
            {
                return Unauthorized();
            }

            var accessToken = _tokenService.CreateToken(userInDb);
            await _context.SaveChangesAsync();

            return Ok(new AuthResponse
            {
                Username = userInDb.UserName,
                Email = userInDb.Email,
                Token = accessToken,
            });
        }

        //[HttpGet]
        //public ActionResult GenerateApiKey()
        //{
        //    var key = new byte[128];
        //    using (var generator = RandomNumberGenerator.Create())
        //    {
        //        generator.GetBytes(key);
        //    }
        //    return Ok(new { Key = Convert.ToBase64String(key)});
        //}
    }
}
