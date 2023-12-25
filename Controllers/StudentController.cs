using AutoMapper;
using CollegeApp.Data;
using CollegeApp.Model;
using CollegeApp.MyLogging;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        //Logger Injection
        //This is Default build in logger
        private readonly ILogger<StudentController> _logger;

        //EntityFramework Injection
        //Removed in memory repository and use Entity framework 
        //For Dependancy Injection We need to registe in Program.cs
        private readonly CollegeDBContext _dbContext;

        //AutoMapper Injection
        private readonly IMapper _mapper;

        public StudentController(ILogger<StudentController> logger, CollegeDBContext dbContext, IMapper mapper)
        {
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudentsAsync()
        {
            _logger.LogInformation("GetStudent method started");

            //To return the all Columns data from student table      
            var students = await _dbContext.Students.ToListAsync();

            //Manual copy
            #region Manual copy
            // To return the particular columns from student table use "StudentDTO"
            /*
            var students = await _dbContext.Students.Select(item => new StudentDTO()
            {
                //*Here you can observe that we are copying each and every property from Student entity class to StudentDTO class
                //- like these you can see below there are several times copied for single contrller. so there are several lines of codes for copying properties
                //- To reduce the lines of code and time consuming we can use "AutoMapper"
                //- we can write single line of code instead of below lines of codes using AutoMapper library
                
                //Manual copy
                Id = item.Id,
                StudentName = item.StudentName,
                Email = item.Email,
                Address = item.Address,
                DOB = item.DOB, //if the DOB is String Type use =>  { DOB = item.DOB.ToShortDateString(); }
            }).ToListAsync();
            */
            #endregion

            //AutoMapper
            //Using auto mapper for copy Instead of using Manual copy. and here we reduced 6 lines of code
            var studentDTO = _mapper.Map<List<StudentDTO>>(students);

            return Ok(studentDTO);

        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentDTO>> GetStudentByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Bad Request");
                return BadRequest();
            }

            var student = await _dbContext.Students.Where(n => n.Id == id).FirstOrDefaultAsync();

            if (student == null)
            {
                _logger.LogError("Student not found with given ID");
                return NotFound($" Student with id {id} not found");
            }

            //Manual copy
            #region Manual copy
            /*
            var studentDTO = new StudentDTO()
            {
                Id = student.Id,
                StudentName = student.StudentName,
                Email = student.Email,
                Address = student.Address,
                DOB = student.DOB,
            };
            */
            #endregion

            //AutoMapper
            //Using auto mapper for copy Instead of using Manual copy. and here we reduced 6 lines of code
            var studentDTO = _mapper.Map<StudentDTO>(student);

            return Ok(studentDTO);
        }

        [HttpGet("{name:alpha}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentDTO>> GetStudentByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest();
            }

            var student = await _dbContext.Students.Where(n => n.StudentName == name).FirstOrDefaultAsync();

            if (student == null)
            {
                return NotFound($" Student with id {name} not found");
            }

            //Manual copy
            #region Manual copy
            /*
            var studentDTO = new StudentDTO()
            {
                Id = student.Id,
                StudentName = student.StudentName,
                Email = student.Email,
                Address = student.Address,
                DOB = student.DOB,
            };
            */
            #endregion

            //AutoMapper
            //Using auto mapper for copy Instead of using Manual copy. and here we reduced 6 lines of code
            var studentDTO = _mapper.Map<StudentDTO>(student);

            return Ok(studentDTO);

        }


        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<StudentDTO>> CreateStudentAsync([FromBody] StudentDTO studentDTO) //[FromBody] is called as PAYLOAD
        {

            if (studentDTO == null)
            {
                return BadRequest();
            }

            //No need this line of code bcz Id is auto increment 
            //int newId = _dbContext.Students.LastOrDefault().Id + 1;

            //Manual Copy
            #region Manual Copy
            /*
            Student student = new Student
            {
                //Id = newId,
                StudentName = studentDTO.StudentName,
                Email = studentDTO.Email,
                Address = studentDTO.Address,
                DOB = studentDTO.DOB,

            };
            */
            #endregion 

            //AutoMapper
            //Using auto mapper for copy Instead of using Manual copy. and here we reduced 6 lines of code
            var student = _mapper.Map<Student>(studentDTO);

            await _dbContext.Students.AddAsync(student);
            await _dbContext.SaveChangesAsync();

            studentDTO.Id = student.Id;
            return Ok(CreatedAtRoute("GetStudentById", new { id = studentDTO.Id }, studentDTO));


        }


        [HttpPut]
        [Route("Update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult> UpdateStudentAsync([FromBody] StudentDTO studentDTO)
        {
            if (studentDTO == null || studentDTO.Id <= 0)
                return BadRequest();


            //The existingStudent ID is already tracked So, we will face error bcz we can not create new record using existing student ID.
            /*
            var existingStudent = _dbContext.Students.Where(s => s.Id == model.Id).FirstOrDefault();
            */

            //AsNoTracking()
            //To Create the new record with existingStudent ID we need to UnTrack the Id for that we need to add "AsNoTracking()"
            var existingStudent = await _dbContext.Students.AsNoTracking().Where(s => s.Id == studentDTO.Id).FirstOrDefaultAsync();


            if (existingStudent== null)
            {
                return null;
            }

            //Manual Copy
            #region
            //Instead of updating "existingStudent" record, Here Iam creating new record with existing student id
            /*
            var newRecord = new Student()
            {
                Id = existingStudent.Id, // we are creating new record using existing student id
                StudentName= studentDTO.StudentName,
                Email= studentDTO.Email,
                Address = studentDTO.Address,
                DOB = studentDTO.DOB,

            };*/
            #endregion

            //AutoMapper
            //Using auto mapper for copy Instead of using Manual copy.
            var newRecord = _mapper.Map<Student>(studentDTO);

            _dbContext.Students.Update(newRecord);

            /*
            existingStudent.StudentName = model.StudentName;
            existingStudent.Email = model.Email;
            existingStudent.Address = model.Address;
            existingStudent.DOB = model.DOB; //if the DOB is String Type use =>  { existingStudent.DOB = Convert.ToDateTime(model.DOB); }
            */

            await _dbContext.SaveChangesAsync();

            return NoContent();

        }
       

      
        [HttpPatch]
        [Route("{id:int}/UpdateParcial")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult> UpdateStudentParcialAsync(int id, [FromBody] JsonPatchDocument<StudentDTO> patchDocument)
        {

            if (patchDocument == null || id <= 0)
            {
                return BadRequest();
            }

            var existingStudent = await _dbContext.Students.AsNoTracking().Where(s => s.Id== id).FirstOrDefaultAsync();

            if (existingStudent == null)
            {
                return NotFound();
            }
            //Manual Copy
            #region Manual Copy
            /*
           var studentDTO = new StudentDTO
           {
               Id = existingStudent.Id,
               StudentName = existingStudent.StudentName,
               Email = existingStudent.Email,
               Address = existingStudent.Address,
               DOB = existingStudent.DOB,
           };
           */
            #endregion

            //AutoMapper
            //Using auto mapper for copy Instead of using Manual copy
            var studentDTO = _mapper.Map<StudentDTO>(existingStudent);

            patchDocument.ApplyTo(studentDTO, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //Manual Copy
            /*
            existingStudent.StudentName = studentDTO.StudentName;
            existingStudent.Email = studentDTO.Email;
            existingStudent.Address = studentDTO.Address;
            existingStudent.DOB = studentDTO.DOB;
            */

            //AutoMapper
            existingStudent =_mapper.Map<Student>(studentDTO);

            _dbContext.Students.Update(existingStudent);

            await _dbContext.SaveChangesAsync();

            //204 - NoContent
            return NoContent();

        }
       

        [HttpDelete("Delete/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> DeleteStudentByIdAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var student = await _dbContext.Students.Where(n => n.Id == id).FirstOrDefaultAsync();

            if (student == null)
            {
                return NotFound($" Student with id {id} not found");
            }
            _dbContext.Students.Remove(student);
            await _dbContext.SaveChangesAsync();

            return Ok(true);
        }
    }
}
