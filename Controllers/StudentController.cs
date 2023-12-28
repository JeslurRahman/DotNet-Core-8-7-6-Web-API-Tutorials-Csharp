using AutoMapper;
using CollegeApp.Data;
using CollegeApp.Data.Repository;
using CollegeApp.Model;
using CollegeApp.MyLogging;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;
using System.Linq.Expressions;
using System.Net;

/*                                                                  Mendatory Note!! Read It for clarification of previous studies
                                                                  
 * You can see here now there are no any "Database or high level operations" inside this low level component
 
 * You can see here we have used "Common Repository pattern" eg:(Collegerepository) for this "StudentController.cs". - This is the best practice

 * If you want to use "speific Repository pattern" eg:(StudentRepository).you can check the "StudentControllerSpecificRepositoryPatternNotes.cs" file. - This is not best practice
  
 * I have cleared all the comments which i have written as notes in this file.
 - For the previous notes up to "Specific  Repository pattern". check the "StudentControllerSpecificRepositoryPatternNotes.cs" file
 */

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;
        private readonly IMapper _mapper;

        //Here we are going to use Common Repository pattern instead of using Specific Repository
        /*
         private readonly IStudentRepository _studentRepository;
        */

        //Common Repository
        private readonly ICollegeRepository<Student> _studentRepository;

        public StudentController(ILogger<StudentController> logger, ICollegeRepository<Student> studentRepository, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _studentRepository = studentRepository;

        }

        [HttpGet("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudentsAsync()
        {
            _logger.LogInformation("GetStudent method started");
            var students = await _studentRepository.GetAllAsync();
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

            var student = await _studentRepository.GetByIdAsync(student => student.Id == id); //Here we have passed delegate parameter instead of "id"

            if (student == null)
            {
                _logger.LogError("Student not found with given ID");
                return NotFound($" Student with id {id} not found");
            }

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

            /*we dont need any case sensitive for that we have used {ToLower()} func
            -and instead of exact match {student.StudentName == Name}, we used partial match {student.StudentName.Contains(name)} */
            var student = await _studentRepository.GetByNameAsync(student => student.StudentName.ToLower().Contains(name.ToLower())); 

            if (student == null)
            {
                return NotFound($" Student with id {name} not found");
            }

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

            var student = _mapper.Map<Student>(studentDTO);
            var studentAfterCreation = await _studentRepository.CreateAsync(student);

            studentDTO.Id = studentAfterCreation.Id;
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

            var existingStudent = await _studentRepository.GetByIdAsync(student => student.Id == studentDTO.Id, true);

            if (existingStudent== null)
            {
                return null;
            }

            var newRecord = _mapper.Map<Student>(studentDTO);
            await _studentRepository.UpdateAsync(newRecord);

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
            var existingStudent = await _studentRepository.GetByIdAsync(student => student.Id == id, true);

            if (existingStudent == null)
            {
                return NotFound();
            }
            var studentDTO = _mapper.Map<StudentDTO>(existingStudent);

            patchDocument.ApplyTo(studentDTO, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            existingStudent = _mapper.Map<Student>(studentDTO);
            await _studentRepository.UpdateAsync(existingStudent);

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

            var student = await _studentRepository.GetByIdAsync(student => student.Id == id);

            if (student == null)
            {
                return NotFound($" Student with id {id} not found");
            }

            await _studentRepository.DeleteAsync(student);

            return Ok(true);
        }
    }
}
