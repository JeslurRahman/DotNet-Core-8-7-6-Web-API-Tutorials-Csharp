//using CollegeApp.Model;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.JsonPatch;
//using Microsoft.AspNetCore.Mvc;

//namespace CollegeApp.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class StudentControllerNotes : ControllerBase
//    {

//        //2. Loosely Coupled Technique
//        /*
//        private readonly IMyLogger _myLogger;
//        public StudentController(IMyLogger myLogger)
//        {
//            _myLogger = myLogger;
//        }
//        */

//        //we can add route in two different ways
//        /*
//        //1.
//        [HttpGet]
//        [Route("{id:int}", Name = "GetStudentById")]

//        //2.
//        [HttpGet("{id:int}")]

//        Eg: 
//            http://localhost/api/student/{id}
//        */

//        [HttpGet]

//        //Documenting web api responses
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//        public ActionResult<IEnumerable<StudentDTO>> GetStudents()
//        {
//            //using Forech you we can conver the student list into the student DTO list
//            /*
//            var students = new List<StudentDTO>();
//            foreach (var item in CollegeRepository.Students)
//            {
//                StudentDTO obj = new StudentDTO()
//                {
//                    Id = item.Id,
//                    StudentName = item.StudentName,
//                    Email= item.Email,
//                    Address= item.Address,
//                };
//                students.Add(obj);

//            }
//            */

//            //Link you is the better choice because its reduce the lines of codes
//            //using Link you we can conver the student list into the student DTO list
//            var students = CollegeRepository.Students.Select(item => new StudentDTO()
//            {
//                Id = item.Id,
//                StudentName = item.StudentName,
//                Email = item.Email,
//                Address = item.Address,
//            });
//            //OK- 200 - Success
//            return Ok(CollegeRepository.Students);
//        }

//        [HttpGet("{id:int}")]
//        /*
//         [HttpGet]
//         [Route("{id:int}", Name = "GetStudentById")]
//         */

//        //Documenting web api responses
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//        public ActionResult<StudentDTO> GetStudentById(int id)
//        {
//            //BadRequest 400 - client error
//            if (id <= 0)
//            {
//                return BadRequest();
//            }

//            var student = CollegeRepository.Students.Where(n => n.Id == id).FirstOrDefault();
//            //NotFound - 404 - client error
//            if (student == null)
//            {
//                return NotFound($" Student with id {id} not found");
//            }

//            var studentDTO = new StudentDTO()
//            {
//                Id = student.Id,
//                StudentName = student.StudentName,
//                Email = student.Email,
//                Address = student.Address,
//            };
//            //OK- 200 - Success
//            return Ok(student);
//        }

//        [HttpGet("{name:alpha}")]
//        //Documenting web api responses
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//        public ActionResult<StudentDTO> GetStudentByName(string name)
//        {
//            //BadRequest 400 - client error
//            if (string.IsNullOrEmpty(name))
//            {
//                return BadRequest();
//            }

//            var student = CollegeRepository.Students.Where(n => n.StudentName == name).FirstOrDefault();

//            //NotFound - 404 - client error
//            if (student == null)
//            {
//                return NotFound($" Student with id {name} not found");
//            }
//            var studentDTO = new StudentDTO()
//            {
//                Id = student.Id,
//                StudentName = student.StudentName,
//                Email = student.Email,
//                Address = student.Address,
//            };

//            return Ok(student);

//        }

//        [HttpPost]
//        [Route("Create")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

//        public ActionResult<StudentDTO> CreateStudent([FromBody] StudentDTO model) //[FromBody] is called as PAYLOAD
//        {
//            //Without [ApiController] Validating student Model
//            /*
//            if (!ModelState.IsValid)
//                return BadRequest(ModelState);
//            */

//            if (model == null)
//            {
//                return BadRequest();
//            }

//            //Validating Date and Time atribute using 2 methods
//            /*
//              1.Directy adding error message to modelstate
//              2. Using Custom attribute (Check the validators folder) 
//            */

//            //1.Directy adding error message to modelstate

//            /*
//            if (model.AdmissionDate < DateTime.Now)
//            {
//                ModelState.AddModelError("AdmissionDate Error", "Admission date must be greater than equel to todays date");
//                return BadRequest(ModelState);
//            }
//            */

//            int newId = CollegeRepository.Students.LastOrDefault().Id + 1;

//            Student student = new Student
//            {
//                Id = newId,
//                StudentName = model.StudentName,
//                Email = model.Email,
//                Address = model.Address,

//            };
//            CollegeRepository.Students.Add(student);
//            model.Id = student.Id;
//            return Ok(CreatedAtRoute("GetStudentById", new { id = model.Id }, model));


//        }


//        [HttpPut]
//        [Route("Update")]
//        [ProducesResponseType(StatusCodes.Status204NoContent)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

//        public ActionResult UpdateStudent([FromBody] StudentDTO model)
//        {
//            if (model == null || model.Id <= 0)
//                return BadRequest();

//            var existingStudent = CollegeRepository.Students.Where(s => s.Id == model.Id).FirstOrDefault();

//            if (existingStudent == null)
//            {
//                return null;
//            }

//            existingStudent.StudentName = model.StudentName;
//            existingStudent.Email = model.Email;
//            existingStudent.Address = model.Address;

//            return NoContent();

//        }



//        [HttpPatch] //  To update the one filed use Patch
//        [Route("{id:int}/UpdateParcial")]
//        [ProducesResponseType(StatusCodes.Status204NoContent)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

//        public ActionResult UpdateStudentParcial(int id, [FromBody] JsonPatchDocument<StudentDTO> patchDocument)
//        {
//            //*To support the Patch. Need to instal 2 libraries
//            //* 1.Microsoft.AspNetCore.JsonPatch
//            //* 2.Microsoft.AspNetCore.Mvc.NewtonsoftJson 

//            if (patchDocument == null || id <= 0)
//            {
//                return BadRequest();
//            }

//            var existingStudent = CollegeRepository.Students.Where(s => s.Id == id).FirstOrDefault();

//            if (existingStudent == null)
//            {
//                return NotFound();
//            }

//            var studentDTO = new StudentDTO
//            {
//                Id = existingStudent.Id,
//                StudentName = existingStudent.StudentName,
//                Email = existingStudent.Email,
//                Address = existingStudent.Address

//            };

//            patchDocument.ApplyTo(studentDTO, ModelState);

//            if (!ModelState.IsValid)
//                return BadRequest(ModelState);

//            existingStudent.StudentName = studentDTO.StudentName;
//            existingStudent.Email = studentDTO.Email;
//            existingStudent.Address = studentDTO.Address;
//            //204 - NoContent
//            return NoContent();

//        }


//        [HttpDelete("Delete/{id:int}")]
//        //Documenting web api responses
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//        public ActionResult<bool> DeleteStudentById(int id)
//        {
//            //BadRequest 400 - client error
//            if (id <= 0)
//            {
//                return BadRequest();
//            }

//            var student = CollegeRepository.Students.Where(n => n.Id == id).FirstOrDefault();

//            //NotFound - 404 - client error
//            if (student == null)
//            {
//                return NotFound($" Student with id {id} not found");
//            }
//            CollegeRepository.Students.Remove(student);
//            return Ok(true);
//        }
//    }
//}
