using Domain.Enities.Notifications;
using Domain.Enities.NovinEntity.Remittances;
using Domain.NovinEntity.Cheques;
using Domain.NovinEntity.Customers;
using Domain.NovinEntity.Documents;
using Domain.NovinEntity.Expense;
using Domain.NovinEntity.Materials;
using Domain.NovinEntity.Workers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Reflection;

namespace Infrastructure.Utilities
{
    public static class ModelBuilderExtensions
    {

        public static void ConfigureNovinDbContext(this ModelBuilder builder)
        {

            builder.Entity<Document>(b =>
            {
                b.Property(r => r.Serial).IsRequired().ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
                b.Property(r => r.Description).HasMaxLength(150);
                b.HasIndex(b => b.Id);
                b.HasIndex(b => b.CustomerId);
                b.HasIndex(b => b.Serial);
            });

            builder.Entity<Customer>(b =>
            {
                b.HasIndex(b => b.Id);
                b.Property(b => b.CusId).IsRequired().ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
                b.Property(r => r.Name).HasMaxLength(50).IsRequired();
                b.Property(r => r.NationalCode).HasMaxLength(10).IsRequired();
                b.Property(r => r.Mobile).HasMaxLength(20).IsRequired();
                b.Property(r => r.Address).HasMaxLength(150);
                b.Property(r => r.IsActive).HasDefaultValue(true);
            });

            builder.Entity<Personel>(b =>
            {
                b.HasIndex(t => t.PersonnelId);
                b.Property(t => t.FullName).HasMaxLength(50).IsRequired();
                b.Property(t => t.NationalCode).HasMaxLength(10);
                b.Property(t => t.Mobile).IsRequired();
                b.Property(t => t.Address).HasMaxLength(150);
                b.Property(t => t.Description).HasMaxLength(200);
                b.Property(t => t.AccountNumber).HasMaxLength(26).IsRequired();
                b.Property(r => r.IsActive).HasDefaultValue(true);
                b.Property(t => t.JobTitle).HasMaxLength(50).IsRequired();
            });

            builder.Entity<Expense>(b =>
            {
                b.HasIndex(t => t.Id);
                b.Property(t => t.Expensetype).HasMaxLength(150).IsRequired();
                b.Property(t => t.Description).HasMaxLength(250);
                b.Property(t => t.Receiver).HasMaxLength(100);
            });

            builder.Entity<Pun>(b =>
            {

                b.HasIndex(t => t.Id);
                b.Property(r => r.IsActive).HasDefaultValue(true);
                b.Property(t => t.Name).HasMaxLength(100).IsRequired();
                b.Property(t => t.Serial).HasMaxLength(50);
                b.Property(t => t.PhysicalAddress).HasMaxLength(100);
                b.HasOne(t => t.Unit)
                .WithMany(s => s.Materials)
                .HasForeignKey(t => t.UnitId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Cheque>(b =>
            {
                b.HasIndex(t => t.Id);
                b.Property(r => r.Serial).IsRequired().ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
                b.Property(t => t.Cheque_Number).HasMaxLength(100).IsRequired();
                b.Property(t => t.Cheque_Owner).HasMaxLength(50).IsRequired();
                b.Property(t => t.Bank_Name).HasMaxLength(50).IsRequired();
                b.Property(t => t.Accunt_Number).HasMaxLength(100);
                b.Property(t => t.Bank_Branch).HasMaxLength(50);
                b.HasOne(t => t.Document)
                .WithMany(s => s.Cheques)
                .HasForeignKey(t => t.DocumetnId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Units>(b =>
            {
                b.HasIndex(t => t.Id);
                b.Property(t => t.Name).HasMaxLength(30);
            });

            builder.Entity<Salary>(b =>
            {
                b.Property(t => t.Description).HasMaxLength(200);
                b.HasIndex(t => t.WorkerId);
                b.HasIndex(t => t.PersianYear);
                b.HasIndex(t => t.PersianMonth);
                b.HasOne(t => t.Worker)
                .WithMany(s => s.Salaries)
                .HasForeignKey(t => t.WorkerId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Function>(b =>
            {
                b.HasOne(t => t.Worker)
                .WithMany(s => s.Functions)
                .HasForeignKey(t => t.WorkerId)
                .OnDelete(DeleteBehavior.Cascade);

                b.Property(t => t.Description).HasMaxLength(200);
                b.HasIndex(t => t.PersianYear);
                b.HasIndex(t => t.PersianMonth);
                b.HasIndex(t => t.WorkerId);
            });

            builder.Entity<FinancialAid>(b =>
            {
                b.HasOne(t => t.Worker)
                .WithMany(s => s.Aids)
                .HasForeignKey(t => t.WorkerId)
                .OnDelete(DeleteBehavior.Cascade);

                b.Property(t => t.Description).HasMaxLength(200);
                b.HasIndex(t => t.PersianMonth);
                b.HasIndex(t => t.PersianYear);
                b.HasIndex(t => t.WorkerId);
            });

            builder.Entity<BuyRemittance>(b =>
            {
                b.Property(t => t.Description).HasMaxLength(100);

                b.HasOne(t => t.Material)
                .WithMany(s => s.BuyRemittances)
                .HasForeignKey(t => t.MaterialId)
                .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(t => t.Document)
                .WithMany(s => s.BuyRemittances)
                .HasForeignKey(t => t.DocumentId)
                .OnDelete(DeleteBehavior.Cascade);

                b.HasIndex(t => t.DocumentId);
            });

            builder.Entity<SellRemittance>(b =>
            {
                b.Property(t => t.Description).HasMaxLength(100);

                b.HasOne(t => t.Material)
                .WithMany(s => s.SellRemittances)
                .HasForeignKey(t => t.MaterialId)
                .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(t => t.Document)
                .WithMany(s => s.SellRemittances)
                .HasForeignKey(t => t.DocumentId)
                .OnDelete(DeleteBehavior.Cascade);

                b.HasIndex(t => t.DocumentId);
            });
        }


        public static void ConfigureBaseDbContext(this ModelBuilder builder)
        {
            builder.Entity<Notification>(b =>
            {
                b.HasIndex(b => b.Id);
                b.HasIndex(b => b.DocumentId);
                b.Property(r => r.Titel).HasMaxLength(50).IsRequired();
                b.Property(r => r.Message).HasMaxLength(150).IsRequired();
            });
        }


        /// <summary>
        /// Set NEWSEQUENTIALID() sql function for all columns named "Id"
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="mustBeIdentity">Set to true if you want only "Identity" guid fields that named "Id"</param>
        public static void AddSequentialGuidForIdConvention(this ModelBuilder modelBuilder)
        {
            modelBuilder.AddDefaultValueSqlConvention("Id", typeof(Guid), "NEWSEQUENTIALID()");
        }

        /// <summary>
        /// Set DefaultValueSql for sepecific property name and type
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="propertyName">Name of property wants to set DefaultValueSql for</param>
        /// <param name="propertyType">Type of property wants to set DefaultValueSql for </param>
        /// <param name="defaultValueSql">DefaultValueSql like "NEWSEQUENTIALID()"</param>
        public static void AddDefaultValueSqlConvention(this ModelBuilder modelBuilder, string propertyName, Type propertyType, string defaultValueSql)
        {
            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
            {
                IMutableProperty property = entityType.GetProperties().SingleOrDefault(p => p.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));
                if (property != null && property.ClrType == propertyType)
                    property.SetDefaultValueSql(defaultValueSql);
            }
        }

        /// <summary>
        /// Set DeleteBehavior.Restrict by default for relations
        /// </summary>
        /// <param name="modelBuilder"></param>
        public static void AddRestrictDeleteBehaviorConvention(this ModelBuilder modelBuilder)
        {
            IEnumerable<IMutableForeignKey> cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);
            foreach (IMutableForeignKey fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;
        }

        /// <summary>
        /// Dynamicaly load all IEntityTypeConfiguration with Reflection
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="assemblies">Assemblies contains Entities</param>
        public static void RegisterEntityTypeConfiguration(this ModelBuilder modelBuilder, params Assembly[] assemblies)
        {
            MethodInfo applyGenericMethod = typeof(ModelBuilder).GetMethods().First(m => m.Name == nameof(ModelBuilder.ApplyConfiguration));

            IEnumerable<Type> types = assemblies.SelectMany(a => a.GetExportedTypes())
                .Where(c => c.IsClass && !c.IsAbstract && c.IsPublic);

            foreach (Type type in types)
            {
                foreach (Type iface in type.GetInterfaces())
                {
                    if (iface.IsConstructedGenericType && iface.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
                    {
                        MethodInfo applyConcreteMethod = applyGenericMethod.MakeGenericMethod(iface.GenericTypeArguments[0]);
                        applyConcreteMethod.Invoke(modelBuilder, new object[] { Activator.CreateInstance(type) });
                    }
                }
            }
        }

        /// <summary>
        /// Dynamicaly register all Entities that inherit from specific BaseType
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="baseType">Base type that Entities inherit from this</param>
        /// <param name="assemblies">Assemblies contains Entities</param>
        public static void RegisterAllEntities<BaseType>(this ModelBuilder modelBuilder, params Assembly[] assemblies)
        {
            IEnumerable<Type> types = assemblies.SelectMany(a => a.GetExportedTypes())
                .Where(c => c.IsClass && !c.IsAbstract && c.IsPublic && typeof(BaseType).IsAssignableFrom(c));

            foreach (Type type in types)
                modelBuilder.Entity(type);
        }
    }
}
