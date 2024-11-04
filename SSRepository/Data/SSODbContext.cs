using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SSRepository.Data
{
    public partial class SSODbContext : DbContext
    {
        public SSODbContext()
        {
        }

        public SSODbContext(DbContextOptions<SSODbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblUserMas> TblUserMas { get; set; } = null!;
      

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=154.61.77.18;database=ssodb;uid=ssodbuser;pwd=Su#2fg!3;TrustServerCertificate=True");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            base.OnModelCreating(modelBuilder);           
        }
    }
}
