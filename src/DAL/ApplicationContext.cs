using BLL;
using BLL.Entities;
using DAL.Configurations;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class ApplicationContext : DbContext
    {
        private const string HerokuConnectionString =
            "Host=ec2-54-216-185-51.eu-west-1.compute.amazonaws.com;Database=dfr68d3vuhel57;Username=plcarodozmppsj;Password=a19cfcb97a057e9b1228be58c7cee68c07693692ea608320de3b53edde63685d;sslmode=Require;TrustServerCertificate=true";
        
        private const string LocalConnectionString =
            "Host=localhost;Database=test_task_db;Username=postgres;Password=qweasd123";
        
        public DbSet<Project> Projects { get; private set; }
        public DbSet<ProjectTask> Tasks { get; private set; }
        
        public ApplicationContext() { }
        
        public ApplicationContext(DbContextOptions options) : base(options) { }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(HerokuConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);
            builder.HasAnnotation("Relational:Collation", "Russian_Russia.1251");

            builder.ApplyConfiguration(new ProjectConfiguration());
            builder.ApplyConfiguration(new ProjectTaskConfiguration());
        }
    }
}