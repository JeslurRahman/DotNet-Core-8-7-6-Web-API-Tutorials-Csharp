using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollegeApp.Data.Config
{
    //Creating separate EntityTypeConfiguration file for each table
    public class StudentConfig : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            //Migrate tables with proper schema Datatype, size, null/not null… in EF Code First.
            builder.ToTable("Students");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(n => n.StudentName).IsRequired();
            builder.Property(n => n.StudentName).HasMaxLength(250);
            builder.Property(n => n.Address).IsRequired().HasMaxLength(250);
            builder.Property(n => n.Email).IsRequired(false).HasMaxLength(250);
            builder.Property(n => n.DOB).IsRequired();

            //Add default data to tables in EF Code First
            builder.HasData(new List<Student>() 
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
        }
    }
}
