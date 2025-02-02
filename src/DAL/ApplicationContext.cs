﻿using BLL;
using BLL.Entities;
using DAL.Configurations;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class ApplicationContext : DbContext
    {
        private const string HerokuConnectionString =
            "Host=ec2-52-17-1-206.eu-west-1.compute.amazonaws.com;Database=dffpbqtmhdfcpd;Username=icqhlkbvsgzghu;Password=c49fe35a2f55f43cf7c48d1299654724c2ca2829b763df922a7cfb858ed26158;sslmode=Require;TrustServerCertificate=true";
        
        private const string LocalConnectionString =
            "Host=localhost;Database=test_task_db;Username=postgres;Password=qweasd123";
        
        public DbSet<Project> Projects { get; private set; }
        public DbSet<ProjectTask> Tasks { get; private set; }
        
        public ApplicationContext() { }
        
        public ApplicationContext(DbContextOptions options) : base(options) { }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(HerokuConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);
            builder.HasAnnotation("Relational:Collation", "Russian_Russia.1251");

            builder.ApplyConfiguration(new ProjectConfiguration());
            builder.ApplyConfiguration(new ProjectTaskConfiguration());
        }
    }
}