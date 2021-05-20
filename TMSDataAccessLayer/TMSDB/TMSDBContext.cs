using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TMSDataAccessLayer.TMSDB
{
    public partial class TMSDBContext : DbContext
    {
        public TMSDBContext()
        {
        }

        public TMSDBContext(DbContextOptions<TMSDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Branch> Branch { get; set; }
        public virtual DbSet<Credentials> Credentials { get; set; }
        public virtual DbSet<Crimes> Crimes { get; set; }
        public virtual DbSet<Districts> Districts { get; set; }
        public virtual DbSet<DriverLicense> DriverLicense { get; set; }
        public virtual DbSet<PersonalDetails> PersonalDetails { get; set; }
        public virtual DbSet<PersonInformation> PersonInformation { get; set; }
        public virtual DbSet<Positions> Positions { get; set; }
        public virtual DbSet<RecoveryData> RecoveryData { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<TrafficMonitorTransactions> TrafficMonitorTransactions { get; set; }
        public virtual DbSet<VehicleInsurance> VehicleInsurance { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.;Database=TMSDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Branch>(entity =>
            {
                entity.Property(e => e.BranchId).HasColumnName("BranchID");

                entity.Property(e => e.BranchName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.DistrictId).HasColumnName("DistrictID");

                entity.HasOne(d => d.District)
                    .WithMany(p => p.Branch)
                    .HasForeignKey(d => d.DistrictId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Branch_Districts1");
            });

            modelBuilder.Entity<Credentials>(entity =>
            {
                entity.HasKey(e => e.CredentialId);

                entity.Property(e => e.CredentialId).HasColumnName("CredentialID");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PersonalDetailsId).HasColumnName("PersonalDetailsID");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.PersonalDetails)
                    .WithMany(p => p.Credentials)
                    .HasForeignKey(d => d.PersonalDetailsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Credentials_PersonalDetails");
            });

            modelBuilder.Entity<Crimes>(entity =>
            {
                entity.HasKey(e => e.CrimeChargeId);

                entity.Property(e => e.Charge).HasColumnType("decimal(22, 2)");

                entity.Property(e => e.CrimeName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Description).HasMaxLength(300);
            });

            modelBuilder.Entity<Districts>(entity =>
            {
                entity.HasKey(e => e.DistrictId);

                entity.Property(e => e.DistrictId).HasColumnName("DistrictID");

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.DistrictName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<DriverLicense>(entity =>
            {
                entity.HasKey(e => e.LicenseNumber);

                entity.HasIndex(e => e.PersonId)
                    .HasName("U_PersonID")
                    .IsUnique();

                entity.Property(e => e.LicenseNumber)
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.CountryIssued)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.DateIssued).HasColumnType("date");

                entity.Property(e => e.DriverRestriction)
                    .IsRequired()
                    .HasMaxLength(3);

                entity.Property(e => e.ExpiryDate).HasColumnType("date");

                entity.Property(e => e.FirstIssue).HasColumnType("date");

                entity.Property(e => e.LicenseCode)
                    .IsRequired()
                    .HasMaxLength(4);

                entity.Property(e => e.PersonId).HasColumnName("PersonID");

                entity.Property(e => e.Trn)
                    .IsRequired()
                    .HasColumnName("TRN")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Person)
                    .WithOne(p => p.DriverLicense)
                    .HasForeignKey<DriverLicense>(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DriverLicense_PersonDetails");
            });

            modelBuilder.Entity<PersonalDetails>(entity =>
            {
                entity.Property(e => e.PersonalDetailsId).HasColumnName("PersonalDetailsID");

                entity.Property(e => e.BranchId).HasColumnName("BranchID");

                entity.Property(e => e.DateOfBirth).HasColumnType("datetime");

                entity.Property(e => e.EmailAddress)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PositionId).HasColumnName("PositionID");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.HasOne(d => d.Position)
                    .WithMany(p => p.PersonalDetails)
                    .HasForeignKey(d => d.PositionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PersonalDetails_PersonalDetails");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.PersonalDetails)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PersonalDetails_Roles");
            });

            modelBuilder.Entity<PersonInformation>(entity =>
            {
                entity.HasKey(e => e.PersonId);

                entity.Property(e => e.PersonId).HasColumnName("PersonID");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.DateCreated).HasColumnType("date");

                entity.Property(e => e.DateModified).HasColumnType("date");

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.EmailAddress)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Firstname)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Lastname)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Middlename)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(25);
            });

            modelBuilder.Entity<Positions>(entity =>
            {
                entity.HasKey(e => e.PositionId);

                entity.Property(e => e.PositionId).HasColumnName("PositionID");

                entity.Property(e => e.Abbreviation)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.PositionName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<RecoveryData>(entity =>
            {
                entity.Property(e => e.RecoveryDataId).HasColumnName("RecoveryDataID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(30);

                entity.Property(e => e.Otp)
                    .HasColumnName("OTP")
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.HasKey(e => e.RoleId);

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.RoleName).HasMaxLength(50);
            });

            modelBuilder.Entity<TrafficMonitorTransactions>(entity =>
            {
                entity.HasKey(e => e.TransactionId);

                entity.Property(e => e.TransactionId).HasColumnName("TransactionID");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CrimeCharge).HasColumnType("decimal(22, 2)");

                entity.Property(e => e.CrimeName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.LicenseNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.VehicleRegistrationNumber)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<VehicleInsurance>(entity =>
            {
                entity.HasKey(e => e.VehicleId);

                entity.Property(e => e.DateEffective).HasColumnType("date");

                entity.Property(e => e.ExpiryDate).HasColumnType("date");

                entity.Property(e => e.IssuedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.RegistrationNumber)
                    .IsRequired()
                    .HasMaxLength(20);
            });
        }
    }
}
