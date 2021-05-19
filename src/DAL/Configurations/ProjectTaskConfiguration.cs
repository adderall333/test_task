using BLL.Entities;
using BLL.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DAL.Configurations
{
    public class ProjectTaskConfiguration : IEntityTypeConfiguration<ProjectTask>
    {
        public void Configure(EntityTypeBuilder<ProjectTask> builder)
        {
            builder.ToTable("Task");

            builder.HasKey(t => t.Id);
            
            var nameConverter = new ValueConverter<Name, string>(
                n => n.Value,
                n => Name.Create(n).Value);
            builder
                .Property(t => t.Name)
                .HasConversion(nameConverter)
                .HasColumnName("Name")
                .HasColumnType("text")
                .IsRequired();
            
            var priorityConverter = new ValueConverter<Priority, string>(
                p => p.Value.ToString(),
                p => Priority.Create(p).Value);
            builder
                .Property(t => t.Priority)
                .HasConversion(priorityConverter)
                .HasColumnName("Priority")
                .HasColumnType("text")
                .IsRequired();
            
            var statusConverter = new ValueConverter<Status, string>(
                s => s.Value.ToString(),
                s => Status.Create(s).Value);
            builder
                .Property(t => t.Status)
                .HasConversion(statusConverter)
                .HasColumnName("Status")
                .HasColumnType("text")
                .IsRequired();
        }
    }
}