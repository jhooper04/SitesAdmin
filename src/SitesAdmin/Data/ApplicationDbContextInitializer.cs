using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SitesAdmin.Features.Category;
using SitesAdmin.Features.Identity;
using SitesAdmin.Features.Project;
using SitesAdmin.Features.Site;
using SitesAdmin.Features.Tag;

namespace SitesAdmin.Data
{
    public static class InitialiserExtensions
    {
        public static async Task InitialiseDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

            await initialiser.InitialiseAsync();

            await initialiser.SeedAsync();
        }
    }

    public class ApplicationDbContextInitialiser
    {
        private readonly ILogger<ApplicationDbContextInitialiser> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task InitialiseAsync()
        {
            try
            {
                await _context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initialising the database.");
                throw;
            }
        }

        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        private async Task _AddDefaultRole(string role)
        {
            var identityRole = new IdentityRole(role);

            if (_roleManager.Roles.All(r => r.Name != identityRole.Name))
            {
                await _roleManager.CreateAsync(identityRole);
            }
        }

        private async Task _AddDefaultUser(ApplicationUser user, string password)
        {
            if (_userManager.Users.All(u => u.UserName != user.UserName))
            {
                await _userManager.CreateAsync(user, password);
                await _userManager.AddToRolesAsync(user, [user.Role]);
            }
        }

        public async Task TrySeedAsync()
        {
            // Add Default roles
            foreach (var roleProp in typeof(Role).GetFields())
            {
                await _AddDefaultRole(roleProp.Name);
            }

            // Add Default users

            List<(ApplicationUser User, string Password)> defaultUsers = new() {
                (new ApplicationUser { UserName = "contact@jakehooper.pro", Email = "contact@jakehooper.pro", Role=Role.Administrator  }, "Admin1!"),
                (new ApplicationUser { UserName = "jakeh04@gmail.com", Email = "jakeh04@gmail.com", Role=Role.User }, "Jake1!"),
            };

            foreach (var user in defaultUsers)
            {
                await _AddDefaultUser(user.User, user.Password);
            }

            await _context.SaveChangesAsync();

            Site? site = null;

            if (!_context.Sites.Any())
            {
                site = new Site 
                { 
                    Name = "Jake Hooper's Portfolio", 
                    Description = "My portfolio site showcasing my projects, a technical blog, and my experience.", 
                    BaseUrl = "https://jakehooper.pro/" 
                };

                _context.Sites.Add(site);

                await _context.SaveChangesAsync();

            }
            //else if (_context.Sites.Count() == 1)
            //{
            //    site = _context.Sites.FirstOrDefault();
            //}

            if (!_context.Projects.Any() && site != null)
            {
                var project = new Project
                {
                    Name = "Time Trace",
                    Description = "Time and bug tracking applicaiton",
                    Body = "Time and issue tracking application for contractors that allows recording of time per project issue to facilitate generation of invoices and project management",

                    Demo = "https://demo.jakehooper.pro/timetrace",
                    Github = "https://github.com/jhooper04/TimeTrace",
                    SiteId = site.Id,
                };

                _context.Projects.Add(project);

                await _context.SaveChangesAsync();
            }

            if (!_context.Categories.Any() && site != null)
            {
                var category = new Category
                {
                    Name = "Devlog",
                    Description = "",
                    SiteId = site.Id,
                };

                var category2 = new Category
                {
                    Name = "Guides",
                    Description = "",
                    SiteId = site.Id,
                };

                var category3 = new Category
                {
                    Name = "News",
                    Description = "",
                    SiteId = site.Id,
                };

                _context.Categories.Add(category);
                _context.Categories.Add(category2);
                _context.Categories.Add(category3);

                await _context.SaveChangesAsync();
            }

            if (!_context.Tags.Any() && site != null)
            {
                var tag = new Tag
                {
                    Name = "Asp.Net",
                    Description = "",
                    SiteId = site.Id,
                };

                var tag2 = new Tag
                {
                    Name = "Typescript",
                    Description = "",
                    SiteId = site.Id,
                };

                var tag3 = new Tag
                {
                    Name = "React",
                    Description = "",
                    SiteId = site.Id,
                };

                _context.Tags.Add(tag);
                _context.Tags.Add(tag2);
                _context.Tags.Add(tag3);

                await _context.SaveChangesAsync();
            }
        }
    }
}

