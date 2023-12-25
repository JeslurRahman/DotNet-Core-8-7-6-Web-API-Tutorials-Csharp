using AutoMapper;
using CollegeApp.Data;
using CollegeApp.Model;

namespace CollegeApp.Configurations
{
    //Refer the Documentation of AutoMapper Link: https://docs.automapper.org/

    //We need to add this Mapping configuration into our web api for that 
    //add into the "Program.cs"
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig() 
        {
            /*
            CreateMap<Student, StudentDTO>(); // Map student to StudentDTO
            CreateMap<StudentDTO, Student>(); //Map StudentDTO to Student
            */

            //we can write single line of code instead of writing above 2 lines          
            CreateMap<Student, StudentDTO>().ReverseMap(); // Map both of student to StudentDTO and studentDTO to Student

            //Like these add other classes also which we need to map
            //eg: CreateMap<Course, CourseDTO>().ReverseMap(); 
        }
    }
}
