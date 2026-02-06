using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using VetSystemModels.Entities;

namespace VetSystemInfrastructure.Configuration;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<AppointmentStatus> AppointmentStatuses { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<Pet> Pets { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Specialization> Specializations { get; set; }

    public virtual DbSet<Species> Species { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=DESKTOP-M8RKUTS;Database=VetDataBase;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("PK__Appointm__8ECDFCC295797577");

            entity.HasOne(d => d.AppointmentStatus).WithMany(p => p.Appointments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Appointme__Appoi__619B8048");

            entity.HasOne(d => d.Pet).WithMany(p => p.Appointments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Appointme__PetId__60A75C0F");

            entity.HasOne(d => d.Schedule).WithMany(p => p.Appointments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Appointments_Schedules");

            entity.HasOne(d => d.Service).WithMany(p => p.Appointments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Appointme__Servi__5EBF139D");
            entity.HasQueryFilter(a => !a.IsDeleted);
        });

        modelBuilder.Entity<AppointmentStatus>(entity =>
        {
            entity.HasKey(e => e.AppointmentStatusId).HasName("PK__Appointm__A619B6607CCDB414");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.ClientId).HasName("PK__Clients__E67E1A240FBEDE4A");

            entity.HasOne(d => d.User).WithOne(p => p.Client).HasConstraintName("FK__Clients__UserId__440B1D61");

            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__7AD04F11FABD6578");

            entity.HasOne(d => d.Specialization).WithMany(p => p.Employees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Employees__Speci__5AEE82B9");

            entity.HasOne(d => d.User).WithOne(p => p.Employee).HasConstraintName("FK__Employees__UserI__5629CD9C");

            entity.HasQueryFilter(e => !e.IsDeleted);

        });

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.HasKey(e => e.GenderId).HasName("PK__Genders__4E24E9F70AF980D6");
        });

        modelBuilder.Entity<Pet>(entity =>
        {
            entity.HasKey(e => e.PetId).HasName("PK__Pets__48E538621514FCA8");

            entity.HasOne(d => d.Client).WithMany(p => p.Pets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Pets__ClientId__5EBF139D");

            entity.HasOne(d => d.Gender).WithMany(p => p.Pets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Pets__GenderId__5DCAEF64");

            entity.HasOne(d => d.Species).WithMany(p => p.Pets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Pets__SpeciesId__5CD6CB2B");

     
            entity.HasQueryFilter(p => !p.IsDeleted && !p.Client.IsDeleted);
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK__Schedule__9C8A5B4938FBDC9D");

            entity.HasOne(d => d.Employee).WithMany(p => p.Schedules)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Schedules__Emplo__5FB337D6");
            entity.HasQueryFilter(s => s.IsAvailable && !s.Employee.IsDeleted);
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Services__C51BB00AC6DAA85E");
            entity.HasQueryFilter(p => !p.IsDeleted);
        });

        modelBuilder.Entity<Specialization>(entity =>
        {
            entity.HasKey(e => e.SpecializationId).HasName("PK__Speciali__5809D86FD9387485");
        });

        modelBuilder.Entity<Species>(entity =>
        {
            entity.HasKey(e => e.SpeciesId).HasName("PK__Species__A938045FCB7EC867");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Role");

            entity.HasQueryFilter(e => e.IsActive);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
