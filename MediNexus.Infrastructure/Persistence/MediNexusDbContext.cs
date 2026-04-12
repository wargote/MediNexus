using MediNexus.Domain;
using MediNexus.Domain.Administrator;
using MediNexus.Domain.Contracts;
using MediNexus.Domain.Insurers;
using MediNexus.Domain.Location;
using MediNexus.Domain.Medicines;
using MediNexus.Domain.NavegationMenus;
using MediNexus.Domain.ParametersContracts;
using MediNexus.Domain.Providers;
using MediNexus.Domain.Patients;
using MediNexus.Domain.Triages;
using MediNexus.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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

        // Users
        public DbSet<User> Users => Set<User>();
        public DbSet<DocumentType> DocumentTypes => Set<DocumentType>();
        public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();
        public DbSet<UserStatus> UserStatuses => Set<UserStatus>();
        // Localization
        public DbSet<Country> Countries { get; set; } = null!;
        public DbSet<State> States { get; set; } = null!;
        public DbSet<City> Cities { get; set; } = null!;
        // Menus
        public DbSet<NavigationModule> NavigationModules { get; set; }
        public DbSet<NavigationMenu> NavigationMenus { get; set; }
        public DbSet<NavigationSubMenu> NavigationSubMenus { get; set; }
        public DbSet<RoleMenuPermission> RoleMenuPermissions { get; set; }
        public DbSet<RoleSubMenuPermission> RoleSubMenuPermissions { get; set; }

        public DbSet<AdministratorType> AdministratorTypes { get; set; } = null!;
        public DbSet<Insurer> Insurers { get; set; } = null!;
        public DbSet<Provider> Providers => Set<Provider>();
        public DbSet<Patient> Patients { get; set; } = null!;
        public DbSet<Triage> Triages { get; set; } = null!;
        public DbSet<Sex> Sexes { get; set; } = null!;
        public DbSet<Disability> Disabilities { get; set; } = null!;
        public DbSet<Zone> Zones { get; set; } = null!;
        public DbSet<BloodGroup> BloodGroups { get; set; } = null!;
        public DbSet<RhFactor> RhFactors { get; set; } = null!;
        public DbSet<MaritalStatus> MaritalStatuses { get; set; } = null!;

        //Medicines
        public DbSet<Medicine> Medicines => Set<Medicine>();
        public DbSet<MeasurementUnit> MeasurementUnits => Set<MeasurementUnit>();
        public DbSet<AdministrationRoute> AdministrationRoutes => Set<AdministrationRoute>();
        public DbSet<PharmaceuticalForm> PharmaceuticalForms => Set<PharmaceuticalForm>();
        public DbSet<Presentation> Presentations => Set<Presentation>();
        public DbSet<MedicineGroup> MedicineGroups => Set<MedicineGroup>();

        //Contracts
        public DbSet<ValueMethod> ValueMethods => Set<ValueMethod>();
        public DbSet<BenefitPlanContractType> BenefitPlanContractTypes => Set<BenefitPlanContractType>();
        public DbSet<EpsRegime> EpsRegimes => Set<EpsRegime>();
        public DbSet<PaymentModality> PaymentModalities => Set<PaymentModality>();
        public DbSet<HealthUserType> HealthUserTypes => Set<HealthUserType>();
        public DbSet<Coverage> Coverages => Set<Coverage>();
        public DbSet<ContractStatus> ContractStatuses => Set<ContractStatus>();
        public DbSet<Contract> Contracts => Set<Contract>();


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

                b.Property(u => u.Phone)
                    .IsRequired()
                    .HasMaxLength(25);

                b.Property(u => u.Telephone)
                    .IsRequired()
                    .HasMaxLength(25);

                b.Property(u => u.Signature)
                    .HasMaxLength(1000);

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
            ConfigureNavegationMenu(modelBuilder);
            ConfigureProviders(modelBuilder);
            ConfigurationMedicine(modelBuilder);
            ConfigureMeasurementUnit(modelBuilder);
            ConfigureAdministrationRoute(modelBuilder);
            ConfigurePharmaceuticalForm(modelBuilder);
            ConfigurePresentation(modelBuilder);
            ConfigureMedicineGroup(modelBuilder);
            ConfigureSex(modelBuilder);
            ConfigureDisability(modelBuilder);
            ConfigureZone(modelBuilder);
            ConfigureBloodGroup(modelBuilder);
            ConfigureRhFactor(modelBuilder);
            ConfigureMaritalStatus(modelBuilder);
            ConfigurePatient(modelBuilder);
            ConfigureTriage(modelBuilder);

            ConfigureValueMethod(modelBuilder);
            ConfigureBenefitPlanContractType(modelBuilder);
            ConfigureEpsRegime(modelBuilder);
            ConfigurePaymentModality(modelBuilder);
            ConfigureHealthUserType(modelBuilder);
            ConfigureCoverage(modelBuilder);
            ConfigureContractStatus(modelBuilder);
            ConfigureContract(modelBuilder);
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
                .IsUnique()
                .HasFilter("\"IsActive\" = true");

            entity.HasIndex(e => e.Code)
                .IsUnique()
                .HasFilter("\"Code\" IS NOT NULL AND \"IsActive\" = true");
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

        protected static void ConfigureProviders(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Provider>(entity =>
            {
                entity.ToTable("Providers", "public");
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name).HasMaxLength(250).IsRequired();
                entity.Property(x => x.IdentificationTypeId).IsRequired();
                entity.Property(x => x.Nit).HasMaxLength(50).IsRequired();

                entity.Property(x => x.VerificationDigit).HasMaxLength(5);
                entity.Property(x => x.Address).HasMaxLength(300);
                entity.Property(x => x.Phone).HasMaxLength(50);

                entity.Property(x => x.LegalRepresentative).HasMaxLength(250);
                entity.Property(x => x.DocumentType).HasMaxLength(50);
                entity.Property(x => x.DocumentNumber).HasMaxLength(50);

                entity.Property(x => x.DianResolution).HasMaxLength(100);
                entity.Property(x => x.Prefix).HasMaxLength(20);

                entity.Property(x => x.Email).HasMaxLength(150);
                entity.Property(x => x.EnableCode).HasMaxLength(100);
                entity.Property(x => x.Regimen).HasMaxLength(100);
                entity.Property(x => x.InvoiceIssuerName).HasMaxLength(250);

                // Mantener DATE en Postgres para estos campos
                entity.Property(x => x.ResolutionFromDate).HasColumnType("date");
                entity.Property(x => x.ResolutionToDate).HasColumnType("date");

                entity.Property(x => x.ApplyTax).IsRequired();

                // Índices
                entity.HasIndex(x => x.Nit)
                      .HasDatabaseName("IX_Providers_Nit");

                entity.HasIndex(x => x.CityId)
                      .HasDatabaseName("IX_Providers_CityId");

                // Recomendado: Nit único (si tu negocio lo requiere)
                // Si ya tienes datos duplicados, no lo actives hasta limpiar.
                entity.HasIndex(x => x.Nit)
                      .IsUnique()
                      .HasDatabaseName("UX_Providers_Nit");
            });
        }
        protected static void ConfigurationMedicine(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<Medicine>();

            entity.ToTable("Medicines");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(1000);

            entity.Property(x => x.Cum)
                .HasMaxLength(50);

            entity.Property(x => x.Concentration)
                .HasMaxLength(100);

            entity.Property(x => x.MeasurementUnitSidamId)
                .HasMaxLength(4);

            entity.Property(x => x.AdministrationRouteCode)
                .HasMaxLength(3);

            entity.Property(x => x.PharmaceuticalFormCode)
                .HasMaxLength(10);

            entity.Property(x => x.PresentationCode)
                .HasMaxLength(2);

            entity.Property(x => x.MedicineGroupCode)
                .HasMaxLength(2);

            entity.Property(x => x.Atc)
                .HasMaxLength(20);

            entity.Property(x => x.Invima)
                .HasMaxLength(50);

            entity.Property(x => x.Price)
                .HasColumnType("numeric(18,2)");

            entity.Property(x => x.CreatedAt)
                .HasDefaultValueSql("now()");

            entity.Property(x => x.UpdatedAt)
                .HasDefaultValueSql("now()");

            // Relaciones (FKs)
            entity.HasOne(x => x.MeasurementUnit)
                .WithMany() // si no tienes colección inversa
                .HasForeignKey(x => x.MeasurementUnitSidamId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.AdministrationRoute)
                .WithMany()
                .HasForeignKey(x => x.AdministrationRouteCode)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.PharmaceuticalForm)
                .WithMany()
                .HasForeignKey(x => x.PharmaceuticalFormCode)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Presentation)
                .WithMany()
                .HasForeignKey(x => x.PresentationCode)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.MedicineGroup)
                .WithMany()
                .HasForeignKey(x => x.MedicineGroupCode)
                .OnDelete(DeleteBehavior.Restrict);

            // Índices útiles
            entity.HasIndex(x => x.Name);
            entity.HasIndex(x => x.Cum);
            entity.HasIndex(x => x.Atc);
            entity.HasIndex(x => x.Invima);
        }

        private static void ConfigureAdministrationRoute(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<AdministrationRoute>();

            entity.ToTable("AdministrationRoutes");

            entity.HasKey(x => x.Code);             

            entity.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(3);

            entity.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(250);

            entity.Property(x => x.MipresEnabled)
                .IsRequired();

            entity.Property(x => x.MipresVersion)
                .HasColumnType("numeric(4,1)")
                .IsRequired();

            entity.Property(x => x.EffectiveDate)
                .HasColumnType("date")
                .IsRequired();
        }

        private static void ConfigureMeasurementUnit(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<MeasurementUnit>();

            entity.ToTable("MeasurementUnits");

            entity.HasKey(x => x.SidamId);

            entity.Property(x => x.SidamId)
                .IsRequired()
                .HasMaxLength(4);

            entity.Property(x => x.UnitCode)
                .IsRequired()
                .HasMaxLength(60);

            entity.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(250);

            entity.Property(x => x.MipresEnabled)
                .IsRequired();

            entity.Property(x => x.MipresVersion)
                .HasColumnType("numeric(4,1)")
                .IsRequired();

            entity.Property(x => x.EffectiveDate)
                .HasColumnType("date")
                .IsRequired();

            entity.HasIndex(x => x.UnitCode);
        }

        private static void ConfigurePharmaceuticalForm(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<PharmaceuticalForm>();

            entity.ToTable("PharmaceuticalForms");

            entity.HasKey(x => x.Code);

            entity.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(10);

            entity.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(300);

            entity.Property(x => x.MipresEnabled)
                .IsRequired();

            entity.Property(x => x.MipresVersion)
                .HasColumnType("numeric(4,1)")
                .IsRequired();

            entity.Property(x => x.EffectiveDate)
                .HasColumnType("date")
                .IsRequired();
        }
        private static void ConfigurePresentation(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<Presentation>();

            entity.ToTable("Presentations");

            entity.HasKey(x => x.Code);

            entity.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(2);

            entity.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(300);

            entity.Property(x => x.MipresEnabled)
                .IsRequired();

            entity.Property(x => x.MipresVersion)
                .HasColumnType("numeric(4,1)")
                .IsRequired();

            entity.Property(x => x.EffectiveDate)
                .HasColumnType("date")
                .IsRequired();
        }

        private static void ConfigureMedicineGroup(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<MedicineGroup>();

            entity.ToTable("MedicineGroups");

            entity.HasKey(x => x.Code);

            entity.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(2);

            entity.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(250);
        }

        private static void ConfigureValueMethod(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<ValueMethod>();

            entity.ToTable("ValueMethods");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(100);
        }
        private static void ConfigureBenefitPlanContractType(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<BenefitPlanContractType>();

            entity.ToTable("BenefitPlanContractTypes");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(150);
        }
        private static void ConfigureEpsRegime(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<EpsRegime>();

            entity.ToTable("EpsRegimes");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(100);
        }
        private static void ConfigurePaymentModality(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<PaymentModality>();

            entity.ToTable("PaymentModalities");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(150);
        }
        private static void ConfigureHealthUserType(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<HealthUserType>();

            entity.ToTable("HealthUserTypes");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(200);
        }
        private static void ConfigureCoverage(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<Coverage>();

            entity.ToTable("Coverages");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(250);
        }
        public static void ConfigureContractStatus(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<ContractStatus>();

            entity.ToTable("ContractStatuses");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(50);
        }

        public static void ConfigureContract(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<Contract>();

            entity.ToTable("Contracts");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.ContractNumber)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(x => x.ContractName)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(x => x.StartDate)
                .IsRequired();

            entity.Property(x => x.EndDate)
                .IsRequired();

            entity.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            entity.HasIndex(x => new { x.InsurerId, x.ContractNumber })
                .IsUnique();

            entity.HasOne(x => x.Insurer)
                .WithMany()
                .HasForeignKey(x => x.InsurerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.ValueMethod)
                .WithMany()
                .HasForeignKey(x => x.ValueMethodId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.BenefitPlanContractType)
                .WithMany()
                .HasForeignKey(x => x.BenefitPlanContractTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.EpsRegime)
                .WithMany()
                .HasForeignKey(x => x.EpsRegimeId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.HealthUserType)
                .WithMany()
                .HasForeignKey(x => x.HealthUserTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.PaymentModality)
                .WithMany()
                .HasForeignKey(x => x.PaymentModalityId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Coverage)
                .WithMany()
                .HasForeignKey(x => x.CoverageId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.ContractStatus)
                .WithMany(x => x.Contracts)
                .HasForeignKey(x => x.ContractStatusId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public static void ConfigurePatient(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<Patient>();

            entity.ToTable("Patients");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.DocumentNumber)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.MiddleName)
                .HasMaxLength(100);

            entity.Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.SecondLastName)
                .HasMaxLength(100);

            entity.Property(x => x.BirthDate)
                .IsRequired()
                .HasColumnType("date");

            entity.Property(x => x.SexId);

            entity.Property(x => x.ZoneId);

            entity.Property(x => x.Address)
                .HasMaxLength(250);

            entity.Property(x => x.Phone)
                .HasMaxLength(20);

            entity.Property(x => x.Email)
                .HasMaxLength(150);

            entity.Property(x => x.MaritalStatusId);

            entity.Property(x => x.DisabilityId);

            entity.Property(x => x.BloodGroupId);

            entity.Property(x => x.RhFactorId);

            entity.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            entity.Property(x => x.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("now()");

            entity.Property(x => x.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("now()");

            entity.HasOne(x => x.Insurer)
                .WithMany()
                .HasForeignKey(x => x.InsurerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Contract)
                .WithMany()
                .HasForeignKey(x => x.ContractId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.DocumentType)
                .WithMany()
                .HasForeignKey(x => x.DocumentTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.BirthCountry)
                .WithMany()
                .HasForeignKey(x => x.BirthCountryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.ResidenceCountry)
                .WithMany()
                .HasForeignKey(x => x.ResidenceCountryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.State)
                .WithMany()
                .HasForeignKey(x => x.StateId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.City)
                .WithMany()
                .HasForeignKey(x => x.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Sex)
                .WithMany()
                .HasForeignKey(x => x.SexId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Zone)
                .WithMany()
                .HasForeignKey(x => x.ZoneId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.MaritalStatus)
                .WithMany()
                .HasForeignKey(x => x.MaritalStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Disability)
                .WithMany()
                .HasForeignKey(x => x.DisabilityId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.BloodGroup)
                .WithMany()
                .HasForeignKey(x => x.BloodGroupId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.RhFactor)
                .WithMany()
                .HasForeignKey(x => x.RhFactorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(x => new { x.DocumentTypeId, x.DocumentNumber })
                .IsUnique()
                .HasDatabaseName("IX_Patients_DocumentTypeId_DocumentNumber");

            entity.HasIndex(x => x.InsurerId)
                .HasDatabaseName("IX_Patients_InsurerId");

            entity.HasIndex(x => x.ContractId)
                .HasDatabaseName("IX_Patients_ContractId");

            entity.HasIndex(x => x.BirthCountryId)
                .HasDatabaseName("IX_Patients_BirthCountryId");

            entity.HasIndex(x => x.ResidenceCountryId)
                .HasDatabaseName("IX_Patients_ResidenceCountryId");

            entity.HasIndex(x => x.StateId)
                .HasDatabaseName("IX_Patients_StateId");

            entity.HasIndex(x => x.CityId)
                .HasDatabaseName("IX_Patients_CityId");

            entity.HasIndex(x => x.SexId)
                .HasDatabaseName("IX_Patients_SexId");

            entity.HasIndex(x => x.DisabilityId)
                .HasDatabaseName("IX_Patients_DisabilityId");

            entity.HasIndex(x => x.ZoneId)
                .HasDatabaseName("IX_Patients_ZoneId");

            entity.HasIndex(x => x.BloodGroupId)
                .HasDatabaseName("IX_Patients_BloodGroupId");

            entity.HasIndex(x => x.RhFactorId)
                .HasDatabaseName("IX_Patients_RhFactorId");

            entity.HasIndex(x => x.MaritalStatusId)
                .HasDatabaseName("IX_Patients_MaritalStatusId");
        }

        private static void ConfigureSex(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<Sex>();
            entity.ToTable("Sexes");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.HasIndex(e => e.Name).IsUnique();
        }

        private static void ConfigureDisability(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<Disability>();
            entity.ToTable("Disabilities");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(150);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.HasIndex(e => e.Name).IsUnique();
        }

        private static void ConfigureZone(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<Zone>();
            entity.ToTable("Zones");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.HasIndex(e => e.Name).IsUnique();
        }

        private static void ConfigureBloodGroup(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<BloodGroup>();
            entity.ToTable("BloodGroups");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(10);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.HasIndex(e => e.Name).IsUnique();
        }

        private static void ConfigureRhFactor(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<RhFactor>();
            entity.ToTable("RhFactors");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.HasIndex(e => e.Name).IsUnique();
        }

        private static void ConfigureMaritalStatus(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<MaritalStatus>();
            entity.ToTable("MaritalStatuses");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.HasIndex(e => e.Name).IsUnique();
        }

        private static void ConfigureTriage(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<Triage>();

            entity.ToTable("Triages");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Priority)
                .IsRequired()
                .HasMaxLength(5);

            entity.Property(x => x.ConsultationReason)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(x => x.BloodPressure)
                .HasMaxLength(30);

            entity.Property(x => x.Weight)
                .HasPrecision(10, 2);

            entity.Property(x => x.Height)
                .HasPrecision(10, 2);

            entity.Property(x => x.Temperature)
                .HasPrecision(5, 2);

            entity.Property(x => x.CreatedAt)
                .HasDefaultValueSql("now()");

            entity.Property(x => x.UpdatedAt)
                .HasDefaultValueSql("now()");

            entity.HasOne(x => x.Patient)
                .WithMany()
                .HasForeignKey(x => x.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(x => x.PatientId)
                .HasDatabaseName("IX_Triages_PatientId");
        }
    }
}
