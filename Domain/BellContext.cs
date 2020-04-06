using Microsoft.EntityFrameworkCore;

namespace Domain
{
    public class BellContext : DbContext
    {
        public BellContext() : base()
        {

        }
        public BellContext(DbContextOptions options) : base(options) 
        {
        }

        public virtual DbSet<Week> Weeks { get; set; } 

        public virtual DbSet<Price> Prices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Week>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK_Weeks");

                entity.HasIndex(e => new { e.Year, e.WeekNumber }).IsUnique(true);
            });

            modelBuilder.Entity<Price>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK_Prices");

                entity.HasOne(e => e.Week)
                .WithMany(e => e.Prices)
                .HasForeignKey(e => e.WeekId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Price_Week");

                entity.HasIndex(e => new { e.WeekId, e.IsMorning }).IsUnique(true);
            });
        }
    }
}
