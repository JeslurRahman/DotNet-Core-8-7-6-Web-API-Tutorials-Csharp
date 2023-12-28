
/*                                                                  NOTES
                                                              
 // IN this file you can see the notes up to "Specific  Repository pattern".

             * before Using this file comment the StudentController.cs file
             * Here we have used the specific Repository pattern eg:{StudentRepository} for  this controller file
             * We have used Common Repository pattern for StudentController.cs file which is the best practice.
             * You can check StudentController.cs file 
 */

//using AutoMapper;
//using CollegeApp.Data;
//using CollegeApp.Data.Repository;
//using CollegeApp.Model;
//using CollegeApp.MyLogging;
//using Microsoft.AspNetCore.JsonPatch;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.ModelBinding;
//using Microsoft.EntityFrameworkCore;
//using System.Dynamic;
//using System.Net;

///*
// * You can see here now there are no any Database or high level operations inside this low level component
// */

//namespace CollegeApp.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class StudentController : ControllerBase
//    {
//        //Logger Injection
//        //This is Default build in logger
//        private readonly ILogger<StudentController> _logger;

//        //EntityFramework Injection
//        //Removed in memory repository and use Entity framework 
//        //For Dependancy Injection We need to registe in Program.cs
//        //Database or high level operations Moved to Repository class.
//        /*
//        private readonly CollegeDBContext _dbContext;
//        */

//        //AutoMapper Injection
//        private readonly IMapper _mapper;

//        //Repository Injection
//        private readonly IStudentRepository _studentRepository;

//        public StudentController(ILogger<StudentController> logger, IStudentRepository studentRepository, IMapper mapper)
//        {
//            _logger = logger;
//            _mapper = mapper;
//            _studentRepository = studentRepository;
//            //_dbContext = dbContext; //Database or high level operations Moved to Repository class.

//        }

//        //Repository pattern
//        #region Repository pattern
//        /*
//         * We are doing all the database operations directly inside the controller class. So, this is not a good practice. 
//          So, we are going to separate these DB operations with the help of abstraction layer called "Repository pattern"
//        */
//        #endregion

//        [HttpGet("All")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudentsAsync()
//        {
//            _logger.LogInformation("GetStudent method started");

//            //To return the all Columns data from student table
//            //Here we are applying Repostiory pattern. That mean we have seperated DB operations from low level component "StudentController"  and move to "StudentRepositoy" .
//            //Database or high level operations Moved to StudentRepository.
//            /*
//            var students = await _dbContext.Students.ToListAsync();
//            */

//            //Calling StudentRepository
//            //Now you can see here there is no any DB or high level operations
//            var students = await _studentRepository.GetAllAsync();

//            //Manual copy
//            #region Manual copy
//            // To return the particular columns from student table use "StudentDTO"
//            /*
//            var students = await _dbContext.Students.Select(item => new StudentDTO()
//            {
//                //*Here you can observe that we are copying each and every property from Student entity class to StudentDTO class
//                //- like these you can see below there are several times copied for single contrller. so there are several lines of codes for copying properties
//                //- To reduce the lines of code and time consuming we can use "AutoMapper"
//                //- we can write single line of code instead of below lines of codes using AutoMapper library

//                //Manual copy
//                Id = item.Id,
//                StudentName = item.StudentName,
//                Email = item.Email,
//                Address = item.Address,
//                DOB = item.DOB, //if the DOB is String Type use =>  { DOB = item.DOB.ToShortDateString(); }
//            }).ToListAsync();
//            */
//            #endregion

//            //AutoMapper
//            //Using auto mapper for copy Instead of using Manual copy. and here we reduced 6 lines of code
//            var studentDTO = _mapper.Map<List<StudentDTO>>(students);

//            return Ok(studentDTO);

//        }

//        [HttpGet("{id:int}")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//        public async Task<ActionResult<StudentDTO>> GetStudentByIdAsync(int id)
//        {
//            if (id <= 0)
//            {
//                _logger.LogWarning("Bad Request");
//                return BadRequest();
//            }

//            //Database or high level operations Moved to StudentRepository.
//            /*
//            var student = await _dbContext.Students.Where(n => n.Id == id).FirstOrDefaultAsync();
//            */

//            //Calling StudentRepository
//            //Now you can see here there is no any DB or high level operations
//            var student = await _studentRepository.GetByIdAsync(id);

//            if (student == null)
//            {
//                _logger.LogError("Student not found with given ID");
//                return NotFound($" Student with id {id} not found");
//            }

//            //Manual copy
//            #region Manual copy
//            /*
//            var studentDTO = new StudentDTO()
//            {
//                Id = student.Id,
//                StudentName = student.StudentName,
//                Email = student.Email,
//                Address = student.Address,
//                DOB = student.DOB,
//            };
//            */
//            #endregion

//            //AutoMapper
//            //Using auto mapper for copy Instead of using Manual copy. and here we reduced 6 lines of code
//            var studentDTO = _mapper.Map<StudentDTO>(student);

//            return Ok(studentDTO);
//        }

//        [HttpGet("{name:alpha}")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//        public async Task<ActionResult<StudentDTO>> GetStudentByNameAsync(string name)
//        {
//            if (string.IsNullOrEmpty(name))
//            {
//                return BadRequest();
//            }

//            //Database or high level operations Moved to StudentRepository.
//            /*
//            var student = await _dbContext.Students.Where(n => n.StudentName == name).FirstOrDefaultAsync();
//            */

//            //Calling StudentRepository
//            //Now you can see here there is no any DB or high level operations
//            var student = await _studentRepository.GetByNameAsync(name);

//            if (student == null)
//            {
//                return NotFound($" Student with id {name} not found");
//            }

//            //Manual copy
//            #region Manual copy
//            /*
//            var studentDTO = new StudentDTO()
//            {
//                Id = student.Id,
//                StudentName = student.StudentName,
//                Email = student.Email,
//                Address = student.Address,
//                DOB = student.DOB,
//            };
//            */
//            #endregion

//            //AutoMapper
//            //Using auto mapper for copy Instead of using Manual copy. and here we reduced 6 lines of code
//            var studentDTO = _mapper.Map<StudentDTO>(student);

//            return Ok(studentDTO);

//        }


//        [HttpPost]
//        [Route("Create")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

//        public async Task<ActionResult<StudentDTO>> CreateStudentAsync([FromBody] StudentDTO studentDTO) //[FromBody] is called as PAYLOAD
//        {

//            if (studentDTO == null)
//            {
//                return BadRequest();
//            }

//            //No need this line of code bcz Id is auto increment 
//            //int newId = _dbContext.Students.LastOrDefault().Id + 1;

//            //Manual Copy
//            #region Manual Copy
//            /*
//            Student student = new Student
//            {
//                //Id = newId,
//                StudentName = studentDTO.StudentName,
//                Email = studentDTO.Email,
//                Address = studentDTO.Address,
//                DOB = studentDTO.DOB,

//            };
//            */
//            #endregion 

//            //AutoMapper
//            //Using auto mapper for copy Instead of using Manual copy. and here we reduced 6 lines of code
//            var student = _mapper.Map<Student>(studentDTO);

//            //Database or high level operations Moved to StudentRepository.
//            /*
//            await _dbContext.Students.AddAsync(student);
//            await _dbContext.SaveChangesAsync();
//            */

//            //Calling StudentRepository
//            //Now you can see here there is no any DB or high level operations
//            var id = await _studentRepository.CreateAsync(student);

//            studentDTO.Id = id;
//            return Ok(CreatedAtRoute("GetStudentById", new { id = studentDTO.Id }, studentDTO));


//        }


//        [HttpPut]
//        [Route("Update")]
//        [ProducesResponseType(StatusCodes.Status204NoContent)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

//        public async Task<ActionResult> UpdateStudentAsync([FromBody] StudentDTO studentDTO)
//        {
//            if (studentDTO == null || studentDTO.Id <= 0)
//                return BadRequest();


//            //The existingStudent ID is already tracked So, we will face error bcz we can not create new record using existing student ID.
//            /*
//            var existingStudent = _dbContext.Students.Where(s => s.Id == model.Id).FirstOrDefault();
//            */

//            //AsNoTracking()
//            //To Create the new record with existingStudent ID we need to UnTrack the Id for that we need to add "AsNoTracking()"
//            //Database or high level operations Moved to StudentRepository.
//            /*
//            var existingStudent = await _dbContext.Students.AsNoTracking().Where(s => s.Id == studentDTO.Id).FirstOrDefaultAsync();
//            */

//            //Calling StudentRepository
//            //Now you can see here there is no any DB or high level operations
//            var existingStudent = await _studentRepository.GetByIdAsync(studentDTO.Id, true);

//            if (existingStudent == null)
//            {
//                return null;
//            }

//            //Manual Copy
//            #region
//            //Instead of updating "existingStudent" record, Here Iam creating new record with existing student id
//            /*
//            var newRecord = new Student()
//            {
//                Id = existingStudent.Id, // we are creating new record using existing student id
//                StudentName= studentDTO.StudentName,
//                Email= studentDTO.Email,
//                Address = studentDTO.Address,
//                DOB = studentDTO.DOB,

//            };*/
//            #endregion

//            //AutoMapper
//            //Using auto mapper for copy Instead of using Manual copy.
//            var newRecord = _mapper.Map<Student>(studentDTO);

//            //Database or high level operations Moved to StudentRepository.
//            /*
//            _dbContext.Students.Update(newRecord);
//            */

//            //Calling StudentRepository
//            await _studentRepository.UpdateAsync(newRecord);


//            /*
//            existingStudent.StudentName = model.StudentName;
//            existingStudent.Email = model.Email;
//            existingStudent.Address = model.Address;
//            existingStudent.DOB = model.DOB; //if the DOB is String Type use =>  { existingStudent.DOB = Convert.ToDateTime(model.DOB); }
//            */

//            //Database or high level operations Moved to StudentRepository.
//            /*
//            await _dbContext.SaveChangesAsync();
//            */

//            return NoContent();

//        }



//        [HttpPatch]
//        [Route("{id:int}/UpdateParcial")]
//        [ProducesResponseType(StatusCodes.Status204NoContent)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

//        public async Task<ActionResult> UpdateStudentParcialAsync(int id, [FromBody] JsonPatchDocument<StudentDTO> patchDocument)
//        {

//            if (patchDocument == null || id <= 0)
//            {
//                return BadRequest();
//            }

//            //Database or high level operations Moved to StudentRepository.
//            /*
//            var existingStudent = await _dbContext.Students.AsNoTracking().Where(s => s.Id== id).FirstOrDefaultAsync();
//            */

//            //Calling StudentRepository
//            var existingStudent = await _studentRepository.GetByIdAsync(id, true);

//            if (existingStudent == null)
//            {
//                return NotFound();
//            }
//            //Manual Copy
//            #region Manual Copy
//            /*
//           var studentDTO = new StudentDTO
//           {
//               Id = existingStudent.Id,
//               StudentName = existingStudent.StudentName,
//               Email = existingStudent.Email,
//               Address = existingStudent.Address,
//               DOB = existingStudent.DOB,
//           };
//           */
//            #endregion

//            //AutoMapper
//            //Using auto mapper for copy Instead of using Manual copy
//            var studentDTO = _mapper.Map<StudentDTO>(existingStudent);

//            patchDocument.ApplyTo(studentDTO, ModelState);

//            if (!ModelState.IsValid)
//                return BadRequest(ModelState);

//            //Manual Copy
//            #region Manual Copy
//            /*
//            existingStudent.StudentName = studentDTO.StudentName;
//            existingStudent.Email = studentDTO.Email;
//            existingStudent.Address = studentDTO.Address;
//            existingStudent.DOB = studentDTO.DOB;
//            */
//            #endregion

//            //AutoMapper
//            existingStudent = _mapper.Map<Student>(studentDTO);

//            //Database or high level operations Moved to StudentRepository.
//            /*
//            _dbContext.Students.Update(existingStudent);
//            await _dbContext.SaveChangesAsync();
//            */

//            await _studentRepository.UpdateAsync(existingStudent);

//            //204 - NoContent
//            return NoContent();

//        }


//        [HttpDelete("Delete/{id:int}")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//        public async Task<ActionResult<bool>> DeleteStudentByIdAsync(int id)
//        {
//            if (id <= 0)
//            {
//                return BadRequest();
//            }

//            //Database or high level operations Moved to StudentRepository.
//            /*
//            var student = await _dbContext.Students.Where(n => n.Id == id).FirstOrDefaultAsync();
//            */

//            //Calling StudentRepository
//            var student = await _studentRepository.GetByIdAsync(id);

//            if (student == null)
//            {
//                return NotFound($" Student with id {id} not found");
//            }

//            //Database or high level operations Moved to StudentRepository.
//            /*
//            _dbContext.Students.Remove(student);
//            await _dbContext.SaveChangesAsync();
//            */

//            //Calling StudentRepository
//            await _studentRepository.DeleteAsync(student);

//            return Ok(true);
//        }
//    }
//}
