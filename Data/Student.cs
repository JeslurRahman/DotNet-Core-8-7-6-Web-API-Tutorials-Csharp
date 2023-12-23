using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CollegeApp.Data
{
    public class Student
    {
        //When we add this key attribute added on top of any property or variable
        //so, you will recognize that as primaey key column of particular table (eg: Student)
        //All Configuration separated for each table check StudentConfig.cs file
        /*
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        */
        public int Id { get; set; }
        public string StudentName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime DOB { get; set; }
    }
}
