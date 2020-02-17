namespace licenta.DatabaseConnection
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class TehnicalDepartmentDb : DbContext
    {
        public TehnicalDepartmentDb()
            : base("name=TehnicalDepartmentDb1")
        {
        }

        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<File> Files { get; set; }
        public virtual DbSet<Request> Requests { get; set; }
        public virtual DbSet<RequestHistory> RequestHistories { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<File>()
                .HasMany(e => e.RequestHistories)
                .WithOptional(e => e.File)
                .HasForeignKey(e => e.attachmentsId);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Requests)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.createdBy)
                .WillCascadeOnDelete(false);
        }
    }
}
