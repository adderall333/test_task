using BLL.Entities;
using BLL.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DAL.Configurations
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.ToTable("Project");
            
            builder.HasKey(p => p.Id);

            var nameConverter = new ValueConverter<Name, string>(
                n => n.Value,
                n => Name.Create(n).Value);
            builder
                .Property(p => p.Name)
                .HasConversion(nameConverter)
                .HasColumnName("Name")
                .HasColumnType("text")
                .IsRequired();

            var priorityConverter = new ValueConverter<Priority, string>(
                p => p.Value.ToString(),
                p => Priority.Create(p).Value);
            builder
                .Property(p => p.Priority)
                .HasConversion(priorityConverter)
                .HasColumnName("Priority")
                .HasColumnType("text")
                .IsRequired();
        }
    }
}