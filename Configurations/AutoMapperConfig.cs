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
            //Map both of student to StudentDTO and studentDTO to Student

            //Auto mapper with different property names,we need to configuire by adding "ForMember" and "MapFrom" 
            CreateMap<StudentDTO, Student>().ForMember(n => n.StudentName, opt => opt.MapFrom(x => x.Name)).ReverseMap(); //It will map reverse StudentDTO to StudentDTO

            //we can do like this also. It will map reverse StudentDTO to StudentDTO
            /*
             CreateMap<StudentDTO, Student>().ReverseMap().ForMember(n => n.Name, opt => opt.MapFrom(x => x.StudentName)); 
            */

            //Auto mapper with property ignore. configure using "Ignore"
            /*
            CreateMap<StudentDTO, Student>().ReverseMap().ForMember(n => n.Email, opt => opt.Ignore()); // here we are saying dont map "Email"
            */

            //Auto mapper with property transform. configure using "AddTransform"
            //If there is null value it will retuen this message( for example if there is null value in Address it will say "No address found")
            /*
            CreateMap<StudentDTO, Student>().ReverseMap().AddTransform<string>(n => string.IsNullOrEmpty(n)? "No address found" : n);
            */

            //Like these add other classes also which we need to map
            //eg: CreateMap<Course, CourseDTO>().ReverseMap(); 
        }
    }
}
