using CollegeApp.Model;
using CollegeApp.MyLogging;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        //This is Default build in logger
        private readonly ILogger<StudentController> _logger;

        public StudentController(ILogger<StudentController> logger)
        {
            _logger = logger;
        }

        [HttpGet("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<StudentDTO>> GetStudents()
        {
            _logger.LogInformation("GetStudent method started");

            var students = CollegeRepository.Students.Select(item => new StudentDTO()
            {
                Id = item.Id,
                StudentName = item.StudentName,
                Email = item.Email,
                Address = item.Address,
            });
            return Ok(CollegeRepository.Students);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentDTO> GetStudentById(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Bad Request");
                return BadRequest();
            }

            var student = CollegeRepository.Students.Where(n => n.Id == id).FirstOrDefault();

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
            };

            return Ok(student);
        }

        [HttpGet("{name:alpha}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentDTO> GetStudentByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest();
            }

            var student = CollegeRepository.Students.Where(n => n.StudentName == name).FirstOrDefault();

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
            };

            return Ok(student);

        }


        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public ActionResult<StudentDTO> CreateStudent([FromBody] StudentDTO model) //[FromBody] is called as PAYLOAD
        {

            if (model == null)
            {
                return BadRequest();
            }

            int newId = CollegeRepository.Students.LastOrDefault().Id + 1;

            Student student = new Student
            {
                Id = newId,
                StudentName = model.StudentName,
                Email = model.Email,
                Address = model.Address,

            };
            CollegeRepository.Students.Add(student);
            model.Id = student.Id;
            return Ok(CreatedAtRoute("GetStudentById", new { id = model.Id }, model));


        }


        [HttpPut]
        [Route("Update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public ActionResult UpdateStudent([FromBody] StudentDTO model)
        {
            if (model == null || model.Id <= 0)
                return BadRequest();

            var existingStudent = CollegeRepository.Students.Where(s => s.Id == model.Id).FirstOrDefault();

            if (existingStudent== null)
            {
                return null;
            }

            existingStudent.StudentName = model.StudentName;
            existingStudent.Email = model.Email;
            existingStudent.Address = model.Address;

            return NoContent();

        }
       

      
        [HttpPatch]
        [Route("{id:int}/UpdateParcial")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public ActionResult UpdateStudentParcial(int id, [FromBody] JsonPatchDocument<StudentDTO> patchDocument)
        {

            if (patchDocument == null || id <= 0)
            {
                return BadRequest();
            }

            var existingStudent = CollegeRepository.Students.Where(s => s.Id== id).FirstOrDefault();

            if (existingStudent == null)
            {
                return NotFound();
            }

            var studentDTO = new StudentDTO
            {
                Id = existingStudent.Id,
                StudentName = existingStudent.StudentName,
                Email = existingStudent.Email,
                Address = existingStudent.Address

            };

            patchDocument.ApplyTo(studentDTO, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            existingStudent.StudentName = studentDTO.StudentName;
            existingStudent.Email = studentDTO.Email;
            existingStudent.Address = studentDTO.Address;
            //204 - NoContent
            return NoContent();

        }
       

        [HttpDelete("Delete/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<bool> DeleteStudentById(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var student = CollegeRepository.Students.Where(n => n.Id == id).FirstOrDefault();

            if (student == null)
            {
                return NotFound($" Student with id {id} not found");
            }
            CollegeRepository.Students.Remove(student);
            return Ok(true);
        }
    }
}
