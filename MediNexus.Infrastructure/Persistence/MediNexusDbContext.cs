using MediNexus.Domain.Administrator;
using MediNexus.Domain.Insurer;
using MediNexus.Domain.Location;
using MediNexus.Domain.NavegationMenus;
using MediNexus.Domain.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediNexus.Infrastructure.Persistence
{
    public class MediNexusDbContext : DbContext
    {
        public MediNexusDbContext(DbContextOptions<MediNexusDbContext> options)
            : base(options)
        {
        }

        // 🔹 DbSets
        public DbSet<User> Users => Set<User>();
        public DbSet<DocumentType> DocumentTypes => Set<DocumentType>();
        public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();
        public DbSet<UserStatus> UserStatuses => Set<UserStatus>();
        public DbSet<Country> Countries { get; set; } = null!;
        public DbSet<State> States { get; set; } = null!;
        public DbSet<City> Cities { get; set; } = null!;
        public DbSet<AdministratorType> AdministratorTypes { get; set; } = null!;
        public DbSet<Insurer> Insurers { get; set; } = null!;
        public DbSet<NavigationModule> NavigationModules { get; set; }
        public DbSet<NavigationMenu> NavigationMenus { get; set; }
        public DbSet<NavigationSubMenu> NavigationSubMenus { get; set; }
        public DbSet<RoleMenuPermission> RoleMenuPermissions { get; set; }
        public DbSet<RoleSubMenuPermission> RoleSubMenuPermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de User
            modelBuilder.Entity<User>(b =>
            {
                b.HasKey(u => u.Id);

                b.Property(u => u.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                b.Property(u => u.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                b.Property(u => u.Username)
                    .IsRequired()
                    .HasMaxLength(100);

                b.Property(u => u.DocumentNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                b.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(200);

                b.Property(u => u.PasswordHash)
                    .IsRequired();

                b.HasIndex(u => u.Email).IsUnique();
                b.HasIndex(u => u.Username).IsUnique();
                b.HasIndex(u => u.DocumentNumber).IsUnique();

                b.HasOne(u => u.DocumentType)
                    .WithMany(d => d.Users)
                    .HasForeignKey(u => u.DocumentTypeId);

                b.HasOne(u => u.UserProfile)
                    .WithMany(p => p.Users)
                    .HasForeignKey(u => u.UserProfileId);

                b.HasOne(u => u.UserRole)
                    .WithMany(r => r.Users)
                    .HasForeignKey(u => u.UserRoleId);

                b.HasOne(u => u.UserStatus)
                    .WithMany(s => s.Users)
                    .HasForeignKey(u => u.UserStatusId);
            });

            // 🔹 Configuración básica de catálogos + seeds
            modelBuilder.Entity<DocumentType>(b =>
            {
                b.HasKey(d => d.Id);
                b.Property(d => d.Code).IsRequired().HasMaxLength(20);
                b.Property(d => d.Name).IsRequired().HasMaxLength(100);
                b.Property(d => d.IsActive).HasDefaultValue(true);


            });

            modelBuilder.Entity<UserStatus>(b =>
            {
                b.HasKey(s => s.Id);
                b.Property(s => s.Code).IsRequired().HasMaxLength(20);
                b.Property(s => s.Name).IsRequired().HasMaxLength(100);
                b.Property(s => s.IsActive).HasDefaultValue(true);

            });

            modelBuilder.Entity<UserProfile>(b =>
            {
                b.HasKey(p => p.Id);
                b.Property(p => p.Name).IsRequired().HasMaxLength(100);
                b.Property(p => p.Description).HasMaxLength(250);
                b.Property(p => p.IsActive).HasDefaultValue(true);

                b.HasOne(p => p.UserRole)
                    .WithMany(r => r.UserProfiles)
                    .HasForeignKey(p => p.UserRoleId);
            });


            modelBuilder.Entity<UserRole>(b =>
            {
                b.HasKey(r => r.Id);
                b.Property(r => r.Name).IsRequired().HasMaxLength(100);
                b.Property(r => r.Description).HasMaxLength(250);
                b.Property(r => r.IsActive).HasDefaultValue(true);
            });

            ConfigureCountry(modelBuilder);
            ConfigureState(modelBuilder);
            ConfigureCity(modelBuilder);
            ConfigureAdministratorType(modelBuilder);
            ConfigureInsurer(modelBuilder);
        }

        private static void ConfigureCountry(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<Country>();

            entity.ToTable("Countries");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(10);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(e => e.IsActive)
                .HasDefaultValue(true);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()");

            entity.HasIndex(e => e.Code)
                .IsUnique();

            entity.HasIndex(e => e.Name)
                .IsUnique();
        }

        private static void ConfigureState(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<State>();

            entity.ToTable("States");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(e => e.DaneCode)
                .HasMaxLength(10);

            entity.Property(e => e.IsActive)
                .HasDefaultValue(true);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()");

            entity.HasOne(e => e.Country)
                .WithMany(c => c.States)
                .HasForeignKey(e => e.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => new { e.CountryId, e.Name })
                .IsUnique();

            entity.HasIndex(e => e.DaneCode)
                .IsUnique()
                .HasFilter("\"DaneCode\" IS NOT NULL");
        }

        private static void ConfigureCity(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<City>();

            entity.ToTable("Cities");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(10);

            entity.Property(e => e.FuripsCode)
                .HasMaxLength(10);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(e => e.IsActive)
                .HasDefaultValue(true);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()");

            entity.HasOne(e => e.State)
                .WithMany(s => s.Cities)
                .HasForeignKey(e => e.StateId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.Code)
                .IsUnique();

            entity.HasIndex(e => new { e.StateId, e.Name })
                .IsUnique();
        }

        private static void ConfigureAdministratorType(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<AdministratorType>();

            entity.ToTable("AdministratorTypes");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(e => e.ShortName)
                .HasMaxLength(50);

            entity.Property(e => e.IsActive)
                .HasDefaultValue(true);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()");

            entity.HasIndex(e => e.Name)
                .IsUnique();

            entity.HasIndex(e => e.ShortName)
                .IsUnique()
                .HasFilter("\"ShortName\" IS NOT NULL");

        }

        private static void ConfigureInsurer(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<Insurer>();

            entity.ToTable("Insurers");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.Nit)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(e => e.VerificationDigit)
                .HasColumnType("smallint");

            entity.Property(e => e.Code)
                .HasMaxLength(20);

            entity.Property(e => e.Address)
                .HasMaxLength(250);

            entity.Property(e => e.Phone1)
                .HasMaxLength(20);

            entity.Property(e => e.Phone2)
                .HasMaxLength(20);

            entity.Property(e => e.Email)
                .HasMaxLength(150);

            entity.Property(e => e.IsActive)
                .HasDefaultValue(true);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()");

            entity.HasOne(e => e.City)
                .WithMany(c => c.Insurers)
                .HasForeignKey(e => e.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.AdministratorType)
                .WithMany(a => a.Insurers)
                .HasForeignKey(e => e.AdministratorTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.Nit)
                .IsUnique();

            entity.HasIndex(e => e.Code)
                .IsUnique()
                .HasFilter("\"Code\" IS NOT NULL");
        }

        protected static void ConfigureNavegationMenu(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<NavigationModule>(entity =>
            {
                entity.ToTable("NavigationModules");
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            });

            // NavigationMenu
            modelBuilder.Entity<NavigationMenu>(entity =>
            {
                entity.ToTable("NavigationMenus");
                
                entity.HasOne(m => m.Module)
                   .WithMany(mod => mod.Menus)
                   .HasForeignKey(m => m.ModuleId);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Route)
                    .HasMaxLength(200);

                entity.Property(e => e.Icon)
                    .HasMaxLength(100);
            });

            // NavigationSubMenu
            modelBuilder.Entity<NavigationSubMenu>(entity =>
            {
                entity.ToTable("NavigationSubMenus");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Route)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(e => e.Menu)
                    .WithMany(m => m.SubMenus)
                    .HasForeignKey(e => e.MenuId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // RoleMenuPermission
            modelBuilder.Entity<RoleMenuPermission>(entity =>
            {
                entity.ToTable("RoleMenuPermissions");

                entity.HasOne(e => e.Role)
                    .WithMany(r => r.MenuPermissions)
                    .HasForeignKey(e => e.RoleId);

                entity.HasOne(e => e.Menu)
                    .WithMany(m => m.RolePermissions)
                    .HasForeignKey(e => e.MenuId);
            });

            // RoleSubMenuPermission
            modelBuilder.Entity<RoleSubMenuPermission>(entity =>
            {
                entity.ToTable("RoleSubMenuPermissions");

                entity.HasOne(e => e.Role)
                    .WithMany(r => r.SubMenuPermissions)
                    .HasForeignKey(e => e.RoleId);

                entity.HasOne(e => e.SubMenu)
                    .WithMany(sm => sm.RolePermissions)
                    .HasForeignKey(e => e.SubMenuId);
            });
        }
    }
}
