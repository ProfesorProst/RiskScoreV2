using DependencyCheck.Models;
using System.Data.Entity;

namespace DependencyCheck.Data
{
    class Context : DbContext
    {
        public Context()
            : base("DbConnection")
        { }

        public DbSet<DependencyVulnerabilityDB> dependencyVulnerabilityDBs { get; set; }
        public DbSet<DependencyDB> dependencyDBs { get; set; }
        public DbSet<VulnerabilityDB> vulnerabilityDBs { get; set; }
        public DbSet<UserDB> users { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
