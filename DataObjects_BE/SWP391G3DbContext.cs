using DataObjects_BE.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects_BE
{
    public class SWP391G3DbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Child> Childs { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<ProductList> ProductLists { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceOrder> ServiceOrders { get; set; }
        public DbSet<ReportProduct> ReportProducts { get; set; }
        public DbSet<Appointment> Appointments { get; set; }


        public SWP391G3DbContext(DbContextOptions options): base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Account>()
                .HasOne(a => a.Manager)
                .WithOne(m => m.Account)
                .HasForeignKey<Manager>(m => m.AccountId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Account>()
                .HasOne(a => a.Doctor)
                .WithOne(d => d.Account)
                .HasForeignKey<Doctor>(d => d.AccountId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Account>()
                .HasOne(a => a.Parent)
                .WithOne(p => p.Account)
                .HasForeignKey<Parent>(p => p.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Doctor)
                .WithMany(d => d.Feedbacks)
                .HasForeignKey(f => f.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Parent)
                .WithMany(p => p.Ratings)
                .HasForeignKey(r => r.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Feedback)
                .WithMany(f => f.Ratings)
                .HasForeignKey(r => r.FeedbackId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Child>()
                .HasOne(c => c.Parent)
                .WithMany(p => p.Childs)
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Report>()
                .HasOne(r => r.Child)
                .WithMany(c => c.Reports)
                .HasForeignKey(r => r.ChildId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Report>();
            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Report)
                .WithMany(r => r.Feedbacks)
                .HasForeignKey(f => f.ReportId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ServiceOrder>()
                .HasOne(s => s.Parent)
                .WithMany(p => p.ServiceOrders)
                .HasForeignKey(s => s.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ServiceOrder>()
                .HasOne(s => s.Service)
                .WithMany(so => so.ServiceOrders)
                .HasForeignKey(s => s.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ProductList>()
                .Property(p => p.Price)
                .HasPrecision(18, 4); // Thay đổi precision và scale phù hợp với yêu cầu của bạn
            modelBuilder.Entity<Service>(entity =>
            {
                entity.HasKey(e => e.ServiceId);

                entity.Property(e => e.ServiceId)
                      .ValueGeneratedOnAdd() // Set Identity (1,1)
                      .UseIdentityColumn(1, 1);

                entity.Property(e => e.ServiceName)
                      .IsRequired()
                      .HasMaxLength(255);

                entity.Property(e => e.ServicePrice)
                      .HasColumnType("decimal(18,2)");
            });
            modelBuilder.Entity<ReportProduct>()
                .HasOne(rp => rp.Report)
                .WithMany(r => r.ReportProducts)
                .HasForeignKey(rp => rp.ReportId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ReportProduct>()
                .HasOne(rp => rp.ProductList)
                .WithMany(p => p.ReportProducts)
                .HasForeignKey(rp => rp.ProductListId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
