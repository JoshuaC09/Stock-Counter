using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication1.DatabaseContext
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        public DbSet<ItemCount> ItemCounts { get; set; }
        public DbSet<CountSheet> CountSheets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CountSheet>(entity =>
            {
                entity.ToTable("count_sheet");
                entity.HasKey(e => e.CountCode);
                entity.Property(e => e.CountCode)
                    .HasColumnName("cnt_code")
                    .HasMaxLength(50)
                    .IsRequired();
                entity.Property(e => e.CountSheetEmployee)
                    .HasColumnName("cnt_emp")
                    .HasMaxLength(10)
                    .IsRequired();
                entity.Property(e => e.CountDescription)
                    .HasColumnName("cnt_desc")
                    .HasMaxLength(200)
                    .IsRequired();
                entity.Property(e => e.CountDate)
                    .HasColumnName("cnt_date")
                    .IsRequired();
                entity.Property(e => e.CountStatus)
                    .HasColumnName("cnt_status")
                    .IsRequired()
                    .HasDefaultValue(0);
            });

            modelBuilder.Entity<ItemCount>(entity =>
            {
                entity.ToTable("item_count");
                entity.HasKey(e => e.ItemCounter);
                entity.Property(e => e.ItemKey)
                    .HasColumnName("itm_key")
                    .HasMaxLength(45)
                    .IsRequired();
                entity.Property(e => e.ItemCounter)
                    .HasColumnName("itm_ctr")
                    .IsRequired();
                entity.Property(e => e.ItemCountCode)
                    .HasColumnName("itm_cntcode")
                    .HasMaxLength(50)
                    .IsRequired();
                entity.Property(e => e.ItemCode)
                    .HasColumnName("itm_code")
                    .HasMaxLength(50)
                    .IsRequired();
                entity.Property(e => e.ItemDescription)
                    .HasColumnName("itm_description")
                    .HasMaxLength(100)
                    .IsRequired();
                entity.Property(e => e.ItemUom)
                    .HasColumnName("itm_uom")
                    .HasMaxLength(20)
                    .IsRequired();
                entity.Property(e => e.ItemBatchLotNumber)
                    .HasColumnName("itm_batchlot")
                    .HasMaxLength(50)
                    .IsRequired();
                entity.Property(e => e.ItemExpiry)
                    .HasColumnName("itm_expiry")
                    .HasMaxLength(20)
                    .IsRequired();
                entity.Property(e => e.ItemQuantity)
                    .HasColumnName("itm_quantity")
                    .IsRequired();
                entity.Property(e => e.ItemDateLog)
                    .HasColumnName("itm_date_log")
                    .IsRequired();
            });
        }
    }
}
