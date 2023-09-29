using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PolicyCoverageApi.Models;

namespace PolicyCoverageApi.data;

public partial class PolicyDbContext : DbContext
{
    public PolicyDbContext()
    {
    }

    public PolicyDbContext(DbContextOptions<PolicyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Coverage> Coverages { get; set; }

    public virtual DbSet<Policy> Policies { get; set; }

    public virtual DbSet<Policycoverage> Policycoverages { get; set; }

    public virtual DbSet<Vehicle> Vehicles { get; set; }

    public virtual DbSet<Policyvehicle> Policyvehicles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("server=VM-104; database=autopastraining;User Id=apastraining;Password=apastraining123.;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Coverage>(entity =>
        {
            entity.HasKey(e => e.CoverageId).HasName("PRIMARY");

            entity.ToTable("coverages");

            entity.HasIndex(e => e.CoverageId, "id_UNIQUE").IsUnique();

            entity.HasIndex(e => e.SortOrder, "sortorder_UNIQUE").IsUnique();

            entity.Property(e => e.CovCd)
                .HasMaxLength(45)
                .IsFixedLength();
            entity.Property(e => e.CoverageName)
                .HasMaxLength(45)
                .IsFixedLength();
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.EffectiveDt).HasColumnType("date");
            entity.Property(e => e.ExpirationDt).HasColumnType("date");
            entity.Property(e => e.SortOrder).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<Policy>(entity =>
        {
            entity.HasKey(e => e.PolicyId).HasName("PRIMARY");

            entity.ToTable("policy");

            entity.HasIndex(e => e.AppUserId, "Appuser2id_idx");

            entity.HasIndex(e => e.PolicyNumber, "PolicyNumber_UNIQUE").IsUnique();

            entity.HasIndex(e => e.QuoteNumber, "QuoteNumber_UNIQUE").IsUnique();

            entity.HasIndex(e => e.PolicyId, "id_UNIQUE").IsUnique();

            entity.Property(e => e.PolicyId)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.Cgst)
                .HasPrecision(10)
                .HasColumnName("CGST");
            entity.Property(e => e.EligibleForNcb)
                .HasDefaultValueSql("'0'")
                .HasColumnName("EligibleForNCB");
            entity.Property(e => e.Igst)
                .HasPrecision(10)
                .HasColumnName("IGST");
            entity.Property(e => e.PaymentType)
                .HasMaxLength(45)
                .IsFixedLength();
            entity.Property(e => e.PolicyEffectiveDt).HasColumnType("date");
            entity.Property(e => e.PolicyExpirationDt).HasColumnType("date");
            entity.Property(e => e.QuoteNumber).ValueGeneratedOnAdd();
            entity.Property(e => e.RateDt).HasColumnType("date");
            entity.Property(e => e.ReceiptNumber)
                .HasMaxLength(45)
                .IsFixedLength();
            entity.Property(e => e.Sgst)
                .HasPrecision(10)
                .HasColumnName("SGST");
            entity.Property(e => e.Status)
                .HasMaxLength(45)
                .IsFixedLength();
            entity.Property(e => e.TotalFees).HasPrecision(10);
            entity.Property(e => e.TotalPremium).HasPrecision(10);
        });

        modelBuilder.Entity<Policycoverage>(entity =>
        {
            entity.HasKey(e => e.PolicyCoverageId).HasName("PRIMARY");

            entity.ToTable("policycoverage");

            entity.HasIndex(e => e.CoverageId, "CoverageId_idx");

            entity.HasIndex(e => e.PolicyCoverageId, "Id_UNIQUE").IsUnique();

            entity.HasIndex(e => e.PolicyId, "PolicyId_idx");

            entity.Property(e => e.PolicyCoverageId)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.PolicyId)
                .HasMaxLength(50)
                .IsFixedLength();

            entity.HasOne(d => d.Coverage).WithMany(p => p.Policycoverages)
                .HasForeignKey(d => d.CoverageId)
                .HasConstraintName("CoverageId");

            entity.HasOne(d => d.Policy).WithMany(p => p.Policycoverages)
                .HasForeignKey(d => d.PolicyId)
                .HasConstraintName("PolicyId");
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.VehicleId).HasName("PRIMARY");

            entity.ToTable("vehicle");

            entity.HasIndex(e => e.BodyTypeId, "BodyId_idx");

            entity.HasIndex(e => e.BrandId, "BrandId_idx");

            entity.HasIndex(e => e.FuelTypeId, "FuelTypeId_idx");

            entity.HasIndex(e => e.ModelId, "ModelId_idx");

            entity.HasIndex(e => e.Rtoid, "RTOId_idx");

            entity.HasIndex(e => e.TransmissionTypeId, "TransmissionTypeId_idx");

            entity.HasIndex(e => e.VariantId, "VariantId_idx");

            entity.HasIndex(e => e.VehicleTypeid, "VehicleTypeId_idx");

            entity.HasIndex(e => e.VehicleId, "id_UNIQUE").IsUnique();

            entity.Property(e => e.VehicleId)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.ChasisNumber)
                .HasMaxLength(45)
                .IsFixedLength();
            entity.Property(e => e.Color)
                .HasMaxLength(45)
                .IsFixedLength();
            entity.Property(e => e.DateOfPurchase).HasColumnType("date");
            entity.Property(e => e.EngineNumber)
                .HasMaxLength(45)
                .IsFixedLength();
            entity.Property(e => e.ExShowroomPrice).HasPrecision(10);
            entity.Property(e => e.Idv)
                .HasPrecision(10)
                .HasColumnName("IDV");
            entity.Property(e => e.RegistrationNumber)
                .HasMaxLength(45)
                .IsFixedLength();
            entity.Property(e => e.Rtoid).HasColumnName("RTOId");
        });

        modelBuilder.Entity<Policyvehicle>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("policyvehicle");

            entity.HasIndex(e => e.PolicyId, "fk_polid");

            entity.HasIndex(e => e.VehicleId, "fk_vehicleid");

            entity.Property(e => e.PolicyId)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.VehicleId)
                .HasMaxLength(50)
                .IsFixedLength();
        });

     

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
