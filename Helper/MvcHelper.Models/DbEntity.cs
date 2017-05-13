using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Models
{
    public class DbEntity : DbContext, IDbContext
    {
        public DbEntity()
            : base("Data Source=210.29.65.96;Initial Catalog=ImageClassification;User Id=ImageClassification; Password=ImageClassification;")
        {
        }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        //public DbSet<Student> Students { get; set; }
        public DbSet<Volunteer> Volunteers { get; set; }
        public DbSet<Type> Types { get; set; }
        public DbSet<Label> Labels { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<VolunteerToken> VolunteerTokens { get; set; }



        //Fluent API 
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Fluent API 代码
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            //modelBuilder.Entity<CottonYarnSalesList>().Property(p => p.WeightAmount).HasPrecision(18, 5);


        }
    }
}