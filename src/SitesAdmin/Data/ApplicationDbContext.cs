using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SitesAdmin.Features.Sites.Data;
using SitesAdmin.Features.Categories.Data;
using SitesAdmin.Features.Identity.Data;
using SitesAdmin.Features.Messages.Data;
using SitesAdmin.Features.Posts.Data;
using SitesAdmin.Features.Tags.Data;
using SitesAdmin.Features.Projects.Data;
using SitesAdmin.Features.Assets.Data;
using System.Reflection;

namespace SitesAdmin.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        public DbSet<Site> Sites => Set<Site>();
        public DbSet<Post> Posts => Set<Post>();
        public DbSet<Message> Messages => Set<Message>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<Folder> Folders => Set<Folder>();
        public DbSet<Asset> Assets => Set<Asset>();

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }

    //public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    //{

    //    ApplicationDbContext IDesignTimeDbContextFactory<ApplicationDbContext>.CreateDbContext(string[] args)
    //    {
    //        DbContextOptionsBuilder<ApplicationDbContext> builder = new DbContextOptionsBuilder<ApplicationDbContext>();
    //        IConfiguration config = new ConfigurationBuilder()
    //        .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))

    //        .AddJsonFile("appsettings.json")
    //        .Build();

    //        var connectionString = config.GetConnectionString("DefaultConnection");

    //        Guard.Against.Null(connectionString, message: "Connection string 'DefaultConnection' not found.");

    //        string[] envDbKeys = new string[] { "DB_SERVER", "DB_SCHEMA", "DB_USER", "DB_PASS" };

    //        foreach (var envDbKey in envDbKeys)
    //        {
    //            connectionString = connectionString.Replace($"{{{envDbKey}}}", config.GetValue<string>(envDbKey)); //?? throw new InvalidOperationException("Missing connection string configuration"));
    //        }

    //        builder.UseMySQL(connectionString);


    //        return new ApplicationDbContext(builder.Options);
    //    }
    //}
}
