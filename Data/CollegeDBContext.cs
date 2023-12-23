using CollegeApp.Data.Config;
using Microsoft.EntityFrameworkCore;

namespace CollegeApp.Data
{
    //Code First Approach in EF core
    //We can say taht this class will act as a DataBase inside EntityFramework
    public class CollegeDBContext :DbContext
    {
        public CollegeDBContext(DbContextOptions<CollegeDBContext>options) : base(options)
        {

        }
        DbSet<Student> Students { get; set; }
                
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Check StudentConfig.cs file
            #region separated EntityTypeConfiguration file for each table (eg: StudentConfig)
            //Add default data to tables in EF Code First
            /*
            modelBuilder.Entity<Student>().HasData(new List<Student>()
             {
                 new Student {
                     Id=1,
                     StudentName="jesslur",
                     Email="jeslur@gmail.com",
                     Address="Colombo",
                     DOB= new DateTime(1999,12,3)
                 },
                 new Student {
                     Id=2,
                     StudentName="Rahman",
                     Email="rahman@gmail.com",
                     Address="Colombo",
                     DOB= new DateTime(1998,5,3)
                 }
             });
            */

            //Migrate tables with proper schema Datatype, size, null/not null… in EF Code First.
            /*
            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(n => n.StudentName).IsRequired();
                entity.Property(n => n.StudentName).HasMaxLength(250);
                entity.Property(n => n.Address).IsRequired().HasMaxLength(250);
                entity.Property(n => n.Email).IsRequired(false).HasMaxLength(250);
                entity.Property(n => n.DOB).IsRequired();

            });
            */
            #endregion

            //How does the DBContext File know about the particular table related configuration?
            //By adding below line of code

            //Student Table
            modelBuilder.ApplyConfiguration(new StudentConfig());
        }


    }

}
