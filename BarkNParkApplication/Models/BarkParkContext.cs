using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BarkNParkApplication.Models
{
    public partial class BarkParkContext : DbContext
    {
        public BarkParkContext()
        {

        }
        public BarkParkContext(DbContextOptions<BarkParkContext> options): base(options) {

        }

        public virtual DbSet<Appointments> Appointments { get; set; }
        public virtual DbSet<Stations> Stations { get; set; }
        public virtual DbSet<Transactions> Transactions { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(@"Data Source=barkparkdb.cffo0eijbrmb.us-east-1.rds.amazonaws.com;Initial Catalog=system;Integrated Security=False;User ID=admin;Password=sse-691-18");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointments>(entity =>
            {
                entity.HasKey(e => e.ApptId);

                entity.ToTable("appointments");

                entity.Property(e => e.ApptId)
                    .HasColumnName("appt_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.ApptCheckin)
                    .HasColumnName("appt_checkin")
                    .HasColumnType("datetime");

                entity.Property(e => e.ApptCheckout)
                    .HasColumnName("appt_checkout")
                    .HasColumnType("datetime");

                entity.Property(e => e.ApptStation).HasColumnName("appt_station");

                entity.Property(e => e.ApptUser).HasColumnName("appt_user");
            });

            modelBuilder.Entity<Stations>(entity =>
            {
                entity.HasKey(e => e.StationId);

                entity.ToTable("stations");

                entity.Property(e => e.StationId)
                    .HasColumnName("station_id")
                    .ValueGeneratedNever();
            });

            modelBuilder.Entity<Transactions>(entity =>
            {
                entity.HasKey(e => e.TransId);

                entity.ToTable("transactions");

                entity.Property(e => e.TransId)
                    .HasColumnName("trans_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.TransReceipt)
                    .IsRequired()
                    .HasColumnName("trans_receipt")
                    .HasColumnType("text");

                entity.Property(e => e.TransSubtotal)
                    .HasColumnName("trans_subtotal")
                    .HasColumnType("money");

                entity.Property(e => e.TransTax)
                    .IsRequired()
                    .HasColumnName("trans_tax")
                    .HasColumnType("char(1)");

                entity.Property(e => e.TransTotal)
                    .HasColumnName("trans_total")
                    .HasColumnType("money");

                entity.Property(e => e.TransType)
                    .IsRequired()
                    .HasColumnName("trans_type")
                    .HasColumnType("char(1)");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("users");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.UserFirstname)
                    .IsRequired()
                    .HasColumnName("user_firstname")
                    .HasColumnType("nchar(10)");

                entity.Property(e => e.UserLastname)
                    .IsRequired()
                    .HasColumnName("user_lastname")
                    .HasColumnType("nchar(10)");

                entity.Property(e => e.UserPaypal)
                    .IsRequired()
                    .HasColumnName("user_paypal")
                    .HasColumnType("nchar(100)");
            });
        }
    }
}
