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
        //This is Default build in logger
        private readonly ILogger<StudentController> _logger;

        //Removed in memory repository and use Entity framework 
        //For Dependancy Injection We need to registe in Program.cs
        private readonly CollegeDBContext _dbContext;

        public StudentController(ILogger<StudentController> logger, CollegeDBContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudentsAsync()
        {
            _logger.LogInformation("GetStudent method started");

            //To return the all Columns data from student table
            /*
            var students = _dbContext.Students.ToListAsync();
            */

            // To return the particular columns frome student table use "StudentDTO"
            var students = await _dbContext.Students.Select(item => new StudentDTO()
            {
                Id = item.Id,
                StudentName = item.StudentName,
                Email = item.Email,
                Address = item.Address,
                DOB = item.DOB, //if the DOB is String Type use =>  { DOB = item.DOB.ToShortDateString(); }
                
            }).ToListAsync();
            
            return Ok(students);

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

            var studentDTO = new StudentDTO()
            {
                Id = student.Id,
                StudentName = student.StudentName,
                Email = student.Email,
                Address = student.Address,
                DOB = student.DOB,
            };

            return Ok(student);
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
            var studentDTO = new StudentDTO()
            {
                Id = student.Id,
                StudentName = student.StudentName,
                Email = student.Email,
                Address = student.Address,
                DOB = student.DOB,
            };

            return Ok(student);

        }


        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<StudentDTO>> CreateStudentAsync([FromBody] StudentDTO model) //[FromBody] is called as PAYLOAD
        {

            if (model == null)
            {
                return BadRequest();
            }

            //No need this line of code bcz Id is auto increment 
            //int newId = _dbContext.Students.LastOrDefault().Id + 1;

            Student student = new Student
            {
                //Id = newId,
                StudentName = model.StudentName,
                Email = model.Email,
                Address = model.Address,
                DOB = model.DOB,

            };
            await _dbContext.Students.AddAsync(student);
            await _dbContext.SaveChangesAsync(); 
            
            model.Id = student.Id;
            return Ok(CreatedAtRoute("GetStudentById", new { id = model.Id }, model));


        }


        [HttpPut]
        [Route("Update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult> UpdateStudentAsync([FromBody] StudentDTO model)
        {
            if (model == null || model.Id <= 0)
                return BadRequest();


            //The existingStudent ID is already tracked So, we will face error bcz we can not create new record using existing student ID.
            /*
            var existingStudent = _dbContext.Students.Where(s => s.Id == model.Id).FirstOrDefault();
            */

            //AsNoTracking()
            //To Create the new record with existingStudent ID we need to UnTrack the Id for that we need to add "AsNoTracking()"
            var existingStudent = await _dbContext.Students.AsNoTracking().Where(s => s.Id == model.Id).FirstOrDefaultAsync();


            if (existingStudent== null)
            {
                return null;
            }

            //Instead of updating "existingStudent" record, Here Iam creating new record with existing student id
            var newRecord = new Student()
            {
                Id = existingStudent.Id, // we are creating new record using existing student id
                StudentName=model.StudentName,
                Email=model.Email,
                Address = model.Address,
                DOB = model.DOB,

            };

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

            var existingStudent = await _dbContext.Students.Where(s => s.Id== id).FirstOrDefaultAsync();

            if (existingStudent == null)
            {
                return NotFound();
            }

            var studentDTO = new StudentDTO
            {
                Id = existingStudent.Id,
                StudentName = existingStudent.StudentName,
                Email = existingStudent.Email,
                Address = existingStudent.Address,
                DOB = existingStudent.DOB,


            };

            patchDocument.ApplyTo(studentDTO, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            existingStudent.StudentName = studentDTO.StudentName;
            existingStudent.Email = studentDTO.Email;
            existingStudent.Address = studentDTO.Address;
            existingStudent.DOB = studentDTO.DOB;

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
