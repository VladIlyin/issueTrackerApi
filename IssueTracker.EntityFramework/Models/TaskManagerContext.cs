using System;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace IssueTracker.EntityFramework.Models
{
    public partial class TaskManagerContext : DbContext
    {
        public TaskManagerContext()
            : base()
        {
        }

        public TaskManagerContext(DbContextOptions<TaskManagerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<TaskStatus> TaskStatuses { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserProject> UserProjects { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=IssueTracker;Username=postgres;Password=123");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Russian_Russia.1251");

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("Project");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(32);
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.ToTable("Task");

                entity.HasIndex(e => e.ProjectId, "fki_Task_ProjectId_Id");

                entity.HasIndex(e => e.Status, "fki_Task_Status_TaskStatusId");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Status).HasDefaultValueSql("1");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Task_ProjectId_Id");

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.Status)
                    .HasConstraintName("Task_Status_TaskStatusId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("User_Id_UserId");
            });

            modelBuilder.Entity<TaskStatus>(entity =>
            {
                entity.ToTable("TaskStatus");

                entity.Property(e => e.TaskStatusId).ValueGeneratedNever();

                entity.Property(e => e.StatusName)
                    .IsRequired()
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.FirstName).HasColumnType("character varying");

                entity.Property(e => e.LastName).HasColumnType("character varying");

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<UserProject>(entity =>
            {
                entity.ToTable("UserProject");

                entity.HasIndex(e => e.ProjectId, "fki_Project_Id_ProjectId");

                entity.HasIndex(e => e.UserId, "fki_User_Id_UserId");

                entity.Property(e => e.UserProjectId).UseIdentityAlwaysColumn();

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.UserProjects)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Project_Id_ProjectId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserProjects)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("User_Id_UserId");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
