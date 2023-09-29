using Microsoft.EntityFrameworkCore;
using PolicyCoverageApi.models;

namespace PolicyCoverageApi.data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }

        public DbSet<PortalUser> portalUsers { get; set; }
        public DbSet<UserPolicyList> userPolicyLists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserPolicyList>()
                .HasOne(upl => upl.User)
                .WithMany(user => user.Policies)
                .HasForeignKey(upl => upl.UserId);
        }

    }
}
