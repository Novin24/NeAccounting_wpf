using Domain.NovinEntity.Customers;
using Domain.NovinEntity.Documents;
using Domain.NovinEntity.Materials;
using Domain.NovinEntity.Workers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Reflection;

namespace Infrastructure.Utilities
{
    public static class ModelBuilderExtensions
    {

        public static void ConfigureDbContext(this ModelBuilder builder)
        {

            builder.Entity<Document>(b =>
            {
                b.Property(r => r.Serial).IsRequired().ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

                b.HasIndex(b => b.Id);
                b.HasIndex(b => b.Serial);
            });

            builder.Entity<Customer>(b =>
            {
                b.HasIndex(b => b.Id);
                b.Property(r => r.Name).IsRequired();
                b.Property(r => r.NationalCode).IsRequired();
                b.Property(r => r.Mobile).IsRequired();
            });


            builder.Entity<Worker>(b =>
            {
                b.HasIndex(t => t.PersonnelId);
                b.Property(t => t.FullName).IsRequired();
                b.Property(t => t.Mobile).IsRequired();
                b.Property(t => t.NationalCode).IsRequired();
                b.Property(t => t.Address).IsRequired();
                b.Property(t => t.AccountNumber).IsRequired();
                b.Property(t => t.JobTitle).IsRequired();
            });

            builder.Entity<Material>(b =>
            {
                b.HasIndex(t => t.Id);
                b.HasOne(t => t.Unit)
                .WithMany(s => s.Materials)
                .HasForeignKey(t => t.UnitId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Salary>(b =>
            {
                b.HasIndex(t => t.WorkerId);
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

                b.HasIndex(t => t.PersianYear);
                b.HasIndex(t => t.PersanMonth);
                b.HasIndex(t => t.WorkerId);
            });

            builder.Entity<FinancialAid>(b =>
            {
                b.HasOne(t => t.Worker)
                .WithMany(s => s.Aids)
                .HasForeignKey(t => t.WorkerId)
                .OnDelete(DeleteBehavior.Cascade);

                b.HasIndex(t => t.PersanMonth);
                b.HasIndex(t => t.PersianYear);
                b.HasIndex(t => t.WorkerId);
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
