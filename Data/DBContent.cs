using Microsoft.EntityFrameworkCore;
using First_web_project.Models;

namespace First_web_project.Data
{
    public class DBContent : DbContext
    {
        public DBContent(DbContextOptions<DBContent> options) : base(options) { }

        public DbSet<Class> Jobs => Set<Class>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Class>().HasData(
                new Class { jobId = 1, jobName = ".Net dev", jobDescription = "Web" },
                new Class { jobId = 2, jobName = "React", jobDescription = "full stack" },
                new Class { jobId = 3, jobName = "C++", jobDescription = "back" },
                new Class { jobId = 4, jobName = "C++", jobDescription = "game dev" }
            );
        }
    }
}
